using Hl7.Fhir.Model;
using POS_display.Repository.Recipe;
using POS_display.Views.Erecipe.Dispense;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Dispense;
using TamroUtilities.HL7.Models.Encounter;

namespace POS_display.wpf.ViewModel
{
    public class DispenseListViewModel : BaseViewModel
    {
        #region Commands
        public BaseAsyncCommand RefreshCommand { get; set; }
        public BaseAsyncCommand SignCommand { get; set; }
        public BaseAsyncCommand ConfirmCommand { get; set; }
        public BaseAsyncCommand GetDispensePdfCommand { get; set; }
        public BaseAsyncCommand GetRecipePdfCommand { get; set; }
        public BaseAsyncCommand CancelCommand { get; set; }
        public BaseAsyncCommand CreateEncounterCommand { get; set; }
        public BaseAsyncCommand PatientDispenseCommand { get; set; }
        public BaseAsyncCommand UpdateDataCommand { get; set; }
        public BaseAsyncCommand UpdateOrganizationRefCommand { get; set; }
        public BaseAsyncCommand GetDispenseConfirmListCommand { get; set; }
        public BaseAsyncCommand PageIndexChangedCommand { get; set; }
        public BaseAsyncCommand EditCommand { get; set; }
        #endregion

        public DispenseListViewModel()
        {
            SignCommand = new BaseAsyncCommand(Sign);
            ConfirmCommand = new BaseAsyncCommand(Confirm);
            GetDispensePdfCommand = new BaseAsyncCommand(GetDispensePdf);
            GetRecipePdfCommand = new BaseAsyncCommand(GetRecipePdf);
            RefreshCommand = new BaseAsyncCommand(Refresh);
            CancelCommand = new BaseAsyncCommand(Cancel);
            CreateEncounterCommand = new BaseAsyncCommand(CreateEncounter);
            PatientDispenseCommand = new BaseAsyncCommand(PatientDispense);
            UpdateDataCommand = new BaseAsyncCommand(UpdateData);
            UpdateOrganizationRefCommand = new BaseAsyncCommand(UpdateOrganizationRef);
            PageIndexChangedCommand = new BaseAsyncCommand(PageIndexChanged);
            EditCommand = new BaseAsyncCommand(Edit);
        }

        private async Task GetDispenseList()
        {
            DispenseListDto dispenseList = new DispenseListDto();
            if (FromDB)
            {
                decimal userId = 0;
                if (!string.IsNullOrWhiteSpace(FilterPractitionerId))
                    userId = Session.User.id;
                var dispenseToConfirm = await DB.eRecipe.GetDispenseListAsync(Status, Confirmed, DocStatus, userId, DateFrom, DateTo, Note2);
                CompositionIds = (from el in dispenseToConfirm.AsEnumerable()
                                  select el["composition_id"].ToString()).ToList();
                if (CompositionIds.Count > int.MaxValue)
                    throw new Exception("Per didelė duomenų imtis.\nSusiaurinkite paieškos kriterijus.");
                int startIndex = 0;
                int amountToTake = 100;
                int counter = Math.Ceiling(CompositionIds.Count.ToDecimal() / 100).ToInt();
                DispenseListDto dispenseListDto = new DispenseListDto
                {
                    DispenseList = new List<DispenseDto>(),
                    Page = 1,
                    PageSize = 0,
                    Total = 0
                };
                for (int i = 0; i < counter; i++)
                {
                    var listAmountToTake = CompositionIds.Skip(startIndex).Take(amountToTake).ToList();
                    var currentList = await Session.eRecipeUtils.GetDispenseList<DispenseListDto>("", "", FilterOrganizationId, "", "", "", amountToTake, PageIndex, "", listAmountToTake);
                    dispenseListDto.DispenseList.AddRange(currentList.DispenseList);
                    dispenseListDto.PageSize = dispenseListDto.PageSize.ToInt() + currentList.PageSize.ToInt();
                    dispenseListDto.Total = dispenseListDto.Total.ToInt() + currentList.Total.ToInt();
                    startIndex += amountToTake;
                }
                dispenseList = dispenseListDto;
            }
            else
                dispenseList = await Session.eRecipeUtils.GetDispenseList<DispenseListDto>(PatientId, FilterPractitionerId, FilterOrganizationId, Status, Confirmed, DocStatus, GridSize, PageIndex, "", CompositionIds);
            if ((dispenseList?.DispenseList?.Count ?? 0) <= 0)
                throw new Exception("Nepavyksta gauti išdavimo dokumentų pagal pasirinktus kriterijus");
            DispenseDto = dispenseList;
            NotifyPropertyChanged("IsEnabled");
        }

        #region Command definitions
        private async Task Sign()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                RecipeRepository recipeRepository = new RecipeRepository();
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");

                var first_recpe = SelectedDispenseItems.First();
                var signed_first = await Session.eRecipeUtils.SignDispense(first_recpe.CompositionId);//sign first sync to get pharmacist pin code

                if (signed_first)
                    await DB.eRecipe.UpdateErecipeDocumentStatusAsync(first_recpe.CompositionId.ToDecimal(), "Signed");

                foreach (var el in SelectedDispenseItems.Where(gv => gv.CompositionId != first_recpe.CompositionId))//sign left
                {
                    ExecuteAsyncAction(async () =>
                    {
                        var signed = await Session.eRecipeUtils.SignDispense(el.CompositionId);
                        if (signed)
                            await DB.eRecipe.UpdateErecipeDocumentStatusAsync(el.CompositionId.ToDecimal(), "Signed");
                    });
                }

                var recipeNumbers = await recipeRepository.GetRecipeNoByCompositionIds(
                    SelectedDispenseItems.Select(e => e.CompositionId.ToDecimal()).ToList());

                foreach (var recipeNo in recipeNumbers)
                {
                    await Session.eRecipeUtils.RetryLockRecipe(recipeNo.ToString(), false, string.Empty);
                }
                await GetDispenseList();
            });
        }

        private async Task Confirm()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var count_successful = 0;
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (Session.User.postname.ToLower().StartsWith("farmakot"))
                    throw new Exception("Farmakotechnikas negali patvirtinti el. receptų");
                foreach (var el in SelectedDispenseItems)
                {
                    var confirmed = await Session.eRecipeUtils.ConfirmRecipeDispense(el.Dispense.MedicationDispenseId, Session.PractitionerItem.PractitionerRef, true);
                    if (confirmed)
                        await DB.eRecipe.UpdateErecipeConfirmedAsync(el.CompositionId.ToDecimal(), 1);
                    count_successful++;
                    if (count_successful == SelectedDispenseItems.Count)
                        await GetDispenseList();
                }
            });
        }

        private async Task GetDispensePdf()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                foreach (var selected in SelectedDispenseItems)
                    await Session.eRecipeUtils.GetDispensePdf(selected.CompositionId);
            });
        }

        private async Task GetRecipePdf()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                foreach (var selected in SelectedDispenseItems)
                    await Session.eRecipeUtils.GetDispensePdf(selected.Dispense.PrescriptionDocumentNumber);
            });
        }

        private async Task Refresh()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                Session.eRecipeUtils.cts = new System.Threading.CancellationTokenSource();
                await GetDispenseList();
            });
        }

        private async Task PageIndexChanged()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if ((DispenseDto?.DispenseList?.Count ?? 0) <= 0)
                    throw new Exception("");
                var pageIndex = PageIndex;
                if (!FromDB)
                {
                    await GetDispenseList();
                    pageIndex = 1;
                }
                SetDispenseList(pageIndex);
            });
        }

        private async Task Edit() 
        {
            await ExecuteWithWaitAsync(async () => 
            {
                var selectedRowItem = SelectedDispenseItems.First();
                var recipeEditModel = await new RecipeRepository().GetRecipeEditDataByCompositionId(selectedRowItem.CompositionId.ToDecimal()) ??
                throw new Exception("Išdavimas gali būti redaguojamas tik vaistinėje kurioje buvo išduotas!");

                using (DispenseEditView dispenseEditView = new DispenseEditView()) 
                {
                    await dispenseEditView.Init(selectedRowItem.CompositionId);
                    if (dispenseEditView.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                    {
                        helpers.alert(Enumerator.alert.info,"Išdavimas buvo sėkmingai redaguotas ir išsaugotas");
                    }
                }
            });
        }

        private async Task Cancel()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (SelectedDispenseItems.Count > 1)
                    throw new Exception("Galima pasitinkti tik po vieną eilutę!");
                var selectedRowItem = SelectedDispenseItems.First();
                ProgressDialog dlg = new ProgressDialog("CancelRecipe", selectedRowItem.CompositionId);
                dlg.ShowDialog();
                if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (await Session.eRecipeUtils.CancelRecipeDispense(selectedRowItem.CompositionId, dlg.Result, null))
                    {
                        var data_by_composition_id = await DB.eRecipe.GetErecipeByCompositionIdAsync(selectedRowItem.CompositionId.ToDecimal());
                        decimal posd_id = 0;
                        decimal recipe_no = 0;
                        decimal recipe_id = 0;
                        if (data_by_composition_id?.Rows?.Count > 0)
                        {
                            posd_id = data_by_composition_id.Rows[0]["posd_id"].ToDecimal();
                            recipe_no = data_by_composition_id.Rows[0]["recipe_no"].ToDecimal();
                            recipe_id = data_by_composition_id.Rows[0]["recipe_id"].ToDecimal();
                        }
                        else
                        {
                            if (Session.RecipesOnly == false)
                            {
                                ProgressDialog dlg2 = new ProgressDialog("", "Įveskite kvito eilutės id (posd id)");
                                dlg2.ShowDialog();
                                if (dlg2.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    posd_id = dlg2.Result.ToDecimal();
                                    var recipe_data = await DB.recipe.getRecipeByPosdId(posd_id);
                                    if (recipe_data?.Rows?.Count > 0)
                                    {
                                        recipe_no = recipe_data.Rows[0]["recipe_no"].ToDecimal();
                                        recipe_id = recipe_data.Rows[0]["recipe_id"].ToDecimal();
                                    }
                                }
                                dlg2.Dispose();
                                dlg2 = null;
                            }
                            if (recipe_id == 0)
                            {
                                ProgressDialog dlg2 = new ProgressDialog("", "Įveskite recepto id (recipe id). Nekompensuojamo recepto atveju - 0");
                                dlg2.ShowDialog();
                                if (dlg2.DialogResult == System.Windows.Forms.DialogResult.OK && dlg2.Result.ToDecimal() > 0)
                                {
                                    recipe_id = dlg2.Result.ToDecimal();
                                    var recipe_data = await DB.recipe.asyncSearchRecipe(recipe_id);
                                    if (recipe_data?.Rows?.Count > 0)
                                    {
                                        recipe_no = recipe_data.Rows[0]["recipeno"].ToDecimal();
                                    }
                                }
                                dlg2.Dispose();
                                dlg2 = null;
                            }
                            if (recipe_no == 0)
                            {
                                ProgressDialog dlg2 = new ProgressDialog("", "Įveskite recepto numerį");
                                dlg2.ShowDialog();
                                if (dlg2.DialogResult == System.Windows.Forms.DialogResult.OK && dlg2.Result.ToDecimal() > 0)
                                {
                                    recipe_no = dlg2.Result.ToDecimal();
                                }
                                dlg2.Dispose();
                                dlg2 = null;
                            }
                        }
                        await DB.eRecipe.MarkErecipeAsync(posd_id, recipe_no, recipe_id, dlg.Result);
                        await GetDispenseList();
                    }
                }
                dlg.Dispose();
                dlg = null;
            });
        }

        private async Task CreateEncounter()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (SelectedDispenseItems.Count > 1)
                    throw new Exception("Galima pasitinkti tik po vieną eilutę!");
                var selectedRowItem = SelectedDispenseItems.First();
                await Session.eRecipeUtils.CreateEncounter(selectedRowItem.Dispense.PatientId,
                    selectedRowItem.Dispense.PatientRef,
                    Session.PractitionerItem.PractitionerRef,
                    Session.PractitionerItem.OrganizationRef,
                    string.Empty,
                    null,
                    EncounterReason.Prescription
                    );
                await GetDispenseList();
            });
        }

        private async Task PatientDispense()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (SelectedDispenseItems.Count > 1)
                    throw new Exception("Galima pasitinkti tik po vieną eilutę!");
                var selectedRowItem = SelectedDispenseItems.First();
                PatientId = selectedRowItem.Dispense.PatientId;
                FilterPractitionerId = "";
                FilterOrganizationId = "";
                Status = "completed";
                Confirmed = "";
                DocStatus = "";
                PageIndex = 1;
                await GetDispenseList();
            });
        }

        private async Task UpdateData()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (SelectedDispenseItems.Count > 1)
                    throw new Exception("Galima pasitinkti tik po vieną eilutę!");
                var selectedRowItem = SelectedDispenseItems.First();
                using (ProgressDialog dlg = new ProgressDialog("UpdateERecipe", "Įveskite id iš receipe erecipe lentos."))
                {
                    dlg.ShowDialog();
                    if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        var valid_encounter = Session.eRecipeEncounterList?.Where(en => en.PatientId == selectedRowItem.Dispense.PatientId);
                        await DB.eRecipe.UpdateErecipeAsync(dlg.Result.ToDecimal(),
                                                        selectedRowItem.Dispense.CompositionId.ToDecimal(),
                                                        selectedRowItem.Dispense.CompositionRef,
                                                        selectedRowItem.Dispense.CompositionStatus,
                                                        selectedRowItem.Dispense.MedicationDispenseId.ToDecimal(),
                                                        selectedRowItem.Dispense.Status,
                                                        selectedRowItem.Dispense.Status == "completed" ? 1 : 0,
                                                        string.IsNullOrWhiteSpace(selectedRowItem.Dispense.ConfirmedBy) ? 0 : 1,
                                                        selectedRowItem.Dispense.DocumentStatus,
                                                        ""
                                                        );
                    }
                }
            });
        }
                
        private async Task UpdateOrganizationRef()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (SelectedDispenseItems.Count == 0)
                    throw new Exception("Nepasirinkta nei viena eilutė!");
                if (SelectedDispenseItems.Count > 1)
                    throw new Exception("Galima pasitinkti tik po vieną eilutę!");
                if (!helpers.alert(Enumerator.alert.confirm, "Šis veiksmas gali negrįžtamai sugadinti išdavimo duomenis!\nAr tikrai norite pakeisti išdavusią įstaigą?", true))
                    throw new Exception("");
                string organization_ref = "";
                if (helpers.alert(Enumerator.alert.confirm, "Jei norite OrganizationRef nurodyti patys, spauskite Taip, jei norite naudoti pagal prisijungimą - spauskite Ne", true))
                {
                    using (ProgressDialog dlg = new ProgressDialog("UpdateERecipe", "Įveskite OrganizationRef"))
                    {
                        dlg.ShowDialog();
                        if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            organization_ref = dlg.Result;
                        }
                        else
                            throw new Exception("");
                    }
                }
                else
                    organization_ref = Session.OrganizationItem.OrganizationRef;
                //
                string dueDate;
                using (ProgressDialog dlg = new ProgressDialog("UpdateERecipe", "Įveskite Pakanka iki datą. Jei keisti nereikia - palikti tuščią."))
                {
                    dlg.ShowDialog();
                    if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        dueDate = dlg.Result;
                    }
                    else
                        throw new Exception("");
                }
                var selectedRowItem = SelectedDispenseItems.First();

                var request = new UpdateDispenseRequest()
                {
                    PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                    CompositionId = selectedRowItem.CompositionId.ToLong(),
                    DueDate = string.IsNullOrWhiteSpace(dueDate) ? DateTime.MinValue : dueDate.ToDateTime(),
                };
                Session.eRecipeUtils.UpdateDispense(request);
                await GetDispenseList();
            });
        }

        private void SetDispenseList(int pageIndex)
        {
            var toSkip = (pageIndex - 1) * Session.eRecipeGridSize;
            DispenseList = (from el in DispenseDto?.DispenseList
                            select new wpf.Model.dispenseListModel(el)).Skip(toSkip).Take(Session.eRecipeGridSize).ToList();
        }
        #endregion

        #region Variables
        private DispenseListDto _DispenseDto;
        public DispenseListDto DispenseDto
        {
            get
            {
                return _DispenseDto;
            }
            set
            {
                _DispenseDto = value;
                decimal total = _DispenseDto?.Total ?? 0;
                PageCount = (int)Math.Ceiling(total / Session.eRecipeGridSize);
                SetDispenseList(PageIndex);
            }
        }

        private List<wpf.Model.dispenseListModel> _DispenseList;
        public List<wpf.Model.dispenseListModel> DispenseList
        {
            get
            {
                return _DispenseList;
            }
            set
            {
                _DispenseList = value;
                NotifyPropertyChanged("DispenseList");
            }
        }

        public bool IsEnabled
        {
            get
            {
                return (DispenseDto?.DispenseList?.Count ?? 0) > 0;
            }
        }

        public bool IsContextMenuEnabled
        {
            get
            {
                return Session.Admin;
            }
        }

        private List<wpf.Model.dispenseListModel> SelectedDispenseItems
        {
            get
            {
                return DispenseList.Where(w => w.selected == true).ToList();
            }
        }
        public string PatientId { get; set; }
        public string FilterPractitionerId { get; set; }
        public string FilterOrganizationId { get; set; }
        public string Status { get; set; }
        public string Confirmed { get; set; }
        public string DocStatus { get; set; }
        public int GridSize { get; set; }
        public bool FromDB { get; set; }
        public List<string> CompositionIds { get; set; } = new List<string>();
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Note2 { get; set; }
        private int _PageIndex;
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
                NotifyPropertyChanged("PageIndex");
            }
        }

        private int _PageCount;
        public int PageCount {
            get
            {
                return _PageCount;
            }
            set
            {
                _PageCount = value;
                NotifyPropertyChanged("PageCount");
            }
        }
        #endregion
    }
}

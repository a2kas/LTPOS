using Microsoft.Extensions.DependencyInjection;
using POS_display.Helpers;
using POS_display.Items.eRecipe;
using POS_display.Models;
using POS_display.Models.Recipe;
using POS_display.Models.TLK;
using POS_display.Popups.display1_popups.ERecipe;
using POS_display.Presenters.ERecipe;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Pos;
using POS_display.Repository.Recipe;
using POS_display.Utils.Logging;
using POS_display.Views.Erecipe;
using POS_display.Views.Erecipe.Dispense;
using POS_display.Views.Erecipe.PaperRecipe;
using POS_display.Views.Erecipe.Prepayment;
using POS_display.Views.HomeMode;
using POS_display.wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Tamroutilities.Client;
using TamroUtilities.HL7.Fhir.Core.Extension;
using TamroUtilities.HL7.Models;

namespace POS_display
{
    public partial class eRecipeV2 : FormBase, Views.ERecipe.IERecipe
    {
        private readonly ERecipePresenter _presenterList;
        private Items.posd posdItem;
        private UserControl _ucRecipeEdit;
        private wpf.View.wpfeRecipePdf eRecipeWPF = new wpf.View.wpfeRecipePdf(null);
        private wpf.View.Navigation RecipeListNavigation = new wpf.View.Navigation();
        private wpf.View.eRecipe.wpfVaccinationPdf vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(null);
        public wpf.ViewModel.BaseCommand RecipeListNavigationRefreshCommand;
        private wpf.View.RecipeList recipeList;
        private wpf.View.DispenseList dispenseList;
        private wpf.View.DispenseList dispenseListPatient;
        private wpf.View.VaccineList vaccineList;
        private VaccinationEntry _vaccineEntry = null;
        private const long ManufacturingPharmacyId = 5872182989;

        #region WS Callbacks
        public void _ucRecipeEdit_Show()
        {
            SuspendLayout();
            try
            {
                if (eRecipeItem.eRecipe_CompensationTag == true) //kompensuojamas receptas
                    _ucRecipeEdit = new ucRecipeEditV2(eRecipeItem, posdItem, PickedUpByRef);
                else //nekompensuojamas receptas
                    _ucRecipeEdit = new ucRecipeEdit_notCompV2(eRecipeItem, posdItem, PickedUpByRef);
                _ucRecipeEdit.Dock = DockStyle.Fill;
                panel1.Controls.Add(_ucRecipeEdit);
                _ucRecipeEdit.BringToFront();
            }
            finally 
            {
                ResumeLayout();
            }
        }

        private async void ucRecipeEdit_ControlRemoved(object sender, ControlEventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            { 
                ucRecipeEditBase uc = e.Control as ucRecipeEditBase;
                if (uc.out_form_action == "recipe_saved")
                {
                    if (eRecipeItem.IsDispenseBySubtances)
                    {
                        if (helpers.alert(Enumerator.alert.info, $"Ar norite pildyti išdavimą kitai šio recepto veikliajai medžiagai?", true))
                        {
                            await _presenterList.GetRecipeDispenseCount();
                            btnSell_Click(this, new EventArgs());
                        }
                        else
                        {
                            IsBusy = true;
                            await Controllers.eRecipe.PerformGroupDispenseSaving();
                            await RecipeListRefresh();
                            IsBusy = false;
                        }
                    }
                    else                    
                        await RecipeListRefresh();
                }
                else if (uc.out_form_action == "recipe_closed")
                    DeletePosd();
            },false);
        }

        private async void rb_CheckedChanged(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (eRecipeItem?.Patient?.PatientId.ToDecimal() > 0)
                {
                    RecipeListNavigation.PageIndex = 1;
                    await RecipeListRefresh();
                }
            });
        }

        private void GetDispensePdf_cb(bool success, string resultXML, XElement inParams)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                try
                {
                    if (success)
                    {

                        XElement root = XElement.Parse(resultXML);
                        var error = from el in root.Descendants("t")
                                    select new
                                    {
                                        txt = (string)el,
                                        error = (string)el.Element("err")
                                    };

                        if (error.First().error != null)
                            throw new Exception(error.First().error);

                        var result = from el in root.Descendants("t")
                                     select new
                                     {
                                         PdfBase64Encoded = inParams.Descendants("t").First().Element("ProcName").Value == "GetSignedDispensePdf" ?
                                         (string)el.Element("SignedPdfBase64Encoded") : (string)el.Element("UnsignedPdfBase64Encoded")
                                     };
                        if (result.First().PdfBase64Encoded != null)
                            helpers.ConvertBase64toPDF(result.First().PdfBase64Encoded, inParams.Descendants("t").First().Element("CompositionId").Value);
                        IsBusy = false;
                    }
                    else
                        throw new Exception("");
                }
                catch (Exception ex)
                {
                    if (inParams.Descendants("t").First().Element("ProcName").Value == "GetSignedDispensePdf")
                        Session.eRecipeUtils.GetUnsignedDispensePdf(inParams.Descendants("t").First().Element("CompositionId").Value, GetDispensePdf_cb);
                    else
                    {
                        IsBusy = false;
                        helpers.alert(Enumerator.alert.error, ex.Message);
                    }
                }
            }));
        }

        private void ReserveRecipe_cb(bool success, string resultXML, XElement inParams)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(async delegate
            {
                try
                {
                    if (success)
                    {

                        XElement root = XElement.Parse(resultXML);
                        var error = from el in root.Descendants("t")
                                    select new
                                    {
                                        txt = (string)el,
                                        error = (string)el.Element("err")
                                    };

                        if (error.First().error != null)
                            throw new Exception(error.First().error);
                        await RecipeListRefresh();
                    }
                    else
                        throw new Exception("");
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    helpers.alert(Enumerator.alert.warning, ex.Message);
                }
            }));
        }
        #endregion

        private async void DeletePosd(bool refresh = true)
        {
            if (!Session.RecipesOnly && posdItem != null)
            {
                await DB.POS.AsyncDeletePosd(posdItem.hid, posdItem.id);
                await _presenterList.DeleteHomeDeliveryDetail(posdItem.hid, posdItem.id);
                await Session.eRecipeUtils.RetryLockRecipe(eRecipeItem?.eRecipe?.RecipeNumber, false, posdItem.eRecipeDispenseBySubstancesGroupId);
            }

            if (refresh)
                await RecipeListRefresh();
        }

        public eRecipeV2()
        {
            InitializeComponent();
            _presenterList = new ERecipePresenter(this, new PosRepository(), new HomeModeRepository(), new RecipeRepository());
          
            vaccineUserControl.SelectionChanged += VaccineUserControl_SelectionChanged;
            vaccineUserControl.PanelChangedEvent += VaccineUserControl_LoadEvent;
            vaccineUserControl.OperationAborted += VaccineUserControl_OperationAborted;

            dtpVaccineFrom.Value = DateTime.Now.AddDays(-1);
            dtpVaccineTo.Value = DateTime.Now.AddDays(1);
        }

        private async void VaccineUserControl_OperationAborted(object sender, object data)
        {
            if (data != null && data is Items.posd)
            {
                await DB.POS.AsyncDeletePosd((data as Items.posd).hid, (data as Items.posd).id);
                await Program.Display1.RefreshPosh();
            }
        }

        private async void eRecipe_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (Session.RecipesOnly)
                {
                    this.Location = helpers.middleScreen2(this, false);
                    this.ShowInTaskbar = true;
                    this.MinimizeBox = true;
                    this.MaximizeBox = true;
                    await Program.TaskAsync();
                    lblSystem.Text = Session.Database + " / " + Session.DatabaseName;
                }
                if (Session.ExtendedPracticePractitioner is null)
                {
                    tabPanel1.TabPages.Remove(tabVaccination);
                    tabPanel1.TabPages.Remove(tabVaccinationSelection);
                }
                else
                {
                    if (Session.RecipesOnly)
                        tabPanel1.TabPages.Remove(tabVaccination);
                }
                if (string.IsNullOrWhiteSpace(Session.PractitionerItem?.PractitionerId ?? "") || string.IsNullOrWhiteSpace(Session.OrganizationItem?.OrganizationId ?? ""))
                {
                    helpers.alert(Enumerator.alert.error, "Nėra ryšio su Registrų centru. Mėginkite vėliau.");
                    Application.Exit();
                    return;
                }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DB.POS.UpdateSession("E - Receptas", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                tbPersonalCode.Select();
                lblPractitioner.Text = Session.PractitionerItem.Roles.First().Name + ": " + Session.PractitionerItem.GivenName.First() + " " + Session.PractitionerItem.FamilyName.First();

                Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    { "1", "Vaistinės" },
                    { "2", "Mano vaistinėje" }
                };
                //dic.Add("3", "Visi mano");
                cmbFilter.DataSource = new BindingSource(dic, null);
                cmbFilter.DisplayMember = "Value";
                cmbFilter.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "", "Visi" },
                    { "completed", "Išduoti" },
                    { "entered in error", "Panaikinti" }
                };
                cmbStatus.DataSource = new BindingSource(dic, null);
                cmbStatus.DisplayMember = "Value";
                cmbStatus.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "", "Visi" },
                    { "Signed", "Pasirašyti" },
                    { "final", "Nepasirašyti" },
                    { "entered in error", "Panaikinti" }
                };
                cmbDocStatus.DataSource = new BindingSource(dic, null);
                cmbDocStatus.DisplayMember = "Value";
                cmbDocStatus.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "", "Visi" },
                    { "true", "Taip" },
                    { "false", "Ne" }
                };
                cmbConfirmed.DataSource = new BindingSource(dic, null);
                cmbConfirmed.DisplayMember = "Value";
                cmbConfirmed.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "1", "Vaistinės" },
                    { "2", "Mano" },
                    { "3", "Mano vaistinėje" }
                };
                cmbVaccineDoc.DataSource = new BindingSource(dic, null);
                cmbVaccineDoc.DisplayMember = "Value";
                cmbVaccineDoc.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "1", "Visi" },
                    { "2", "Skyrimas" },
                    { "3", "Išdavimas" }
                };
                cmbDocType.DataSource = new BindingSource(dic, null);
                cmbDocType.DisplayMember = "Value";
                cmbDocType.ValueMember = "Key";
                
                dic = new Dictionary<string, string>
                {
                    { "", "Visi" },
                    { "final,preliminary", "Išduoti\\Paskirti" },
                    { "entered in error", "Panaikinti" }
                };
                cmbVaccineStatus.DataSource = new BindingSource(dic, null);
                cmbVaccineStatus.DisplayMember = "Value";
                cmbVaccineStatus.ValueMember = "Key";

                dic = new Dictionary<string, string>
                {
                    { "", "Visi" },
                    { "Signed", "Pasirašyti" },
                    { "final", "Nepasirašyti" },
                    { "entered in error", "Panaikinti" }
                };
                cmbVaccineDocStatus.DataSource = new BindingSource(dic, null);
                cmbVaccineDocStatus.DisplayMember = "Value";
                cmbVaccineDocStatus.ValueMember = "Key";

                if (Session.Develop == true)
                {//tbPersonalCode.Text = "37805310494";
                    tbPersonalCode.Text = "34010230353";
                }
                SendMessage(tbPersonalCode.Handle, 0x1501, 1, "Asmens kodas");
                ClearRecipeList();
                ClearDispenseList();
                ClearVaccineList();
                ClearVaccineOrderList();
                SetDefaultValues();
                SetWindowSize();
            });            
        }


        private void SetWindowSize()
        {
            if (Size.Height > Screen.PrimaryScreen.Bounds.Height ||
                Size.Width > Screen.PrimaryScreen.Bounds.Width)
            WindowState = FormWindowState.Maximized;
        }

        private void SetDefaultValues()
        {
            if (cmbVaccineDoc.Items.Count > 0)
                cmbVaccineDoc.SelectedIndex = 1;

            if (cmbVaccineStatus.Items.Count > 1)
                cmbVaccineStatus.SelectedIndex = 1;

            if (cmbVaccineDocStatus.Items.Count > 2)
                cmbVaccineDocStatus.SelectedIndex = 2;
        }

        private bool _IsBusy;
        internal override bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                if (recipeList != null)
                    recipeList.IsBusy = value;
                if (dispenseList != null)
                    dispenseList.IsBusy = value;
                if (dispenseListPatient != null)
                    dispenseListPatient.IsBusy = value;
                if (vaccineList != null)
                    vaccineList.IsBusy = value;
                if (_IsBusy == true)
                {
                    this.UseWaitCursor = true;
                    this.Cursor = Cursors.WaitCursor;
                    eRecipeWPF.form_wait(_IsBusy);
                }
                else
                {
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                    eRecipeWPF.form_wait(_IsBusy);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Session.RecipesOnly)
                Application.Exit();
            else
                this.DialogResult = DialogResult.Cancel;
        }

        private void tabPanel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabPanel1.SelectedIndex)
            {
                case 0:
                    {
                        eRecipeWPF = new wpf.View.wpfeRecipePdf(eRecipeItem);
                        ehRecipe.Child = eRecipeWPF;
                        break;
                    }
                case 1:
                    {
                        eRecipeWPF = new wpf.View.wpfeRecipePdf(dispenseItem);
                        ehRecipe.Child = eRecipeWPF;
                        break;
                    }
                case 2:
                    {
                        vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(vaccineUserControl.SelectedVaccineEntry);
                        ehRecipe.Child = vaccinationOrderWPF;
                        break;
                    }
                case 3:
                    {
                        vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(_vaccineEntry);
                        ehRecipe.Child = vaccinationOrderWPF;
                        break;
                    }
            }
        }

        #region RecipeListEvents
        private void tbPersonalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                btnFind_Click(btnFind, new EventArgs());
        }

        private void cmbRelated_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            Execute(() =>
            {
                if (cmbRelated.SelectedIndex > 0)
                {
                    var selected_item = cmbRelated.SelectedItem;
                    Type type = selected_item.GetType();
                    string user_id = (string)type.GetProperty("Reference").GetValue(selected_item, null);
                    string patient_ref = eRecipeItem.Patient.PatientRef;
                    tbPersonalCode.Text = "";
                    PickedForUserId = user_id.Replace("Patient/", "");
                    var patient_name = $"{PatientName} {Surname}";
                    btnFind_Click(new object(), new EventArgs());
                    lblRepresented.Text = "Atsiimti atėjęs klientas: " + patient_name;
                    PickedUpByRef = patient_ref;
                }
            });
        }

        private void cmbRepresented_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBusy)
                return;
            Execute(() =>
            {
                if (cmbRepresented.SelectedIndex > 0)
                {
                    var selected_item = cmbRepresented.SelectedItem;
                    Type type = selected_item.GetType();
                    string personal_code = (string)type.GetProperty("PersonalCode").GetValue(selected_item, null);
                    string patient_ref = eRecipeItem.Patient.PatientRef;
                    tbPersonalCode.Text = personal_code;
                    var patient_name = $"{PatientName} {Surname}";
                    btnFind_Click(new object(), new EventArgs());
                    lblRepresented.Text = "Atsiimti atėjęs klientas: " + patient_name;
                    PickedUpByRef = patient_ref;
                }
            });
        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                ClearRecipeList();
                if (tbPersonalCode.Text == "" && PickedForUserId == "")
                    throw new Exception("");
                Session.eRecipeUtils.cts = new CancellationTokenSource();
                RecipeListNavigation.PageCount = 0;
                RecipeListNavigation.PageIndex = 1;
                _presenterList.FilterValues.Clear();
                RecipeList_enableButtons();
                await _presenterList.FindPatient();
                btnPrepaymentInfo.Enabled = eRecipeItem.Patient != null;
                if (eRecipeItem.Patient == null)
                    throw new Exception("Įvestas neteisingas asmens kodas!");

                bool CompApplies = (eRecipeItem.HasLowIncome?.HasLowIncome ?? false) || 
                ((eRecipeItem.AccumulatedSurcharge?.SurchargeEligible ?? false) && eRecipeItem?.eRecipe?.Type != "mpp");

                SetCompAppliesLabel(CompApplies);

				await RecipeListRefresh();

                if (await _presenterList.IsPossibleApplyUkrainianReffugeInsurance())
                {
                    helpers.alert(Enumerator.alert.warning, "Pacientui gali būti taikomas Ukrainos pabėgelių draudimas");
                }

                ExecuteAsyncAction(async () =>
                {
                    await _presenterList.GetAllergies();
                    await _presenterList.GetRepresentedPersons();
                });
                this.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        dispenseListPatient = new wpf.View.DispenseList(eRecipeItem.Patient.PatientId, "", "", "completed", "", "", Session.eRecipeGridSize, false, new DateTime(), new DateTime(), "");
                        ehDispenseListPatient.Child = dispenseListPatient;
                        await dispenseListPatient.refreshGrid();
                    }
                    catch (Exception ex) 
                    {
                        Serilogger.GetLogger().Error(ex,ex.Message);
                    }
                }));
            });
        }

        private void ClearRecipeList()
        {
            SetCompAppliesLabel(false);
            SelectedRecipeItems = null;
            eRecipeItem = new Items.eRecipe.Recipe();
            eRecipeItem.eRecipe = new RecipeDto();
            eRecipeWPF = new wpf.View.wpfeRecipePdf(eRecipeItem);
            ehRecipe.Child = eRecipeWPF;
            RecipeListNavigationRefreshCommand = new wpf.ViewModel.BaseCommand(btnRecipeListNavigation_Click);
            RecipeListNavigation.NavigationClick = RecipeListNavigationRefreshCommand;
            ehRecipeListNavigation.Child = RecipeListNavigation;
            PatientName = "";
            Surname = "";
            BirthDate = "";
            RepresentedPersons = null;
            rtbAllergies.Text = "";
            lblRepresented.Text = "";
            PickedUpByRef = "";
            recipeList = new wpf.View.RecipeList(null);
            ehRecipeList.Child = recipeList;
            dispenseListPatient = new wpf.View.DispenseList();
            ehDispenseListPatient.Child = dispenseListPatient;
            RecipeList_enableButtons();
        }

        private async Task RecipeListRefresh()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (Session.RecipesOnly == false)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    while (Program.Display1.IsBusy)
                    {
                        if (sw.ElapsedMilliseconds > 60000)//wait 1 min for display 1 refreshes
                            throw new TimeoutException();
                    }
                    sw.Stop();
                    //Program.Display1.IsBusy = true;
                    await Program.Display1.RefreshPosh();
                }
                string type = "";
                if (rbAll.Checked)
                    type = "active,on hold,completed";
                if (rbActive.Checked)
                    type = "active,on hold";
                /*if (Session.Admin)
                    type = "";*/
                eRecipeItem.DispenseWarnings = new List<Items.eRecipe.Issue>();
                await _presenterList.GetRecipeList(Session.eRecipeGridSize, RecipeListNavigation.PageIndex, type);
            }, false);
        }

        private async void gvRecipeList_SelectionChanged(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                eRecipeItem.RecipeDispense = null;
                eRecipeItem.DispenseCount = 0;
                eRecipeItem.DispensedQty = 0;
                eRecipeItem.PastValidTo = new DateTime();
                eRecipeItem.CompositionId = 0;
                eRecipeItem.ValidationPeriod = 0;
                SelectedRecipeItems = sender as List<RecipeDto>;
                if (SelectedRecipeItems?.Count == 1)
                    eRecipeItem.eRecipe = SelectedRecipeItems.First();
                else
                    eRecipeItem.eRecipe = new RecipeDto();
                await _presenterList.GetRecipeDispenseCount();
                btnDispensesInfo.Enabled = eRecipeItem.DispenseList.Total > 0;
                eRecipeWPF = new wpf.View.wpfeRecipePdf(eRecipeItem);
                ehRecipe.Child = eRecipeWPF;
                RecipeList_enableButtons();
            }, false);
        }

        private void RecipeList_enableButtons()
        {
            if (SelectedRecipeItems?.Count == 1)//po vieną
            {
                var selected_row = SelectedRecipeItems.First();
                btnGetRecipePdf.Enabled = true;
                btnGetDispensePdf.Enabled = (selected_row.Status?.ToLower() == "completed" || (selected_row.PrescriptionTagsLongTag?.ToLower() == "true" && eRecipeItem.CompositionId > 0));
                if (selected_row.Status == "active" || selected_row.Status == "reserved")
                {
                    btnSell.Enabled = true;
                    btnReserve.Enabled = true;
                    btnReserve.Text = "Rezervuoti";
                    btnSuspend.Enabled = true;
                    btnSuspend.Text = "Sustabdyti";
                }
                else
                {
                    if (selected_row.Status == "onhold")
                    {
                        if (selected_row.StatusReasonCode == "reserved")
                        {
                            btnReserve.Enabled = true;
                            btnReserve.Text = "Rez. atšaukimas";
                        }
                        else
                        {
                            btnReserve.Enabled = false;
                            btnReserve.Text = "Rezervuoti";
                        }
                        if (selected_row.StatusReasonCode == "for review")
                        {
                            btnSuspend.Enabled = true;
                            btnSuspend.Text = "Sus. atšaukimas";
                        }
                        else
                        {
                            btnSuspend.Enabled = false;
                            btnSuspend.Text = "Sustabdyti";
                        }
                    }
                    else
                    {
                        btnSell.Enabled = false;
                        btnReserve.Enabled = false;
                        btnReserve.Text = "Rezervuoti";
                        btnSuspend.Enabled = false;
                        btnSuspend.Text = "Sustabdyti";
                    }
                }
                //Admin only
                if (Session.Admin == true)
                {
                    btnSell.Enabled = true;
                    btnFindObsolete.Visible = true;
                }
                btnUnlock.Enabled = !string.IsNullOrEmpty(selected_row.LockWho) && selected_row.Status != "reserved";
                btnFilter.Enabled = true;
                btnPaperRecipe.Enabled = true;
            }
            else
            {
                btnSell.Enabled = false;
                btnReserve.Enabled = false;
                btnReserve.Text = "Rezervuoti";
                btnSuspend.Enabled = false;
                btnSuspend.Text = "Sustabdyti";
                btnGetRecipePdf.Enabled = false;
                btnGetDispensePdf.Enabled = false;
                btnUnlock.Enabled = false;
                btnFilter.Enabled = false;
                btnPaperRecipe.Enabled = false;
            }
            btnFilter.BackColor = _presenterList.FilterValues.Count == 0 ? System.Drawing.Color.Transparent : System.Drawing.Color.Green;
        }


        private async void btnSell_Click(object sender, EventArgs e)
        {
            try
            {

                int display2Timer = Program.Display1.display2_timer;
                if (display2Timer > 0)
                {
                    helpers.alert(Enumerator.alert.error, $"Blokuojamas receptinių vaistų pardavimas\nLiko laiko {display2Timer} s");
                    return;
                }

                btnSell.Enabled = false;
                await ExecuteWithWaitAsync(async () =>
                    await Session.eRecipeUtils.RetryLockRecipe(eRecipeItem.eRecipe.RecipeNumber, true, eRecipeItem.DispenseBySubstancesGroupId),
                    false,
                    null,
                    new FormWait() { Notification = "Receptas užrakinamas, prašome palaukti!" });

                Display2Tag display2Tag = new Display2Tag
                {
                    IsPartialDispense = !string.IsNullOrEmpty(eRecipeItem.DispenseBySubstancesGroupId)
                };

                var selectedProduct = Program.Display2.ExecuteFromRemote(eRecipeItem.eRecipe, ref display2Tag);
                if (selectedProduct == null)
                {
                    await ExecuteWithWaitAsync(async () =>
                         await Session.eRecipeUtils.RetryLockRecipe(eRecipeItem.eRecipe.RecipeNumber, false, eRecipeItem.DispenseBySubstancesGroupId),
                         false,
                         null,
                         new FormWait() { Notification = "Receptas atrakinamas, prašome palaukti!" });
                    return;
                }

                if (!await _presenterList.IsDiagnosisValidByNpakid7(selectedProduct?.NpakId7 ?? 0m) &&
                    eRecipeItem.eRecipe_CompensationTag &&
                    eRecipeItem.eRecipe_CompensationCode != 4)
                {
                    if (!helpers.alert(Enumerator.alert.warning, $"Vaistas nėra skirtas nurodytai ligai gydyti. Gausite TLK pranešimą."))
                    {
                        return;
                    }
                }

                eRecipeItem.IsDispenseBySubtances = display2Tag.IsPartialDispense;
                await Sell(async (barcodeModel) =>
                {
                    if (Session.HomeMode)
                    {
                        using (HomeModeQuantityView homeModeQtyView = new HomeModeQuantityView(selectedProduct))
                        {
                            homeModeQtyView.Location = helpers.middleScreen(this, homeModeQtyView);
                            homeModeQtyView.ShowDialog();

                            if (homeModeQtyView.DialogResult == DialogResult.OK)
                            {
                                barcodeModel.HomeModeQuantities = homeModeQtyView.GetQuantites();
                                var qtyStr = $@"D{Math.Round(barcodeModel.HomeModeQuantities.HomeQuantityByRatio + barcodeModel.HomeModeQuantities.RealQuantityByRatio, 4)
                                    .ToString().Replace(",", ".")}";
                                string barcodeStr = await _presenterList.GetBarcodeByProductId(selectedProduct.ProductId);
                                SetRecipeDataToBarcode(barcodeModel);
                                barcodeModel.BarcodeStr = $"{qtyStr}*{barcodeStr}";
                                barcodeModel.NpakId7 = selectedProduct?.NpakId7 ?? 0m;
                                barcodeModel.ProductIdBySecondScreen = selectedProduct?.ProductId ?? 0m;
                                barcodeModel.QtyStr = qtyStr;
                                if (!await ValidateBarcodeByNpakid7(barcodeStr, barcodeModel.NpakId7))
                                {
                                    barcodeModel.MppBarcode = string.Empty;
                                    helpers.alert(Enumerator.alert.error, $"Išduodami kompensuojamo MPP receptą pasirinkote prekė kurios barkodo nėra tarp kompensuojų barkodų");
                                    return;
                                }
                                else
                                {
                                    barcodeModel.MppBarcode = barcodeStr;
                                }
                                await Program.Display1.SubmitBarcode(barcodeModel);
                            }
                        }
                    }
                    else if (selectedProduct?.BarcodeModel == null)
                    {
                        var vm = new wpf.ViewModel.SubmitBarcode()
                        {
                            ProductName = selectedProduct?.ShortName ?? "Skenuokite prekę",
                            IsRecipeCompensated = eRecipeItem.eRecipe_CompensationTag,
                            RecipeType = eRecipeItem.eRecipe.Type,
                            Npakid7 = selectedProduct?.NpakId7 ?? 0m,
                            SelectedItemProductId = selectedProduct?.ProductId ?? 0m,
                            SecondScreenScan = false
                        };
                        var wpf = new wpf.View.SubmitBarcode()
                        {
                            DataContext = vm
                        };
                        using (var d = new Popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                        {
                            d.Location = helpers.middleScreen(this, d);
                            d.ShowDialog();
                            if (d.DialogResult == DialogResult.OK)
                            {
                                SetRecipeDataToBarcode(barcodeModel);
                                barcodeModel.BarcodeStr = vm.Barcode;
                                barcodeModel.NpakId7 = selectedProduct?.NpakId7 ?? 0m;
                                barcodeModel.ProductIdBySecondScreen = selectedProduct?.ProductId ?? 0m;
                                barcodeModel.MppBarcode = vm.MppBarcode;
                                await Program.Display1.SubmitBarcode(barcodeModel);
                            }
                        }
                    }
                    else
                    {
                        barcodeModel.BarcodeStr = selectedProduct.BarcodeModel.BarcodeStr;
                        await Program.Display1.SubmitBarcode(barcodeModel);
                    }

                    if (string.IsNullOrEmpty(barcodeModel?.BarcodeStr))
                    {
                        await ExecuteWithWaitAsync(async () =>
                            await Session.eRecipeUtils.RetryLockRecipe(eRecipeItem.eRecipe.RecipeNumber, false, eRecipeItem.DispenseBySubstancesGroupId),
                            false,
                            null,
                            new FormWait() { Notification = "Receptas atrakinamas, prašome palaukti!" });
                    }

                    if (eRecipeItem.eRecipe_CompensationTag && eRecipeItem.eRecipe.Type.ToLowerInvariant() == "mpp")
                    {
                        eRecipeItem.MppBarcode = barcodeModel.MppBarcode;
                    }

                    if (NeedToShowFMD(Program.Display1.currentPosdRow))
                    {
                        var viewModel = new wpf.ViewModel.FMDposd()
                        {
                            CurrentPosdRow = Program.Display1.currentPosdRow
                        };

                        var wpfFMDposd = new wpf.View.FMDposd()
                        {
                            DataContext = viewModel
                        };

                        using (var dlg = new Popups.wpf_dlg(wpfFMDposd, "FMD eilutės langas"))
                        {
                            dlg.Location = helpers.middleScreen(this, dlg);
                            dlg.ShowDialog();
                        }
                    }
                });
            }
            finally
            {
                btnSell.Enabled = true;
            }
        }


        private async Task<bool> ValidateBarcodeByNpakid7(string barcode, decimal npakid7)
        {
            try
            {
                if (eRecipeItem.eRecipe_CompensationTag && eRecipeItem.eRecipe.Type.ToLowerInvariant() == "mpp")
                {
                    var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                    var response = await tamroClient.GetAsync<List<BarcodeModel>>(string.Format(Session.CKasV1GetVlkBarcodes, npakid7));
                    if (response == null || response.Count == 0)
                        return false;

                    var currentDate = DateTime.Now;
                    return response.Any(e => e.Code == barcode && e.StartDate < currentDate && e.EndDate > currentDate);

                }
                return true;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Information($"[ValidateBarcodeByNpakid7] Pharmacy: {Session.SystemData.kas_client_id}; Error: {ex.Message}");
                return true;
            }
        }

        private bool NeedToShowFMD(Items.posd currentPosdRow) 
        {
            if (currentPosdRow is null)
                return false;

            if (!currentPosdRow.fmd_required)
                return false;

            return currentPosdRow.qty != currentPosdRow.fmd_model?.Count;
        }

        private void SetRecipeDataToBarcode(Models.Barcode barcodeModel) 
        {
            barcodeModel.LowIncomeTag = eRecipeItem?.HasLowIncome?.HasLowIncome ?? false;
            if (Program.Display2.PricesVM.FirstPrescriptionUncomp)
                barcodeModel.CompPercent = 0;
            barcodeModel.CheapestPrescription = Program.Display2.PricesVM.gvPricesSelectedCheapest;
            barcodeModel.FirstPrescriptionReason = Program.Display2.PricesVM.FirstPrescriptionReason;
        }

        private bool SellAlert(string msg)
        {
            DeletePosd();
            return helpers.alert(Enumerator.alert.error, msg);
        }

        private async Task Sell(Func<Models.Barcode, Task> functionToExcute)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (Program.Display1.IsBusy)
                    throw new Exception("Blokuojamas elektroninio recepto išdavimas. Atidarykite elektronino recepto langą iš naujo.");
                if (Session.RecipesOnly)//todo
                    _ucRecipeEdit_Show();
                else
                {
                    Models.Barcode barcodeModel = new Models.Barcode();
                    if (eRecipeItem.eRecipe_CompensationTag)
                    {
                        DataTable temp = null;
                        temp = await DB.recipe.getCompPercent(eRecipeItem.eRecipe_CompensationCode);
                        if (temp != null && temp.Rows.Count > 0)
                            barcodeModel.CompPercent = temp.Rows[0]["comppercent"].ToInt();
                    }
                    barcodeModel.FirstPrescription = (eRecipeItem.eRecipe.PrescriptionTagsFirstPrescribingTag.ToBool() //first prescription
                                            && eRecipeItem.eRecipe.CompensationTag.ToBool() //tik kompensuojami
                                            && string.IsNullOrWhiteSpace(eRecipeItem.eRecipe.ExtemporaneousIngredientContainedMedicationActiveSubstances));//ne firminis
                    await functionToExcute(barcodeModel);
                    if (barcodeModel.CompPercent == 0)
                        eRecipeItem.eRecipe_CompensationCode = 4;
                    var t = await DB.eRecipe.GetPosdDataAsync(barcodeModel?.PosdId ?? 0);
                    if (t?.Count() > 0)
                    {
                        posdItem = t.First();
                        if (!string.IsNullOrWhiteSpace(barcodeModel?.FirstPrescriptionReason))
                            eRecipeItem.AdditionalInstructionsForPatient = barcodeModel.FirstPrescriptionReason;
      
                        MedicationDto productMedication = new MedicationDto();
                        MedicationListDto containedMedicationList = new MedicationListDto();

                        if (Session.ExclusiveProducts.ContainsKey(barcodeModel?.ProductId ?? 0))
                            barcodeModel.NpakId7 = Session.ExclusiveProducts[barcodeModel.ProductId];

                        if (eRecipeItem.eRecipe.Type == "ev" || eRecipeItem.eRecipe.Type == "vv")//ekstemporalus arba vardinis vaistas
                        {
                            productMedication.MedicationId = 1;
                            productMedication.MedicationRef = "#1";
                        }
                        else if (barcodeModel?.NpakId7 != 0)
                        {
                            var medicationList = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", barcodeModel.NpakId7.ToString())?.MedicationList;
                            if (medicationList?.Count > 0)
                                productMedication = medicationList.First();

                            if (!string.IsNullOrWhiteSpace(eRecipeItem.eRecipe.MedicationId))
                            {
                                var countMedication = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("ContainedMedicationId", eRecipeItem.eRecipe.MedicationId, "1")?.Total ?? "0";
                                containedMedicationList = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("ContainedMedicationId", eRecipeItem.eRecipe.MedicationId, countMedication);
                            }
                        }
                        else
                        {
                            throw new Exception($"Nerastas prekės identifikatorius prekių sąraše! \n" +
                                                "Jeigu tai yra ekstemporalus arba vardinis vaistas, tada recepto tipas privalo būti 'ev' arba 'vv'. \n" +
                                                $" Šio recepto tipas yra '{eRecipeItem.eRecipe.Type}'. Klientas turi kreiptis į gyd. išdavusi receptą.");
                        }

                        if ((productMedication?.MedicationId ?? 0) != 0)
                        {
                            if (eRecipeItem.eRecipe.Type == "va" && !String.IsNullOrWhiteSpace(eRecipeItem.eRecipe.MedicationId) &&
                            (containedMedicationList?.MedicationList?.Where(cml => cml.MedicationId == productMedication.MedicationId).Count() ?? 0) == 0)
                            {
                                if (!helpers.alert(Enumerator.alert.confirm, "Elektroniniame recepte išrašytas firminis produktas nesutampa su parduodamu produktu!\nAr vistiek norite tęsti pardavimą?", true))
                                    throw new Exception("");
                            }
                            eRecipeItem.Medication = productMedication;
                            _ucRecipeEdit_Show();

                        }
                        else
                            throw new Exception("Negauta prekės " + barcodeModel.NpakId7.ToString() + " informacija iš Registrų centro");
                    }
                    else
                        IsBusy = false;
                }
            }, true, SellAlert);
        }

        private async Task SellVaccine(Func<Models.Barcode, Task> functionToExcute)
        {
            await ExecuteWithWaitAsync(async () =>
            {

                Models.Barcode barcodeModel = new Models.Barcode();
                await functionToExcute(barcodeModel);

                var t = await DB.eRecipe.GetPosdDataAsync(barcodeModel?.PosdId ?? 0);
                if (t?.Count() > 0)
                {
                    posdItem = t.First();
                }
                else
                    IsBusy = false;

            }, true, SellAlert);
        }

        private async void btnReserve_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (eRecipeItem.eRecipe.Status == "active")
                {
                    using (ProgressDialog dlg = new ProgressDialog("ReserveRecipe", eRecipeItem.eRecipe.GenericName, this))
                    {
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            DateTime.TryParse(dlg.Result, out DateTime dt);
                            string deliveryDate = dt.ToString("yyyy-MM-dd'T'HH:mm:ssK");

                            await Session.eRecipeUtils.ReserveRecipe<RecipeDto>(eRecipeItem.eRecipe.MedicationPrescriptionId,
                                                               Session.PractitionerItem.PractitionerRef,
                                                               "vaisto rezervavimas",
                                                               deliveryDate);

                            await Session.eRecipeUtils.LockRecipe<RecipeDto>(eRecipeItem.eRecipe.MedicationPrescriptionId, 
                                                                             Session.PractitionerItem.PractitionerRef, 
                                                                             Session.OrganizationItem.OrganizationRef);
                        }
                    }
                }
                if (eRecipeItem.eRecipe.Status == "onhold" && eRecipeItem.eRecipe.StatusReasonCode == "reserved")
                {
                    if (Session.getParam("ERECIPE", "UNLOCKCHECK") == "1" && !CanUnlock(eRecipeItem.eRecipe))
                    {
                        throw new Exception("Anuliuoti rezervaciją gali tik receptą rezervavusi vaistinė\n " +
                            "Rezervavusi vaistinė: BENU Vastinė Lietuva, UAB, Gamybos g. 4, Ramučių k. Karmėlavos sen., Kauno r. sav.");
                    }

                    await Session.eRecipeUtils.CancelRecipeReservation<RecipeDto>(eRecipeItem.eRecipe.MedicationPrescriptionId,
                                                                                  Session.PractitionerItem.PractitionerRef,
                                                                                  "rezervavimo atšaukimas");

                    await Session.eRecipeUtils.UnlockRecipe<RecipeDto>(eRecipeItem.eRecipe.MedicationPrescriptionId);
                }
                await RecipeListRefresh();
            });
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (eRecipeItem.eRecipe.Status == "active")
                {
                    ProgressDialog dlg = new ProgressDialog("SuspendRecipe", eRecipeItem.eRecipe.GenericName, this);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                        Session.eRecipeUtils.SuspendRecipe(eRecipeItem.eRecipe.MedicationPrescriptionId, Session.PractitionerItem.PractitionerRef, dlg.Result,ReserveRecipe_cb);
                    dlg.Dispose();
                    dlg = null;
                }
                if (eRecipeItem.eRecipe.Status == "onhold" && eRecipeItem.eRecipe.StatusReasonCode == "for review")
                    Session.eRecipeUtils.CancelRecipeSuspension(eRecipeItem.eRecipe.MedicationPrescriptionId, Session.PractitionerItem.PractitionerRef, "sustabdymo atšaukimas", ReserveRecipe_cb);
            });
        }

        private async void btnGetDispensePdf_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (eRecipeItem.CompositionId <= 0)
                    await _presenterList.GetRecipeDispenseCount();
                Session.eRecipeUtils.GetSignedDispensePdf(eRecipeItem.CompositionId.ToString(), GetDispensePdf_cb);
            });
        }

        private void btnGetRecipePdf_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                Session.eRecipeUtils.GetSignedDispensePdf(eRecipeItem.eRecipe.CompositionId, GetDispensePdf_cb);
            });
        }

        private async void btnRecipeListNavigation_Click(object obj)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                Session.eRecipeUtils.cts = new CancellationTokenSource();
                await RecipeListRefresh();
            });
        }
        
        private async void VaccineUserControl_LoadEvent(object sender, System.EventArgs e)
        {
            if (sender == null) return;
            if (!(sender is VaccineSellUserControlV2)) return;
            await SellVaccine(async (barcodeModel) =>
            {
                var vm = new wpf.ViewModel.SubmitBarcode()
                {
                    ProductName = "Įveskite arba įskenuokite prekės barkodą"
                };
                var wpf = new wpf.View.SubmitBarcode()
                {
                    DataContext = vm
                };
                using (var d = new Popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                {
                    d.Location = helpers.middleScreen(this, d);
                    d.ShowDialog();
                    if (d.DialogResult == DialogResult.OK)
                    {
                        barcodeModel.BarcodeStr = vm.Barcode;
                        await Program.Display1.SubmitBarcode(barcodeModel);
                        if (barcodeModel?.PosdId < 0 || 
                            Program.Display1.PoshItem.PosdItems == null ||
                            Program.Display1.PoshItem.PosdItems.Count == 0)
                        {
                            (sender as VaccineSellUserControlV2).Close();
                        }
                        else
                        {

                            Items.posd posd = Program.Display1.PoshItem?.PosdItems.FirstOrDefault();
                            var npakId7 = await Session.SamasUtils.GetNpakid7ByProductId(posd.productid);
                            if (npakId7 <= 0)
                            {
                                helpers.alert(Enumerator.alert.warning, "Prekės NpakId7 nerastas!");
                                (sender as VaccineSellUserControlV2).Close();
                            }

                            MedicationDto medication = null;
                            var medication_list = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", npakId7.ToString())?.MedicationList;
                            if (medication_list?.Count > 0)
                            {
                                medication = medication_list.FirstOrDefault();
                            }
                            else
                            {
                                helpers.alert(Enumerator.alert.warning, $"Medikamentas su NpakId7: {npakId7} neegzistuoja E-Sveikatos sistemoje!");
                                (sender as VaccineSellUserControlV2).Close();
                            }
                            (sender as VaccineSellUserControlV2).FillVaccineData(posd, medication);
                        }
                    }
                    else
                        (sender as VaccineSellUserControlV2).Close();
                }
            });
        }
        private void VaccineUserControl_SelectionChanged(VaccinationEntry vaccineEntry)
        {
            vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(vaccineEntry);
            if (ehRecipe != null)
                ehRecipe.Child = vaccinationOrderWPF;
        }

        #endregion

        #region DispenseListEvents
        private async void btnDispFindDispense_Click(object sender, EventArgs e)
        {
            await FindDispense(true);
        }

        private async void btnFindObsolete_Click(object sender, EventArgs e)
        {
            await FindDispense(false);
        }

        private async Task FindDispense(bool fromDB)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                ClearDispenseList();
                var filter_key = ((KeyValuePair<string, string>)cmbFilter.SelectedItem).Key;
                var filterPractitionerId = filter_key == "1" ? "" : Session.PractitionerItem.PractitionerId;
                var filterOrganizationId = Session.OrganizationItem.OrganizationId;
                dispenseList = new wpf.View.DispenseList("", filterPractitionerId,
                        filterOrganizationId,
                        ((KeyValuePair<string, string>)cmbStatus.SelectedItem).Key,
                        ((KeyValuePair<string, string>)cmbConfirmed.SelectedItem).Key,
                        ((KeyValuePair<string, string>)cmbDocStatus.SelectedItem).Key,
                        Session.eRecipeGridSize,
                        fromDB,
                        dtpDateFrom.Value,
                        dtpDateTo.Value,
                        cbNarc.Checked ? "NARC" : "");
                dispenseList.SelectionChanged_Event += new EventHandler(gvDispenseList_SelectionChanged);
                ehDispenseList.Child = dispenseList;
                await dispenseList.refreshGrid();
            });
        }

        private void ClearDispenseList()
        {
            dispenseItem = new Items.eRecipe.Recipe();
            dispenseItem.eRecipe = new RecipeDto();
            ehDispenseList.Child = new wpf.View.DispenseList();
            eRecipeWPF = new wpf.View.wpfeRecipePdf(dispenseItem);
            ehRecipe.Child = eRecipeWPF;
        }
        private async void gvDispenseList_SelectionChanged(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var SelectedDispense = sender as DispenseDto;
                if (SelectedDispense != null)
                    dispenseItem.eRecipe = await Session.eRecipeUtils.GetRecipe<RecipeDto>(SelectedDispense.PrescriptionDocumentNumber, true);
                else
                    dispenseItem.eRecipe = new RecipeDto();
                if (dispenseItem.eRecipe_PrescriptionTagsLongTag == true)
                    await _presenterList.GetDispenseCount();
                eRecipeWPF = new wpf.View.wpfeRecipePdf(dispenseItem);
                ehRecipe.Child = eRecipeWPF;
            }, false);
        }
        #endregion

        #region VaccineListEvents
        private async void btnFindVaccine_Click(object sender, EventArgs e)
        {
            await FindVaccine();
        }
        private async Task FindVaccine()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                ClearVaccineList();
                var filter_key = ((KeyValuePair<string, string>)cmbVaccineDoc.SelectedItem).Key;
                var filterPractitionerId = string.Empty;
                var filterOrganizationId = string.Empty;
                if (filter_key == "1")
                {
                    filterPractitionerId = "";
                    filterOrganizationId = Session.OrganizationItem.OrganizationId;
                }
                else if (filter_key == "2")
                {
                    filterPractitionerId = Session.ExtendedPracticePractitioner.PractitionerId;
                    filterOrganizationId = "";
                }
                else
                {
                    filterPractitionerId = Session.ExtendedPracticePractitioner.PractitionerId;
                    filterOrganizationId = Session.OrganizationItem.OrganizationId;
                }

                vaccineList = new wpf.View.VaccineList("", filterPractitionerId,
                        filterOrganizationId,
                        ((KeyValuePair<string, string>)cmbDocType.SelectedItem).Key,
                        ((KeyValuePair<string, string>)cmbVaccineStatus.SelectedItem).Key,
                        ((KeyValuePair<string, string>)cmbVaccineDocStatus.SelectedItem).Key,
                        Session.eRecipeGridSize,
                        dtpVaccineFrom.Value,
                        dtpVaccineTo.Value);
                vaccineList.SelectionChanged_Event += gvVaccineList_SelectionChanged;
                ehVaccineList.Child = vaccineList;
                await vaccineList.RefreshGrid();
            });
        }
        private void ClearVaccineList()
        {
            _vaccineEntry = null;
            ehVaccineList.Child = new wpf.View.VaccineList();
            vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(new VaccinationEntry());
        }
        private void ClearVaccineOrderList()
        {
            vaccineUserControl.ClearVaccineEntriesList();
        }
        private void gvVaccineList_SelectionChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _vaccineEntry = sender as VaccinationEntry;                
                vaccinationOrderWPF = new wpf.View.eRecipe.wpfVaccinationPdf(_vaccineEntry);
                ehRecipe.Child = vaccinationOrderWPF;
            },false);
        }
        #endregion

        #region Variables Patient
        private string BirthDate
        {
            get { return tbBirthDate.Text; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    DateTime now = DateTime.Now;
                    DateTime birthDate = helpers.getXMLDateOnly(value);
                    tbBirthDate.Text = birthDate.ToString("yyyy-MM-dd");
                    int age = now.Year - birthDate.Year;
                    if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))//not had bday this year yet
                        age--;
                    tbAge.Text = age.ToString();
                }
                else
                {
                    tbBirthDate.Text = value;
                    tbAge.Text = value;
                }
            }
        }

        private string _PickedUpById;
        public string PickedForUserId
        {
            get
            {
                return _PickedUpById;
            }
            set
            {
                _PickedUpById = value;
            }
        }
        private string _PickedUpByRef;
        public string PickedUpByRef
        {
            get
            {
                return _PickedUpByRef;
            }
            set
            {
                _PickedUpByRef = value;
            }
        }
        #endregion

        #region Variables
        private List<RecipeDto> SelectedRecipeItems { get; set; }

        public string PersonalCode

        {
            get
            {
                return tbPersonalCode.Text;
            }

            set
            {
                tbPersonalCode.Text = value;
            }
        }

        public Recipe dispenseItem { get; set; }

        private Items.eRecipe.Recipe _eRecipeItem;
        public Recipe eRecipeItem
        {
            get
            {
                return _eRecipeItem;
            }

            set
            {
                _eRecipeItem = value;
                PatientName = _eRecipeItem?.Patient?.GivenName?.First();
                BirthDate = _eRecipeItem?.Patient?.BirthDate;
                Surname = _eRecipeItem?.Patient?.FamilyName.First();
                var relatedPersons = new List<dynamic> { new { Name = "Susiję asmenys:", Reference = "" }};
                if (string.IsNullOrEmpty(PickedUpByRef))
                {
                    var personsList = eRecipeItem?.Patient?.RelatedPersons ?? new List<RelatedPerson>();
                    relatedPersons.AddRange(personsList.Select(el => new
                    {
                        Name = $"{el.GivenName} {el.FamilyName}",
                        el.Reference
                    }));
                }
                cmbRelated.DataSource = relatedPersons.ToList();
                cmbRelated.DisplayMember = "Name";
                cmbRelated.ValueMember = "Reference";
                gbType.Enabled = true;
            }
        }

        public string PatientName
        {
            get
            {
                return tbPatientName.Text;
            }
            set
            {
                tbPatientName.Text = value;
            }
        }

        public string Surname
        {
            get
            {
                return tbSurname.Text;
            }
            set
            {
                tbSurname.Text = value;
            }
        }

        private AllergyIntoleranceDto _Allergies;
        public AllergyIntoleranceDto Allergies
        {
            get
            {
                return _Allergies;
            }

            set
            {
                _Allergies = value;
                this.BeginInvoke(new Action(() =>
                {
                    foreach (var el in Allergies.Allergies)
                        rtbAllergies.Text += el.Code + " " + el.Name + " | " + el.Description + Environment.NewLine;
                }));
            }
        }

        private RepresentedPersonsDto _RepresentedPersons;
        public RepresentedPersonsDto RepresentedPersons
        {
            get
            {
                return _RepresentedPersons;
            }
            set
            {
                _RepresentedPersons = value;
                var RepresentedIems = (from el in _RepresentedPersons?.Persons ?? new List<PatientDto>()
                                       select new
                                       {
                                           PersonalCode = el.PersonalCode,
                                           PatientRef = el.PatientRef,
                                           Name = el.GivenName?.First() + " " + el.FamilyName?.First()
                                       }).ToList();
                RepresentedIems.Insert(0, new { PersonalCode = "", PatientRef = "", Name = "Įgaliojusieji asmenys:" });
                this.BeginInvoke(new Action(() =>
                {
                    cmbRepresented.DataSource = RepresentedIems;
                    cmbRepresented.DisplayMember = "Name";
                    cmbRepresented.ValueMember = "PersonalCode";
                }));
            }
        }

        private RecipeListDto _RecipeList;
        public RecipeListDto RecipeList
        {
            get
            {
                return _RecipeList;
            }
            set
            {
                _RecipeList = value;
                int count = _RecipeList?.Total ?? 0;
                if (count < Session.eRecipeGridSize)
                    count = Session.eRecipeGridSize;
                RecipeListNavigation.PageCount = (int)Math.Ceiling((decimal)count / Session.eRecipeGridSize);
                RecipeListNavigation.PageIndex = RecipeListNavigation.PageIndex;//refresh number
                recipeList = new wpf.View.RecipeList(_RecipeList);
                recipeList.SelectionChanged_Event += new EventHandler(gvRecipeList_SelectionChanged);
                ehRecipeList.Child = recipeList;
                RecipeList_enableButtons();
                if (_RecipeList.RecipeList.Count <= 0)
                    helpers.alert(Enumerator.alert.warning, "Pacientas neturi išrašytų el. receptų");
            }
        }
        #endregion

        private void SetCompAppliesLabel(bool active) 
        {
            lblCompensationApplies.Visible = active;

		}
        private void btnCancelTask_Click(object sender, EventArgs e)
        {
            if (helpers.alert(Enumerator.alert.confirm, "Ar tikrai norite atšaukti vykdomą operaciją?", true))
            {
                IsBusy = false;
                Session.eRecipeUtils.cts.Cancel();
                vaccineUserControl.ResetControls();
            }
        }

        private void btnPrepaymentInfo_Click(object sender, EventArgs e)
        {
            using (PrepaymentView prepaymentView = new PrepaymentView()) 
            {
                prepaymentView.SetData(_eRecipeItem);
                prepaymentView.ShowDialog();
            }
        }    

        private bool CanUnlock(RecipeDto recipe) 
        {
            if (recipe == null) 
                return false;

            if (!string.IsNullOrWhiteSpace(recipe.LockDepartment))
            {
                var lockedDepartmentId = recipe.LockDepartment.ExtractId().ToLong();
                var currentPharmacyId = Session.OrganizationItem.OrganizationRef.ExtractId().ToLong();

                if (lockedDepartmentId != ManufacturingPharmacyId)
                    return true;

                return currentPharmacyId == ManufacturingPharmacyId;
            }
            return true;
        }

        private void eRecipe_FormClosing(object sender, FormClosingEventArgs e)
        {
            ucRecipeEditBase uc = _ucRecipeEdit as ucRecipeEditBase;
            if (string.IsNullOrWhiteSpace(uc?.out_form_action))
                DeletePosd(false);
        }

        private void btnDispensesInfo_Click(object sender, EventArgs e)
        {
            using (DispensesView dispensesView = new DispensesView())
            {
                dispensesView.SetData(eRecipeItem);
                dispensesView.ShowDialog();
            }
        }

        private async void btnUnlock_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var recipeDto = await Session.eRecipeUtils.UnlockRecipe<RecipeDto>(eRecipeItem.eRecipe.MedicationPrescriptionId);
                if (recipeDto != null)
                    await RecipeListRefresh();
            });
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            using (RecipeFilterView recipeFilterView = new RecipeFilterView(_presenterList.FilterValues))
            {
                recipeFilterView.Init();
                recipeFilterView.ShowDialog();
                if (recipeFilterView.DialogResult == DialogResult.OK) 
                {
                    _presenterList.FilterValues = recipeFilterView.FilterValues;
                }
                else 
                {
                    _presenterList.FilterValues.Clear();
                }
                await RecipeListRefresh(); ;
            }
        }

        private async void btnPaperRecipe_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var vm = new SubmitBarcode()
                {
                    ProductName = "Skenuokite prekę"
                };
                var wpf = new wpf.View.SubmitBarcode()
                {
                    DataContext = vm
                };
                using (var d = new Popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                {
                    d.Location = helpers.middleScreen2(d, true);
                    if (d.ShowDialog() == DialogResult.OK)
                    {
                        var barcodeModel = new Barcode() { BarcodeStr = vm.Barcode };
                        Presenters.BarcodePresenter BCPresenter = new Presenters.BarcodePresenter(Program.Display1, barcodeModel);
                        await BCPresenter.GetDataFromBarcode();

                        if (barcodeModel.BarcodeID == 0)
                        {
                            throw new Exception($"'{barcodeModel.BarcodeStr}' barkodas nerastas");
                        }

                        var selectedProduct = Program.Display2.ExecuteFromRemote(barcodeModel);
                        if (selectedProduct == null)
                            return;

                        barcodeModel.SelectedItem = selectedProduct;
                        await Program.Display1.SubmitBarcode(barcodeModel);
                        var posDetail = Program.Display1.PoshItem?.PosdItems?.FirstOrDefault();
                        if (posDetail?.productid == selectedProduct.ProductId)
                        {
                            using (PaperRecipeSelectionView paperRecipeSelectionView = new PaperRecipeSelectionView())
                            {
                                paperRecipeSelectionView.SetGenericItem(selectedProduct);
                                paperRecipeSelectionView.SetERecipeItem(eRecipeItem);
                                paperRecipeSelectionView.SetPosDetail(posDetail);
                                paperRecipeSelectionView.ShowDialog();
                            }
                        }
                    }
                }
            });
        }
    }
}
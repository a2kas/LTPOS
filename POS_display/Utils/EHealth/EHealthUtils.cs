using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Exceptions;
using POS_display.Items.eRecipe;
using POS_display.Models.Recipe;
using POS_display.Properties;
using POS_display.Utils.Logging;
using POS_display.Utils.Signature;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TamroUtilities.Exceptions.Models;
using TamroUtilities.HL7.Fhir.Core.Extension;
using TamroUtilities.HL7.Gateway.Services;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.AccumulatedSurcharge;
using TamroUtilities.HL7.Models.Dispense;
using TamroUtilities.HL7.Models.Encounter;
using TamroUtilities.HL7.Models.Medication;
using TamroUtilities.HL7.Models.MedicationPrescription;
using TamroUtilities.HL7.Models.Parameters;
using TamroUtilities.HL7.Models.Patient;
using TamroUtilities.HL7.Models.Vaccination;

namespace POS_display.Utils.EHealth
{
    public class EHealthUtils : IEHealthUtils
    {
        public delegate void SOAPCallback(bool success, string result, XElement Params);
        public delegate void SOAPClassCallback<T>(bool success, T result, XElement Params);
        public delegate void SOAPDelegate(XElement Params, SOAPCallback callback);
        public delegate void SOAPClassDelegate<T>(XElement Params, SOAPClassCallback<T> callback);
        private CancellationTokenSource _cts;

        public bool Init()
        {
            return true;
        }

        public T GetPractitioner<T>(string StampCode, string personalCode = "")
        {
            var practitionerService = Program.ServiceProvider.GetRequiredService<IPractitionerService>();
            try
            {
                var practitionerListDto = practitionerService.Get(StampCode, personalCode, Session.OrganizationItem.JAR);
                if (practitionerListDto is T practitioner)
                {
                    return practitioner;
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return default;
        }

        public T GetOrganization<T>(string SVEIDRAID)
        {
            var orgnaizationService = Program.ServiceProvider.GetRequiredService<IOrganizationService>();
            try
            {
                var organizationDto = orgnaizationService.Get(SVEIDRAID);
                if (organizationDto is T organization)
                {
                    return organization;
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return default;
        }

        public async Task<T> GetPatient<T>(string SearchTxt, string PatientId)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();
            return await Task.Run(() =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(PatientId))
                    {
                        var request = new GetPatientRequest
                        {
                            PatientId = PatientId.ToLong(),
                            PractitionerId = Session.PractitionerItem.PractitionerId.ToLong()
                        };

                        var patientDto = patientService.Get(request);
                        if (patientDto is T patient)
                        {
                            return patient;
                        }
                    }
                    else
                    {
                        var request = new SearchPatientRequest
                        {
                            PersonalCode = IsPersonalCode(SearchTxt) ? SearchTxt : string.Empty,
                            Esi = IsPersonalCode(SearchTxt) ? string.Empty : SearchTxt,
                            PractitionerId = Session.PractitionerItem.PractitionerId.ToLong()
                        };

                        var patientDto = patientService.Search(request);
                        if (patientDto is T patient)
                        {
                            return patient;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> GetLowIncome<T>(string searchTxt)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();

            return await Task.Run(() =>
            {
                try
                {
                    var lowIncomeRequest = new LowIncomeRequest()
                    {
                        PatientCode = IsPersonalCode(searchTxt) ? searchTxt : string.Empty,
                        PatientDIK = IsPersonalCode(searchTxt) ? string.Empty : searchTxt
                    };

                    var lowIncomeDto = patientService.GetLowIncome(lowIncomeRequest, Session.PractitionerItem.PractitionerId.ToLong());
                    if (lowIncomeDto is T lowIncome)
                    {
                        return lowIncome;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> GetAccumulatedSurcharge<T>(string personalCode, string DIK, string validDate)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();

            return await Task.Run(() =>
            {
                try
                {
                    var searchAccumulatedSurcharge = new SearchAccumulatedSurcharge()
                    {
                        PersonalCode = personalCode,
                        Dik = DIK,
                        ValidDate = validDate.ToDateTime(),
                        PractitionerId = Session.PractitionerItem.PractitionerId.ToLong()
                    };

                    var accumulatedSurchargeDto = patientService.GetAccumulatedSurcharge(searchAccumulatedSurcharge);
                    if (accumulatedSurchargeDto is T accumulatedSurcharge)
                    {
                        return accumulatedSurcharge;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex, false);
                }
                return (T)Activator.CreateInstance(typeof(T));
            });
        }

        public async Task<T> GetAllergies<T>(string PatientId)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();

            return await Task.Run(() =>
            {
                try
                {
                    var allergyIntoleranceDto = patientService.GetAllergies(PatientId, Session.PractitionerItem.PractitionerId.ToLong());
                    if (allergyIntoleranceDto is T allergyIntolerance)
                    {
                        return allergyIntolerance;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex, false);
                }
                return default;
            });
        }

        public async Task<T> GetRepresentedPersons<T>(string PatientId)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();

            return await Task.Run(() =>
            {
                try
                {
                    var representedPersonsDto = patientService.GetRepresentedPersons(PatientId, Session.PractitionerItem.PractitionerId.ToLong());
                    if (representedPersonsDto is T representedPersons)
                    {
                        return representedPersons;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex, false);
                }
                return default;
            });
        }

        public async Task<EncounterDto> CreateEncounter(string PatientId, string PatientRef, string PractitionerRef, string OrganizationRef, string patientEsi, bool? isPatientInsured, EncounterReason reason)
        {
            var encounterService = Program.ServiceProvider.GetRequiredService<IEncounterService>();

            return await Task.Run(() =>
            {
                try
                {
                    var createEncounterRequest = new CreateEncounterRequest()
                    {
                        PatientRef = PatientRef,
                        PractitionerRef = PractitionerRef,
                        OrganizationRef = OrganizationRef,
                        PatientEsi = patientEsi,
                        IsPatientInsured = isPatientInsured,
                        Reason = reason
                    };

                    var enounterDto = encounterService.Create(createEncounterRequest);
                    if (enounterDto is EncounterDto enounter)
                    {
                        Items.eRecipe.Encounter encounter_item = new Items.eRecipe.Encounter()
                        {
                            PatientId = PatientId,
                            EncounterItem = enounter
                        };
                        Session.eRecipeEncounterList.Add(encounter_item);
                        return enounterDto;
                    }
                }
                catch (Exception ex)
                {
                    Serilogger.GetLogger().Error($"[CreateEncounter]: {ex.Message}");
                    HandleExceptions(ex);
                }
                return new EncounterDto();
            });
        }

        public async Task<T> GetRecipeList<T>(string PatientId, int PageSize, int Page, string Status)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionListRequest = new MedicationPrescriptionListRequest()
                    {
                        PatientId = PatientId.ToLong(),
                        PageNumber = Page,
                        PageSize = PageSize,
                        PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                        PrescriptionStatus = GetPrescriptionStatuses(Status)
                    };

                    var recipeListDto = medicationPrescriptionService.Search(medicationPrescriptionListRequest);
                    if (recipeListDto is T recipeList)
                    {
                        return recipeList;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> GetDispenseList<T>(string FilterPatientId, string FilterPractitionerId, string FilterOrganizationId, string Status, string StatusConfirmed, string DocStatus, int PageSize, int Page, string FilterMedicationPrescriptionId, List<string> compositionIds)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionListRequest = new DispenseSearchRequest()
                    {
                        PatientId = string.IsNullOrWhiteSpace(FilterPatientId) ? default : FilterPatientId.ToLong(),
                        PageNumber = Page,
                        PageSize = PageSize,
                        PractitionerId = string.IsNullOrWhiteSpace(FilterPractitionerId) ? default : FilterPractitionerId.ToLong(),
                        OrganizationId = string.IsNullOrWhiteSpace(FilterOrganizationId) ? default : FilterOrganizationId.ToLong(),
                        Status = Status,
                        StatusConfirmed = StatusConfirmed,
                        DocStatus = DocStatus,
                        MedicationPrescriptionId = string.IsNullOrWhiteSpace(FilterMedicationPrescriptionId) ? default : FilterMedicationPrescriptionId.ToLong(),
                        CompositionIds = compositionIds,
                        IncludeAuthor = true,
                        IncludeCustodian = true
                    };

                    var dispenseListDto = dispenseService.Search(medicationPrescriptionListRequest);
                    if (dispenseListDto is T dispenseList)
                    {
                        return dispenseList;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public DispenseListDto UpdateDispense(UpdateDispenseRequest updateDispenseRequest)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();

            try
            {
                var dispenseListDto = dispenseService.Update(updateDispenseRequest);
                var issues = dispenseListDto.DispenseList
                            .Where(e => e.Issues != null) 
                            .SelectMany(e => e.Issues)
                            .ToList();
                var uniqueIssues = issues
                        .GroupBy(issue => issue.Code)
                        .Select(group => group.First())
                        .ToList();

                if (uniqueIssues.Any(x => x.IssueSeverity.ToString().ToLower() == "error"))
                    throw new Exception(uniqueIssues.First(e => e.IssueSeverity.ToString().ToLower() == "error").Display);

                if (uniqueIssues.Any(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                {
                    foreach (var issue in uniqueIssues.Where(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                    {
                        using (ProgressDialog dlg = new ProgressDialog("dispense_warning", issue.Display))
                        {
                            dlg.ShowDialog();
                            if (dlg.DialogResult == DialogResult.OK)
                                issue.IgnoreReason = dlg.Result;
                            else
                                throw new Exception("");
                        }
                        updateDispenseRequest.Issues.Add(new IssueDto()
                        {
                            Code = issue.Code,
                            Display = issue.Display,
                            IssueSeverity = issue.IssueSeverity,
                            IgnoreReason = issue.IgnoreReason
                        });
                    }
                    return UpdateDispense(updateDispenseRequest);
                }


                return dispenseListDto;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
                return null;
            }
        }

        public async Task<T> GetRecipe<T>(string RecipeNumber, bool IncludePatient)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var recipeDto = medicationPrescriptionService.GetMedicationPrescriptionByCompositionId(
                        RecipeNumber.ToLong(), 
                        Session.PractitionerItem.PractitionerId.ToLong());

                    if (recipeDto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> LockRecipe<T>(string MedicationPrescriptionId, string PractitionerRef, string OrganizationRef, MedicationPrescriptionStatus? Status = MedicationPrescriptionStatus.OnHold)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionRequest = new MedicationPrescriptionRequest()
                    {
                        MedicationPrescriptionId = MedicationPrescriptionId,
                        PractitionerId = Session.PractitionerItem.PractitionerId,
                        PractitionerRef = PractitionerRef,
                        LockOrganizationId = Session.OrganizationItem.OrganizationId
                    };

                    if (Status.HasValue)
                        medicationPrescriptionRequest.Status = Status.Value;

                    var recipeDto = medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                    if (recipeDto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> UnlockRecipe<T>(string MedicationPrescriptionId)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var recipeDto = medicationPrescriptionService.UnlockPrescription(
                        MedicationPrescriptionId.ToLong(),
                        Session.PractitionerItem.PractitionerId.ToLong());

                    if (recipeDto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public T GetMedicationList<T>(string parameter, string value, string PageSize = "", string Page = "")
        {
            var medicationService = Program.ServiceProvider.GetRequiredService<IMedicationService>();
            try
            {
                var medicationListDto = medicationService.Search(BuildSearchMedicationRequest(parameter, value, PageSize, Page));
                if (medicationListDto is T medicationList)
                {
                    return medicationList;
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return default;
        }

        public async Task<RecipeDispenseDto> CreateRecipeDispense(Recipe eRecipeItem, string PickedUpByRef, DateTime DispenseDueDate, decimal PriceRetail, decimal PricePaid, decimal PriceCompensated, decimal prepCompSum, decimal Quantity, DateTime DispenseDate, bool confirmDispense, bool isCheapest, int durationOfUse)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();
            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();

            try
            {
                var createDispenseRequest = BuildCreateDispenseRequest(eRecipeItem,
                                                                       PickedUpByRef,
                                                                       DispenseDueDate,
                                                                       PriceRetail,
                                                                       PricePaid,
                                                                       PriceCompensated,
                                                                       prepCompSum,
                                                                       Quantity,
                                                                       DispenseDate,
                                                                       confirmDispense,
                                                                       isCheapest,
                                                                       durationOfUse);

                var recipeDispenseDto = dispenseService.Create(createDispenseRequest);

                if (recipeDispenseDto.Issues.Any())
                {
                    eRecipeItem.DispenseWarnings = new List<Items.eRecipe.Issue>();
                    var existingDispenseList = await GetDispenseList<DispenseListDto>(
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        "completed",
                        string.Empty,
                        string.Empty,
                        5,
                        1,
                        eRecipeItem.eRecipe.MedicationPrescriptionId.ToString(),
                        new List<string>());

                    var existingDispense = existingDispenseList?.DispenseList?.FirstOrDefault(x =>
                                            x.DateWritten.ToDateTime() < DateTime.Now &&
                                            x.DateWritten.ToDateTime() >= (DateTime.Now.Subtract(TimeSpan.FromHours(1))));

                    if (existingDispense != null)
                    {
                        eRecipeItem.RecipeDispense = new RecipeDispenseDto
                        {
                            CompositionId = existingDispense.CompositionId,
                            CompositionRef = existingDispense.CompositionRef,
                            MedicationDispenseId = existingDispense.MedicationDispenseId,
                            ConfirmationDate = existingDispense.ConfirmationDate
                        };
                    }

                    if (recipeDispenseDto.Issues.Any(x => x.IssueSeverity.ToString().ToLower() == "error") && existingDispense is null)
                        throw new Exception(recipeDispenseDto.Issues.First(e=> e.IssueSeverity.ToString().ToLower() == "error").Display);

                    if (recipeDispenseDto.Issues.Any(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                    {
                        foreach (var issue in recipeDispenseDto.Issues.Where(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                        {
                            using (ProgressDialog dlg = new ProgressDialog("dispense_warning", issue.Display))
                            {
                                dlg.ShowDialog();
                                if (dlg.DialogResult == DialogResult.OK)
                                    issue.IgnoreReason = dlg.Result;
                                else
                                    throw new Exception("");
                            }
                            eRecipeItem.DispenseWarnings.Add(mapper.Map<Items.eRecipe.Issue>(issue));
                        }

                        return await CreateRecipeDispense(
                            eRecipeItem,
                            PickedUpByRef,
                            DispenseDueDate,
                            PriceRetail,
                            PricePaid,
                            PriceCompensated,
                            prepCompSum,
                            Quantity,
                            DispenseDate,
                            confirmDispense,
                            isCheapest,
                            durationOfUse);
                    }
                }
                return recipeDispenseDto;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return default;
        }

        public async Task<List<RecipeDispenseDto>> CreateRecipeDispenseMultiple(List<GroupDispenseRequest> groupDispenseRequests)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();
            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();

            try
            {
                var createDispenseRequests = new List<CreateDispenseRequest>();
                foreach (var groupDispenseRequest in groupDispenseRequests)
                {
                    bool isCheapest = Session.TLKCheapests.Where(val => val.StartDate <= DateTime.Now && val.Npakid7 == groupDispenseRequest.eRecipeItem?.Medication?.NPAKID7.ToString())
                          .OrderByDescending(val => val.PriceListVersion)
                          .Select(val => val.IsCheapest)
                          .FirstOrDefault();

                    var createDispenseRequest = BuildCreateDispenseRequest(groupDispenseRequest.eRecipeItem,
                                                                           groupDispenseRequest.PickedUpByRef,
                                                                           groupDispenseRequest.TillDate,
                                                                           groupDispenseRequest.TotalSum,
                                                                           groupDispenseRequest.PaySum,
                                                                           groupDispenseRequest.CompSum,
                                                                           groupDispenseRequest.PrepCompSum,
                                                                           groupDispenseRequest.GQty,
                                                                           DateTime.Now,
                                                                           groupDispenseRequest.ConfirmDispense,
                                                                           isCheapest,
                                                                           groupDispenseRequest.DurationOfUse);

                    createDispenseRequest.ExternalId = groupDispenseRequest.eRecipeId.ToString();
                    createDispenseRequests.Add(createDispenseRequest);
                }


                var recipeDispenseResults = dispenseService.Create(createDispenseRequests);
                var issues = recipeDispenseResults.SelectMany(e => e.Issues).ToList();
                var uniqueIssues = issues
                        .GroupBy(issue => issue.Code)
                        .Select(group => group.First())
                        .ToList();

                if (uniqueIssues.Any())
                {
                    if (uniqueIssues.Any(x => x.IssueSeverity.ToString().ToLower() == "error"))
                        throw new Exception(issues.First(e => e.IssueSeverity.ToString().ToLower() == "error").Display);

                    var dispenseWarnings = new List<Items.eRecipe.Issue>();
                    if (uniqueIssues.Any(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                    {
                        foreach (var issue in uniqueIssues) 
                        {
                            using (ProgressDialog dlg = new ProgressDialog("dispense_warning", issue.Display))
                            {
                                dlg.ShowDialog();
                                if (dlg.DialogResult == DialogResult.OK)
                                    issue.IgnoreReason = dlg.Result;
                                else
                                    throw new Exception("");
                            }
                            dispenseWarnings.Add(mapper.Map<Items.eRecipe.Issue>(issue));
                        }
                    }

                    groupDispenseRequests.ForEach(groupDispenseRequest => groupDispenseRequest.eRecipeItem.DispenseWarnings = dispenseWarnings);
                    return await CreateRecipeDispenseMultiple(groupDispenseRequests);
                }

                return recipeDispenseResults.ToList();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
                return new List<RecipeDispenseDto>();
            }
        }

        public async Task GetDispensePdf(string CompositionId, string proc_name = "GetSignedDispensePdf", string pdf_name = "SignedPdfBase64Encoded")
        {
            var doc = await GetDispenseDocument(CompositionId);
            if (doc != null)
                helpers.ConvertByteArrayToPDF(doc, CompositionId);
        }

        private async Task<byte[]> GetDispenseDocument(string CompositionId) 
        {
            var documentsService = Program.ServiceProvider.GetRequiredService<IDocumentsService>();
            var getDocumentRequest = new GetDocumentRequest
            {
                CompositionId = CompositionId.ToLong(),
                PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                Status = TamroUtilities.HL7.Models.Document.PdfDocStatus.Unsigned
            };

            try
            {
                var doc = await documentsService.Get(getDocumentRequest);
                if (doc != null)
                    return doc;
            }
            catch
            {
                getDocumentRequest.Status = TamroUtilities.HL7.Models.Document.PdfDocStatus.Signed;

                try
                {
                    var doc = await documentsService.Get(getDocumentRequest);
                    if (doc != null)
                        return doc;
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
            }
            return null;
        }

        private async Task<bool> Sign(string CompositionId, string practitionerId) 
        {
            var documentsService = Program.ServiceProvider.GetRequiredService<IDocumentsService>();
            var pdfSignature = new PdfSignature(Session.SignatureParams);

            try
            {
                var getDocumentRequest = new GetDocumentRequest
                {
                    CompositionId = CompositionId.ToLong(),
                    PractitionerId = practitionerId.ToLong(),
                    Status = TamroUtilities.HL7.Models.Document.PdfDocStatus.Unsigned
                };

                var doc = await documentsService.Get(getDocumentRequest);
                if (doc is null)
                    return false;

                var signedDoc = pdfSignature.SignInMemory(doc);
                await documentsService.Confirm(CompositionId.ToLong(), signedDoc);
                return true;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, true, nameof(Sign));
                pdfSignature.Dispose();
                return false;
            }
        }

        public void GetSignedDispensePdf(string CompositionId, SOAPCallback callback)
        {
            Task.Run(async () =>
            {
                await GetDispensePdf(CompositionId);
            });
        }

        public void GetUnsignedDispensePdf(string CompositionId, SOAPCallback callback)
        {
            Task.Run(async () =>
            {
                await GetDispensePdf(CompositionId);
            });
        }

        public void ConfirmSignedDispensePdf(string CompositionId, string SignedPdfBase64Encoded, SOAPCallback callback)
        {
        }

        public async Task<bool> SignDispense(string CompositionId)
        {
            return await Sign(CompositionId, Session.PractitionerItem.PractitionerId);
        }

        public async Task<bool> SignVaccine(string CompositionId)
        {
            return await Sign(CompositionId, Session.ExtendedPracticePractitioner.PractitionerId);
        }

        public async Task<bool> CancelRecipeDispense(string CompositionId, string StatusReason, List<Items.eRecipe.Issue> issues)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();
            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();

            try
            {
                var cancelDispenseRequest = new CancelDispenseRequest
                {
                    PractitionerRef = Session.PractitionerItem.PractitionerRef,
                    CompositionId = CompositionId.ToLong(),
                    PrescriptionStatusReason = StatusReason,
                    DispenseStatusReason = StatusReason,
                    PrescriptionStatus = MedicationPrescriptionStatus.Active
                };

                if (issues != null && issues.Count > 0)
                    cancelDispenseRequest.Issues = mapper.Map<List<IssueDto>>(issues);

                var recipeDispenseDto = dispenseService.Cancel(cancelDispenseRequest);

                if (recipeDispenseDto.Issues.Any())
                {
                    if (recipeDispenseDto.Issues.Any(x => x.IssueSeverity.ToString().ToLower() == "error"))
                        throw new Exception(recipeDispenseDto.Issues.First().Display);

                    if (recipeDispenseDto.Issues.Any(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                    {
                        List<Items.eRecipe.Issue> DispenseWarnings = new List<Items.eRecipe.Issue>();
                        foreach (var issue in recipeDispenseDto.Issues.Where(x => x.IssueSeverity.ToString().ToLower() == "warning"))
                        {
                            using (ProgressDialog dlg = new ProgressDialog("dispense_warning", issue.Display))
                            {
                                dlg.ShowDialog();
                                if (dlg.DialogResult == DialogResult.OK)
                                    issue.IgnoreReason = dlg.Result;
                                else
                                    throw new Exception("");
                            }
                            DispenseWarnings.Add(mapper.Map<Items.eRecipe.Issue>(issue));
                        }
                        return await CancelRecipeDispense(CompositionId, "Panaikintas kvitas", DispenseWarnings);
                    }
                }

                return !recipeDispenseDto.Issues.Any();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
                return false;
            }
        }

        public async Task<T> ReserveRecipe<T>(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, string deliveryDate)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionRequest = new MedicationPrescriptionRequest
                    {
                        PractitionerId = Session.PractitionerItem.PractitionerId,
                        MedicationPrescriptionId = MedicationPrescriptionId,
                        ExpectedDeliveryDate = deliveryDate,
                        StatusReason = StatusReason,
                        PractitionerRef = PractitionerRef,
                        Status = MedicationPrescriptionStatus.OnHold
                    };

                    var recipeDto = medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                    if (recipeDto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> ChangeRecipeStatus<T>(string MedicationPrescriptionId, MedicationPrescriptionStatus status, string StatusReason)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionRequest = new MedicationPrescriptionRequest
                    {
                        MedicationPrescriptionId = MedicationPrescriptionId,
                        PractitionerId = Session.PractitionerItem.PractitionerId,
                        PractitionerRef = Session.PractitionerItem.PractitionerRef,
                        StatusReason = StatusReason,
                        Status = status

                    };

                    var recipeDeto = medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                    if (recipeDeto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> CancelRecipeReservation<T>(string MedicationPrescriptionId, string PractitionerRef, string StatusReason)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            return await Task.Run(() =>
            {
                try
                {
                    var medicationPrescriptionRequest = new MedicationPrescriptionRequest
                    {
                        MedicationPrescriptionId = MedicationPrescriptionId,
                        PractitionerId = Session.PractitionerItem.PractitionerId,
                        PractitionerRef = PractitionerRef,
                        StatusReason = StatusReason,
                        Status = MedicationPrescriptionStatus.Active

                    };

                    var recipeDeto = medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                    if (recipeDeto is T recipe)
                    {
                        return recipe;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public void SuspendRecipe(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, SOAPCallback callback)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            try
            {
                var medicationPrescriptionRequest = new MedicationPrescriptionRequest
                {
                    MedicationPrescriptionId = MedicationPrescriptionId,
                    PractitionerId = Session.PractitionerItem.PractitionerId,
                    PractitionerRef = PractitionerRef,
                    StatusReason = StatusReason,
                    StatusReasonCode = "for review",
                    Status = MedicationPrescriptionStatus.OnHold
                };

                medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                callback?.Invoke(true, new XElement("ds", new XElement("t")).ToString(), null);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        public void CancelRecipeSuspension(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, SOAPCallback callback)
        {
            var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();

            try
            {
                var medicationPrescriptionRequest = new MedicationPrescriptionRequest
                {
                    MedicationPrescriptionId = MedicationPrescriptionId,
                    PractitionerId = Session.PractitionerItem.PractitionerId,
                    PractitionerRef = PractitionerRef,
                    StatusReason = StatusReason,
                    Status = MedicationPrescriptionStatus.Active

                };

                medicationPrescriptionService.ChangeStatus(medicationPrescriptionRequest);
                callback?.Invoke(true, new XElement("ds", new XElement("t")).ToString(), null);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
        }

        public async Task<bool> ConfirmRecipeDispense(string MedicationDispenseId, string PractitionerRef, bool ConfirmDispense)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();
            return await Task.Run(() =>
            {
                try
                {
                    return dispenseService.Confirm(new ConfirmDispenseRequest()
                    {
                        MedicationDispenseId = MedicationDispenseId,
                        PractitionerRef = PractitionerRef,
                        ConfirmDispense = ConfirmDispense
                    });
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                    return false;
                }  
            });
        }

        public List<string> GetClassifier(string ClassifierName, string DateFrom)
        {
            var classifierService = Program.ServiceProvider.GetRequiredService<IClassifierService>();

            try
            {
                var request = new TamroUtilities.HL7.Models.Classifier.SearchClassifierRequest()
                {
                    PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                    ClassifierName = ClassifierName
                };

                if (!string.IsNullOrWhiteSpace(DateFrom))
                    request.DateFrom = DateFrom.ToDateTimeExact();

                return classifierService.Search(request);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, false);
                return new List<string>();
            }
        }

        public async Task<T> GetVaccinationData<T>(
            string patientId,
            string practitionerId,
            string organizationId,
            int docType,
            string status,
            string docStatus,
            int pageSize,
            int page,
            string dtFrom,
            string dtTo,
            string orderIds)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationSearchRequest = new VaccinationSearchRequest
                    {
                        PatientId = patientId.ToLong(),
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId.ToLong(),
                        AuthorPractitionerId = practitionerId.ToLong(),
                        OrganizationId = organizationId.ToLong(),
                        PageSize = pageSize,
                        PageNumber = page,
                        DocStatus = docStatus,
                        Status = status,
                        DocumentType = (VaccinationDocumentType)docType,
                        IncludeFullInfo = true
                    };

                    if (!string.IsNullOrWhiteSpace(dtFrom))
                        vaccinationSearchRequest.From = dtFrom.ToDateTimeExact();

                    if (!string.IsNullOrWhiteSpace(dtTo))
                        vaccinationSearchRequest.To = dtTo.ToDateTimeExact();

                    var vaccinationDataListDto = vaccinationService.Search(vaccinationSearchRequest);
                    if (vaccinationDataListDto is T vaccinationDataList)
                    {
                        return vaccinationDataList;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> CheckESI<T>(string esiNr)
        {
            var patientService = Program.ServiceProvider.GetRequiredService<IPatientService>();
            return await Task.Run(() =>
            {
                try
                {
                    var esiIdentityDto = patientService.CheckESI(esiNr, Session.PractitionerItem.PractitionerId.ToLong());
                    if (esiIdentityDto is T esiIdentity)
                    {
                        return esiIdentity;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> CancelVaccinationPrescription<T>(string compositionId, string reason)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationCancelRequest = new VaccinationCancelRequest
                    {
                        CompositionId = compositionId.ToLong(),
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId.ToLong(),
                        PractitionerRef = Session.ExtendedPracticePractitioner.PractitionerRef,
                        StatusReason = reason
                    };

                    var vaccineOrderResponseDto = vaccinationService.CancelPrescription(vaccinationCancelRequest);
                    if (vaccineOrderResponseDto is T vaccineOrderResponse)
                    {
                        return vaccineOrderResponse;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> CancelVaccinationDispensation<T>(string compositionId, string reason)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationCancelRequest = new VaccinationCancelRequest
                    {
                        CompositionId = compositionId.ToLong(),
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId.ToLong(),
                        PractitionerRef = Session.ExtendedPracticePractitioner.PractitionerRef,
                        StatusReason = reason
                    };

                    var vaccineOrderResponseDto = vaccinationService.CancelDispensation(vaccinationCancelRequest);
                    if (vaccineOrderResponseDto is T vaccineOrderResponse)
                    {
                        return vaccineOrderResponse;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> MakeVaccinationPrescription<T>(
            string patientRef,
            string encounterRef,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            int doseNumber,
            string notes)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();
            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationPrescriptionCreateRequest = new VaccinationPrescriptionCreateRequest
                    {
                        PatientRef = patientRef,
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId,
                        PractitionerRef = Session.ExtendedPracticePractitioner.PractitionerRef,
                        PractitionerOrgRef = Session.ExtendedPracticePractitioner.OrganizationRef,
                        OrganizationRef = Session.OrganizationItem.OrganizationRef,
                        EncounterRef = encounterRef,
                        InfectiousDiseaseCode = infectiousDiseaseCode,
                        InfectiousDiseaseDisplay = infectiousDiseaseDisplay,
                        DoseNumber = doseNumber,
                        VaccineNPAKID7 = medicationNPAKID,
                        VaccineName = medicationName,
                        Date = DateTime.Now,
                        Description = notes
                    };

                    var vaccineOrderResponseDto = vaccinationService.CreatePrescription(vaccinationPrescriptionCreateRequest);
                    if (vaccineOrderResponseDto is T vaccineOrderResponse)
                    {
                        return vaccineOrderResponse;
                    }
                }
                catch (Exception ex) 
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> MakeVaccinationDispensation<T>(
            string patientRef,
            string medicationRef,
            string encounterRef,
            string orderRef,
            string orderResponseId,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            string routeCode,
            string routeDisplay,
            string vaccineSerie,
            int doseNumber,
            DateTime vaccinationDate,
            DateTime medicineExpiryDate)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationDispensationCreateRequest = new VaccinationDispensationCreateRequest
                    {
                        PatientRef = patientRef,
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId,
                        PractitionerRef = Session.ExtendedPracticePractitioner.PractitionerRef,
                        PractitionerOrgRef = Session.ExtendedPracticePractitioner.OrganizationRef,
                        OrganizationRef = Session.OrganizationItem.OrganizationRef,
                        EncounterRef = encounterRef,
                        InfectiousDiseaseCode = infectiousDiseaseCode,
                        InfectiousDiseaseDisplay = infectiousDiseaseDisplay,
                        DoseNumber = doseNumber,
                        VaccineNPAKID7 = medicationNPAKID,
                        VaccineName = medicationName,
                        MedicationRef = medicationRef,
                        OrderRef = orderRef,
                        RouteCode = routeCode,
                        RouteDisplay = routeDisplay,
                        MedicineExpiryDate = medicineExpiryDate,
                        VaccineSerieNumber = vaccineSerie,
                        Description = "Skiepų išdavimas",
                        Date = DateTime.Now
                    };

                    var vaccineOrderResponseDto = vaccinationService.CreateDispensation(vaccinationDispensationCreateRequest);
                    if (vaccineOrderResponseDto is T vaccineOrderResponse)
                    {
                        return vaccineOrderResponse;
                    }
                }
                catch(Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task<T> MakeVaccinationDispensationV2<T>(
            string patientRef,
            string medicationRef,
            string encounterRef,
            string orderRef,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            string routeCode,
            string routeDisplay,
            string vaccineSerie,
            int doseNumber,
            DateTime medicineExpiryDate)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            return await Task.Run(() =>
            {
                try
                {
                    var vaccinationDispensationCreateRequest = new VaccinationDispensationCreateRequest
                    {
                        PatientRef = patientRef,
                        PractitionerId = Session.ExtendedPracticePractitioner.PractitionerId,
                        PractitionerRef = Session.ExtendedPracticePractitioner.PractitionerRef,
                        PractitionerOrgRef = Session.ExtendedPracticePractitioner.OrganizationRef,
                        OrganizationRef = Session.OrganizationItem.OrganizationRef,
                        EncounterRef = encounterRef,
                        InfectiousDiseaseCode = infectiousDiseaseCode,
                        InfectiousDiseaseDisplay = infectiousDiseaseDisplay,
                        DoseNumber = doseNumber,
                        VaccineNPAKID7 = medicationNPAKID,
                        VaccineName = medicationName,
                        MedicationRef = medicationRef,
                        OrderRef = orderRef,
                        RouteCode = routeCode,
                        RouteDisplay = routeDisplay,
                        MedicineExpiryDate = medicineExpiryDate,
                        VaccineSerieNumber = vaccineSerie,
                        Description = "Skiepų išdavimas",
                        Date = DateTime.Now
                    };

                    var vaccineOrderResponseDto = vaccinationService.CreateDispensationV2(vaccinationDispensationCreateRequest);
                    if (vaccineOrderResponseDto is T vaccineOrderResponse)
                    {
                        return vaccineOrderResponse;
                    }
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }


        public T GetImmunization<T>(string patientId, string practitionerId, string dateFrom, string dateTo, int pageSize = 100, int page = 1)
        {
            var vaccinationService = Program.ServiceProvider.GetRequiredService<IVaccinationService>();

            try
            {
                var getImmunizationRequest = new GetImmunizationRequest
                {
                    PatientId = patientId.ToLong(),
                    PractitionerId = practitionerId.ToLong(),
                    From = dateFrom.ToDateTimeExact(),
                    To = dateTo.ToDateTimeExact(),
                    PageSize = pageSize,
                    PageNumber = page
                };

                var immunizationListDto = vaccinationService.GetImmunizationList(getImmunizationRequest);
                if (immunizationListDto is T immunizationList)
                {
                    return immunizationList;
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            return default;
        }

        public async Task<T> CreatePaperRecipeDispense<T>(CreateDispenseRequest request)
        {
            var dispenseService = Program.ServiceProvider.GetRequiredService<IDispenseService>();
            return await Task.Run(() =>
            {
                try
                {
                    var recipeDispenseDto = dispenseService.CreatePaper(request);
                    if (recipeDispenseDto is T recipeDispense)
                    {
                        return recipeDispense;
                    } 
                }
                catch (Exception ex)
                {
                    HandleExceptions(ex);
                }
                return default;
            });
        }

        public async Task RetryLockRecipe(string compositionId, bool locking, string dispenseBySubstancesGroupId)
        {
            try
            {
                if (Session.getParam("ERECIPE", "V2") == "0" || string.IsNullOrEmpty(compositionId))
                    return;

                if (!string.IsNullOrEmpty(dispenseBySubstancesGroupId) &&
                    Program.Display1.PoshItem?.PosdItems.Count(e => e.eRecipeDispenseBySubstancesGroupId == dispenseBySubstancesGroupId) > 1)                 
                    return;
                
                var medicationPrescriptionService = Program.ServiceProvider.GetRequiredService<IMedicationPrescriptionService>();
                var recipeDto = medicationPrescriptionService.GetMedicationPrescriptionByCompositionId(
                     compositionId.ToLong(),
                     Session.PractitionerItem.PractitionerId.ToLong());

                if (recipeDto == null || recipeDto?.Type == "pp" || recipeDto?.Status == "completed")
                    return;

                bool isLocked = !string.IsNullOrWhiteSpace(recipeDto.LockWho);

                if (isLocked && recipeDto.LockWho.ExtractId() != Session.PractitionerItem.PractitionerId && locking) 
                {
                    var practitionerService = Program.ServiceProvider.GetRequiredService<IPractitionerService>();
                    var practitionerDto = practitionerService.Get(recipeDto.LockWho.ExtractId().ToLong(), true);
                    var practitionerName = $"{practitionerDto.FamilyName.FirstOrDefault()} {practitionerDto.GivenName.FirstOrDefault()}";
                    helpers.alert(Enumerator.alert.error,$"Receptas yra užrakintas specialisto: {practitionerName} Spaudo ID: {practitionerDto.StampCode}\n" +
                        $"Vaistinėje: {practitionerDto.OrganizationName} JAR: {practitionerDto.OrganizationJAR} ");
                    return;
                }

                if (!isLocked && locking)
                {
                    await LockRecipe<RecipeDto>(recipeDto.MedicationPrescriptionId,
                        Session.PractitionerItem.PractitionerRef,
                        Session.OrganizationItem.OrganizationRef,
                        null);
                }

                if (isLocked && !locking)
                {
                    await UnlockRecipe<RecipeDto>(recipeDto.MedicationPrescriptionId);
                }
            }
            catch (Exception ex)
            {
                HandleExceptions(ex, false);
            }
        }

        public void SetRequestorId(string practitionerId)
        {
            var practitionerService = Program.ServiceProvider.GetRequiredService<IPractitionerService>();
            try
            {
                practitionerService.SetRequestorId(practitionerId.ToLong());
            }
            catch (Exception ex)
            {
                HandleExceptions(ex,false);
            }
        }

        public CancellationTokenSource cts
        {
            get
            {
                if (_cts == null)
                    _cts = new CancellationTokenSource();
                return _cts;
            }
            set
            {
                _cts = value;
            }
        }

        #region Private methods

        private void HandleExceptions(Exception ex, bool showAlert = true, string methodName = "")
        {
            if (!showAlert) 
                return;

            if (ex is FhirOperationException)
            {
                var fhirOperationException = (FhirOperationException)ex;
                if (fhirOperationException.Outcome != null)
                {
                    helpers.alert(HasOutcomeFatalError(fhirOperationException.Outcome) ?
                        Enumerator.alert.error :
                        Enumerator.alert.warning,
                        BuildOutcomeErrorMessage(fhirOperationException.Outcome));
                }
                else
                {
                    helpers.alert(Enumerator.alert.error, fhirOperationException.Message);
                }
            }
            else if (ex is InternalErrorException)
            {
                if (methodName.ToLowerInvariant().Contains("sign")) 
                {
                    helpers.alert(Enumerator.alert.error,
                        (ex as InternalErrorException).Type.ToLowerInvariant().Contains("signed") ?
                        "Dokumentas jau pasirašytas" :
                        ex.Message);
                }
            }
            else if (ex is PdfSignatureException)
            {
                helpers.alert(Enumerator.alert.error,
                    ex.Message.ToLowerInvariant().Contains("signed") ?
                    "Dokumentas jau pasirašytas" :
                    ex.Message);
            }
            else
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

        private bool HasOutcomeFatalError(OperationOutcome oo) 
        {
            foreach (var issue in oo.Issue) 
            {
                if (!issue.Severity.HasValue)
                    continue;

                if (issue.SeverityElement.Value == OperationOutcome.IssueSeverity.Fatal ||
                    issue.SeverityElement.Value == OperationOutcome.IssueSeverity.Error)
                    return true;
            }
            return false;
        }

        private string BuildOutcomeErrorMessage(OperationOutcome oo) 
        {
            var errorMessage = string.Empty;
            foreach (var issue in oo.Issue)
            {
                errorMessage += $"{issue.Details}\n\n";
            }
            return errorMessage;
        }

        private CreateDispenseRequest BuildCreateDispenseRequest(Recipe eRecipeItem, string PickedUpByRef, DateTime DispenseDueDate, decimal PriceRetail, decimal PricePaid, decimal PriceCompensated, decimal prepCompSum, decimal Quantity, DateTime DispenseDate, bool confirmDispense, bool isCheapest, int durationOfUse)
        {
            ResourceManager rm = Resources.ResourceManager;
            var PrivateKeyXml = rm.GetString("_" + Session.SystemData.ecode);
            var eHealthEndpoint = Session.Develop ?
                Settings.Default.mokymai_eRecipe_EndPoint :
                Settings.Default.eRecipe_EndPoint;

            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();

            XElement Params = new XElement("ds",
                         new XElement("t",
                                     new XElement("ProcName", "CreateRecipeDispense"),
                                     new XElement("EndPoint", eHealthEndpoint),
                                     new XElement("ConsumerId", Session.SystemData.ecode),
                                     new XElement("PrivateKeyXml", PrivateKeyXml),
                                     new XElement("PractitionerId", Session.PractitionerItem.PractitionerId),
                                     new XElement("PractitionerRef", Session.PractitionerItem.PractitionerRef),
                                     new XElement("PatientRef", eRecipeItem.Patient.PatientRef),
                                     new XElement("PickedUpByRef", PickedUpByRef != "" ? PickedUpByRef : eRecipeItem.Patient.PatientRef),
                                     new XElement("ConfirmationPractitionerRef", Session.PractitionerItem.PractitionerRef),
                                     new XElement("OrganizationRef", Session.OrganizationItem.OrganizationRef),
                                     new XElement("EncounterRef", eRecipeItem.Encounter.EncounterRef),
                                     new XElement("ConfirmDispense", confirmDispense),
                                     new XElement("PrescriptionCompositionId", eRecipeItem.eRecipe.RecipeNumber),
                                     new XElement("MedicationPrescriptionId", eRecipeItem.eRecipe.MedicationPrescriptionId),
                                     new XElement("PrescriptionStatusCode", ResolveDispenseStatus(eRecipeItem, Quantity)),
                                     new XElement("CompensationTag", eRecipeItem.eRecipe_CompensationTag),
                                     new XElement("DispenseDueDate", DispenseDueDate),
                                     new XElement("PriceRetail", PriceRetail),
                                     new XElement("PricePaid", PricePaid),
                                     new XElement("PriceCompensated", PriceCompensated),
                                     new XElement("Quantity", Quantity),
                                     new XElement("QuantityMeasureUnits", "Vienetas"),
                                     new XElement("QuantityMeasureCode", "vnt."),
                                     new XElement("MedicationRef", eRecipeItem.Medication?.MedicationRef),
                                     new XElement("DispenseDate", DispenseDate.ToUniversalTime()),
                                     new XElement("AdditionalInstructionsForPatient", eRecipeItem.AdditionalInstructionsForPatient),
                                     new XElement("LowIncomeTag", eRecipeItem.HasLowIncome?.HasLowIncome ?? false),
                                     new XElement("LowIncomeSurchargeCompensated", prepCompSum),
                                     new XElement("IsCheapest", isCheapest)
                         )
            );

            if (Session.getParam("ERECIPE", "SURCHARGEELIGABLE") == "1")
            {
                Params.Element("t").Add(new XElement("SurchargeEligible", eRecipeItem.AccumulatedSurcharge?.SurchargeEligible ?? false));
            }

            if (eRecipeItem.eRecipe.Type == "ev" || eRecipeItem.eRecipe.Type == "vv")//ekstemporalus arba vardinis vaistas
            {
                Params.Element("t").Add(new XElement("ContainedMedicationId", eRecipeItem.Medication.MedicationId),
                                     new XElement("ContainedMedicationName", eRecipeItem.eRecipe.Type == "ev" ? "Gaminami vaistai" : eRecipeItem.eRecipe.GenericName),
                                     new XElement("ContainedMedicationPharmaceuticalFormCode", eRecipeItem.eRecipe.PharmaceuticalFormCode),
                                     new XElement("ContainedMedicationPharmaceuticalFormDisplay", eRecipeItem.eRecipe.PharmaceuticalForm),
                                     new XElement("ContainedMedicationActiveSubstances", eRecipeItem.eRecipe.GenericName != "" ? eRecipeItem.eRecipe.GenericName : "Gaminami vaistai"),
                                     new XElement("ContainedMedicationStrength", eRecipeItem.eRecipe.Strength),
                                     new XElement("ContainedMedicationExtemporaneous", eRecipeItem.eRecipe.Type == "ev" ? "true" : "false"));
                if (eRecipeItem.eRecipe.ExtemporaneousDescription != "" && eRecipeItem.eRecipe.ExtemporaneousMethod != "")
                    Params.Element("t").Add(new XElement("ContainedMedicationExtemporaneousDescription", eRecipeItem.eRecipe.ExtemporaneousDescription),
                                         new XElement("ContainedMedicationExtemporaneousMethod", eRecipeItem.eRecipe.ExtemporaneousMethod)
                                         );
                if (eRecipeItem.eRecipe.ExtemporaneousVolumeWeightQuantityValue != "")
                {
                    Params.Element("t").Add(new XElement("ContainedMedicationExtemporaneousVolumeWeightValue", eRecipeItem.eRecipe.ExtemporaneousVolumeWeightQuantityValue.Replace(',', '.')),
                                     new XElement("ContainedMedicationExtemporaneousVolumeWeightUnits", eRecipeItem.eRecipe.ExtemporaneousVolumeWeightQuantityUnits),
                                     new XElement("ContainedMedicationExtemporaneousVolumeWeightCode", eRecipeItem.eRecipe.ExtemporaneousVolumeWeightQuantityCode),
                                     new XElement("ContainedMedicationExtemporaneousVolumeWeightSystem", eRecipeItem.eRecipe.ExtemporaneousVolumeWeightQuantitySystem)
                        );
                }
                if (eRecipeItem.eRecipe.ExtemporaneousIngredientItem != "")
                {
                    Params.Element("t").Add(new XElement("ContainedMedicationIngredients", "true"),
                                     new XElement("ContainedMedicationIngredientItemReference", eRecipeItem.eRecipe.ExtemporaneousIngredientItem));
                    if (eRecipeItem.eRecipe.ExtemporaneousIngredientAmountNumeratorValue != "")
                        Params.Element("t").Add(new XElement("ContainedMedicationIngredientAmountNumeratorValue", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountNumeratorValue.Replace(',', '.')),
                                     new XElement("ContainedMedicationIngredientAmountNumeratorUnits", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountNumeratorUnits),
                                     new XElement("ContainedMedicationIngredientAmountNumeratorSystem", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountNumeratorSystem),
                                     new XElement("ContainedMedicationIngredientAmountNumeratorCode", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountNumeratorCode));
                    if (eRecipeItem.eRecipe.ExtemporaneousIngredientAmountDenominatorValue != "")
                        Params.Element("t").Add(new XElement("ContainedMedicationIngredientAmountDenominatorValue", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountDenominatorValue.Replace(',', '.')),
                                     new XElement("ContainedMedicationIngredientAmountDenominatorUnits", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountDenominatorUnits),
                                     new XElement("ContainedMedicationIngredientAmountDenominatorSystem", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountDenominatorSystem),
                                     new XElement("ContainedMedicationIngredientAmountDenominatorCode", eRecipeItem.eRecipe.ExtemporaneousIngredientAmountDenominatorCode)
                        );
                }
            }
            if (eRecipeItem.DispenseWarnings != null && eRecipeItem.DispenseWarnings.Count > 0)
            {
                foreach (var issue in eRecipeItem.DispenseWarnings)
                {
                    if (issue.Code != "" && issue.Details != "" && issue.IgnoreReason != "")
                    {
                        Params.Element("t").Add(new XElement("Issue",
                                         new XElement("Code", issue.Code),
                                         new XElement("Display", issue.Details),
                                         new XElement("IgnoreReason", issue.IgnoreReason)));
                    }
                }
            }

            if (Session.getParam("ERECIPE", "V2") == "1")
            {
                Params.Element("t").Add(new XElement("DataModelVersionValue", "http://esveikata.lt/Profile/MedicationDispense/v29"),
                    new XElement("DispenseTagsDispenseBySubstancesTag", eRecipeItem.IsDispenseBySubtances ? "true" : "false"),
                    new XElement("DispenseTagsDispenseBySubstancesGroupId", eRecipeItem.DispenseBySubstancesGroupId),
                    new XElement("DispenseTagsNoValidPrescriptionTag", eRecipeItem.IsValidToDispenseWithoutValidPrescription() ? "true" : "false"),
                    new XElement("DispenseTagsOverLimitQuantityTag", eRecipeItem.IsOverLimitQty ? "true" : "false"),
                    new XElement("DurationOfUse", durationOfUse),
                    new XElement("MppBarcode", eRecipeItem.MppBarcode));
            }

            var dp = helpers.FromXElement<DispenseParams>(Params.Element("t"));
            var createDispenseRequest = mapper.Map<CreateDispenseRequest>(dp);
            createDispenseRequest.Qualification = Session.PractitionerItem.Qualification;
            return createDispenseRequest;
        }

        private SearchMedicationRequest BuildSearchMedicationRequest(string parameter, string value, string PageSize = "", string Page = "")
        {
            var searchMedicationListRequest = new SearchMedicationRequest()
            {
                PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                PageSize = string.IsNullOrEmpty(PageSize) ? 1 : PageSize.ToInt(),
                PageNumber = string.IsNullOrEmpty(Page) ? 1 : Page.ToInt()
            };

            if (parameter.ToLowerInvariant() == "npakid7")
            {
                searchMedicationListRequest.Npakid7 = value.ToLong();
            }
            else if (parameter.ToLowerInvariant() == "containedmedicationid")
            {
                searchMedicationListRequest.ContainedMedicationId = value.ToLong();
            }
            return searchMedicationListRequest;
        }

        private List<PrescriptionStatus> GetPrescriptionStatuses(string status)
        {
            List<PrescriptionStatus> prescriptionStatuses = new List<PrescriptionStatus>();
            if (status.ToLowerInvariant().Contains("active"))
                prescriptionStatuses.Add(PrescriptionStatus.Active);
            if (status.ToLowerInvariant().Contains("on hold"))
                prescriptionStatuses.Add(PrescriptionStatus.OnHold);
            if (status.ToLowerInvariant().Contains("completed"))
                prescriptionStatuses.Add(PrescriptionStatus.Completed);
            return prescriptionStatuses;
        }

        private string ResolveDispenseStatus(Recipe eRecipeItem, decimal Quantity)
        {

            if (Session.getParam("ERECIPE", "V2") == "1")
            {
                return eRecipeItem.PrescriptionStatus;
            }

            if (eRecipeItem.eRecipe_PrescriptionTagsLongTag)
            {
                bool hasRepeatsLeft = eRecipeItem.DispenseCount < (eRecipeItem.eRecipe_NumberOfRepeatsAllowed - 1);
                bool hasRemainingQuantity = (eRecipeItem.DispensedQty + Quantity) < eRecipeItem.eRecipe.QuantityValue.ToDecimal();

                return (hasRepeatsLeft && hasRemainingQuantity) ? "active" : "completed";
            }

            return "completed";
        }

        private bool IsPersonalCode(string value) 
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            if (value.Length != 11)
            {
                return false;
            }
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}

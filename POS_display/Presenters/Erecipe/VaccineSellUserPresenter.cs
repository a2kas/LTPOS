using POS_display.Repository.Recipe;
using POS_display.Views;
using POS_display.Views.ERecipe;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;

namespace POS_display.Presenters.ERecipe
{
    public class VaccineSellUserPresenter : ErrorHandling
    {
        #region Members
        private readonly IBusy _busy;
        private readonly IVaccineSellUser _view;
        private readonly IRecipeRepository _recipeRepository = null;
        #endregion

        #region Constructor
        public VaccineSellUserPresenter(IBusy busy, IVaccineSellUser view, IRecipeRepository recipeRepository)
        {
            _busy = busy;
            _view = view;
            _recipeRepository = recipeRepository;
        }
        #endregion

        #region Override
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                _busy.IsBusy = value;
                base.IsBusy = value;
            }
        }
        #endregion

        #region Public methods
        public async Task CreateVaccineDispensation(Items.posd posd, VaccinationDataDto prescription)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                Models.Barcode barcodeModel = new Models.Barcode() { BarcodeStr = posd.barcode };
                BarcodePresenter BCPresenter = new BarcodePresenter(null, barcodeModel);
                await BCPresenter.GetDataFromBarcode();
                decimal npakId7 = await Session.SamasUtils.GetNpakid7ByProductId(barcodeModel.ProductId);
                if (npakId7 <= 0)
                    throw new Exception("Nepavyko rasti prekės NPAKID7 kodo!");

                if (prescription.ImmunizationRecommendation.VaccineNPAKID7 != npakId7.ToString())              
                    throw new Exception("Parduodamos vakcinos NPAKID7 kodas nesutampa su skiepų skyrime nurodytos vakcinos NPAKID7 kodu!");
                
                MedicationDto medication = null;
                var medication_list = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", npakId7.ToString())?.MedicationList;
                if (medication_list?.Count > 0)
                {
                    medication = medication_list.FirstOrDefault();
                    if (medication == null)
                        throw new Exception($"Nepavyksta rasti medikamento su NPAKID7 kodu {npakId7}!");

                }
                Session.eRecipeUtils.cts = new CancellationTokenSource();
                var dispensationResult = await Session.eRecipeUtils.MakeVaccinationDispensation<VaccineOrderResponseDto> (_view.Patient.PatientRef,
                                                                medication.MedicationRef,
                                                                prescription.Encounter?.EncounterRef,
                                                                prescription.OrderRef,
                                                                prescription.OrderResponse != null ? prescription.OrderResponse?.OrderResponseId : string.Empty,
                                                                prescription.ImmunizationRecommendation?.InfectiousDiseaseCode,
                                                                prescription.ImmunizationRecommendation?.InfectiousDiseaseDisplay,
                                                                medication.ProprietaryName,
                                                                medication.NPAKID7.ToString(),
                                                                (_view.VaccinationTypeindex + 1).ToString(),
                                                                _view.VaccinationType != null ? _view.VaccinationType.ToString() : string.Empty,
                                                                _view.SerialNumber,
                                                                !string.IsNullOrEmpty(_view.VaccinatedDose) ? int.Parse(_view.VaccinatedDose) : 0,
                                                                _view.VaccinationDate,
                                                                _view.VaccineExpiryDate);

                if (dispensationResult != null)
                {
                    helpers.alert(
                        Enumerator.alert.info,
                        $"Skiepų išdavimas sėkmingai atliktas. Išdavimo dok. E063 nr: {dispensationResult.CompositionId}");

                    await _recipeRepository.CreateVaccination(posd.hid.ToLong(),
                                                              posd.id.ToLong(),
                                                              prescription.OrderId.ToLong(),
                                                              prescription.CompositionId.ToLong(),
                                                              prescription.CompositionRef,
                                                              dispensationResult.CompositionId.ToLong(),
                                                              dispensationResult.CompositionRef,
                                                              _view.Patient.PatientId.ToLong());

                }

                _view.RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Closed);
            });
        }
        #endregion
    }
}

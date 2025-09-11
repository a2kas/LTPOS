using Microsoft.Extensions.DependencyInjection;
using POS_display.Items.Prices;
using POS_display.Models;
using POS_display.Models.Pos;
using POS_display.Models.Price;
using POS_display.Models.TransactionService.Medication;
using POS_display.Models.TransactionService.Stock;
using POS_display.Repository.Pos;
using POS_display.Repository.Price;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Tamroutilities.Client;
using TamroUtilities.HL7.Models;

namespace POS_display.wpf.ViewModel.display2
{
    public class PricesViewModel : BaseViewModel, IAsyncInitialization, ICloneable
    {
        #region Members
        private const string NotSet = "Nenurodyta";
        private const string SupplyStatusSuspended = "SUPPLY_SUSPENDED";
        private const string SupplyStatusTemporarySupplyDisruption = "TEMPORARY_SUPPLY_DISRUPTION";
        private const string SupplyStatusNotStartedToSupply = "NOT_STARTED_TO_SUPPLY";
        private const string SupplyStatusSuppliedForeignLanguage = "SUPPLIED_FOREIGN_LANGUAGE";
        private const string SupplyStatusNotSupplied = "NOT_SUPPLIED";
        private List<string> _formsOfUse = new List<string>();
        private List<string> _strengths = new List<string>();
        private List<TLKPriceDetail> _tlkPriceDetails;
        private List<GenericItemAdditionalData> _genericItemsAdditionalData;
        private Dictionary<string,List<MedicationProduct>> _medicationProductsCache;
        private Display2Tag _tags;
        #endregion

        #region Overrides
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }
        #endregion

        #region Constructor
        public PricesViewModel(RecipeDto recipeDTO, ref Display2Tag tags)
        {
            RecipeDto = recipeDTO;
            PriceClass = Session.PriceClass;
            var compensationpercent = ExtractCompensationPercent(recipeDTO?.CompensationName ?? string.Empty);
            DefaultPrice = compensationpercent;
            model.Percentage = compensationpercent;
            Dosage = ExtractDosageAmount(recipeDTO);
            FirstPrescription = recipeDTO.PrescriptionTagsFirstPrescribingTag.ToBool();
            SeparateActiveSubstancesMode = tags.IsPartialDispense;
            CanEditFormOfUse = true;
            CanEditStrength = true;
            CanScanItem = true;
            GenericName = recipeDTO.GenericName;
            _tags = tags;
            Initialization = InitializeByRecipeAsync();
        }

        public PricesViewModel(Barcode barcode)
        {
            RecipeDto = new RecipeDto();
            PriceClass = Session.PriceClass;
            DefaultPrice = 0;
            model.Percentage = 0;
            Dosage = barcode.Dosage;
            FirstPrescription = false;
            CanEditFormOfUse = true;
            CanEditStrength = true;
            CanScanItem = true;
            SeparateActiveSubstancesMode = false;
            CanActivateSeparateActiveSubstances = false;
            Initialization = InitializeByBarcodeAsync(barcode);
        }
        #endregion

        #region Commands

        private IAsyncCommand _ScanItemCommand;
        public IAsyncCommand ScanItemCommand
        {
            get
            {
                return _ScanItemCommand ?? (_ScanItemCommand = new BaseAsyncCommand(ScanItem));
            }
        }

        private IAsyncCommand _RefreshCommand;
        public IAsyncCommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = RecipeDto != null ? new BaseAsyncCommand(InitializeByRecipeAsync) : new BaseAsyncCommand(InitializeAsync));
            }
        }

        private ICommand _F1Command;
        public ICommand F1Command
        {
            get
            {
                return _F1Command ?? (_F1Command = new BaseCommand(new Action<object>((obj) =>
                { ShowInfoForPrice(new Model.display2.PricesComp() { Percentage = 100, GIVisible = model.GIVisible }); })));
            }
        }

        private ICommand _F2Command;
        public ICommand F2Command
        {
            get
            {
                return _F2Command ?? (_F2Command = new BaseCommand(new Action<object>((obj) =>
                { ShowInfoForPrice(new Model.display2.PricesComp() { Percentage = 90, GIVisible = model.GIVisible }); })));
            }
        }

        private ICommand _F3Command;
        public ICommand F3Command
        {
            get
            {
                return _F3Command ?? (_F3Command = new BaseCommand(new Action<object>((obj) =>
                { ShowInfoForPrice(new Model.display2.PricesComp() { Percentage = 80, GIVisible = model.GIVisible }); })));
            }
        }

        private ICommand _F4Command;
        public ICommand F4Command
        {
            get
            {
                return _F4Command ?? (_F4Command = new BaseCommand(new Action<object>((obj) =>
                { ShowInfoForPrice(new Model.display2.PricesComp() { Percentage = 50, GIVisible = model.GIVisible }); })));
            }
        }

        private ICommand _F5Command;
        public ICommand F5Command
        {
            get
            {
                return _F5Command ?? (_F5Command = new BaseCommand(new Action<object>((obj) =>
                { ShowInfoForPrice(new Model.display2.PricesUnComp() { Percentage = 0, GIVisible = model.GIVisible }); })));
            }
        }

        private ICommand _DoubleClickCommand;
        public ICommand DoubleClickCommand
        {
            get
            {
                return _DoubleClickCommand ?? (_DoubleClickCommand = new BaseCommand(DoubleClickEvent));
            }
        }

        private ICommand _F6Command;
        public ICommand F6Command
        {
            get
            {
                return _F6Command ?? (_F6Command = new BaseCommand(new Action<object>((obj) =>
                {
                    model.GIVisible = !model.GIVisible;
                    NotifyPropertyChanged("model");
                })));
            }
        }

        private ICommand _NonTamroCommand;
        public ICommand NonTamroCommand
        {
            get
            {
                return _NonTamroCommand ?? (_NonTamroCommand = new BaseCommand(new Action<object>((obj) =>
                {
                    ExecuteAsyncAction(async () =>
                    {
                        var content = new
                        {
                            PharmacyId = Session.SystemData.kas_client_id.ToInt(),
                            NPakId7 = gvPricesSelectedRow.NpakId7.ToInt(),
                            ProductId = gvPricesSelectedRow.ProductId
                        };
                        Model.TamroGatewayResponse result = await Session.TamroGateway.PutAsync<Model.TamroGatewayResponse>("api/v1/Item/putrequesteditems", content);
                        if (result?.Status == true)
                            helpers.alert(Enumerator.alert.info, "Asortimento skyrius informuotas");
                        NotifyPropertyChanged("gvPricesItemsSource");
                    });
                })));
            }
        }

        private ICommand _LoadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _LoadedCommand ?? (_LoadedCommand = new BaseAsyncCommand(Loaded));
            }
        }

        private IAsyncCommand _SeparateSubstancesCommand;
        public IAsyncCommand SeparateSubstancesCommand
        {
            get
            {
                return _SeparateSubstancesCommand ?? (_SeparateSubstancesCommand = new BaseAsyncCommand(PerformSeparateSubstances));
            }
        }

        #endregion

        private void ShowInfoForPrice(Model.display2.Prices m)
        {
            ExecuteWithWait(() =>
            {
                Generics generics;
                if (m.Percentage > 0)
                    generics = compItems;
                else
                    generics = unCompItems;

                if (generics == null) return;
                generics.ItemsAllList.ForEach(f => f.IsSelected = false);
                var selected = generics.ItemsAllList.Where(f => f.ProductId == ProductId);
                if (selected?.Count() > 0)
                    selected.First().IsSelected = true;

                foreach (GenericItem maxGiChain in generics.ItemsAllList.Where(o => o.RxPriority > 0)) //new rx priority mark calculation
                {
                    maxGiChain.IsMaxGIChain = true;
                }

                model = m;
                gvPricesItemsSource = generics.ItemsAllList;
            }, false);
        }

        private void DoubleClickEvent(object obj)
        {
            ExecuteWithWait(() =>
            {
                if (FirstPrescription && IsSelectedCompensated && !gvPricesSelectedCheapest)
                {
                    using (var dlg = new ProgressDialog("erecipe_first_prescription", "Išrašytas el. receptas su žyma: Pirmas išdavimas\nPasirinkote ne pigiausią prekę iš sąrašo.\nPasirinkite priežastį."))
                    {
                        dlg.ShowDialog();
                        if (dlg.DialogResult != DialogResult.OK)
                            throw new Exception("");
                        FirstPrescriptionUncomp = dlg.cb.SelectedValue.ToString() == "0";
                        FirstPrescriptionReason = dlg.Result;
                    }
                }

                if (gvPricesSelectedRow.NotHasItemCard)
                    throw new Exception("Šia preke neprekiaujame!");

                CloseEvent(DialogResult.OK);
            });
        }

        private async Task FillUncompenstatedItemsData(List<GenericItem> genericItems, decimal dosage)
        {
            PriceRepository priceRepository = new PriceRepository();
            decimal qty;
            foreach (GenericItem ims in genericItems)
            {
                try
                {
                    if (ims.CompensationName != "U") continue;

                    qty = dosage == 0 ? 1 : ims.BarcodeRatio;
                    ims.Qty = qty;

                    if (ims.ProductId > 0)
                    {
                        ims.CurrentBalanceQty = Math.Round(await priceRepository.SearchProductQty(ims.ProductId), 3);
                        ims.MaxUnitPrice = await priceRepository.GetSalesPriceWithDiscount(ims.ProductId);
                        decimal vatproc = _genericItemsAdditionalData?.FirstOrDefault(e => e.ProductId == ims.ProductId)?.VatSize ?? 0;
                        decimal price_no_vat = Math.Round(ims.MaxUnitPrice / (1 + vatproc / 100), 4, MidpointRounding.AwayFromZero);
                        if (vatproc == 21 && price_no_vat > 300)//2017-01-01
                        {
                            vatproc = 5;
                            ims.MaxUnitPrice = Math.Round(price_no_vat * (1 + vatproc / 100), 2, MidpointRounding.AwayFromZero);
                        }
                        SetProductLocations(ims);
                    }

                    ims.CompensatedPrice = Math.Round((ims.MaxUnitPrice) * qty, 2);
                }
                catch (Exception ex)
                {
                    Serilogger.GetLogger().Warning($"[GetItemsForSecondaryIDuncomp] KAS ID: {Session.SystemData.kas_client_id} " +
                        $"failed to load generics product ID: {ims.ProductId}. Error: {ex.Message}");
                }
            }
            await AssignTamroQty(genericItems);
        }

        private async Task FillCompenstatedItemsData(List<GenericItem> genericItems, decimal dosage, string priceClass)
        {
            PriceRepository priceRepository = new PriceRepository();
            decimal qty;
            foreach (GenericItem ims in genericItems)
            {
                if (ims.CompensationName != "C") continue;
                decimal Price100 = ims.MaxUnitPrice;
                decimal Price90 = ims.MaxUnitPrice;
                decimal Price80 = ims.MaxUnitPrice;
                decimal Price50 = ims.MaxUnitPrice;
                try
                {
                    qty = dosage == 0 ? 1 : ims.BarcodeRatio;
                    ims.Qty = qty;

                    if (ims.ProductId > 0)
                    {
                        var pricelist_price = await priceRepository.GetCompPriceWithDiscount(ims.ProductId);
                        if (pricelist_price > 0)
                            ims.MaxUnitPrice = pricelist_price;
                        ims.CurrentBalanceQty = Math.Round(await priceRepository.SearchProductQty(ims.ProductId), 3);
                        Price100 = await priceRepository.GetSalesPriceComp(ims.ProductId, ims.VKBPrice100, priceClass);
                        Price90 = await priceRepository.GetSalesPriceComp(ims.ProductId, ims.VKBPrice90, priceClass);
                        Price80 = await priceRepository.GetSalesPriceComp(ims.ProductId, ims.VKBPrice80, priceClass);
                        Price50 = await priceRepository.GetSalesPriceComp(ims.ProductId, ims.VKBPrice50, priceClass);
                        SetProductLocations(ims);
                    }

                    //check ar nevirsija TLK kainyno kainu
                    // 100
                    var tlk100 = _tlkPriceDetails.FirstOrDefault(e => e.Npakid7 == ims.NpakId7.ToString() && e.Percent == 100);
                    if (tlk100 != null)
                    {
                        var maxmkaina = tlk100.RetailPrice;
                        if (Price100 > maxmkaina && maxmkaina > 0)
                        {
                            Price100 = maxmkaina;
                            ims.VKBPrice100 = Math.Round(Price100 - ims.Premium100, 2);
                        }
                    }
                    //90
                    var tlk90 = _tlkPriceDetails.FirstOrDefault(e => e.Npakid7 == ims.NpakId7.ToString() && e.Percent == 90);
                    if (tlk90 != null)
                    {
                        var maxmkaina = tlk90.RetailPrice;
                        if (Price90 > maxmkaina && maxmkaina > 0)
                        {
                            Price90 = maxmkaina;
                            ims.VKBPrice90 = Math.Round(Price90 - ims.Premium90, 2);
                        }
                    }
                    //80
                    var tlk80 = _tlkPriceDetails.FirstOrDefault(e => e.Npakid7 == ims.NpakId7.ToString() && e.Percent == 80);
                    if (tlk80 != null)
                    {
                        var maxmkaina = tlk80.RetailPrice;
                        if (Price80 > maxmkaina && maxmkaina > 0)
                        {
                            Price80 = maxmkaina;
                            ims.VKBPrice80 = Math.Round(Price80 - ims.Premium80, 2);
                        }
                    }
                    //50
                    var tlk50 = _tlkPriceDetails.FirstOrDefault(e => e.Npakid7 == ims.NpakId7.ToString() && e.Percent == 50);
                    if (tlk50 != null)
                    {
                        var maxmkaina = tlk50.RetailPrice;
                        if (Price50 > maxmkaina && maxmkaina > 0)
                        {
                            Price50 = maxmkaina;
                            ims.VKBPrice50 = Math.Round(Price50 - ims.Premium50, 2);
                        }
                    }

                    ims.PrepComp100 = Math.Round((Price100 - ims.VKBPrice100) * qty, 2);
                    if (ims.PrepComp100 < 0)
                        ims.PrepComp100 = 0;

                    ims.PrepComp90 = Math.Round((Price90 - ims.VKBPrice90) * qty, 2);
                    if (ims.PrepComp90 < 0)
                        ims.PrepComp90 = 0;

                    ims.PrepComp80 = Math.Round((Price80 - ims.VKBPrice80) * qty, 2);
                    if (ims.PrepComp80 < 0)
                        ims.PrepComp80 = 0;

                    ims.PrepComp50 = Math.Round((Price50 - ims.VKBPrice50) * qty, 2);
                    if (ims.PrepComp50 < 0)
                        ims.PrepComp50 = 0;

                    ims.CompensatedPrice = Math.Round((ims.MaxUnitPrice) * qty, 2);
                }
                catch (Exception ex)
                {
                    Serilogger.GetLogger().Warning($"[GetItemsForSecondaryIDcomp] KAS ID: {Session.SystemData.kas_client_id} " +
                        $"failed to load generics product ID: {ims.ProductId}. Error: {ex.Message}");
                }
            }
            await AssignTamroQty(genericItems);
        }

        private string BuildQuery(string genericName, string pharmaceuticalForm, string strength)
        {
            string query = string.Empty;

            if (!string.IsNullOrEmpty(genericName))
                query += $"ActiveSubstance={HttpUtility.UrlEncode(genericName)}";

            if (!strength.Equals(NotSet) && !string.IsNullOrWhiteSpace(strength))
                query += string.IsNullOrEmpty(query) ? $"Strength={HttpUtility.UrlEncode(strength)}" :
                    $"&Strength={HttpUtility.UrlEncode(strength)}";

            if (!pharmaceuticalForm.Equals(NotSet) && !string.IsNullOrEmpty(pharmaceuticalForm))
                query += string.IsNullOrEmpty(query) ? $"PharmaceuticalForm={HttpUtility.UrlEncode(pharmaceuticalForm)}" :
                    $"&PharmaceuticalForm={HttpUtility.UrlEncode(pharmaceuticalForm)}";

            return query;
        }

        private async Task CollectStrengthsAndFormsOfUseByATC(string activeSubstance)
        {
            _formsOfUse.Clear();
            _strengths.Clear();

            var medicationProducts = await Session.TamroGateway.GetAsync<List<MedicationProduct>>
                ($"/api/v1/medicationproducts?ActiveSubstance={HttpUtility.UrlEncode(activeSubstance)}&Take=1000");

            foreach (var medicationProduct in medicationProducts)
            {
                if (!_formsOfUse.Contains(medicationProduct.PharmaceuticalForm) && !string.IsNullOrWhiteSpace(medicationProduct.PharmaceuticalForm))
                    _formsOfUse.Add(medicationProduct.PharmaceuticalForm);

                if (!_strengths.Contains(medicationProduct.Strength) && !string.IsNullOrWhiteSpace(medicationProduct.Strength))
                    _strengths.Add(medicationProduct.Strength);
            }

            _formsOfUse = _formsOfUse.OrderBy(q => q).ToList();
            _formsOfUse.Insert(0, NotSet);

            _strengths = _strengths.OrderBy(c => c.Length).ThenBy(c => c).ToList();
            _strengths.Insert(0, NotSet);
        }

        private async Task<List<MedicationProduct>> GetMedicationProducts(string genericName, string pharmaceuticalForm, string strength) 
        {
            if (_medicationProductsCache == null)
                _medicationProductsCache = new Dictionary<string, List<MedicationProduct>>();
            var key = $"{genericName.ToLowerInvariant()};{pharmaceuticalForm.ToLowerInvariant()};{strength.ToLowerInvariant()}";

            if (_medicationProductsCache.ContainsKey(key))
                return _medicationProductsCache[key];
            else 
            {
                var medicationProducts = await Session.TamroGateway.GetAsync<List<MedicationProduct>>
                    ($"/api/v1/medicationproducts?{BuildQuery(genericName, pharmaceuticalForm, strength)}&Take=1000");
                _medicationProductsCache.Add(key, medicationProducts);
                return medicationProducts;
            }
        }

        public async Task<Generics> GetItemsForSecondary(RecipeDto recipeDto, 
            bool compensated,
            List<MedicationProduct> medicationProducts,
            decimal dosage,
            string priceClass)
        {
            PosRepository posRepository = new PosRepository();
            priceClass = priceClass ?? Session.PriceClass;
            Generics im = new Generics
            {
                ItemsAllList = new List<GenericItem>()
            };

            List<GenericItem> selectedItems = new List<GenericItem>();

            foreach (var medicationProduct in medicationProducts)
            {
                if (medicationProduct.MedicationPackage == null || medicationProduct.MedicationPackage.Count == 0)
                    continue;

                foreach (var medicationPackage in medicationProduct.MedicationPackage)
                {
                    var mapping = medicationPackage.MedicationPackageMappings?.FirstOrDefault(e => e.Company == Session.ParentCompanyCode);

                    //if (mapping == null)                     
                    //    mapping = await Session.SamasUtils.GetMedicationPackageMappingByMedicationPackageId(medicationPackage.MedicationPackageId, true);

                    if (mapping == null)
                        continue;

                    string rekname = compensated ? "C" : "U";
                    decimal compensationPrice = _tlkPriceDetails?.FirstOrDefault(e => e.Npakid7 == medicationPackage.Npakid7.ToString() && e.Percent == 100)?.Compensation ?? 0m;
                    if (compensated && compensationPrice == 0m)
                        continue;

                    int priority = _genericItemsAdditionalData?.FirstOrDefault(e => e.ProductId == mapping?.ItemCode)?.Priority ?? 0;
                    string productName = $"{medicationPackage.MedicationName} {medicationPackage.Numero} {medicationProduct.Strength} {medicationProduct.MarketingAuthorizationHolder}";
                    decimal retailPrice = _tlkPriceDetails?.FirstOrDefault(e => e.Npakid7 == medicationPackage.Npakid7.ToString())?.RetailPrice ?? 0m;

                    GenericItem genericItem = new GenericItem
                    {
                        SecondATCName = medicationProduct.ATCName,
                        NpakId = medicationPackage?.Npakid.ToString() ?? string.Empty,
                        NpakId7 = medicationPackage?.Npakid7 ?? 0m,
                        ShortName = productName,
                        ProductId = mapping?.ItemCode ?? -1,
                        MaxUnitPrice = retailPrice,
                        ItemAtcCode = recipeDto.AtcCode,
                        FormOfUse = medicationProduct.PharmaceuticalForm,
                        RetailPrice = retailPrice,
                        CompensatedPrice = compensationPrice,
                        VKBPrice100 = compensationPrice,
                        VKBPrice90 = compensationPrice * 0.9m,
                        VKBPrice80 = compensationPrice * 0.8m,
                        VKBPrice50 = compensationPrice * 0.5m,
                        Premium100 = retailPrice - compensationPrice,
                        Premium90 = retailPrice - (compensationPrice * 0.9m),
                        Premium80 = retailPrice - (compensationPrice * 0.8m),
                        Premium50 = retailPrice - (compensationPrice * 0.5m),
                        IsCheapest = Session.getParam("TLK", "CHEAPEST_OLD") == "1" ?
                        _tlkPriceDetails?.FirstOrDefault(e => e.Npakid7 == medicationPackage.Npakid7.ToString())?.IsCheapest ?? 0 :
                        Session.TLKCheapests.Where(val => val.StartDate <= DateTime.Now && val.Npakid7 == medicationPackage.Npakid7.ToString())
                        .OrderByDescending(val => val.PriceListVersion)
                        .Select(val => Convert.ToByte(val.IsCheapest))
                        .FirstOrDefault(),
                        RxPriority = priority,
                        CompensationName = rekname,
                        NotHasItemCard = mapping == null,
                        SupplyStatus = medicationPackage.SupplyStatus,
                        LowTherapeuticIndex = medicationProduct.LowTherapeuticIndex.HasValue ? medicationProduct.LowTherapeuticIndex.Value : false,
                        BarcodeRatio = Math.Round(dosage / await GetNumeroByProductId(mapping?.ItemCode ?? -1), 2)
                    };
                    selectedItems.Add(genericItem);
                }
            }

            if (compensated)
                await FillCompenstatedItemsData(selectedItems, dosage, priceClass);
            else
                await FillUncompenstatedItemsData(selectedItems, dosage);

            im.ItemsAllList.AddRange(selectedItems);
            return im;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Private methods

        private async Task<decimal> GetNumeroByProductId(decimal productId) 
        {
            PriceRepository priceRepository = new PriceRepository();
            decimal? ratio = await priceRepository.GetProductRatio(productId);
            return ratio.HasValue ? ratio.Value : 1;
        }

        private async Task PerformSeparateSubstances() 
        {
            if (!CanActivateSeparateActiveSubstances || Session.getParam("ERECIPE","V2") == "0")
                return;

            SeparateActiveSubstancesMode = true;
            _tags.IsPartialDispense = true;

            await _RefreshCommand.ExecuteAsync();
        }

        private async Task ScanItem()
        {
            if (!CanScanItem) 
                return;

            DialogResult dialogResult = DialogResult.None; 
            await ExecuteWithWaitAsync(async () =>
            {
                var vm = new SubmitBarcode()
                {
                    ProductName = "Skenuokite prekę",
                    SecondScreenScan = true
                };
                var wpf = new View.SubmitBarcode()
                {
                    DataContext = vm
                };
                using (var d = new Popups.wpf_dlg(wpf, "Prekės pasirinkimas"))
                {
                    d.Location = helpers.middleScreen2(d, true);
                    dialogResult = d.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        var barcodeModel = new Models.Barcode() { BarcodeStr = vm.Barcode };
                        Presenters.BarcodePresenter BCPresenter = new Presenters.BarcodePresenter(null, barcodeModel);
                        await BCPresenter.GetDataFromBarcode();

                        if (barcodeModel.BarcodeID == 0)
                        {
                            throw new Exception($"'{barcodeModel.BarcodeStr}' barkodas nerastas");
                        }


                        if (Session.ExclusiveProducts.ContainsKey(barcodeModel.ProductId))
                        {
                            gvPricesSelectedRow = new GenericItem() { BarcodeModel = barcodeModel };
                            CloseEvent(DialogResult.OK);
                            return;
                        }

                        try 
                        {
                            var medicationProduct = await GetMedicationProductByProductId(barcodeModel.ProductId);
                            _formOfUse = medicationProduct.PharmaceuticalForm;
                            _strength = medicationProduct.Strength;
                            GenericName = medicationProduct.ActiveSubstances;
                            ProductId = barcodeModel.ProductId;
                        } 
                        catch (Exception ex) 
                        {
                            gvPricesSelectedRow = new GenericItem() { BarcodeModel = barcodeModel };
                            CloseEvent(DialogResult.OK);
                        }
                    }
                }
            });

            if (dialogResult == DialogResult.OK)
                await RefreshCommand.ExecuteAsync();
        }

        private async Task<MedicationProduct> GetMedicationProductByProductId(decimal productId) 
        {
            var medicationPackageMapping = await Session.SamasUtils.GetMedicationPackageMappingByProductId(productId);

            if (medicationPackageMapping == null)
            {
                var message = $"Medikamento pakuotės surišimas, kurios produkto ID: '{productId}' nerastas!\n" +
                $" Jeigu tai vardinis arba ekstemporalus vaistas tada recepto tipas privalo būti 'vv' arba 'ev'.\n" +
                $" Aptarnaujamo recepto tipas yra: '{RecipeDto?.Type}'";
                Serilogger.GetLogger().Error(message);
                throw new Exception(message);
            }

            var medicationPackage = await Session.TamroGateway.GetAsync<MedicationPackage>
            ($"/api/v1/medicationpackages/{medicationPackageMapping.MedicationPackageId}");

            if (medicationPackage == null || medicationPackage.MedicationProductId == null)
            {
                var message = $"Medikamento pakuotė ID: '{medicationPackageMapping.MedicationPackageId}' nerasta!";
                Serilogger.GetLogger().Error(message);
                throw new Exception(message);
            }

            var medicationProduct = await Session.TamroGateway.GetAsync<MedicationProduct>
            ($"/api/v1/medicationproducts/{medicationPackage.MedicationProductId}");

            if (medicationProduct == null || medicationProduct.MedicationProductId == null)
            {
                var message = $"Medikamento produktas ID: '{medicationPackage.MedicationProductId}' nerastas!";
                Serilogger.GetLogger().Error(message);
                throw new Exception(message);
            }
            return medicationProduct;
        }

        private async Task InitializeAsync()
        {
            var medicationProducts = await GetMedicationProducts(GenericName, _formOfUse, _strength);
            var listOfProductIds = medicationProducts?
                    .SelectMany(e => e.MedicationPackage)
                    .SelectMany(e => e.MedicationPackageMappings)
                    .Where(e => e.Company == Session.ParentCompanyCode)
                    .Select(e => e.ItemCode) ?? new List<long>();

            var listOfNpakid7 = medicationProducts?
                    .SelectMany(e => e.MedicationPackage)
                    .Select(e => e.Npakid7.ToString()) ?? new List<string>();

            PosRepository posRepository = new PosRepository();
            _genericItemsAdditionalData = await posRepository.GetGenericItemDataByProductIds(listOfProductIds.ToList());
            _tlkPriceDetails = await posRepository.GetTLKPricesByNpakid7List(listOfNpakid7.ToList());

            compItems = await GetItemsForSecondary(RecipeDto, true, medicationProducts, Dosage, PriceClass);
            unCompItems = await GetItemsForSecondary(RecipeDto, false, medicationProducts, Dosage, PriceClass);

            CanActivateSeparateActiveSubstances = CheckIfCanActivateSeparateActiveSubstances(
                model.Percentage == 0 ?
                unCompItems.ItemsAllList :
                compItems.ItemsAllList);

            if (model.Percentage == 0)            
                ShowInfoForPrice(new Model.display2.PricesUnComp() { Percentage = model.Percentage });            
            else            
                ShowInfoForPrice(new Model.display2.PricesComp() { Percentage = model.Percentage });
            
        }

        private async Task InitializeByBarcodeAsync(Models.Barcode barcode)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await Task.Run(async () =>
                {
                    var medicationProduct = await GetMedicationProductByProductId(barcode.ProductId);
                    ProductId = barcode.ProductId;
                    GenericName = medicationProduct.ActiveSubstances;
                    await CollectStrengthsAndFormsOfUseByATC(GenericName);
                    SetFormOfUse(medicationProduct.PharmaceuticalForm);
                    SetStrength(medicationProduct.Strength);
                    await InitializeAsync();
                });
            });
        }


        private async Task InitializeByRecipeAsync()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await Task.Run(async () =>
                {
                    if (string.IsNullOrWhiteSpace(GenericName)) return;
                    await CollectStrengthsAndFormsOfUseByATC(GenericName);
                    SetFormOfUse(string.IsNullOrWhiteSpace(_formOfUse) ? RecipeDto?.PharmaceuticalForm ?? string.Empty : _formOfUse);
                    SetStrength(string.IsNullOrWhiteSpace(_strength) ? RecipeDto?.Strength ?? string.Empty : _strength);
                    await InitializeAsync();
                });
            });
        }

        private bool CheckIfCanActivateSeparateActiveSubstances(List<GenericItem> items)
        {
            if (!string.IsNullOrEmpty(RecipeDto.ProprietaryName) || items.Count == 0)
                return false;

            return items.All(item => 
                item.SupplyStatus == SupplyStatusSuspended ||
                item.SupplyStatus == SupplyStatusNotStartedToSupply ||
                item.SupplyStatus == SupplyStatusTemporarySupplyDisruption ||
                item.SupplyStatus == SupplyStatusSuppliedForeignLanguage ||
                item.SupplyStatus == SupplyStatusNotSupplied);
        }

        private void SetFormOfUse(string formOfUse)
        {
            if (_formsOfUse == null || string.IsNullOrWhiteSpace(formOfUse))
                _formOfUse = NotSet;
            else
                _formOfUse = _formsOfUse.Contains(formOfUse) ? formOfUse : NotSet;
        }

        private void SetStrength(string strength)
        {
            if (_strengths == null || string.IsNullOrWhiteSpace(strength))
                _strength = NotSet;
            else
                _strength = _strengths.Contains(strength) ? strength : NotSet;
        }

        private int ExtractCompensationPercent(string value)
        {
            var result = Regex.Match(value, @"\d+").Value;
            int percent;
            int.TryParse(result, out percent);
            return percent;
        }

        private int ExtractDosageAmount(RecipeDto recipeDto)
        {
            decimal.TryParse(string.IsNullOrWhiteSpace(recipeDto?.QuantityValue) ? "1" :
                recipeDto?.QuantityValue,
                out decimal dosageAmount);

            decimal.TryParse(string.IsNullOrWhiteSpace(recipeDto?.NumberOfRepeatsAllowed) ? "1" :
                recipeDto?.NumberOfRepeatsAllowed,
                out decimal numberOfRepeats);

            dosageAmount = dosageAmount < 1 ? 1 : dosageAmount;
            numberOfRepeats = numberOfRepeats < 1 ? 1 : numberOfRepeats;

            return (int)(dosageAmount/ numberOfRepeats);
        }

        private void SetProductLocations(GenericItem genericItem) 
        {
            if (genericItem == null) return;
            if (Session.ProductLocations.ContainsKey(genericItem.ProductId))
            {
                var pl = Session.ProductLocations[genericItem.ProductId];
                genericItem.OficinaLocation = pl.Oficina;
                genericItem.Oficina2Location = pl.Oficina2;
                genericItem.StockLocation = pl.Stock;
            }
        }

        private async Task Loaded()
        {
            if (string.IsNullOrWhiteSpace(GenericName))
            {
                await Dispatcher.CurrentDispatcher.BeginInvoke(new Action(async () =>
                {
                    await ScanItem();
                }), DispatcherPriority.Background);
            }
        }

        private async Task AssignTamroQty(List<GenericItem> genericItems) 
        {
            if (Session.HomeMode)
            {
                try
                {
                    var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                    var distinctProductIds = genericItems
                                               .Select(e => e.ProductId.ToString())
                                               .Distinct()
                                               .ToList();

                    var result = await tamroClient.GetAsync<List<StockViewModel>>(
                         string.Format(Session.TransactionV3GetLTCountryStocks,
                                       helpers.BuildQueryString("LocalItemCodes=", distinctProductIds) +
                                       "&StockType=WholeSale"));


                    List<StockViewModel> stockList = result;
                    var stockDictionary = stockList.ToDictionary(stock => stock.ItemRetailCode, stock => stock);
                    decimal.TryParse(Session.getParam("HOMEMODE", "BUFFERSIZE"), out decimal bufferSize);

                    foreach (var genericItem in genericItems)
                    {
                        var productId = genericItem.ProductId.ToString();

                        if (stockDictionary.TryGetValue(productId, out var stockModel))
                        {
                            var tamroQty = (stockModel.Wholesale?.FirstOrDefault()?.TotalQty ?? 0) - (double)bufferSize;
                            genericItem.CurrentTamroQty = tamroQty > 0 ? tamroQty.ToDecimal() : 0m;
                        }
                    }
                }
                catch (Exception ex) 
                {
                    Serilogger.GetLogger().Error($"[AssignTamroQty] Error: {ex.Message}");
                }
            }
        }

        private void ValidateClosing(object Result) 
        {
            if (Result != null &&(System.Windows.Forms.DialogResult)Result == DialogResult.OK && SeparateActiveSubstancesMode && _gvPricesSelectedRow.LowTherapeuticIndex) 
            {
                throw new Exception("Mažo terapinio indekso vaistas negali būti parduodamas atskiromis veikliosiomis medžiagomis");
            }
        }
        #endregion

        #region Override
        protected override void CloseEvent(object Result)
        {
            ValidateClosing(Result);
            base.CloseEvent(Result);
        }

        #endregion

        #region Properties
        public RecipeDto RecipeDto { get; set; }

        private Model.display2.Prices _model = new Model.display2.Prices();
        public Model.display2.Prices model
        {
            get
            {
                return _model;
            }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        private List<GenericItem> _gvPricesItemsSource;
        public List<GenericItem> gvPricesItemsSource
        {
            get
            {
                if (_gvPricesItemsSource == null)
                    return null;
                _gvPricesItemsSource.ForEach(f => f.qtyVKBPrice = model.Percentage == 100 ? f.qtyVKBPrice100 :
                                           model.Percentage == 90 ? f.qtyVKBPrice90 :
                                           model.Percentage == 80 ? f.qtyVKBPrice80 :
                                           model.Percentage == 50 ? f.qtyVKBPrice50 :
                                           0);
                _gvPricesItemsSource.ForEach(f => f.CompensatedPrice = model.Percentage == 100 ? f.PrepComp100 :
                                           model.Percentage == 90 ? f.PrepComp90 :
                                           model.Percentage == 80 ? f.PrepComp80 :
                                           model.Percentage == 50 ? f.PrepComp50 :
                                           f.CompensatedPrice);
                return (from el in _gvPricesItemsSource
                        orderby el.CompensationName == "C" ? el.IsCheapest : 0 descending, el.CompensatedPrice, el.ShortName
                        select el).ToList();
            }
            set
            {
                SetProperty(ref _gvPricesItemsSource, value, null, true);
                NotifyPropertyChanged("alternationCount");
            }
        }

        private GenericItem _gvPricesSelectedRow;
        public GenericItem gvPricesSelectedRow
        {
            get
            {
                return _gvPricesSelectedRow;
            }
            set
            {
                SetProperty(ref _gvPricesSelectedRow, value);
            }
        }

        public bool IsSelectedCompensated
        {
            get
            {
                return gvPricesSelectedRow?.CompensatedPrice != 0;
            }
        }

        public bool gvPricesSelectedCheapest
        {
            get
            {
                return gvPricesSelectedRow?.IsCheapest == 1;
            }
        }

        public decimal SelectedProductId
        {
            get
            {
                return gvPricesSelectedRow?.ProductId ?? -1;
            }
        }

        private int _gvPricesSelectedIndex;
        public int gvPricesSelectedIndex
        {
            get
            {
                return _gvPricesSelectedIndex;
            }
            set
            {
                SetProperty(ref _gvPricesSelectedIndex, value);
            }
        }

        public Generics compItems { get; set; }
        public Generics unCompItems { get; set; }

        public string NpakId { get; set; }
        private decimal _dosage;
        public decimal Dosage 
        {
            get { return _dosage; }
            set 
            {
                _dosage = value;
                NotifyPropertyChanged(nameof(Dosage));
            }
        }

        private string _strength;
        public string Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                NotifyPropertyChanged(nameof(Strength));
                RefreshCommand.Execute(null);
            }
        }

        private string _formOfUse;
        public string FormOfUse
        {
            get { return _formOfUse; }
            set
            {
                _formOfUse = value;
                NotifyPropertyChanged(nameof(FormOfUse));
                RefreshCommand.Execute(null);
            }
        }

        public bool CanScanItem { get; set; }
        public bool CanEditFormOfUse { get; set; }
        public bool CanEditStrength { get; set; }
        public bool SeparateActiveSubstancesMode { get; set; }
        public bool CanActivateSeparateActiveSubstances { get; set; }

        public List<string> PharmaceuticalForms 
        {
            get { return _formsOfUse ?? new List<string>(); }
        }

        public List<string> Strengths
        {
            get { return _strengths ?? new List<string>(); }
        }

        private string _genericName;
        public string GenericName
        {
            get { return _genericName; }
            set 
            { 
                _genericName = value;
                NotifyPropertyChanged(nameof(GenericName));
                RefreshCommand.Execute(null);
            } 
        }

        public decimal ProductId { get; private set; }
        public bool FirstPrescription { get; set; }
        public string FirstPrescriptionReason { get; set; }
        public bool FirstPrescriptionUncomp { get; set; }
        public int DefaultPrice { get; set; }
        private string PriceClass { get; set; }
        public Task Initialization { get; private set; }
        private int _Timer;
        public int Timer
        {
            get
            {
                return _Timer;
            }
            set
            {
                SetProperty(ref _Timer, value);
            }
        }
        #endregion
    }
}

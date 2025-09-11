using Microsoft.Extensions.Configuration;
using Minio;
using POS_display.Models.CRM;
using POS_display.Models.FeedbackTerminal;
using POS_display.Models.NBO;
using POS_display.Models.PersonalPharmacist;
using POS_display.Models.Pos;
using POS_display.Models.Recipe;
using POS_display.Models.Recipe.Classifiers;
using POS_display.Models.TLK;
using POS_display.Properties;
using POS_display.Utils.CRM;
using POS_display.Utils.EHealth;
using POS_display.Utils.Signature;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Reflection;
using TamroUtilities.HL7.Models;
using TamroUtilities.MinIO;

namespace POS_display
{
    public abstract class Session
    {
        #region Items
        public const int IDLength = 11;
        public static string PriceClass { get; set; }
        public static bool CRM { get; set; }
        public static List<PromotionCheque> PromChequeLines { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string LocalIP { get; set; }
        public static string ServerIP { get; set; }
        public static string Port { get; set; }
        public static string Database { get; set; }
        public static decimal DBId { get; set; }
        public static string DatabaseName { get; set; }
        public static string Path { get; set; }
        public static string PHPIncludePath { get; set; }
        public static string ImagePath { get; set; }
        public static string eRecipe_CssPath { get; set; }
        public static string eRecipe_LogoPath { get; set; }
        public static int eRecipeGridSize { get; set; }
        public static int VaccineGridSize { get { return getParam("VACCINE", "GRIDSIZE").ToInt(); } }
        public static bool Develop { get; set; }
        public static bool Admin { get; set; }
        public static int MonitorCount { get; set; }
        public static bool display2checked { get; set; }
        public static bool RecipesOnly { get; set; }
        public static string SqliteDatabase { get { return getParam("SQLITE", "PATH"); } }

        public static bool IsVerticalDisplay2 { get; set; }

        public static bool PaymentInProgress { get; set; } = false;

        public static bool HomeMode { get; set; } = false;
        public static bool WoltMode { get; set; } = false;

        public static readonly Dictionary<string, string> VaccinationInfectiousDiseases = new Dictionary<string, string>()
        {
            { "Erkinis encefalitas" , "4" },
            { "Gripas" , "7" },
            { "COVID-19" , "87" },
            { "Pneumokokinė infekcija" , "14" }
        };

        public static Dictionary<decimal, decimal> ExclusiveProducts { get; set; } = new Dictionary<decimal, decimal>();
        public static List<TLKCheapest> TLKCheapests { get; set; } = new List<TLKCheapest>();

        public static List<php_config> PHPconfig { get; set; }
        public static List<Items.eRecipe.Encounter> eRecipeEncounterList { get; set; } = new List<Items.eRecipe.Encounter>();
        public static List<Items.Params> Params { get; set; }
        public static string getParam(string system, string par)
        {
            return Params?.FirstOrDefault(p => p.system == system && p.par == par)?.value ?? "";
        }
        public static bool IsRoundingEnabled
        {
            get
            {
                string startDateString = getParam("PAYMENT_ROUNDING", "STARTDATE");
                if (string.IsNullOrEmpty(startDateString))
                {
                    return false;
                }

                if (DateTime.TryParse(startDateString, out DateTime startDate))
                {
                    return DateTime.Now >= startDate;
                }
                return false;
            }
        }
        public static Dictionary<decimal, Enumerator.ProductFlag> ProductFlags { get; set; } = new Dictionary<decimal, Enumerator.ProductFlag>();
        public static List<Items.Error> POSErrors { get; set; }
        public static List<Items.Sticker> Stickers { get; set; }
        public static List<byte[]> ImagesAd { get; set; }
        public static List<byte[]> ImagesVerticalAd { get; set; }
        public static List<byte[]> ImagesPOS { get; set; }
        public static List<byte[]> ImagesAnimation { get; set; }
        public static FeedbackTerminalMapping FeedbackTerminalMapping { get; set; }
        public static string TamroGatewayBaseAddress { get; set; }
        public static string POSLogPath { get; set; }
        public static string FeedbackTerminalAPI
        {
            get
            {
                if (Session.Develop == true)
                    return Settings.Default.FeedbackTerminalAPI_TEST;
                else
                    return Settings.Default.FeedbackTerminalAPI;
            }
        }
        public static string AssemblyVersion
        {
            get
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                    return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                else
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private static Items.User _user = null;
        public static Items.User User
        {
            get
            {
                if (null == _user)
                    _user = new Items.User();
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private static Items.RecipeParm _recipeParm = null;
        public static Items.RecipeParm RecipeParm
        {
            get
            {
                if (null == _recipeParm)
                    _recipeParm = new Items.RecipeParm();
                return _recipeParm;
            }
            set
            {
                _recipeParm = value;
            }
        }

        private static Items.SystemData _systemData = null;
        public static Items.SystemData SystemData
        {
            get
            {
                if (null == _systemData)
                    _systemData = new Items.SystemData();
                return _systemData;
            }
            set
            {
                _systemData = value;
            }
        }

        private static Items.Devices _devices = null;
        public static Items.Devices Devices
        {
            get
            {
                if (null == _devices)
                    _devices = new Items.Devices();
                return _devices;
            }
            set
            {
                _devices = value;
            }
        }

        public static PractitionerDto ExtendedPracticePractitioner
        {
            get
            {
                return _allPractitionerItems?
                    .Where(e => e.Qualification.Any(val => val.QualificationCode == "532107" && val.LicenseStatusCode == "VALID"))?
                    .FirstOrDefault();
            }
        }

        private static List<PractitionerDto> _allPractitionerItems = null;
        public static List<PractitionerDto> AllPractitionerItems
        {
            get => _allPractitionerItems;
            set => _allPractitionerItems = value;
        }

        private static PractitionerDto _practitionerItem = null;
        public static PractitionerDto PractitionerItem
        {
            get
            {
                if (null == _practitionerItem)
                    _practitionerItem = new PractitionerDto();
                return _practitionerItem;
            }
            set
            {
                _practitionerItem = value;
            }
        }

        public static OrganizationDto OrganizationItem { get; set; }

        private static FMD.Model.Logins FMDlogin
        {
            get
            {
                string test = Session.Develop ? "_TEST" : "";
                return new FMD.Model.Logins()
                {
                    EndpointId = Session.SystemData.kas_client_id.ToString(),
                    HubUrl = Session.getParam($"FMD{test}", "HUB_URL"),
                    HubAuthorizationUrl = Session.getParam($"FMD{test}", "AUTHORIZATION_URL"),
                    ClientId = Session.getParam($"FMD{test}", "CLIENT_ID"),
                    ClientSecret = Session.getParam($"FMD{test}", "CLIENT_SECRET"),
                    HubLanguage = Session.getParam($"FMD{test}", "HUB_LANGUAGE"),
                    HubVersion = Session.getParam($"FMD{test}", "VERSION"),
                    UserAgent = Session.getParam($"FMD{test}", "USERAGENT"),
                };
            }
        }
        private static FMD.HUB.IHUB _FMDclient;
        public static FMD.HUB.IHUB FMDclient
        {
            get
            {
                if (_FMDclient == null)
                    _FMDclient = new FMD.HUB.LT(FMDlogin);
                return _FMDclient;
            }
        }

        public static PersonalPharmacistData PersonalPharmacistData { get; set; }


        private static NBORecommendationSession _nboSession = null;
        public static NBORecommendationSession NBOSession
        {
            get
            {
                if (null == _nboSession)
                    _nboSession = new NBORecommendationSession();
                return _nboSession;
            }
            set
            {
                _nboSession = value;
            }
        }

        public static List<string> ActiveSubstances { get; set; } = new List<string>();
        public static List<string> MedicationNames { get; set; } = new List<string>();
        public static List<long> WoltProducts { get; set; } = new List<long>();

        public static decimal FifoMode 
        {
             get { return HomeMode ? (decimal)Enumerator.FifoMode.NotReserveQty : Devices.fifomode; }
        }
        #endregion

        #region Utils
        private static Utils.POSUtils _fp550 = null;
        private static IEHealthUtils _eRecipeUtils = null;
        private static Utils.FileUtils _fileUtils = null;
        private static Parser2D.Parser _barcodeParser = null;
        private static Utils.FeedbackTerminal _feedbackTerminal = null;
        private static Utils.TamroGateway _tamroGateway = null;
        private static Utils.EShopGateway _eshopGateway = null;
        private static Utils.BalticPosGateway _balticPosGateway = null;
        private static Utils.NBOUtils _nboUtils = null;
        private static Utils.RemotePharmacyGateway _remotePharmacyGateway;
        private static MinioFileStorage _minioFileStorage = null;
        private static CRMRestUtils _CRMRestUtils = null;
        private static SignatureParams _signatureParams;
        private static Utils.SAMAS.SamasUtils _samasUtils = null;

        public static Utils.POSUtils FP550
        {
            get
            {
                if (null == _fp550)
                    _fp550 = new Utils.POSUtils();
                return _fp550;
            }
        }
        
        public static IEHealthUtils eRecipeUtils
        {
            get
            {
                if (null == _eRecipeUtils)
                    _eRecipeUtils = new EHealthUtils();                

                return _eRecipeUtils;
            }
        }

        public static SignatureParams SignatureParams
        {
            get
            {
                if (null == _signatureParams)
                    _signatureParams = new SignatureParams();

                return _signatureParams;
            }
        }

        public static Utils.FileUtils FileUtils
        {
            get
            {
                if (null == _fileUtils)
                    _fileUtils = new Utils.FileUtils();

                return _fileUtils;
            }
        }

        public static CRMRestUtils CRMRestUtils
        {
            get
            {
                string test = Session.Develop ? "_TEST" : "";
                if (null == _CRMRestUtils)
                    _CRMRestUtils = new CRMRestUtils(
                        Session.Params.FirstOrDefault(x => x.system == $"CRM_REST{test}" && x.par == "ENDPOINT").value,
                        Session.Params.FirstOrDefault(x => x.system == $"CRM_REST{test}" && x.par == "USERNAME").value,
                        Session.Params.FirstOrDefault(x => x.system == $"CRM_REST{test}" && x.par == "PASSWORD").value,
                        Session.Params.FirstOrDefault(x => x.system == $"CRM_REST{test}" && x.par == "VERSION").value
                        );

                return _CRMRestUtils;
            }
        }

        public static Parser2D.Parser BarcodeParser
        {
            get
            {
                if (null == _barcodeParser)
                    _barcodeParser = new Parser2D.Parser();

                return _barcodeParser;
            }
        }

        public static Utils.IKVAP KVAP
        {
            get
            {
                return new Utils.KVAPUtils();
            }
        }

        public static Utils.FeedbackTerminal FeedbackTerminal
        {
            get
            {
                if (null == _feedbackTerminal)
                    _feedbackTerminal = new Utils.FeedbackTerminal();

                return _feedbackTerminal;
            }
        }

        public static Utils.TamroGateway TamroGateway
        {
            get
            {
                if (null == _tamroGateway)
                    _tamroGateway = new Utils.TamroGateway();

                return _tamroGateway;
            }
        }

        public static Utils.EShopGateway EShopGateway
        {
            get
            {
                if (null == _eshopGateway)
                    _eshopGateway = new Utils.EShopGateway();

                return _eshopGateway;
            }
        }

        public static Utils.BalticPosGateway BalticPosGateway
        {
            get
            {
                if (null == _balticPosGateway)
                    _balticPosGateway = new Utils.BalticPosGateway();

                return _balticPosGateway;
            }
        }

        public static Utils.SAMAS.SamasUtils SamasUtils
        {
            get
            {
                if (null == _samasUtils)
                    _samasUtils = new Utils.SAMAS.SamasUtils();

                return _samasUtils;
            }
        }

        public static Utils.NBOUtils NBOUtils =>_nboUtils ?? (_nboUtils = new Utils.NBOUtils());

        public static Utils.RemotePharmacyGateway RemotePharmacyGateway => _remotePharmacyGateway ??
                                                                           (_remotePharmacyGateway = new Utils.RemotePharmacyGateway());

        public static MinioFileStorage MinioStorage
        {
            get
            {
                if (_minioFileStorage != null)
                    return _minioFileStorage;

                string test = Session.Develop ? "_TEST" : "";
                var jsonConfig = new
                {
                    Minio = new
                    {
                        BucketName = Params.FirstOrDefault(x => x.system == $"MINIO{test}" && x.par == "BUCKETNAME").value
                    }
                };

                IConfigurationBuilder builder = new ConfigurationBuilder().
                    AddJsonStream(helpers.StringToStream(jsonConfig.ToJsonString()));

                var endPoint = $"{Params.FirstOrDefault(x => x.system == $"MINIO{test}" && x.par == "ENDPOINT")?.value ?? Session.ServerIP}:" +
                               $"{Params.FirstOrDefault(x => x.system == $"MINIO{test}" && x.par == "PORT").value}";
                var accessKey = Params.FirstOrDefault(x => x.system == $"MINIO{test}" && x.par == "ACCESSKEY").value;
                var secretKey = Params.FirstOrDefault(x => x.system == $"MINIO{test}" && x.par == "SECRETKEY").value;

                _minioFileStorage = new MinioFileStorage(new Minio.MinioClientFactory(client =>
                client
                .WithEndpoint(endPoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .WithRegion("LT")), builder.Build());

                return _minioFileStorage;
            }
        }

        private static List<Models.Recipe.VaccineDays> _vaccineRemindersConfig = null;
        public static List<Models.Recipe.VaccineDays> VaccineRemindersConfig
        {
            get
            {
                if (null == _vaccineRemindersConfig)
                    _vaccineRemindersConfig = new List<Models.Recipe.VaccineDays>();
                return _vaccineRemindersConfig;
            }
            set
            {
                _vaccineRemindersConfig = value;
            }
        }

        public static Dictionary<long, List<CampaignItemsList>> CRMCampaginsCache { get; set; } = new Dictionary<long, List<CampaignItemsList>>();

        public static List<GroupDispenseRequest> GruopDispenseRequests = new List<GroupDispenseRequest>();

        public static string ParentCompanyCode 
        {
            get
            {
                if (_systemData.ecode == "303364097" || _systemData.ecode == "124807124" || _systemData.ecode == "307071729")
                    return "135874035";
                else
                    return _systemData.ecode;
            }
        }

        public static Dictionary<decimal, ProductLocation> ProductLocations { get; set; } = new Dictionary<decimal, ProductLocation>();
        #endregion

        #region WebService Endpoints
        public static string CKasV1GetPresentCard = "/api/v1/ckas/presentcard?{0}";

        public static string CKasV1PostPresentCardIssuer = "/api/v1/ckas/presentCard/{0}/issuer";

        public static string CKasV1PostPresentCardSeller = "/api/v1/ckas/presentCard/{0}/seller";

        public static string CKasV1DeletePresentCardIssuer = "/api/v1/ckas/presentCard/{0}/issuer";

        public static string CKasV1DeletePresentCardSeller = "/api/v1/ckas/presentCard/{0}/seller";

        public static string TransactionV1GetMedicationProductsActiveSubstance = "api/v1/MedicationProducts/ActiveSubstance";

        public static string TransactionV1GetMedicationProductsMedicationName = "api/v1/MedicationProducts/MedicationName";

        public static string ItemV1GetSalesChannelsItemBindings = "api/v1/salesChannels/itemBindings?Country={0}&SalesChannelName={1}";

        public static string CKasV1PatchPartnerUpdate= "api/v1/ckas/updatepartner";

        public static string CKasV1PostPartnerInsert = "api/v1/ckas/insertpartner";

        public static string CKasV1PostPartnerSetAgreement = "api/v1/ckas/setagreement";

        public static string CKasV1PostPartnerUploadSignature = "api/v1/ckas/uploadsignature";

        public static string CKasV1PostPharmacyMarkCommand = "api/v1/ckas/markcommand";

        public static string TransactionV3GetLTCountryStocks = "api/v3/countries/LT/stocks?{0}";

        public static string E1GatewayV1PostOrders = "api/v1/orders";

        public static string DeliveryV1PutShipments = "api/v1/shipments";

        public static string TransactionV1GetMedicationPackageMappingsByItemCode = "/api/v1/medicationpackagesmappings?ItemCode={0}&Company={1}&IsDeleted={2}&Take=1";

        public static string TransactionV1GetMedicationPackageMappingsByMedicationPackageId = "/api/v1/medicationpackagesmappings?MedicationPackageId={0}&Company={1}&IsDeleted={2}&Take=1";

        public static string TransactionV1GetMedicationPackages = "/api/v1/medicationpackages/{0}";

        public static string CKasV1GetVlkPricesCheckCompensation = "/api/v1/ckas/Vlk/Prices?Npakid7={0}&CompensationCode={1}";

        public static string CKasV1GetDiseases = "/api/v1/ckas/Vlk/Diseases?Npakid7={0}&DiseaseCode={1}";

        public static string CKasV1GetVlkBarcodes = "/api/v1/ckas/Vlk/Barcodes?Npakid7={0}";
        #endregion

        #region E-Health Classfiers
        public static List<PharmaceuticalFormMeasureUnitClassifier> PharmaceuticalFormMeasureUnitClassifiers { get; set; } = new List<PharmaceuticalFormMeasureUnitClassifier>();
        public static List<RouteClassifier> RouteClassifiers { get; set; } = new List<RouteClassifier>();
        public static List<PharmaceuticalFormClassifier> PharmaceuticalFormClassifiers { get; set; } = new List<PharmaceuticalFormClassifier>();
        public static List<TLK10AMClassifier> TLK10AMClassifiers { get; set; } = new List<TLK10AMClassifier>();
        public static List<CompensationTypeClassifier> CompensationTypeClassifiers { get; set; } = new List<CompensationTypeClassifier>();
        #endregion
    }
}

using Dapper;
using Dapper.ColumnMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POS_display.Models.ECRReports;
using POS_display.Models.Loyalty;
using POS_display.Models.Pos;
using POS_display.Models.Recipe;
using POS_display.Models.Recipe.Classifiers;
using POS_display.Profiles;
using POS_display.Properties;
using POS_display.Repository.Pos;
using POS_display.Utils.Email;
using POS_display.Utils.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;
using TamroUtilities.HL7.Fhir.Core.Extension;
using TamroUtilities.HL7.Gateway.Builders;
using TamroUtilities.HL7.Gateway.Clients;
using TamroUtilities.HL7.Gateway.Clients.HttpBaseClient;
using TamroUtilities.HL7.Gateway.Gateways;
using TamroUtilities.HL7.Gateway.Profiles;
using TamroUtilities.HL7.Gateway.Profiles.Converters;
using TamroUtilities.HL7.Gateway.Services;
using TamroUtilities.HL7.Gateway.Validators;
using TamroUtilities.HL7.Models;

namespace POS_display
{
    static class Program
    {
        private static Display1View _display1 = null;
        private static Display2View _display2 = null;
        private static Views.ERecipe.IERecipe _erecipe = null;
        private static int MaxDisplayCount = 2;
        #region Threads
        public static async Task TaskAsync()
        {
            if (Session.RecipesOnly == false)
            {
                Session.PromChequeLines = await new PosRepository().GetPromotionCheques();
                Session.Stickers = await DB.POS.getStickers<Items.Sticker>();
            }
            if (Session.eRecipeUtils.Init() && Session.User.StampId != "")
            {
                if (Session.User.Stamp == "")
                {
                    WR_KVAP.VAISTININKAI pharmacist_data = Session.KVAP.GetPharmacists(Session.RecipeParm.tlk_id, Convert.ToDateTime("2000-01-01"), DateTime.Now);
                    WR_KVAP.VAISTININKAIROW current_user = pharmacist_data.ROW != null ? pharmacist_data.ROW.ToList().FirstOrDefault(x => x.ID == Session.User.StampId) : null;
                    if (current_user != null)
                    {
                        if (current_user.SPAUDO_NUMERIS != Session.User.Stamp)
                            await DB.POS.SaveStamp(Session.User.id, current_user.SPAUDO_NUMERIS);
                        Session.User.Stamp = current_user.SPAUDO_NUMERIS;
                    }
                }
                Session.OrganizationItem = Session.eRecipeUtils.GetOrganization<OrganizationDto>(Session.RecipeParm.tlk_id);
                var practitionerListDto = Session.eRecipeUtils.GetPractitioner<PractitionerListDto>(Session.User.Stamp);
                Session.PractitionerItem = Session.Develop ? 
                    practitionerListDto.PractitionerList.FirstOrDefault(e => e.StampCode == Session.User.Stamp) :
                    practitionerListDto.PractitionerList.FirstOrDefault(e => e.StampCode == Session.User.Stamp && e.OrganizationSVEIDRAID != null);
                Session.eRecipeUtils.SetRequestorId(Session.PractitionerItem.PractitionerId);
                Session.AllPractitionerItems = Session.PractitionerItem != null ?
                    Session.eRecipeUtils.GetPractitioner<PractitionerListDto>(string.Empty,Session.PractitionerItem.PersonalCode).PractitionerList :
                    null;
                await LoadEHealthClassifiers();
            }
        }
        #endregion


        public static Display1View Display1
        {
            get
            {
                if (null == _display1)
                    _display1 = new Display1View();
                return _display1;
            }
        }

        public static Display2View Display2
        {
            get
            {
                if (null == _display2)
                    _display2 = new Display2View();
                return _display2;
            }
        }

        public static Views.ERecipe.IERecipe Erecipe
        {
            get
            {
                if (null == _erecipe)
                {
                    if (Session.getParam("ERECIPE", "V2") == "1")
                    {
                        _erecipe = new eRecipeV2();
                    }
                    else
                    {
                        _erecipe = new eRecipe();
                    }
                }
                return _erecipe;
            }
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var eHealthEndpoint = Session.getParam(Session.Develop ? "ERECIPE_TEST" : "ERECIPE", "URL"); 

                    services.AddAutoMapper(Assembly.GetAssembly(typeof(CRMProfile)));
                    services.AddAutoMapper(typeof(OrganizationProfile));
                    services.AddTransient(sp => new OrganizationConverter(new Uri(eHealthEndpoint)));
                    services.AddTransient(sp => new PractitionerConverter(new Uri(eHealthEndpoint)));
                    services.AddTransient<IEmailUtils>(x => new EmailUtils(
                        Session.getParam("EMAIL", "HOST"),
                        Session.getParam("EMAIL", "PORT").ToInt(),
                        Session.getParam("EMAIL", "USERNAME"),
                        Session.getParam("EMAIL", "PASSWORD")));

                    services.AddTransient<ITamroClient>(x => new Client(
                        Session.getParam(Session.Develop ? "TAMRO_TEST" : "TAMRO", "URL"),
                        Session.getParam(Session.Develop ? "TAMRO_TEST" : "TAMRO", "USER"),
                        Session.getParam(Session.Develop ? "TAMRO_TEST" : "TAMRO", "PASSWORD")));

                    ResourceManager rm = Resources.ResourceManager;
                    var PrivateKeyXml = rm.GetString("_" + Session.SystemData.ecode);

                    services.AddScoped<IERecipeFhirClient>(_ => new ERecipeFhirClient(eHealthEndpoint,
                        new ERecipeCredentials(Session.SystemData.ecode, PrivateKeyXml)));

                    services.AddHttpClient<DocumentClient>(DocumentClient.DocumentClientName, configuration =>
                    {
                        configuration.BaseAddress = new Uri($"{eHealthEndpoint}/");
                        configuration.Timeout = TimeSpan.FromMinutes(1);
                    }).AddHttpMessageHandler(x => x.GetService<AuthenticationDelegatingHandler>());

                    services.AddScoped<IEHealthClient, EHealthClient>();
                    services.AddScoped<IValidatorFactory>(_ => new ValidatorFactory(typeof(CreateDispenseRequestValidator).Assembly));
                    services.AddScoped<IPatientService, PatientService>();
                    services.AddScoped<IPractitionerService, PractitionerService>();
                    services.AddScoped<IDispenseService, DispenseService>();
                    services.AddScoped<IMedicationPrescriptionService, MedicationPrescriptionService>();
                    services.AddScoped<IEncounterService, EncounterService>();
                    services.AddScoped<IOrganizationService, OrganizationService>();
                    services.AddTransient<IDocumentsService, DocumentsService>();
                    services.AddScoped<IVaccinationService, VaccinationService>();
                    services.AddScoped<IMedicationService, MedicationService>();
                    services.AddScoped<IClassifierService, ClassifierService>();

                    services.AddScoped<IMedicationPrescriptionsClient, MedicationPrescriptionsClient>();
                    services.AddScoped<IPractitionerClient, PractitionerClient>();
                    services.AddScoped<IPatientClient, PatientClient>();
                    services.AddScoped<IOrganizationClient, OrganizationClient>();
                    services.AddScoped<IDispenseClient, DispenseClient>();
                    services.AddScoped<IEncounterClient, EncounterClient>();
                    services.AddScoped<IMedicationClient, MedicationClient>();
                    services.AddTransient<IDocumentClient, DocumentClient>();
                    services.AddScoped<IVaccinationClient, VaccinationClient>();
                    services.AddScoped<IClassifierClient, ClassifierClient>();

                    services.AddScoped<IPatientDtoBuilder, PatientDtoBuilder>();
                    services.AddScoped<IRecipeBuilder, RecipeBuilder>();
                    services.AddScoped<IEncounterRequestBuilder, EncounterRequestBuilder>();
                    services.AddTransient(provider =>
                    {
                        var logger = provider.GetService<ILogger<AuthenticationDelegatingHandler>>();
                        var ecode = Session.SystemData.ecode;
                        var privateKeyXml = rm.GetString("_" + ecode);
                        var practitionerId = Session.PractitionerItem?.PractitionerId ?? null;

                        return new AuthenticationDelegatingHandler(logger, ecode, privateKeyXml, practitionerId);
                    });
                });
        }

        static void LoadMappings()
        {
            SqlMapper.SetTypeMap(typeof(PaymentMethod), new ColumnTypeMapper(typeof(PaymentMethod)));
            SqlMapper.SetTypeMap(typeof(DeviceSettingValue), new ColumnTypeMapper(typeof(DeviceSettingValue)));
            SqlMapper.SetTypeMap(typeof(ZReportEntry), new ColumnTypeMapper(typeof(ZReportEntry)));
            SqlMapper.SetTypeMap(typeof(EKJEntry), new ColumnTypeMapper(typeof(EKJEntry)));
            SqlMapper.SetTypeMap(typeof(LoyaltyDetail), new ColumnTypeMapper(typeof(LoyaltyDetail)));
            SqlMapper.SetTypeMap(typeof(LoyaltyHeader), new ColumnTypeMapper(typeof(LoyaltyHeader)));
            SqlMapper.SetTypeMap(typeof(NewRecipeData), new ColumnTypeMapper(typeof(NewRecipeData)));
			SqlMapper.SetTypeMap(typeof(NotCompensatedRecipeData), new ColumnTypeMapper(typeof(NotCompensatedRecipeData)));
		}

        static async Task LoadEHealthClassifiers()
        {
            var tasks = new List<Task>
            {
                //Task.Run(() =>
                //{
                //    var pharmaceuticalFormItems = Session.eRecipeUtils.GetClassifier("pharmaceutical-form", string.Empty);
                //    foreach (var item in pharmaceuticalFormItems)
                //    {
                //        Session.PharmaceuticalFormClassifiers.Add(JsonConvert.DeserializeObject<PharmaceuticalFormClassifier>(item));
                //    }
                //}),

                Task.Run(() =>
                {
                    var pharmaceuticalFormMeasureUnitItems = Session.eRecipeUtils.GetClassifier("pharmaceutical-form-measure-unit", string.Empty);
                    foreach (var item in pharmaceuticalFormMeasureUnitItems)
                    {
                        Session.PharmaceuticalFormMeasureUnitClassifiers.Add(JsonConvert.DeserializeObject<PharmaceuticalFormMeasureUnitClassifier>(item));
                    }
                }),

                Task.Run(() =>
                {
                    var routeItems = Session.eRecipeUtils.GetClassifier("route", string.Empty);
                    foreach (var item in routeItems)
                    {
                        Session.RouteClassifiers.Add(JsonConvert.DeserializeObject<RouteClassifier>(item));
                    }
                }),

                Task.Run(() =>
                {
                    var tlk10amItems = Session.eRecipeUtils.GetClassifier("tlk-10-am/code", string.Empty);
                    foreach (var item in tlk10amItems)
                    {
                        Session.TLK10AMClassifiers.Add(JsonConvert.DeserializeObject<TLK10AMClassifier>(item));
                    }
                }),

                Task.Run(() =>
                {
                    var compensationTypes = Session.eRecipeUtils.GetClassifier("compensation-type", string.Empty);
                    foreach (var item in compensationTypes)
                    {
                        Session.CompensationTypeClassifiers.Add(JsonConvert.DeserializeObject<CompensationTypeClassifier>(item));
                    }
                })
            };

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Process[] GetPArry = Process.GetProcesses();
                foreach (Process testProcess in GetPArry)
                {
                    string ProcessName = testProcess.ProcessName;

                    ProcessName = ProcessName.ToLower();
                    if (ProcessName.CompareTo("display2") == 0)
                        testProcess.Kill();
                    if (ProcessName.CompareTo("pos") == 0 && testProcess.Id != Process.GetCurrentProcess().Id && !Session.RecipesOnly && !Session.Develop)
                        throw new Exception("Klaida! Programa jau paleista.");
                }

                DetectTypeOfSecondScreen();
                LoadConfig();

                using (login dlg = new login())
                {
                    dlg.Location = helpers.middleScreen2(dlg, false);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        ServicePointManager.SecurityProtocol = GetSecurityProtocolType();
                        ServicePointManager.Expect100Continue = Session.getParam("SYST", "EXPECT100") == "1";

                        var host = CreateHostBuilder().Build();
                        ServiceProvider = host.Services;
                        LoadMappings();

                        if (!Session.RecipesOnly)
                            Application.Run(Display1);
                        else
                            Application.Run((Form)Erecipe);
                    }
                }
            }
            catch (Exception e)
            {
                helpers.alert(Enumerator.alert.error, e.Message);
                Application.Exit();
            }
        }

        public static SecurityProtocolType GetSecurityProtocolType() 
        {
            SecurityProtocolType protocolType = SecurityProtocolType.Tls12;
            try 
            {
                string tlsVersion = Session.getParam("TLS", "VERSION");
                switch (tlsVersion)
                {
                    case "1.1":
                        protocolType = SecurityProtocolType.Tls11;
                        break;
                    case "1.2":
                        protocolType = SecurityProtocolType.Tls12;
                        break;
                    case "1.3":
                        protocolType = SecurityProtocolType.Tls13;
                        break;
                }
                return protocolType;
            } 
            catch 
            {
                return protocolType;
            }
        }

        public static void LoadConfig()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Session.LocalIP = helpers.GetRegistryValue("LocalIP", ip.ToString());
                }
            }
            string kas_path = "";
            bool b = false;
            bool.TryParse(helpers.GetRegistryValue("eRecipe_only", "false"), out b);
            Session.RecipesOnly = b;
#if DEBUG
            Session.Develop = true;
#else
            Session.Develop = false;
#endif
            var registry_develop = helpers.GetRegistryValue("Develop", "false");
            if (!string.IsNullOrWhiteSpace(registry_develop))
                Session.Develop = bool.Parse(registry_develop);
            Session.PHPIncludePath = helpers.GetRegistryValue("PHP_inc_path", "");
            Session.eRecipeGridSize = helpers.GetRegistryValue("eRecipe_grid_size", "10").ToInt();
            Session.ServerIP = helpers.GetRegistryValue("ServerIP", Session.LocalIP.Substring(0, Session.LocalIP.LastIndexOf('.')) + ".200");
            if (Session.LocalIP.Equals("10.22.35.201"))//pagalba
                Session.ServerIP = Session.LocalIP;
            kas_path = "\\\\" + Session.ServerIP + "\\data\\kas\\centras_devel\\";
            if (Session.LocalIP.Equals("172.22.1.166") || Session.getParam("SYST","PRODPHARM") == "1")//gamybine vaistine
            {
                Session.ServerIP = "kas.tamro.lt";
                kas_path = "\\\\" + Session.LocalIP + "\\data\\kas\\centras_devel\\";
            }
            if (Session.LocalIP.StartsWith("172.22") || Session.LocalIP.StartsWith("192.168*") || Session.LocalIP.StartsWith("10.226."))//devel
            {
                Session.Admin = true;
                kas_path = "\\\\srvdevel\\data\\www\\kas_system\\kas\\centras_devel\\";
                //_UserAccount.Stamp = "testv528";
            }
            if (Session.LocalIP.Equals("192.168.1.126") || Session.LocalIP.Equals("192.168.1.21"))//ASPA
            {
                Session.Develop = true;
                Session.ServerIP = "82.135.147.28";
                Session.Port = "5440";
                kas_path = "C:\\POS\\";
                Session.Database = "postest";
            }
            #region FeedbackTerminalAddress
            var ipArray = Session.LocalIP.Split('.');
            if (ipArray[3].ToInt() == 200)//kai servas
                ipArray[3] = "31";
            else//kai kasa
                ipArray[3] = (ipArray[3].ToInt() + 20).ToString();
            #endregion
            Session.Path = kas_path + "receptai\\importas\\files\\_na\\";

            Session.ImagePath = kas_path + "img\\display2_images\\";
            Session.eRecipe_CssPath = kas_path + "css\\DejaVuSansCondensed.ttf";
            Session.eRecipe_LogoPath = kas_path + "img\\BENU.jpg";
            //if (!String.IsNullOrWhiteSpace(Session.PHPIncludePath) && String.IsNullOrWhiteSpace(Session.ServerIP))
            //{
            //    Session.PHPconfig = helpers.ReadPHPConfigFile(Session.PHPIncludePath + "db_conf.inc");
            //}
            //else
            {
                Session.PHPconfig = helpers.ReadPHPConfigFile("Resources\\db_conf.inc");
                php_config pharmacie = new php_config()
                {
                    id = 66,
                    name = "host",
                    value = Session.ServerIP
                };
                Session.PHPconfig.Add(pharmacie);
            }
            Session.POSLogPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\POS log\";
            if (!Directory.Exists(Session.POSLogPath))
                Directory.CreateDirectory(Session.POSLogPath);
        }

        public static void DetectTypeOfSecondScreen()
        {
            if (Screen.AllScreens.Length > MaxDisplayCount)
                return;

            foreach (var screen in Screen.AllScreens)
            {
                if (Screen.PrimaryScreen.DeviceName.ToLowerInvariant() != screen?.DeviceName?.ToLowerInvariant())
                {
                    Session.IsVerticalDisplay2 = screen.Bounds.Width < screen.Bounds.Height;
                }
            }
        }
    }
}
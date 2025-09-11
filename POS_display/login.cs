using Newtonsoft.Json;
using POS_display.Models.Recipe;
using POS_display.Repository.Pos;
using POS_display.Repository.Recipe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display
{
    public partial class login : Form
    {
        private bool formWaiting = false;
        private login_image _login_image = new login_image();
        private string[] async_steps_list;
        private List<string> async_steps = new List<string>();
        private readonly IRecipeRepository _recipeRepository = null;
        private readonly IPosRepository _posRepository = null;

        public login()
        {
            InitializeComponent();
            _recipeRepository = new RecipeRepository();
            _posRepository = new PosRepository();
        }

        private void login_Load(object sender, EventArgs e)
        {
            try
            {
                form_wait(true);
                cmbConfig.DataSource = Session.PHPconfig.Where(p => p.name == "name").ToList();
                cmbConfig.DisplayMember = "value";
                cmbConfig.ValueMember = "id";
                btnLogin.Text = "Prisijungti";
                statusLabel.Text = "Palaukite...";
                Session.FileUtils.DeleteMyFiles(Session.LocalIP);

                Session.MonitorCount = System.Windows.Forms.Screen.AllScreens.Length;
                if (Session.MonitorCount <= 1)
                {
                    chb2display.Checked = false;
                    chb2display.Enabled = false;
                }

                if (Session.Develop  && (Session.LocalIP.StartsWith("172.22.1.") || Session.LocalIP.StartsWith("10.226."))) //develop
                {
                    tbLogin.Text = "buh";
                    tbPassword.Text = "mark@";
                }

                if (chb2display.Enabled == true && Session.Develop == false)
                {
                    chb2display.Checked = true;
                    this.Focus();
                }
                else
                    form_wait(false);
                async_steps.Add("");//0
                async_steps.Add("Jungiamasi...");//1
                async_steps.Add("Jungiamasi prie lojalumo sistemos");//2
                async_steps.Add("Jungiamasi prie kasos aparato");//3
                async_steps.Add("Prisijungta");//4
                async_steps_list = new string[async_steps.Count()];
                async_steps.CopyTo(async_steps_list);
                statusLabel.Text = async_steps.First();
                Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    //Thread.Sleep(10000);
                    //Background thread runs in background do not need to wait till login handle completes
                    //Session.FileUtils.GetAllImg();
                    _login_image.LoadAnotherImage();
                    Console.WriteLine("BackgroundThread say: image load completed");
                }));
                backgroundThread.Start();
            }
            catch (Exception ex)
            {
                form_wait(false);
                if (ex.Message != "")
                    helpers.alert(Enumerator.alert.error, ex.Message);
                Application.Exit();
            }
        }

        private void login_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
            if (_login_image != null)
                _login_image.Close();
        }

        private void form_wait(bool wait)
        {
            if (wait == true)
            {
                formWaiting = true;
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                btnLogin.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                btnLogin.Text = "Prisijungiama...";
            }
            else
            {
                formWaiting = false;
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                btnLogin.Enabled = true;
                btnLogin.Text = "Prisijungti";
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbLogin.TextLength == 0)
                {
                    tbLogin.Focus();
                    throw new Exception("");
                } //if
                if (tbPassword.TextLength == 0)
                {
                    tbPassword.Focus();
                    throw new Exception("");
                } //if
                if (Session.DBId == 0)
                    cmbConfig_SelectedIndexChanged(sender, e);
                form_wait(true);
                step_completed(0);
                perform_step(1, 1);
                Session.User = DB.POS.Login<Items.User>(tbLogin.Text, tbPassword.Text);
                if (Session.User == null || Session.User.id == 0 || Session.User.locked == 1)
                    throw new Exception("Neteisingi prisijungimo duomenys!");
                if (Session.User.ExpireDate.HasValue && Session.User.ExpireDate.Value < DateTime.Now)
                    throw new Exception("Vartotojo prisijungimo galiojimo data pasibaigusi!");
                _login_image.Location = helpers.middleScreen2(_login_image, false);
                _login_image.Show();
                _login_image.LoadAnotherImage();
                Thread backgroundThread = new Thread(
                new ThreadStart(async () =>
                {
                    try
                    {
                        Session.SystemData = DB.POS.getSearchSystemdata<Items.SystemData>();
                        Session.Params = DB.Settings.getParams<Items.Params>();
                        var update_date = Session.Params.FirstOrDefault(p => p.system == "SYNC" && p.par == "UPDATE_DATE")?.value;
                        if (update_date != null && helpers.format_date(update_date).Date < DateTime.Now.Date)
                            throw new Exception("Privaloma atlikti KAS atnaujinimus pagrindiniame kompiuteryje!\nTik tuomet galėsite tęsti darbą.");
                        if (Session.Params.FirstOrDefault(p => p.system == "SYNC" && p.par == "EXEC")?.value == "1"
                        && DateTime.Now > DateTime.Parse(Session.Params.FirstOrDefault(p => p.system == "SYNC" && p.par == "DATE_FROM")?.value))
                            throw new Exception("Sistemoje turi būti įdiegti pakeitimai!\nPagrindiniame kompiuteryje Atnaujinimų lange paspauskite mygtuką Pakeitimai.\nĮdiegus juos galėsite tęsti darbą.");
                        switch (Session.Params.FirstOrDefault(x => x.system == "PRIC" && x.par == "CLASS").value.ToUpper())
                        {
                            case "SV":
                                Session.PriceClass = "pk1";
                                break;
                            case "G1":
                                Session.PriceClass = "pk2";
                                break;
                            case "G2":
                                Session.PriceClass = "pk3";
                                break;
                            case "G3":
                                Session.PriceClass = "pk4";
                                break;
                        }
                        if (Session.RecipesOnly == false)
                        {
                            Session.Devices = DB.POS.getSearchDevices<Items.Devices>();
                            if (Session.Devices.debtorid == 0)
                                throw new Exception("Prie šio kompiuterio " + Session.LocalIP + " kasa neprijungta!");
                            if (Session.Devices.postype == "")
                                Session.Devices.postype = "DATECS500T";
                            if (Session.Devices.programfile != "POS.application")
                                throw new Exception("Kasa nenustatyta dirbti su nauja programa \"POS.application\". Eikite į Kasa - Nustatymai ir pakeiskite parametrą \"Darbui reikalinga programa\" ");
                            step_completed(1);
                            perform_step(1, 2);
                            if (Session.Params.FirstOrDefault(x => x.system == "CRM" && x.par == "ENABLED").value == "1")
                            {
                                if (! await Session.CRMRestUtils.TestConnection())
                                    helpers.alert(Enumerator.alert.warning, "Nepavyksta prisijungti prie lojalumo sistemos! Visos akcijos laikinai neveiks.");
                            }
                            step_completed(2);
                            perform_step(1, 3);
                            if (Session.Devices.fiscal != 0)
                            {
                                await Session.FP550.Init();
                            }
                            step_completed(3);
                        }
                        else
                        {
                            step_completed(1);
                            step_completed(2);
                            step_completed(3);
                        }
                        Session.VaccineRemindersConfig = await _recipeRepository.GetVaccineDays();
                        Session.ProductLocations = await _posRepository.LoadProductsLocations();
                        Session.RecipeParm = DB.POS.getRecipeParm<Items.RecipeParm>();
                        Session.POSErrors = DB.POS.getPOSErrors<Items.Error>();
                        Session.ProductFlags = await _posRepository.LoadProductFlags();
                        Session.ExclusiveProducts = await _posRepository.LoadExclusiveProducts();
                        Session.TLKCheapests = await _posRepository.GetTLKCheapests();
                        await InitPOSMemo();
                        await _posRepository.WriteDBLog($"USER {Session.User.id} successfully logged in to POS.");
                        step_completed(4);
                    }
                    catch (Exception ex)
                    {
                        perform_step(0, 0);
                        this.BeginInvoke(new Action(() =>
                        {
                            form_wait(false);
                            _login_image.Hide();
                            statusLabel.Text = "";
                        }));
                        helpers.alert(Enumerator.alert.error, ex.Message);
                    }
                }));
                backgroundThread.Start();
            }
            catch (Exception ex)
            {
                perform_step(0, 0);
                form_wait(false);
                statusLabel.Text = "";
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

        private async Task InitPOSMemo() 
        {
            var posMemoParamters = Enum.GetValues(typeof(Enumerator.POSMemoParamter)).Cast<Enumerator.POSMemoParamter>().ToArray();
            foreach (var posMemoParam in posMemoParamters) 
            {
                switch (posMemoParam)
                {
                    case Enumerator.POSMemoParamter.HomeMode:
                        {
                            var memoValue = await _posRepository.GetPosMemoValue(Session.Devices.debtorid, posMemoParam);
                            if (memoValue == null)
                            {
                                await _posRepository.InsertPosMemo(Session.Devices.debtorid, posMemoParam, "0");
                                Session.HomeMode = false;
                            }
                            else
                                Session.HomeMode = memoValue == "1";
                            break;
                        }
                    case Enumerator.POSMemoParamter.Wolt:
                        {
                            var memoValue = await _posRepository.GetPosMemoValue(Session.Devices.debtorid, posMemoParam);
                            if (memoValue == null)
                            {
                                await _posRepository.InsertPosMemo(Session.Devices.debtorid, posMemoParam, "0");
                                Session.WoltMode = false;
                            }
                            else
                                Session.WoltMode = memoValue == "1";
                            break;
                        }
                    case Enumerator.POSMemoParamter.CreateMultipeDispense:
                        {
                            var memoValue = await _posRepository.GetPosMemoValue(Session.Devices.debtorid, posMemoParam);
                            if (memoValue == null)
                            {
                                await _posRepository.InsertPosMemo(Session.Devices.debtorid, posMemoParam, "");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(memoValue))
                                    Session.GruopDispenseRequests = JsonConvert.DeserializeObject<List<GroupDispenseRequest>>(memoValue);
                                else
                                    Session.GruopDispenseRequests = new List<GroupDispenseRequest>();
                            }
                            break;
                        }
                }
            }            
        }


        private void chb2display_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Session.display2checked = cb.Checked;
            this.ShowDisplay2();
        }

        private void cmbConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            php_config selectedItem = (php_config)cmbConfig.SelectedItem;
            Session.ServerIP = Session.PHPconfig.Where(p => p.id == selectedItem.id && p.name == "host").First().value.TrimStart().TrimEnd();
            if (Session.ServerIP == "localhost" && Session.RecipesOnly == true)
                Session.ServerIP = "kasserv";
            Session.DBId = selectedItem.id;
            Session.Username = Session.PHPconfig.Where(p => p.id == selectedItem.id && p.name == "user").First().value.TrimStart().TrimEnd();
            Session.Password = Session.PHPconfig.Where(p => p.id == selectedItem.id && p.name == "password").First().value.TrimStart().TrimEnd();
            Session.Database = Session.PHPconfig.Where(p => p.id == selectedItem.id && p.name == "database").First().value.TrimStart().TrimEnd();
            Session.DatabaseName = Session.PHPconfig.Where(p => p.id == selectedItem.id && p.name == "name").First().value.TrimStart().TrimEnd();
        }

        private void cmbConfig_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if ((e.KeyData >= Keys.A && e.KeyData <= Keys.Z) || (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9) || (e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9))
            {
                var hit = Session.PHPconfig.Where(o => (o.value.TrimStart().StartsWith(e.KeyCode.ToString().ToUpper())));
                if (hit != null && hit.Count() > 0)
                {
                    var el = cb.FindStringExact(hit.First().value);
                    if (el >= 0)
                        cb.SelectedIndex = el;
                    //cb.DroppedDown = true;
                }
            }
        }

        private void ShowDisplay2()
        {
            if (chb2display.Checked == true /*&& (Application.OpenForms[Enumerator.alert.display2] as display2) == null*/)
            {
                Program.Display2.Show();
                Program.Display2.ChangeScreen();
            }

            if (chb2display.Checked == false /*&& (Application.OpenForms[Enumerator.alert.display2] as display2) != null*/)
            {
                Program.Display2.Hide();
                //Application.OpenForms[Enumerator.alert.display2].Close();
            }
            form_wait(false);
        }

        private void perform_step(int step, int step_no)
        {
            this.BeginInvoke(new Action(() =>
            {
            
            switch (step)
            {
                case 0:
                    progressBar.Value = step;
                    if (_login_image != null)
                        _login_image.perform_step(step, statusLabel.Text);
                    break;
                case 7:
                    progressBar.Value = step;
                    if (_login_image != null)
                        _login_image.perform_step(step, statusLabel.Text);
                    break;
                default:
                    for (int i = 0; i < step; i++)
                        progressBar.PerformStep();
                    if (_login_image != null)
                        _login_image.perform_step(step, statusLabel.Text);
                    break;
            }
            }));
        }

        private void step_completed(int step_no)
        {
            Console.WriteLine("STEP " + step_no);
            async_steps.Remove(async_steps_list[step_no]);
            if (async_steps.Count > 0)
                statusLabel.Text = async_steps.First();
            else
            {
                this.BeginInvoke(new Action(() =>
                {
                    form_wait(false);
                    this.DialogResult = DialogResult.OK;
                }));
            }
        }
    }
}
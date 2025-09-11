using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_display.Helpers;
using POS_display.Utils.Logging;

namespace POS_display
{
    public class FormBase : Form
    {
        private volatile FormWait _formWait;

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public FormBase()
        {
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
        }

        private bool _IsBusy;
        internal virtual bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                this.UseWaitCursor = value;
                if (_IsBusy == true)
                    this.Cursor = Cursors.WaitCursor;
                else
                    this.Cursor = Cursors.Default;
            }
        }


        private bool doAlert(string msg)
        {
            return helpers.alert(Enumerator.alert.error, msg);
        }

        private bool doWriteLine(string msg)
        {
            Console.WriteLine(msg);
            return true;
        }

        private Action TryCatchWrapper(Action functionToExcute, Func<string, bool> catchFunction)
        {
            Action tryBlockWrapper = () =>
            {
                try
                {
                    functionToExcute();
                }
                catch (Exception ex)
                {
                    Serilogger.GetLogger().Error(ex, ex.Message);
                    catchFunction(ex.Message);
                }
            };
            return tryBlockWrapper;
        }

        private async Task TryCatchWrapperAsync(Func<Task> functionToExcute, Func<string, bool> catchFunction)
        {
            try
            {
                await functionToExcute();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Serilogger.GetLogger().Error(ex, msg);
                if (ex is TaskCanceledException)
                    msg = "";
                catchFunction(msg);
            }
        }

        private Action FormWaitWrapper(Action functionToExcute, bool checkIfWaiting)
        {
            Action tryBlockWrapper = () =>
            {
                if (checkIfWaiting && IsBusy)
                    return;
                IsBusy = true;
                functionToExcute();
                IsBusy = false;
            };
            return tryBlockWrapper;
        }

        private async Task FormWaitWrapperAsync(Func<Task> functionToExecute, bool checkIfWaiting, FormWait formWait = null)
        {
            if (checkIfWaiting && IsBusy)
                return;

            _formWait = formWait;

            if (_formWait != null && !_formWait.IsDisposed)
            {
                _formWait.Start();
            }

            IsBusy = true;

            try
            {
                await functionToExecute();
            }
            finally
            {
                IsBusy = false;
                if (_formWait != null && !_formWait.IsDisposed)
                {
                    _formWait.Stop();
                }
            }
        }

        internal void Execute(Action functionToExcute)
        {
            var action = TryCatchWrapper(() =>
            {
                functionToExcute();
            }, doAlert);

            action.Invoke();
        }

        internal async Task ExecuteAsync(Func<Task> functionToExcute)
        {
            await TryCatchWrapperAsync(async () =>
            {
                await functionToExcute();
            }, doAlert);
        }

        public void ExecuteAsyncAction(Func<Task> functionToExcute)
        {
            var action = new Action(async () =>
            {
                await TryCatchWrapperAsync(async () =>
                {
                    await functionToExcute();
                }, doWriteLine);
            });

            action.BeginInvoke(null, null);
        }

        internal void ExecuteWithWait(Action functionToExcute, bool checkIfWaiting = true)
        {
            var action = FormWaitWrapper(() =>
            {
                TryCatchWrapper(() =>
                {
                    functionToExcute();
                }, doAlert).Invoke();
            }, checkIfWaiting);

            action.Invoke();
        }

        internal async Task ExecuteWithWaitAsync(Func<Task> functionToExcute, bool checkIfWaiting = true, Func<string, bool> catchFunction = null, FormWait waitForm = null)
        {
            await FormWaitWrapperAsync(async () =>
            {
                await TryCatchWrapperAsync(async () =>
                {
                    await functionToExcute();
                }, catchFunction == null ? doAlert : catchFunction);
            }, checkIfWaiting, waitForm);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy && (Control.ModifierKeys & Keys.Alt) == 0)
                e.Cancel = true;
        }
    }
}

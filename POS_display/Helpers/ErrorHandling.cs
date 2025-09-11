using System;
using System.Threading.Tasks;

namespace POS_display
{
    public class ErrorHandling
    {
        public virtual bool IsBusy { get; set; }

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

        private async Task FormWaitWrapperAsync(Func<Task> functionToExcute, bool checkIfWaiting)
        {
            if (checkIfWaiting && IsBusy)
                return;
            IsBusy = true;
            await functionToExcute();
            IsBusy = false;
        }

        internal void Execute(Action functionToExcute)
        {
            var action = TryCatchWrapper(() =>
            {
                functionToExcute();
            }, doAlert);

            action.Invoke();
        }

        public void ExecuteAsyncAction(Func<Task> functionToExcute, AsyncCallback completed = null)
        {
            var action = new Action(async () =>
            {
                await TryCatchWrapperAsync(async () =>
                {
                    await functionToExcute();
                }, doWriteLine);
            });

            action.BeginInvoke(completed, null);
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

        internal async Task ExecuteWithWaitAsync(Func<Task> functionToExcute, bool checkIfWaiting = true)
        {
            await FormWaitWrapperAsync(async () =>
            {
                await TryCatchWrapperAsync(async () =>
                {
                    await functionToExcute();
                }, doAlert);
            }, checkIfWaiting);
        }
    }
}

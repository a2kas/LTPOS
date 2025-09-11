using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class BaseViewModel : ErrorHandling, INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null, bool force_update = false)
        {
            if (object.Equals(storage, value) && !force_update) return false;

            storage = value;
            this.NotifyPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private bool _IsBusy;
        public override bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

        public event EventHandler CloseEventHandler;
        protected virtual void CloseEvent(object Result)
        {
            var handler = CloseEventHandler;
            if (Result == null)
                Result = System.Windows.Forms.DialogResult.Cancel;
            if (handler != null)
                handler(Result, new EventArgs());
        }

        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new BaseCommand(CloseEvent));
            }
        }

        private ICommand _CopyCommand;
        public ICommand CopyCommand
        {
            get
            {
                return _CopyCommand ?? (_CopyCommand = new BaseCommand(CopyCommand_Executed));
            }
        }

        private void CopyCommand_Executed(object sender)
        {
            try
            {
                System.Windows.Controls.DataGrid dg = sender as System.Windows.Controls.DataGrid;
                var h = dg.CurrentCell.Column.SortMemberPath;
                var item = dg.CurrentItem;
                var prop = item.GetType().GetProperty(h);
                var value = prop.GetValue(item, null);
                Clipboard.SetText(value.ToString());
            }
            catch (Exception ex) { }
        }
    }

    public class BaseCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _method;
        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<object> method)
            : this(method, null)
        {
        }

        public BaseCommand(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await Task.Run(() => _method(parameter));
        }
    }

    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    public class BaseAsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public BaseAsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}

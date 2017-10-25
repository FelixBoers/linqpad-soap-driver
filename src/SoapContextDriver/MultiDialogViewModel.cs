using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SoapContextDriver
{
    public class MultiDialogViewModel : INotifyPropertyChanged
    {
        private readonly ConnectionInfoAdapter _connectionInfo;

        public IRelayCommand LoadCommand { get; } 
        public ObservableCollection<ConnectionModel> Connections { get; }

        public MultiDialogViewModel(ConnectionInfoAdapter connectionInfo, ConnectionHistoryReader connectionHistoryReader)
        {
            _connectionInfo = connectionInfo;
            Connections = new ObservableCollection<ConnectionModel>();
            LoadCommand = new RelayCommand(Load);
        }

        private void Load()
        {
            Connections.Clear();
            foreach (var conn in _connectionInfo)
                Connections.Add(new ConnectionModel(conn));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IRelayCommand : ICommand
    {
        void OnCanExecuteChanged();
        bool CanExecute();
        void Execute();
    }

    public interface IRelayCommand<in T> : ICommand
    {
        bool CanExecute(T parameter);
        void Execute(T parameter);
        void OnCanExecuteChanged();
    }

    public class RelayCommand<T> : IRelayCommand<T>
    {
        private readonly Action<T> _action;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> action, Func<T, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T) parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(T parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(T parameter)
        {
            _action(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand : RelayCommand<object>, IRelayCommand
    {
        public RelayCommand(Action action, Func<bool> canExecute = null)
            : base(_ => action(), _ => canExecute?.Invoke() ?? true)
        {
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute()
        {
            Execute(null);
        }
    }
}
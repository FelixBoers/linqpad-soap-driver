using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace SoapContextDriver
{
    public class ConnectionModel : INotifyPropertyChanged
    {
        private readonly ConnectionAdapter _adapter;
        private string _url;
        private string _bindingName;
        private string _namespaceName;
        private string[] _bindings;
        private bool _isLoading;
        private IDiscovery _discovery;

        public ConnectionModel(ConnectionAdapter adapter)
        {
            _adapter = adapter;
            _url = _adapter.Url;
            _bindingName = _adapter.BindingName;
            _namespaceName = _adapter.NamespaceName;
        }

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged();
                DiscoverBindings();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();               
            }
        }

        private void DiscoverBindings()
        {
            try
            {
                IsLoading = true;
                _discovery = CreateDiscovery(_url);
                _discovery.GetServices().Select(s => s.Bindings);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public string[] Binding
        {
            get { return _bindings; }
            private set
            {
                _bindings = value;
                OnPropertyChanged();
            }
        }

        public string BindingName
        {
            get { return _bindingName; }
            set
            {
                _bindingName = value;
                OnPropertyChanged();
            }
        }

        public string NamespaceName
        {
            get { return _namespaceName; }
            set
            {
                _namespaceName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual IDiscovery CreateDiscovery(string url)
        {
            return new Discovery(url, CredentialCache.DefaultNetworkCredentials);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            _adapter.Url = Url;
            _adapter.BindingName = BindingName;
            _adapter.NamespaceName = NamespaceName;
        }
    }
}
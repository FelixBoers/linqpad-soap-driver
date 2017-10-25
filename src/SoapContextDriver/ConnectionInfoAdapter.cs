using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;

namespace SoapContextDriver
{
    public class ConnectionInfoAdapter : ICollection<ConnectionAdapter>, IEquatable<ConnectionInfoAdapter>
    {
        private readonly IConnectionInfo _info;
        private readonly XElement _connections;

        public ConnectionInfoAdapter(IConnectionInfo info, IEnumerable<string> knownUris = null)
        {
            _info = info;
            KnownUris = knownUris ?? new string[0];
            _connections = info.DriverData.Element("Connections");
            if (null != _connections)
                return;
            _connections = new XElement("Connections");
            info.DriverData.Add(_connections);
        }

        public string DisplayName
        {
            get { return _info.DisplayName; }
            set { _info.DisplayName = value; }
        }

        public bool Persist
        {
            get { return _info.Persist; }
            set { _info.Persist = value; }
        }

        public bool IsProduction
        {
            get { return _info.IsProduction; }
            set { _info.IsProduction = value; }
        }

        public IEnumerable<string> KnownUris { get; }

        protected virtual IEnumerable<XElement> ConnectionElements => _connections.Elements("Connection");

        protected virtual IEnumerable<ConnectionAdapter> Connections
        {
            get { return ConnectionElements.Select(e => new ConnectionAdapter(e)); }
        }

        public IEnumerator<ConnectionAdapter> GetEnumerator()
        {
           return Connections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ConnectionAdapter item)
        {
            if (null == item)
                throw new ArgumentNullException(nameof(item));
            _connections.Add(item.Element);
        }

        public void Clear()
        {
            _connections.RemoveAll();
        }

        public bool Contains(ConnectionAdapter item)
        {
            return null != item && ConnectionElements.Any(e => item.Element == e);
        }

        public void CopyTo(ConnectionAdapter[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ConnectionAdapter item)
        {
            if (null == item)
                return false;
            item.Element.Remove();
            return true;
        }

        public int Count => ConnectionElements.Count();
        public bool IsReadOnly { get; } = false;

        public bool Equals(ConnectionInfoAdapter other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other.Count != Count)
                return false;
            if (!Equals(Persist, other.Persist))
                return false;
            if (!Equals(DisplayName, other.DisplayName))
                return false;
            if (!Equals(IsProduction, other.IsProduction))
                return false;
            var allConnectionsEquals = !other.Where((t, i) => !Equals(other.ElementAt(i), this.ElementAt(i))).Any();
            return allConnectionsEquals;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConnectionInfoAdapter);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_info != null ? _info.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_connections != null ? _connections.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class ConnectionAdapter : IEquatable<ConnectionAdapter>
    {
        public ConnectionAdapter()
            : this(new XElement("Connection"))
        {
        }

        public ConnectionAdapter(XElement element)
        {
            Element = element;
        }

        public XElement Element { get; }

        public string Url
        {
            get { return (string)Element.Element("Url"); }
            set { Element.SetElementValue("Url", value); }
        }

        public string BindingName
        {
            get { return (string) Element.Element("BindingName"); }
            set { Element.SetElementValue("BindingName", value); }
        }

        public string NamespaceName
        {
            get { return (string) Element.Element("NamespaceName"); }
            set { Element.SetElementValue("NamespaceName", value);}
        }

        public bool Equals(ConnectionAdapter other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(Element, other.Element);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ConnectionAdapter);
        }

        public override int GetHashCode()
        {
            return (Element != null ? Element.GetHashCode() : 0);
        }
    }
}
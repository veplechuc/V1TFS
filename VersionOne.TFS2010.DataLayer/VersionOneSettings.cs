
using Integrations.Core.Interfaces;

namespace VersionOne.TFS2010.DataLayer 
{

    public class VersionOneSettings : IVersionOneSettings
    {
        public bool Integrated { get; set; }
        public string Path { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IProxyConnectionSettings ProxySettings { get; set; }

        public VersionOneSettings() 
        {
            ProxySettings = new ProxyConnectionSettings();
        }

        public override bool Equals(object obj) 
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != typeof (VersionOneSettings)) 
            {
                return false;
            }
            
            return ReferenceEquals(this, obj) || Equals((VersionOneSettings) obj);
        }

        private bool Equals(VersionOneSettings other) 
        {
            if (ReferenceEquals(null, other)) 
            {
                return false;
            }
            
            if (ReferenceEquals(this, other)) 
            {
                return true;
            }
            
            return other.Integrated.Equals(Integrated) && Equals(other.Path, Path) && Equals(other.Username, Username) && Equals(other.Password, Password) && Equals(other.ProxySettings, ProxySettings);
        }

        public override int GetHashCode() 
        {
            unchecked 
            {
                var result = Integrated.GetHashCode();
                result = (result*397) ^ (Path != null ? Path.GetHashCode() : 0);
                result = (result*397) ^ (Username != null ? Username.GetHashCode() : 0);
                result = (result*397) ^ (Password != null ? Password.GetHashCode() : 0);
                result = (result*397) ^ (ProxySettings != null ? ProxySettings.GetHashCode() : 0);
                
                return result;
            }
        }
    }
}

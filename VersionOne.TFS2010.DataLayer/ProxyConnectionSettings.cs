namespace VersionOne.TFS2010.DataLayer 
{
    public class ProxyConnectionSettings 
    {
        public bool UseProxy { get; set; }
        public string Url { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj) 
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != typeof (ProxyConnectionSettings)) 
            {
                return false;
            }
            
            return ReferenceEquals(this, obj) || Equals((ProxyConnectionSettings) obj);
        }

        // TODO consider objects equal if UseProxy == false in both of them
        private bool Equals(ProxyConnectionSettings other) 
        {
            if (ReferenceEquals(null, other)) 
            {
                return false;
            }
            
            if (ReferenceEquals(this, other)) 
            {
                return true;
            }
            
            return other.UseProxy.Equals(UseProxy) && Equals(other.Url, Url) && Equals(other.Domain, Domain) && Equals(other.Username, Username) && Equals(other.Password, Password);
        }

        public override int GetHashCode() 
        {
            unchecked 
            {
                var result = UseProxy.GetHashCode();
                result = (result*397) ^ (Url != null ? Url.GetHashCode() : 0);
                result = (result*397) ^ (Domain != null ? Domain.GetHashCode() : 0);
                result = (result*397) ^ (Username != null ? Username.GetHashCode() : 0);
                result = (result*397) ^ (Password != null ? Password.GetHashCode() : 0);
                
                return result;
            }
        }
    }
}
namespace Integrations.Core.Interfaces
{
    public interface IVersionOneSettings
    {
        bool Integrated { get; set; }
        string Path { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        IProxyConnectionSettings ProxySettings { get; set; }
    }
}

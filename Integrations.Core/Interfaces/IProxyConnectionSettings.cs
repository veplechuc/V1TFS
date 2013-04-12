namespace Integrations.Core.Interfaces
{
    public interface IProxyConnectionSettings
    {
        bool UseProxy { get; set; }
        string Url { get; set; }
        string Domain { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}

namespace Integrations.Core.Interfaces
{
    public interface IProxyConnectionSettings
    {
        bool UseProxy { get; }
        string Url { get; }
        string Domain { get; }
        string Username { get; }
        string Password { get; }
    }
}

using System.Net;
using Integrations.Core.DTO;
using Newtonsoft.Json;

namespace VersionOneTFSServerConfig.Configuration
{
    public interface IHttpClient
    {
        byte[] Put(string url, byte[] body);
        byte[] Get(string url);
    }

    public class HttpClient : IHttpClient
{
        private readonly WebClient _client;

        public HttpClient(WebClient client = null)
        {
            _client = client ?? new WebClient();
        }

        public byte[] Put(string url, byte[] body)
        {
            return _client.UploadData(url, body);
        }

        public byte[] Get(string url)
        {
            return _client.DownloadData(url);
        }
}

    public class ConfigurationProxy
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public static string ProbeServerConfig()
        {
            // look in web.config
            // return convention
            // port-scan local box
            return "http://localhost:9090/Configuration/";
        }
        public ConfigurationProxy(IHttpClient client = null, string url = null)
        {
            _client = client ?? new HttpClient();
            _url = url ?? ProbeServerConfig();
        }

        public void Store(TfsServerConfiguration config)
        {
            var json = JsonConvert.SerializeObject(config);
            var result = _client.Put(_url, System.Text.Encoding.UTF8.GetBytes(json));
            var body = System.Text.Encoding.UTF8.GetString(result);
            var response = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(body);
            if (response["status"] != "ok")
            {
                throw new System.Exception("Server didn't accept configuration");
            }
        }

        public TfsServerConfiguration Retrieve()
        {
            var result = _client.Get(_url);
            var body = System.Text.Encoding.UTF8.GetString(result);
            return JsonConvert.DeserializeObject<TfsServerConfiguration>(body);
        }


    }

}
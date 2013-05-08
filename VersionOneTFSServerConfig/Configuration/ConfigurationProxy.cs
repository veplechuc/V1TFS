using System;
using System.Collections.Generic;
using System.Net;
using Integrations.Core.DTO;
using Newtonsoft.Json;
using VersionOneTFS2010.DataLayer.Providers;

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
        private readonly string _baseUrl;

        public static string ProbeServerConfig()
        {
            // look in web.config
            // return convention
            // port-scan local box
            return "http://localhost:9090/";
        }
        public ConfigurationProxy(IHttpClient client = null, string baseListenerUrl = null)
        {
            _client = client ?? new HttpClient();
            if (string.IsNullOrEmpty(baseListenerUrl))
            {
                _baseUrl = new DefaultConfigurationProvider().BaseListenerUrl.ToString();
            }
            else
            {
                _baseUrl = baseListenerUrl;
            }
        }

        public string BaseListenerUrl
        {
            get { return _baseUrl; }
        }

        public string ConfigurationUrl
        {
            get { return new Uri(new Uri(BaseListenerUrl), "configuration/").ToString(); }
        }

        public string ListenerUrl
        {
            get { return new Uri(new Uri(BaseListenerUrl), "service.svc").ToString(); }
        }

        public Dictionary<string, string> Store(TfsServerConfiguration config)
        {
            var json = JsonConvert.SerializeObject(config);
            var result = _client.Put(ConfigurationUrl, System.Text.Encoding.UTF8.GetBytes(json));
            var body = System.Text.Encoding.UTF8.GetString(result);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
        }

        public TfsServerConfiguration Retrieve()
        {
            var result = _client.Get(ConfigurationUrl);
            var body = System.Text.Encoding.UTF8.GetString(result);
            return JsonConvert.DeserializeObject<TfsServerConfiguration>(body);
        }


    }

}
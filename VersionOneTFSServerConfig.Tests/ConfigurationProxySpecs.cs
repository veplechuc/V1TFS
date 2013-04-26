using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;
using Integrations.Core.DTO;
using NSpec;
using Newtonsoft.Json;
using VersionOneTFSServerConfig.Configuration;

namespace VersionOneTFSServer.Tests
{

    public class PretendsToBeConnectedToHttpClient : IHttpClient
    {
        private readonly Dictionary<string, TfsServerConfiguration> _stored = new Dictionary<string, TfsServerConfiguration>();
        private readonly JavaScriptSerializer _serializer;

        public PretendsToBeConnectedToHttpClient()
        {
            _serializer = new JavaScriptSerializer();
        }


        public new byte[] Put(string address, byte[] data)
        {
            var config = _serializer.Deserialize<TfsServerConfiguration>(
                    System.Text.Encoding.UTF8.GetString(data));

            config.TfsUrl.should_not_be_empty();
            config.TfsUserName.should_not_be_empty();
            config.TfsPassword.should_not_be_empty();
            config.VersionOneUrl.should_not_be_empty();
            config.VersionOneUserName.should_not_be_empty();
            config.VersionOnePassword.should_not_be_empty();
            if (config.ProxyIsEnabled)
            {
                config.ProxyUrl.should_not_be_empty();
                config.ProxyUsername.should_not_be_empty();
                config.ProxyPassword.should_not_be_empty();
            }
            _stored[address] = config;
            return System.Text.Encoding.UTF8.GetBytes(_serializer.Serialize(new { status = "ok" })); // what would actual v1tfs server return?
        }

        public new byte[] Get(string url)
        {
            if (!_stored.ContainsKey(url))
            {
                // synthesize web client behavior
                throw new System.Net.WebException("Not Found");
            }
            return System.Text.Encoding.UTF8.GetBytes(_serializer.Serialize(_stored[url]));
        }


    }

    public class ConfigurationProxySpecs : nspec
    {

        readonly TfsServerConfiguration _serializationTarget = new TfsServerConfiguration()
        {
            DebugMode = false,
            IsWindowsIntegratedSecurity = true,
            VersionOnePassword = "admin",
            ProxyDomain = "AD",
            ProxyIsEnabled = true,
            ProxyPassword = "123456",
            ProxyUrl = "http://myproxy:9191/proxy/",
            ProxyUsername = "admin1",
            TfsPassword = "abc123",
            TfsUrl = "http://localhost/tfs/defaultcollection/",
            TfsUserName = "admin3",
            TfsWorkItemRegex = null,
            VersionOneUserName = "admin2",
            VersionOneUrl = "http://www.versionone.com/"
        };

        /// <summary>
        /// Tests that we have a serializable/deserializable chunk of data available to send/recieve.  Indirect relationship to the ConfigurationProxy class.
        /// </summary>
        public void given_configuration_settings_are_being_serialized_and_deserialized()
        {

            context["when i serialize and deserialize configuration settings"] = () =>
                {
                    it["then the json is valid and deserializable"] = () =>
                        {
                            var json = JsonConvert.SerializeObject(_serializationTarget);
                            var deserializedObject = JsonConvert.DeserializeObject<TfsServerConfiguration>(json);
                            deserializedObject.should_be(_serializationTarget);
                        };
                };
        }

        /// <summary>
        /// Exercise mock server
        /// </summary>
        public void given_data_is_being_sent_and_received_to_the_server()
        {
            var mock = new PretendsToBeConnectedToHttpClient();
            var proxy = new ConfigurationProxy(mock);
            it["can submit valid configuration"] = () => proxy.Store(_serializationTarget);
            it["gets back the object it stored"] = () => proxy.Retrieve().should_be(_serializationTarget);
        }


    }
}
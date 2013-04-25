
using System;
using Integrations.Core.DTO;
using Newtonsoft.Json;

namespace VersionOneTFSServerConfig.Serializers
{

    public static class ConfigurationSerializer
    {
        public static void SerializeAndSend(TfsServerConfiguration config)
        {
            var jsonToSend = JsonConvert.SerializeObject(config);
            throw new NotImplementedException();
        }
    }

}
using NSpec;

namespace VersionOneTFSServerConfig.Tests
{
    public class ConfiguationSerializerSpecs : nspec
    {
        public void given_configuration_is_being_serialized()
        {
            before = () =>
                {

                };

            context["when i serialize the configuration to json"] = () =>
                {
                    it["then the json is valid and ready to be sent to the service"] = () =>
                        {

                        };
                };

        }
    }
}
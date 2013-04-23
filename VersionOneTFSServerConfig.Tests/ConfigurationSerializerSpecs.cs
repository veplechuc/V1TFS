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
                    it["then the json is valid"] = () =>
                        {

                        };
                };

        }

        public void given_configuration_is_being_deserialized()
        {
            before = () =>
                {

                };

            context["when i deserialize the configuration from json"] = () =>
                {
                    it["then a valid .net object results"] = () =>
                        {

                        };
                };

        }

    }

    

}
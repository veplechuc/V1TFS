using NSpec;
using VersionOneTFSServer.Interfaces;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class WebConfigProviderSpecs : nspec
    {

        IWebConfigProvider _target = null;

        public void given_app_settings_are_being_retrieved_from_a_web_config()
        {

            //runs before each context
            before = () =>
            {
                _target = new WebConfigProvider();
            };

            context["when i retrieve a setting that does not exist"] = () =>
                {
                    it["then a null value is returned for the key specified"] = () =>
                        {
                            const string keyNames = "SomeSettingThatDoesntExist";
                            var values = _target.GetAppSettings(keyNames);
                            values.should_contain(x => x.Key == keyNames);
                            values[keyNames].should_be(null);
                        };
                };
        }

        public void given_app_settings_are_being_written_to_a_web_config()
        {

            //runs before each context
            before = () =>
            {
                _target = new WebConfigProvider();
            };

            context["when i save a new setting the web config"] = () =>
                {
                    it["then the value is saved successfully"] = () =>
                        {

                        };
                };
        }

        public void given_relevant_app_settings_are_cleared_from_web_config()
        {

            //runs before each context
            before = () =>
            {
                _target = new WebConfigProvider();
            };

            context["when settings are cleared"] = () =>
            {
                it["then there are no relevant values retained in the web config"] = () =>
                    {

                        _target.ClearV1Settings();


                        it["and no other settings are not disturbed"] = () =>
                            {

                            };
                    };
            };
        }

    }
}

using NSpec;
using VersionOneTFSServer.Interfaces;
using VersionOneTFSServer.Providers;

namespace VersionOneTFSServer.Tests
{
    public class WebConfigProviderSpecs : nspec
    {
        public void given_app_settings_are_being_retrieved_from_a_web_config()
        {

            IWebConfigProvider _target = null;

            //runs before each context
            before = () =>
            {
                _target = new WebConfigProvider();
            };

            context["when i retrieve a single setting that does not exist"] = () =>
                {
                    it["then a null value is returned for the key specified"] = () =>
                        {
                            const string keyNames = "SomeSettingThatDoesntExist";
                            var values = _target.GetAppSettings(keyNames);
                            values.should_contain(x => x.Key == keyNames);
                            values[keyNames].should_be(null);
                        };
                };

            //context["when settings are saved for the first time"] = () =>
            //    {
            //        it["then the save completes successfully"] = () =>
            //            {
            //                //throw new NotImplementedException();
            //            };
            //};

        }
    }
}

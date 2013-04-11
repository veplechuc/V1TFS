using NSpec;
using VersionOneTFSServer.Collections;

namespace VersionOneTFSServer.Tests
{
    class AppSettingKeyCollectionSpecs : nspec
    {
        void given_app_setting_keys_are_needed_for_recursion()
        {

            context["when i request a collection of app setting keys"] = () =>
            {
                it["then i receive a collection to iterate over"] = () =>
                {
                    var keys = new AppSettingKeyCollection();

                    keys.Count.should_not_be(0);
                    foreach (var key in keys)
                    {
                        key.should_not_be(null);
                        key.should_not_be(string.Empty);
                        key.Value.should_not_be(null);
                        key.Value.should_not_be(string.Empty);
                    }
                };
            };
        }
    }
}

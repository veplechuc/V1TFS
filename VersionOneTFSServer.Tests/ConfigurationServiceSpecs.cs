using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSpec;

namespace VersionOneTFSServer.Tests
{
    class given_interaction_with_the_configuration_service : nspec
    {

        public void when_a_user_retrieves_settings_from_the_configuration_service()
        {
            it["should return defaults on first call"] = () =>
                {

                };
        }

        void given_the_world_has_not_come_to_an_end()
        {
            it["Hello World should be Hello World"] = () => "Hello World".should_be("Hello World");
        }
    }
}

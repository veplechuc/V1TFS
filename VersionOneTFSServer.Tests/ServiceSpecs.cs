using NSpec;

namespace VersionOneTFSServer.Tests
{
    public class ServiceSpecs : nspec
    {

        public void given_an_exception_occurs_on_a_tfs_checkin_event()
        {

            it["then the exception is returned to the client"] = () =>
                {
                    using (var client = new ServiceProxy.ServiceClient())
                    {
                        client.Notify(null, null);
                    }
                };

        }

    }
}

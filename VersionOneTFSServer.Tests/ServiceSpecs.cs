using System;
using System.ServiceModel;
using NSpec;

namespace VersionOneTFSServer.Tests
{
    public class ServiceSpecs : nspec
    {

        public void given_an_exception_occurs_on_a_tfs_checkin_event()
        {
            it["then the exception is returned to the client"] = () =>
                {
                    var client = new ServiceProxy.ServiceClient();
                    try
                    {
                        expect<FaultException<ExceptionDetail>>(() => client.Notify(null, null));
                    }
                    finally
                    {
                        if (client.State != CommunicationState.Closed) client.Close();
                    }

                };
        }

    }
}

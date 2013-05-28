using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
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

        public void given_a_soap_message_with_a_string_length_greater_than_the_default_of_8192_is_sent()
        {

            var longXml = GetLongXmlString();

            it["then the message is processed successfully"] = () =>
                {
                    var client = new ServiceProxy.ServiceClient();
                    client.Notify(longXml, "<TeamFoundationServer url='http://localhost:8080/tfs/DefaultCollection/Services/v3.0/LocationService.asmx' />");
                };


        }

        private string GetLongXmlString()
        {
            var doc = new XmlDocument();
            doc.Load(@"Resources/LongXML.xml");
            var stringWriter = new StringWriter();
            var xmlTextWriter = new XmlTextWriter(stringWriter);
            doc.WriteTo(xmlTextWriter);
            return stringWriter.ToString();
        }

    }
}

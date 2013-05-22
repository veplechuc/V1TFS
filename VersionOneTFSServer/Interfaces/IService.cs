using System.ServiceModel;

namespace VersionOneTFSServer.Interfaces
{
    [ServiceContract(Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03")]
    public interface IService
    {
        [OperationContract(Action =  "http://schemas.microsoft.com/TeamFoundation/2005/06/Services/Notification/03/Notify")]
        [XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void Notify(string eventXml, string tfsIdentityXml);
    }
}

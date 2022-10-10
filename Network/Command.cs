using Newtonsoft.Json.Linq;


namespace RemoteHealthCare.Network
{
    public interface Command
    {
        void OnCommandReceived(JObject ob);
    }
}

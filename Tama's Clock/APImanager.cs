using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Moni
{
    public class APImanager
    {
        public string requestURL = null;
        public string apiKey = null;

        public APImanager(string url, string key)
        {
            this.requestURL = url;
            this.apiKey = key;
        }
        
        public JObject RequestApi()
        {
            if(requestURL == null || apiKey == null)
            {
                return null;
            }

            string url = requestURL + apiKey;

            WebRequest request = (WebRequest)WebRequest.Create(url);
            Stream response_stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response_stream);
            JObject response = JObject.Parse(reader.ReadToEnd());
            return response;
        }
    }
}

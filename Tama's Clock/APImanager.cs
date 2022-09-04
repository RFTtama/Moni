using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System;

namespace Moni
{
    public class APImanager
    {   
        public JObject RequestApi(string requestUrl)
        {
            string encodeUrl = Uri.EscapeDataString(requestUrl);
            WebRequest request = WebRequest.Create(requestUrl);
            Stream response_stream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response_stream);
            JObject response = JObject.Parse(reader.ReadToEnd());
            return response;
        }
    }
}

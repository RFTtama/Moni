using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System;
using System.Net.Http;

namespace Moni
{
    public class APImanager
    {
        private JObject _jsonData;
        public JObject jsonData
        {
            get
            {
                return _jsonData;
            }
        }

        public void ResetJsonData()
        {
            _jsonData = null;
        }

        public async void RequestApi(string requestUrl)
        {
            ResetJsonData();
            //requestUrl = WebUtility.UrlEncode(requestUrl);
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "moni");
                var res = await client.GetAsync(requestUrl);
                var response = await res.Content.ReadAsStringAsync();
                _jsonData = JObject.Parse(response);
            }
        }
    }
}

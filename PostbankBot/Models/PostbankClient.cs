using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PostbankBot.Models
{
    public class PostbankClient
    {
       
        private string password;
        public string Username { get; set; }
        public PostbankId IDInfo { get; set; }
        private TokenHolder tokenHolder;

        public PostbankClient(string username, string password)
        {
            this.Username = username;
            this.password = password;

        }

        public async Task<PostbankClient> GetAccountInformationAsnyc()
        {
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 certificate = X509Certificate.CreateFromCertFile
            handler.ClientCertificates.Add(certificate);

            using (var client = new HttpClient(handler))
            {
                //client.BaseAddress = new Uri($"https://vision.googleapis.com/v1/images:annotate?key={_APIKey}");
                client.BaseAddress = new Uri($"https://hackathon.postbank.de:443/bank-api/blau/postbankid/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync($"token?username={Username}&password={password}", new StringContent("", Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tokenHolder = (TokenHolder)JsonConvert.DeserializeObject(json, typeof(TokenHolder));

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("x-auth", tokenHolder.token);
                    response = await client.PostAsync($"token?username={Username}&password={password}", new StringContent("", Encoding.UTF8, "application/json"));
                    if(response.IsSuccessStatusCode)
                    {
                        json = await response.Content.ReadAsStringAsync();
                        IDInfo = (PostbankId)JsonConvert.DeserializeObject(json, typeof(PostbankId));
                    }
                    return this;
                }
            }

            return null;
        }


    }
}
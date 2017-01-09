using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Postbank
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

        public async Task<PostbankTransactions> GetTransactionForAccount(PostbankAccount account)
        {
            //WebRequestHandler handler = new WebRequestHandler();
            //var path = HttpContext.Current.Server.MapPath("/Cert/PBS_TESTCLIENT_T2.cer");
            //X509Certificate certificate = X509Certificate.CreateFromCertFile(path);
            //handler.ClientCertificates.Add(certificate);

            using (HttpClient client = NewClient(sha1Thumbprint: "1ccf10b487ba0bd9f7adbf674441a85ecd1d4a53"))
            {
                //client.BaseAddress = new Uri($"https://hackathon.postbank.de:443/bank-api/blau/postbankid/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PostAsync($"token?username={Username}&password={password}", new StringContent("", Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        tokenHolder = (TokenHolder)JsonConvert.DeserializeObject(json, typeof(TokenHolder));

                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("x-auth", tokenHolder.token);
                        response = await client.GetAsync($"accounts/giro/{account.iban}/transactions");
                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            return (PostbankTransactions)JsonConvert.DeserializeObject(json, typeof(PostbankTransactions));
                        }
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public static HttpClient NewClient(string sha1Thumbprint)
        {
            var handler = new WebRequestHandler();
            X509Certificate2 cert;

            using (var store = new X509Store(storeName: StoreName.My, storeLocation: StoreLocation.LocalMachine))
            { 

                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certs = store.Certificates.Find(
                    findType: X509FindType.FindByThumbprint,
                    findValue: sha1Thumbprint,
                    validOnly: false);
                if (certs.Count == 0) { throw new NotSupportedException($"Could not find cert {sha1Thumbprint}"); }
                if (certs.Count > 1) { throw new NotSupportedException($"cert {sha1Thumbprint} was not unique?!?"); }

                cert = certs[0];            }

            handler.ClientCertificates.Add(cert);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<PostbankClient> GetAccountInformationAsnyc()
        {
            //WebRequestHandler handler = new WebRequestHandler();
            //var path = HttpContext.Current.Server.MapPath("/Cert/PBS_TESTCLIENT_T2.cer");
            //X509Certificate certificate = X509Certificate.CreateFromCertFile(path);
            //handler.ClientCertificates.Add(certificate);

            using (HttpClient client = NewClient(sha1Thumbprint: "1ccf10b487ba0bd9f7adbf674441a85ecd1d4a53"))
            {
                //client.BaseAddress = new Uri($"https://hackathon.postbank.de:443/bank-api/blau/postbankid/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PostAsync($"token?username={Username}&password={password}", new StringContent("", Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        tokenHolder = (TokenHolder)JsonConvert.DeserializeObject(json, typeof(TokenHolder));

                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("x-auth", tokenHolder.token);
                        response = await client.GetAsync("");
                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            IDInfo = (PostbankId)JsonConvert.DeserializeObject(json, typeof(PostbankId));
                        }
                        return this;
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }

            return null;
        }


    }
}
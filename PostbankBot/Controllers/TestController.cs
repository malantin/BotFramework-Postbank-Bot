using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PostbankBot.Models;
using System.Threading.Tasks;
using Postbank;

namespace PostbankBot.Controllers
{
    public class TestController : ApiController
    {
        PostbankClient client;

        // GET: api/Test
        public async Task<PostbankAccount> Get()
        {
            //return new string[] { "value1", "value2" };
            client = new PostbankClient("Hackathon5", "test12345");
            client = await client.GetAccountInformationAsnyc();
            if (client != null)
            {
                return client.IDInfo.accounts[0];
            }
            else
            {
                return null;
            }
        }

        // GET: api/Test/5
        public async Task<string> Get(int id)
        {
            try
            {
                client = new PostbankClient("Hackathon5", "test12345");
                client = await client.GetAccountInformationAsnyc();
                return client.Username;
            }
            catch(Exception e)
            {
                throw;
                return e.Message;
            }
        }

        // POST: api/Test
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Test/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Test/5
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PostbankBot.Models;
using System.Threading.Tasks;

namespace PostbankBot.Controllers
{
    public class TestController : ApiController
    {
        PostbankClient client;

        // GET: api/Test
        public async Task<PostbankClient> Get()
        {
            //return new string[] { "value1", "value2" };
            client = new PostbankClient("Hackathon5", "test12345");
            client = await client.GetAccountInformationAsnyc();
            if (client != null)
            {
                return client;
            }
            else
            {
                return null;
            }
        }

        // GET: api/Test/5
        public string Get(int id)
        {
            return "value";
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

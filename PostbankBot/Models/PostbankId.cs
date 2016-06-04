using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostbankBot.Models
{
    public class PostbankId
    {
        public List<object> messages { get; set; }
        public string name { get; set; }
        public List<PostbankAccount> accounts { get; set; }
        public List<Creditcard> creditcards { get; set; }
        public List<Link> links { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostbankBot.Models
{
    public class TokenHolder
    {
        public List<object> messages { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
        public List<Link> links { get; set; }
    }
}
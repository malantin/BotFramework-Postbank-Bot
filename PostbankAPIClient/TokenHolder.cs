using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postbank
{
    public class TokenHolder
    {
        public List<object> messages { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
        public List<PostbankLink> links { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Postbank
{
    public class PostbankReference
    {
        public List<object> messages { get; set; }
        public object iban { get; set; }
        public object bic { get; set; }
        public string paymentName { get; set; }
        public string bankName { get; set; }
        public string accountNumber { get; set; }
        public string accountHolder { get; set; }
        public List<PostbankLink> links { get; set; }
    }

}
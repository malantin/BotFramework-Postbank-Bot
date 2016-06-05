using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostbankBot.Models
{
    public class PostbankTransactionContent
    {
        public List<object> messages { get; set; }
        public string transactionId { get; set; }
        public string amount { get; set; }
        public string balance { get; set; }
        public string currency { get; set; }
        public List<string> purpose { get; set; }
        public object bookingDate { get; set; }
        public long valutaDate { get; set; }
        public PostbankReference reference { get; set; }
        public string transactionType { get; set; }
        public object transactionDetail { get; set; }
        public List<PostbankLink> links { get; set; }
    }
}
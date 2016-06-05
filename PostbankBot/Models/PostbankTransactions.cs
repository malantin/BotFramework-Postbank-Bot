using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostbankBot.Models
{
    public class PostbankTransactions
    {
        public List<object> messages { get; set; }
        public List<PostbankLink> links { get; set; }
        public List<PostbankTransactionContent> content { get; set; }
        public PostbankTransactionPage page { get; set; }
    }

}
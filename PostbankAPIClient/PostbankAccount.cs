using System.Collections.Generic;

namespace Postbank
{
    public class PostbankAccount
    {
        public List<object> messages { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public string paymentName { get; set; }
        public string bankName { get; set; }
        public string accountNumber { get; set; }
        public string accountHolder { get; set; }
        public string productType { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string accountType { get; set; }
        public string ownerType { get; set; }
        public string productDescription { get; set; }
        public object accountReference { get; set; }
        public string creditLimit { get; set; }
        public object futureSales { get; set; }
        public object amountAvailable { get; set; }
        public object currentlyNotAvailableAmount { get; set; }
        public List<PostbankLink> links { get; set; }
    }
}
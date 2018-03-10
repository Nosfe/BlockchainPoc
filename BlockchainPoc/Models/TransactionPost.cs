using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainPoc.Models
{
    public class TransactionPost
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public int Amount { get; set; }
    }
}
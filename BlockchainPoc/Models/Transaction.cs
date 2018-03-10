using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainPoc.Models
{
    public class Transaction
    {
        public byte[] Sender { get; set; }
        public byte[] Recipient { get; set; }
        public int Amount { get; set; }
    }
}
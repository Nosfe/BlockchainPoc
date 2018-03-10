using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainPoc.Models
{
    public class Block
    {
        public Int64 Index { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> BlockTransactions { get; set; }
        public Int64 ProofOfWork { get; set; }
        public byte[] PreviousHash { get; set; }

    }
}
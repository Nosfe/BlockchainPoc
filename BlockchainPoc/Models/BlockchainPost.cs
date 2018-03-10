using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainPoc.Models
{
    public class BlockchainPost
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> CurrentTransations { get; set; }

        [JsonIgnore]
        public List<string> Nodes { get; set; }
    }
}
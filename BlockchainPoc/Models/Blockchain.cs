using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainPoc.Models
{
    public static class Blockchain
    {
        public static List<Block> Chain { get; set; }
        public static List<Transaction> CurrentTransations { get; set; }

        [JsonIgnore]
        public static List<string> Nodes { get; set; }

        public static void InitBlockchain()
        {
            Chain = new List<Block>();
            CurrentTransations = new List<Transaction>();

            //Block 0
            Chain.Add(new Block
            {
                Index = 0,
                Timestamp = DateTime.Now,
                PreviousHash = new byte[] { 0 },
                ProofOfWork = 100
            });
        }

        public static void AddNode(string node)
        {
            Nodes.Add(node);
            Nodes.RemoveAll(item => item == node);
        }

        public static Block GetLastBlock()
        {
            return Chain.Last();
        }

        public static Block AddNewBlock(Int64 proof, byte[] previousHash)
        {

            Block blockToAdd = new Block
            {
                Index = Chain.Last().Index + 1,
                Timestamp = DateTime.Now,
                BlockTransactions = CurrentTransations,
                ProofOfWork = proof,
                PreviousHash = previousHash
            };
            
            //On vide la liste des transactions en cours
            CurrentTransations = new List<Transaction>();
            Chain.Add(blockToAdd);

            return blockToAdd;
        }

        public static Transaction AddNewTransaction(byte[] sender, byte[] recipient, int amount)
        {
            Transaction transactionToAdd = new Transaction
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };

            CurrentTransations.Add(transactionToAdd);

            //On renvoie le nouvel index du futur block
            return transactionToAdd;
        }

        public static Int64 GetProofOfWork(Int64 oldProof)
        {
            return Util.Hash.ProofOfWork(oldProof);
        }

    }
}
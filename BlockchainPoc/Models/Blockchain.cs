using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public static bool AddNode(string node)
        {
            Regex regex = new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");

            if (regex.Match(node).Success)
            {
                Nodes.RemoveAll(item => item == node);
                Nodes.Add(node);
                return true;
            }

            return false;
        }

        public static bool ValidBlockchain(List<Block> chainToValidate)
        {
            int i = 0;

            while (i < chainToValidate.Count)
            {
                if (chainToValidate[i++].PreviousHash != Util.Hash.HashBlock(chainToValidate[i]))
                {
                    return false;
                }
                if (!Util.Hash.ValidateProof(chainToValidate[i].ProofOfWork, chainToValidate[i++].ProofOfWork))
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        public static void ResolveConfilct()
        {
            List<Block> foreignblockchain = new List<Block>();

            foreach (var node in Nodes)
            {
                try
                {
                    foreignblockchain = AsyncTaskCallWebAPIAsync(node + "/chain").Result;
                    if (foreignblockchain.Count > Chain.Count && ValidBlockchain(foreignblockchain))
                    {
                        Chain = foreignblockchain;
                    }
                }
                catch (WebException ex)
                {
                    throw;
                }
            }
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

        private static async Task<List<Block>> AsyncTaskCallWebAPIAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync(url);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    return JsonConvert.DeserializeObject<List<Block>>(await httpResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    return new List<Block>();
                }
            }
        }
    }
}
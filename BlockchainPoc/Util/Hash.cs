using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using BlockchainPoc.Models;
using Newtonsoft.Json;

namespace BlockchainPoc.Util
{
    public static class Hash
    {
        public static byte[] HashBlock(Block block)
        {
            string json = JsonConvert.SerializeObject(block);

            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] hash = sha512.ComputeHash(bytes);

            return hash;
        }

        public static byte[] HashString(string stringToHash)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(stringToHash);
            byte[] hash = sha512.ComputeHash(bytes);

            return hash;
        }

        public static Int64 ProofOfWork(Int64 oldProof)
        {
            Int64 proof = 0;

            while (!ValidateProof(oldProof, proof))
            {
                proof++;
            }

            return proof;
        }

        public static bool ValidateProof(Int64 oldProof, Int64 proof)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(oldProof.ToString() + proof.ToString());
            byte[] hash = sha512.ComputeHash(bytes);

            return GetStringFromHash(hash).StartsWith("0007");
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
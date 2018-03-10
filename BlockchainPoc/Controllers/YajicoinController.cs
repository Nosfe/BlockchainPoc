using BlockchainPoc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlockchainPoc.Controllers
{
    [RoutePrefix("Yajicoin")]
    public class YajicoinController : ApiController
    {
        [HttpGet]
        [Route("mine/{identifier}")]
        public Block Mine(string identifier)
        {
            Block lastBlock = Blockchain.GetLastBlock();
            Int64 proofOfWork = Blockchain.GetProofOfWork(lastBlock.ProofOfWork);
            
            Blockchain.AddNewTransaction(new byte[0], Util.Hash.HashString(identifier), 1);

            return Blockchain.AddNewBlock(proofOfWork, Util.Hash.HashBlock(lastBlock));
        }

        [HttpGet]
        [Route("chain")]
        public List<Block> Chain()
        {
            return Blockchain.Chain;
        }

        [HttpPost]
        [Route("transaction/new")]
        public Transaction Post([FromBody][Required]TransactionPost transaction)
        {
            if (ModelState.IsValid)
            {
                return Blockchain.AddNewTransaction(Util.Hash.HashString(transaction.Sender), Util.Hash.HashString(transaction.Recipient), transaction.Amount);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}

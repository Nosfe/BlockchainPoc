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
    [RoutePrefix("Yajicoin/node")]
    public class NodeController : ApiController
    {
        [HttpPost]
        [Route("add")]
        public bool RegisterNode([FromBody][Required]string node)
        {
            if (ModelState.IsValid)
            {
                Blockchain.AddNode(node);
                return true;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("resolve")]
        public List<Block> Resolve([FromBody][Required]BlockchainPost blockchain)
        {
            if (ModelState.IsValid)
            {
                return new List<Block>();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}

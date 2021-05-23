using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreAPI.Core.Exceptions;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Core.Model.Payloads;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ILogger<PurchasesController> logger;

        public PurchasesController(IRepository repository, ILogger<PurchasesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new purchase record.
        /// </summary>
        /// <response code="200">The purchase record has successfully been stored.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        /// <response code="404">The user that performed the purchase does not exist.</response>
        [HttpPost(Name = "CreatePurchase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePurchaseRecordAsync([FromBody] CreatePurchaseRecordPayload purchaseRecordPayload)
        {
            this.logger.LogTrace($"POST PurchaseRecord.");

            try
            {
                await this.repository.PerformPurchaseAsync(purchaseRecordPayload);
                return this.Ok();
            }
            catch (DocumentNotFoundException<int>)
            {
                return this.NotFound();
            }
        }
    }
}

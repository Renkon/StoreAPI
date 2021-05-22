﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreAPI.Core.Model.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ILogger<PurchasesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a new purchase record.
        /// </summary>
        /// <response code="201">The purchase record has successfully been stored.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePurchaseRecordAsync([FromBody] CreatePurchaseRecordPayload userPayload)
        {
            // TODO.
            return null;
        }
    }
}

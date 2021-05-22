using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StoreAPI.Core.Dto;
using StoreAPI.Core.Exceptions;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Core.Model.Payloads;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ILogger<UsersController> logger;

        public UsersController(IRepository repository, ILogger<UsersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves an user by its nationalId.
        /// </summary>
        /// <param name="id">National id of the user.</param>
        /// <response code="200">Returns an user whose id matches the nationalId of the request.</response>        
        /// <response code="404">The user was not found on the database.</response>        
        [HttpGet("{nationalId}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserAsync(string nationalId)
        {
            this.logger.LogTrace($"GET User with nationalId {nationalId}.");
            // TODO.
            return null;
        }

        /// <summary>
        /// Retrieves a full list of all the users.
        /// </summary>
        /// <response code="200">Returns a list of users from the application.</response>        
        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            this.logger.LogTrace("GET Users.");
            // TODO.
            return null;
        }

        /// <summary>
        /// Creates a new user on the platform.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="201">Returns the newly created user.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        [HttpPost(Name = "CreateUser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody]CreateUserPayload userPayload)
        {
            this.logger.LogTrace("POST User.");

            try
            {
                var user = await this.repository.CreateUserAsync(userPayload);
                return this.CreatedAtRoute("GetUser", new { user.NationalId }, user);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey && ex.WriteError.Code == 11000)
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Updates an existing user on the platform.
        /// </summary>
        /// <param name="id">NationalId of the user.</param>
        /// <response code="200">Returns the user that has been successfully modified.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        /// <response code="404">The user was not found on the database.</response>
        [HttpPut("{nationalId}", Name = "UpdateUser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(int nationalId, [FromBody]UpdateUserPayload userPayload)
        {
            this.logger.LogTrace($"PUT User with nationalId {nationalId}.");
            
            try
            {
                var user = await this.repository.UpdateUserAsync(nationalId, userPayload);
                return this.Ok(user);
            }
            catch (DocumentNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes an user from the platform.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="200">The user was successfully removed from the database.</response>        
        /// <response code="404">The user was not found on the database.</response>
        [HttpDelete("{nationalId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> DeleteUserAsync(int nationalId)
        {
            this.logger.LogTrace($"DELETE Users with nationalId {nationalId}.");
            // TODO.
            return null;
        }
    }
}

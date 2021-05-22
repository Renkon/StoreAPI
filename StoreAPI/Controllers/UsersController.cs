using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreAPI.Core.Dto;
using StoreAPI.Core.Model.Payloads;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retrieves an user by its id.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="200">Returns an user whose id matches the id of the request.</response>        
        /// <response code="404">The user was not found on the database.</response>        
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserAsync(string id)
        {
            // TODO.
            return null;
        }

        /// <summary>
        /// Retrieves a full list of all the users.
        /// </summary>
        /// <response code="200">Returns a list of users from the application.</response>        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            // TODO.
            return null;
        }

        /// <summary>
        /// Creates a new user on the platform.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="201">Returns the newly created user.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody]UpsertUserPayload userPayload)
        {
            // TODO.
            return null;
        }

        /// <summary>
        /// Updates an existing user on the platform.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="200">Returns the user that has been successfully modified.</response>        
        /// <response code="400">The payload received did not match the expected payload and thus the user was not created.</response>        
        /// <response code="404">The user was not found on the database.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(string id, [FromBody] UpsertUserPayload userPayload)
        {
            // TODO.
            return null;
        }

        /// <summary>
        /// Deletes an user from the platform.
        /// </summary>
        /// <param name="id">Id of the user.</param>
        /// <response code="200">The user was successfully removed from the database.</response>        
        /// <response code="404">The user was not found on the database.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> DeleteUserAsync(string id)
        {
            // TODO.
            return null;
        }
    }
}

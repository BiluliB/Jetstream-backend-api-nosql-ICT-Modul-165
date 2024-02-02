using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.DTOs.Responses;
using JetStreamApiMongoDb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace JetStreamApiMongoDb.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns>Ok, Unauthorized, BadRequest, StatusCode</returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var isAuthentiacted = await _userService.Authenticate(userLoginDTO.UserName, userLoginDTO.Password);
                if (!isAuthentiacted)
                {
                    return Unauthorized("Benutzername oder Passwort ist falsch.");
                }
                var user = await _userService.GetUserByUsername(userLoginDTO.UserName);
                var token = _tokenService.GenerateToken(userLoginDTO.UserName, user.Role.ToString());

                return Ok(new
                {
                    Username = userLoginDTO.UserName,
                    Token = token
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userCreateDTO"></param>
        /// <returns>Ok,BadRequest</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCreateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                await _userService.CreateUser(userCreateDTO.UserName, userCreateDTO.Password, userCreateDTO.Role);
                return Ok($"Benutzer {userCreateDTO.UserName} mit der Rolle {userCreateDTO.Role} wurde erfolgreich erstellt.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Unlocks a user.
        /// </summary>
        /// <param name="userUnlockDTO"></param>
        /// <returns>Ok, BadRequest, StatusCode</returns>
        [HttpPost("unlock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserUnlockDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult> Unlock([FromBody] UserUnlockDTO userUnlockDTO)
        {
            try
            {
                await _userService.UnlockUser(userUnlockDTO.UserName);
                return Ok($"Benutzer {userUnlockDTO.UserName} wurde erfolgreich entsperrt.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Not Found, userDTO, BadRequest, Invalid Id format</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            try
            {
                var userDTOs = await _userService.GetAll();
                if (userDTOs == null)
                {
                    return NotFound();
                }
                return Ok(userDTOs);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Id format.");
            }
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Not Found, userDTO, BadRequest, Invalid Id format</returns>
        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            try
            {
                var userDTO = await _userService.GetById(id);
                if (userDTO == null)
                {
                    return NotFound();
                }
                return Ok(userDTO);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Id format.");
            }
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Invalid Id format and ex.Message</returns>
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var objectId = new ObjectId(id);
                var user = await _userService.GetById(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                await _userService.Delete(objectId);
                return Ok(new
                {
                    Benutzer = user.Name,
                    Message = "Erfolgreich gelöscht"
                });

            }
            catch (FormatException)
            {
                return BadRequest("Invalid Id format.");    
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}


using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var isAuthentiacted = await _userService.AuthenticateAsync(userLoginDTO.UserName, userLoginDTO.Password );
                if (!isAuthentiacted)
                {
                    return Unauthorized("Benutzername oder Passwort ist falsch.");
                }
                var user = await _userService.GetUserByUsernameAsync(userLoginDTO.UserName);
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

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserCreateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN")]

        public async Task<ActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                await _userService.CreateUserAsync(userCreateDTO.UserName, userCreateDTO.Password, userCreateDTO.Role);
                return Ok($"Benutzer {userCreateDTO.UserName} mit der Rolle {userCreateDTO.Role} wurde erfolgreich erstellt.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unlock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserUnlockDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult> Unlock([FromBody] UserUnlockDTO userUnlockDTO)
        {
            try
            {
                await _userService.UnlockUserAsync(userUnlockDTO.UserName);
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
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.BusinessLogic;
using RestfulApi.DTOs;
using RestfulApi.Exceptions;
using RestfulApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace RestfulApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtDataControl _jwtDataControl;
        public AuthenticationController(IJwtDataControl jwtDataControl)
        {
            _jwtDataControl = jwtDataControl;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length <= 5 ||
                    string.IsNullOrWhiteSpace(model.Username) || model.Username.Length <= 5)
                {
                    return BadRequest("Invalid Username or Password");
                }

                AccessTokenResponse response = await _jwtDataControl.Login(model);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest("Invalid Username or Password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAccessToken(RefreshTokenDTO token)
        {
            try
            {
                AccessTokenResponse response = await _jwtDataControl.UseRefreshToken(token.RefreshToken);

                if (response != null)
                {
                    return Ok(response);

                }

                return Conflict("Refresh Token Expired");

            }
            catch (ReusedTokenException)
            {
                return Unauthorized();
            }
            catch (TokenNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        [HttpGet("VerifyToken")]
        [Authorize]
        public ActionResult VerifyToken()
        {
            return Ok();
        }
        }
}

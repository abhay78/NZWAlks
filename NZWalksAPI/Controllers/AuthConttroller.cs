using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //step23
    public class AuthConttroller : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthConttroller(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var idetityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identyResult = await userManager.CreateAsync(idetityUser, registerRequestDto.Password);

            if (identyResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDto.Roles != null & registerRequestDto.Roles.Any())
                {
                    identyResult = await userManager.AddToRolesAsync(idetityUser, registerRequestDto.Roles);

                    if (identyResult.Succeeded)
                    {
                        return Ok("User was registered ! Please Login.");
                    }
                }

            }
            return BadRequest("Something went wrong !");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if(user != null) 
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        //Generate token
                        var jwtToken =  tokenRepository.CreateJWtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or password incorreck");

            
        }

    }
}

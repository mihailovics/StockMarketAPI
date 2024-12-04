using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        public AccountController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                return BadRequest(ModelState);
                var user = new User
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email
                };

                var createUser = await userManager.CreateAsync(user, registerDTO.Password);

                if(createUser.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok("User created");
                    }
                    else 
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
    
}
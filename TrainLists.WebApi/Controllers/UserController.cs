using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainLists.WebApi.Modules;
using TrainLists.WebApi.ViewModels;

namespace TrainLists.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        
        List<LoginRequest> users = new List<LoginRequest> {
            new LoginRequest {Login = "admin" , Password = "12345"}
        };

        public UserController()
        {
            
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest model)
        {
            if(users.Any(x=>x.Login == model.Login && x.Password == model.Password))
            {
                var user = users.FirstOrDefault(x=>x.Login == model.Login && x.Password == model.Password);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                    
                };

                var token = AuthManager.GenerateToken(claims);

                return Ok(new LoginViewModel {Token = token});
            }

            return BadRequest("Не верный логин или пароль");

        }


    }
}
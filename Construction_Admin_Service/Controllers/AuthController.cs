using Construction_Admin_Service.Data;
using Construction_Admin_Service.Dtos;
using Construction_Admin_Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Admin_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto user)
        {
            var response = await _authRepo.Register(new User { Username = user.Username }, user.Password);
            if (!response.Success)
            {
                return Ok(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto user)
        {
 
            var response = await _authRepo.Login(user.Username, user.Password);
            if (!response.Success)
            {
                return Ok(response);
            }
            //HttpContext.Session.SetString("UserName", response.Data.ToString());
            return Ok(response);
        }

    }
}

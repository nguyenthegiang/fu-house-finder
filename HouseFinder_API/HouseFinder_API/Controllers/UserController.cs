using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseFinder_API.Authentication;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using BusinessObjects;

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthentication auth;
        private IConfiguration Configuration;
        public UserController(IAuthentication auth, IConfiguration configuration)
        {
            this.auth = auth;
            this.Configuration = configuration;
        }
        private IUserReposiotry userReposiotry = new UserRepository();

        [HttpGet("{UserId}")]
        public IActionResult GetUserById(string UserId)
        {
            UserDTO userDTO = userReposiotry.GetUserByID(UserId);
            if (userDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userDTO);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (login.GoogleUserId != null)
            {
                try
                {
                    var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new string[] { Configuration.GetSection("Google").GetSection("Audience").Value }
                    };
                    var payload = await GoogleJsonWebSignature.ValidateAsync(login.GoogleUserId, validationSettings);
                    login.GoogleUserId = payload.Subject;
                }
                catch (Exception)
                {
                    return Ok(new { Status = 500 });
                }
            }
            ResponseDTO user = userReposiotry.Login(login);
            if (user == null)
                return Ok(new { Status = 404 });
            if (user.StatusId == 0)
            {
                return Ok(new { Status = 403 });
            }
            string token = this.auth.Authenticate(user);
            if (token == null)
                return Ok(new { Status = 404 });
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("User", user.UserId);
            HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            user.AccessToken = token;
            return Ok(new { Status = 200, User = user } );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (register.GoogleIdToken != null)
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[] { Configuration.GetSection("Google").GetSection("Audience").Value }
                };
                try
                {
                    var payload = await GoogleJsonWebSignature.ValidateAsync(register.GoogleIdToken, validationSettings);
                    register.GoogleUserId = payload.Subject;
                    register.Email = payload.Email;
                    register.DisplayName = payload.Name;
                }
                catch (Exception)
                {
                    return Ok(new { Status = 500 });
                }
            }
            ResponseDTO user = userReposiotry.Register(register);
            string token = this.auth.Authenticate(user);
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("User", user.UserId);
            HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            user.AccessToken = token;
            return Ok(new { Status = 200, User = user });
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuthorize()
        {
            return Ok();
        }

        [HttpGet("generate_password")]
        public IActionResult GeneratePassword(string password)
        {
            User user = new User();
            PasswordHasher<User> pw = new PasswordHasher<User>();
            var result = new { Password = pw.HashPassword(user, password) };
            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

        //[Staff][Dashboard] Get list of all landlords
        [HttpGet("landlord")]
        public IActionResult GetLandlords()
        {
            List<UserDTO> landlords = userReposiotry.GetLandlords();
            if (landlords == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(landlords);
            }
        }

        [HttpGet("CountTotalLandlord")]
        public int? CountTotalLandlord()
        {
            return userReposiotry.CountTotalLandlord();
        }
        [HttpGet("CountActiveLandlord")]
        public int? CountActiveLandlord()
        {
            return userReposiotry.CountActiveLandlord();
        }
        [HttpGet("CountInactiveLandlord")]
        public int? CountInactiveLandlord()
        {
            return userReposiotry.CountInactiveLandlord();
        }

        //[Head][Dashboard] Get list of all landlords
        //[HttpGet("staff")]
        //public IActionResult GetStaffs()
        //{
        //    List<UserDTO> staffs = userReposiotry.GetStaffs();
        //    if (staffs == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return Ok(staffs);
        //    }
        //}

    }
}

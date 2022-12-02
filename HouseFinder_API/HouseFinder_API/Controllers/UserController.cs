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
            try
            {
                if (login.GoogleUserId != null)
                {
                    var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new string[] { Configuration.GetSection("Google").GetSection("Audience").Value }
                    };
                    var payload = await GoogleJsonWebSignature.ValidateAsync(login.GoogleUserId, validationSettings);
                    login.GoogleUserId = payload.Subject;
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
                if (user.StatusId == 2)
                {
                    return Ok(new { Status = 201, User = user });
                }
                return Ok(new { Status = 200, User = user });
            }
            catch (Exception)
            {
                return Ok(new { Status = 500, Message = "Login Error!" });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            try
            {
                if (register.GoogleIdToken != null)
                {
                    var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new string[] { Configuration.GetSection("Google").GetSection("Audience").Value }
                    };
                    var payload = await GoogleJsonWebSignature.ValidateAsync(register.GoogleIdToken, validationSettings);
                    register.GoogleUserId = payload.Subject;
                    register.Email = payload.Email;
                    register.DisplayName = payload.Name;
                }
                ResponseDTO user = userReposiotry.Register(register);
                string token = this.auth.Authenticate(user);
                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("User", user.UserId);
                HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                user.AccessToken = token;
                return Ok(new { Status = 200, User = user });
            }
            catch (Exception)
            {
                return Ok(new { Status = 500, Message = "Register Error!" });
            }
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuthorize()
        {
            return Ok(new { UID = HttpContext.Session.GetString("User") });
        }

        [HttpGet("generate_password")]
        public IActionResult GeneratePassword(string password)
        {
            User user = new User();
            PasswordHasher<User> pw = new PasswordHasher<User>();
            var result = new { Password = pw.HashPassword(user, password) };
            return Ok(result);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            foreach (var cookies in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookies);
            }
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

        [HttpGet("LandlordSignupRequest")]
        public ActionResult<IEnumerable<UserDTO>> GetLandlordSignupRequest() => userReposiotry.GetLandlordSignupRequest();

        [Authorize]
        [HttpPut("{userId}/{statusId}")]
        public IActionResult UpdateUserStatus(string userId, int statusId)
        {
            try
            {
                //Get user id from Session as Staff that makes this update
                string uid = HttpContext.Session.GetString("User");
                if (uid == null)
                {
                    return Forbid();
                }

                //Update to Database
                userReposiotry.UpdateUserStatus(userId, statusId, uid);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

    }
}

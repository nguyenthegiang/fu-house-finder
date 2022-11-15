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

namespace HouseFinder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthentication auth;
        public UserController(IAuthentication auth)
        {
            this.auth = auth;
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
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[] { "919349682446-etrauq4d5cluclesaifkcr4bnh4gru2j.apps.googleusercontent.com" }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(login.GoogleUserId, validationSettings);
                login.GoogleUserId = payload.Subject;
                Console.WriteLine(payload.Subject);
            }
            ResponseDTO user = userReposiotry.Login(login);
            if (login.Email != null && login.Password != null && user == null)
                return Forbid();
            else if (user == null)
                return NotFound();
            string token = this.auth.Authenticate(user);
            if (token == null)
                return NotFound();
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("User", user.UserId);
            user.AccessToken = token;
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            Console.WriteLine(register.GoogleIdToken);
            if (register.GoogleIdToken != null)
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[] { "919349682446-etrauq4d5cluclesaifkcr4bnh4gru2j.apps.googleusercontent.com" }
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
            user.AccessToken = token;
            return Ok(user);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet("test")]
        public IActionResult TestAuthorize()
        {
            return Ok();
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
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

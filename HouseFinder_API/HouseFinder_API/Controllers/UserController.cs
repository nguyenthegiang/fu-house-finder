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
        private IUserRepository userRepository = new UserRepository();

        public UserController(IAuthentication auth, IConfiguration configuration)
        {
            this.auth = auth;
            this.Configuration = configuration;
        }

        [HttpGet("{UserId}")]
        public IActionResult GetUserById(string UserId)
        {
            UserDTO userDTO = userRepository.GetUserByID(UserId);
            if (userDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userDTO);
            }
        }

        /**
         * [Login] Login
         */
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
                //Login with Google
                if (login.GoogleUserId != null)
                {
                    var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new string[] { Configuration.GetSection("Google").GetSection("Audience").Value }
                    };
                    var payload = await GoogleJsonWebSignature.ValidateAsync(login.GoogleUserId, validationSettings);
                    login.GoogleUserId = payload.Subject;
                }

                //Admin Login
                ResponseDTO user;
                //Check with account in appsettings
                if (login.Email == Configuration.GetSection("AdminAccount").GetSection("Email").Value
                    && login.Password == Configuration.GetSection("AdminAccount").GetSection("Password").Value)
                {
                    user = new ResponseDTO();
                    user.UserId = "0";
                    user.Email = Configuration.GetSection("AdminAccount").GetSection("Email").Value;
                    user.DisplayName = "Admin";
                    user.RoleName = "Admin";
                    user.StatusId = 1;
                }
                else
                //User Login: Call to DB
                {
                    user = userRepository.Login(login);
                }

                //Response: Not found User
                if (user == null)
                    return Ok(new { Status = 404 });

                //Response: Disabled account
                if (user.StatusId == 0)
                {
                    return Ok(new { Status = 403 });
                }

                //Create token
                string token = this.auth.Authenticate(user);
                if (token == null)
                    return Ok(new { Status = 404 });

                //Set data to Session
                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("User", user.UserId);
                HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                //Response: Landlord Signup Request pending
                if (user.StatusId == 2)
                {
                    return Ok(new { Status = 201, User = user });
                }

                //Response: Allow login
                return Ok(new { Status = 200, User = user });
        }

        /**
         * [Login] Register
         */
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
                ResponseDTO user = userRepository.Register(register);
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

        /**
         * [To be Deleted] Demo method for testing authorization
         */
        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuthorize()
        {
            return Ok(new { UID = HttpContext.Session.GetString("User") });
        }

        /**
         * [To be Deleted] Demo method for generating hashed password
         */
        [HttpGet("generate_password")]
        public IActionResult GeneratePassword(string password)
        {
            User user = new User();
            PasswordHasher<User> pw = new PasswordHasher<User>();
            var result = new { Password = pw.HashPassword(user, password) };
            return Ok(result);
        }

        /**
         * [Login] Logout
         * Delete Session
         */
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
            List<UserDTO> landlords = userRepository.GetLandlords();
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
            return userRepository.CountTotalLandlord();
        }

        [HttpGet("CountActiveLandlord")]
        public int? CountActiveLandlord()
        {
            return userRepository.CountActiveLandlord();
        }

        [HttpGet("CountInactiveLandlord")]
        public int? CountInactiveLandlord()
        {
            return userRepository.CountInactiveLandlord();
        }

        [HttpGet("LandlordSignupRequest")]
        public ActionResult<IEnumerable<UserDTO>> GetLandlordSignupRequest() => userRepository.GetLandlordSignupRequest();

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
                userRepository.UpdateUserStatus(userId, statusId, uid);

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        //[Admin][List staffs] Get list of all staffs
        [HttpGet("staff")]
        public IActionResult GetStaffs()
        {
            List<UserDTO> staffs = userRepository.GetStaffs();
            if (staffs == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(staffs);
            }
        }

        //[Admin][List staffs] Get current user
        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
            //Get user id from Session as Staff that makes this update
            string uid = HttpContext.Session.GetString("User");
            if (uid == null)
            {
                return Forbid();
            }

            //Update to Database
            UserDTO userDTO = userRepository.GetUserByID(uid);

            return Ok(userDTO);
        }

        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile(string userId, string name, string email)
        {
            try
            {
                //Update to Database
                userRepository.UpdateProfile(userId, name, email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("changePassword")]
        public IActionResult ChangePassword(string userId, string newPassword)
        {
            try
            {
                //Update to Database
                userRepository.ChangePassword(userId, newPassword);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

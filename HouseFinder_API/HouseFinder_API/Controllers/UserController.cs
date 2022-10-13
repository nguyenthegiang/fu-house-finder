﻿using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepository;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseFinder_API.Helper;
using HouseFinder_API.Authentication;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Login(LoginDTO login)
        {
            login.Password = Hashing.Encrypt(login.Password);
            ResponseDTO user = userReposiotry.Login(login);
            if (login.Email != null && login.Password != null && user == null)
                return Forbid();
            else if (user == null)
                return NotFound();
            string token = this.auth.Authenticate(user);
            if (token == null)
                return NotFound();
            HttpContext.Session.SetString("Token", token);
            return Ok(user);
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO register)
        {
            ResponseDTO user = userReposiotry.Register(register);
            return Ok(user);
        }
        [Authorize]
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
    }
}

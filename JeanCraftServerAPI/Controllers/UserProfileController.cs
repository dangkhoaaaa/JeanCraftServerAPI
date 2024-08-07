﻿using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JeanCraftServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Profile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var account = await _userService.GetUserDTOByID(userId);
            if (account == null)
            {
                return NotFound("User not found.");
            }
            return Ok(account);
        }

        // PUT: api/Profile/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] AccountDTOs? accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = new Account
            {
                UserId = accountDto.UserId,
                UserName = accountDto.UserName,
                PhoneNumber = accountDto.Phonenumber,
                Addresses = accountDto.Addresses.Select(a => new Address
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Detail = a.Detail
                }).ToList()
            };

            var updatedAccount = await _userService.UpdateUserProfile(account);
            if (updatedAccount == null)
            {
                return BadRequest("Unable to update user profile.");
            }
            return Ok(updatedAccount);
        }
    }
}

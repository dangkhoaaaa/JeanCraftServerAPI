using JeanCraftLibrary.Model;
using JeanCraftServerAPI.Util;
using JeanCraftLibrary.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Identity.Data;
using JeanCraftServerAPI.Services;
using System.Globalization;

namespace JeanCraftServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;


        public UserController(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromForm] GoogleLoginForm userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.FullName))
                {
                    return Ok(ResponseHandle<LoginResponse>.Error("Invalid info user to login"));
                }

                var user = await _userService.GetUserByEmail(userDto.Email);
                if (user == null)
                {
                    //REGISTER NEW ACCOUNT
                    user = await _userService.CreateUserGoogle(userDto);
                }
                else
                {
                    if (user.Status == false)
                    {
                        return Ok(ResponseHandle<LoginResponse>.Error("User has been banned."));
                    }
                }

                var token = GenerateJwtToken(user);
                LoginResponse loginResponse = new LoginResponse
                {
                    UserID = user.UserId,
                    FullName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.RoleId == Guid.Parse(JeanCraftLibrary.Model.Constants.ROLE_USER) ? "User" : "Admin",
                    Token = token,
                };

                return Ok(ResponseHandle<LoginResponse>.Success(loginResponse));
            }
            catch (Exception e)
            {
                return Ok(ResponseHandle<LoginResponse>.Error("Error occur in server"));
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDto loginDto)
        {
            var user = await _userService.GetUserByEmail(loginDto.Email);
            if (user == null)
            {
                return Ok(ResponseHandle<LoginResponse>.Error("Incorect username or password"));
            }
            if (user.Status == false)
            {
                return Ok(ResponseHandle<LoginResponse>.Error("User has been banned."));
            }

            if (!VerifyPassword(user.Password, loginDto.Password))
            {
                return Ok(ResponseHandle<LoginResponse>.Error("Incorect username or password"));
            }

            var token = GenerateJwtToken(user);
            LoginResponse loginResponse = new LoginResponse
            {
                UserID = user.UserId,
                FullName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.RoleId == Guid.Parse(JeanCraftLibrary.Model.Constants.ROLE_USER) ? "User" : "Admin",
                Token = token,
            };

            return Ok(ResponseHandle<LoginResponse>.Success(loginResponse));
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromForm] Account userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password) ||  string.IsNullOrEmpty(userDto.Email))
                {
                    return Ok(ResponseHandle<LoginResponse>.Error("Invalid info user to register"));
                }

                if (userDto.Password.Length < 6)
                {
                    return Ok(ResponseHandle<LoginResponse>.Error("Password must contain at least 6 characters."));
                }

                Account user = await _userService.GetUserByEmail(userDto.Email);
                if (user != null)
                {
                    return Ok(ResponseHandle<LoginResponse>.Error("Email already in exists"));
                }
                user = await _userService.RegisterUser("", userDto);


                var token = GenerateJwtToken(user);
                LoginResponse loginResponse = new LoginResponse
                {
                    UserID = user.UserId,
                    FullName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.RoleId == Guid.Parse(JeanCraftLibrary.Model.Constants.ROLE_USER) ? "User" : "Admin",
                    Token = token,
                };

                return Ok(ResponseHandle<LoginResponse>.Success(loginResponse));
            }
            catch (Exception e)
            {
                return Ok(ResponseHandle<LoginResponse>.Error("Error occur in server"));
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassWordRequest request)
        {
            //var user = await _userservice.getuserbyemail(request.email);
            //if(user == null) {
            //    return ok(responsehandle<loginresponse>.error("invalid email"));
            //}
            

            var otp = await _userService.ResetPassWord(request);
            return Ok(ResponseHandle<string>.Success(otp));
        }

        [HttpPost("reset-password-form")]
        public async Task<IActionResult> ResetPassword([FromBody] RPFormRequest request)
        {
            var response = await _userService.ResetPasswordAsync(request);
            if (response.IsSuccess)
            {
                return Ok(new { Message = response.Message });
            }
            return BadRequest(new { Message = response.Message });
        }

        private string GenerateJwtToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "1")
                    // Add more claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("283CZXVU883423WT34GFJ6458MN23878GH2378Y23RH2785Y34THREWJ")), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        [HttpGet("get-all-accounts")]
        public async Task<IActionResult> GetAllAccounts(string? search, int currentPage, int pageSize)
        {
            var (accounts, totalCount) = await _userService.GetAccountsAsync(search, currentPage, pageSize);
            var response = accounts.Select(a => new AccountDTO
            {
                UserId = a.UserId,
                UserName = a.UserName,
                Phonenumber = a.PhoneNumber,
                Email = a.Email,
                Image = a.Image,
                InsDate = a.InsDate,
                Addresses = a.Addresses.Select(addr => new AddressDTO
                {
                    Id = addr.Id,
                    UserId = addr.UserId,
                    Detail = addr.Detail
                }).ToList()
            });

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(new
            {
                Accounts = response,
                TotalPages = totalPages
            });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetTotalAccountsCount()
        {
            var totalCount = await _userService.GetTotalAccountsCountAsync();
            return Ok(new { TotalCount = totalCount });
        }

        [HttpGet("GetAccountsByDate")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccountsByDate([FromQuery] string date)
        {
            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                var accounts = await _userService.GetAccountsByDateAsync(parsedDate);
                if (accounts == null || accounts.Count == 0)
                {
                    return NotFound("No accounts found for the specified date.");
                }
                return Ok(accounts);
            }
            else
            {
                return BadRequest("Invalid date format. Please use 'dd-MM-yyyy'.");
            }
        }

        [HttpGet("GetAccountCountByDate")]
        public async Task<IActionResult> GetAccountCountByDate([FromQuery] string date)
        {
            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                var accountCount = await _userService.GetAccountCountByDateAsync(parsedDate);

                return Ok(new { Date = parsedDate.ToString("dd-MM-yyyy"), AccountCount = accountCount });
            }
            else
            {
                return BadRequest("Invalid date format. Please use 'dd-MM-yyyy'.");
            }
        }
    }
}
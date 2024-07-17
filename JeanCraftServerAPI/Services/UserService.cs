using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System.Net.Mail;
using System.Net;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;
using JeanCraftLibrary;
using Microsoft.AspNetCore.Identity.Data;
using static System.Net.WebRequestMethods;
using JeanCraftLibrary.Repositories;
using System.Linq.Expressions;

namespace JeanCraftServerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Account?> ChangePassword(ChangePWForm userDto)
        {
            Account? userOld = await GetUserByID(userDto.UserID);
            if (userOld == null)
            {
                return null;
            }

            if (!VerifyPassword(userOld.Password, userDto.OldPassword))
            {
                return null;
            }
            return await _unitOfWork.UserRepository.ChangePassword(userOld, HashPassword(userDto.NewPassword));
        }

        public async Task<Account> CreateUserGoogle(GoogleLoginForm userDto)
        {
            Account user = new Account
            {
                UserId = Guid.NewGuid(),
                UserName = userDto.FullName,
                Email = userDto.Email,
                Password = HashPassword("123456"),
                Status = true,
                InsDate = DateTime.Now,
                RoleId = Guid.Parse(Constants.ROLE_USER),
            };
            return await _unitOfWork.UserRepository.CreateUserGoogle(user);
        }

        public async Task<Account?> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmail(email);
        }

        public async Task<Account?> GetUserByID(Guid userID)
        {
            return await _unitOfWork.UserRepository.GetUserByID(userID);
        }

        public async Task<AccountDTO?> GetUserDTOByID(Guid userID)
        {
            return await _unitOfWork.UserRepository.GetUserDTOByID(userID);
        }

        public async Task<Account> RegisterUser(string? fileName, Account user)
        {
            Account account = new Account()
            {
                RoleId = Guid.Parse(Constants.ROLE_USER),
                UserId = Guid.NewGuid(),
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = HashPassword(user.Password),
                Image = user.Image,
                InsDate = DateTime.Now,
                Status = user.Status,
            };
            return await _unitOfWork.UserRepository.RegisterUser(fileName, account);
        }

        public async Task<string> ResetPassWord(ResetPassWordRequest request)
        {
            return SendOTP(request.Email, Constants.MAIL, Constants.SERVER, Constants.PORT, Constants.USERNAME, Constants.PASSWORD);
        }


        public async Task<Account?> UpdateUserProfile(Account user)
        {
            return await _unitOfWork.UserRepository.UpdateUser(user);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string SendOTP(string toEmail, string fromEmail, string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            string otp = GenerateOTP(6);  // Generate a 6-digit OTP

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (MailMessage mail = new MailMessage(fromEmail, toEmail))
                {
                    mail.Subject = "Your OTP for Password Reset";
                    mail.Body = $"Hello,\nYour OTP for password reset is: {otp}\n\nBest Regards";

                    try
                    {
                        client.Send(mail);
                        Console.WriteLine("OTP sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send OTP: " + ex.Message);
                    }
                }
            }
            return otp;
            // Optional: Save OTP to database for later verification
        }

    private string GenerateOTP(int length)
        {
            string numbers = "1234567890";
            Random random = new Random();
            string otp = string.Empty;

            for (int i = 0; i < length; i++)
            {
                otp += numbers[random.Next(numbers.Length)];
            }

            return otp;
        }

        public async Task<RPFormResponse> ResetPasswordAsync(RPFormRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return new RPFormResponse
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.CommitAsync();

            return new RPFormResponse
            {
                IsSuccess = true,
                Message = "Password has been reset successfully."
            };
        }
        public async Task<(IEnumerable<Account>, int)> GetAccountsAsync(string? search, int currentPage, int pageSize)
        {
            return await _unitOfWork.UserRepository.GetAccountsAsync(search, currentPage, pageSize);
        }

        public async Task<int> GetTotalAccountsCountAsync()
        {
            return await _unitOfWork.UserRepository.GetTotalAccountsCountAsync();
        }

        public async Task<IList<AccountDTO>> GetAccountsByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var accounts = await _unitOfWork.UserRepository.GetAllAsync(o => o.InsDate >= startDate && o.InsDate < endDate);
            return accounts.Select(a => new AccountDTO
            {
                UserId = a.UserId,
                UserName = a.UserName,
                Email = a.Email,
                Phonenumber = a.PhoneNumber,
                InsDate = a.InsDate,
            }).ToList();
        }

        public async Task<int> GetAccountCountByDateAsync(DateTime date)
        {
            return await _unitOfWork.UserRepository.GetAccountCountByDateAsync(date);
        }
    }
}


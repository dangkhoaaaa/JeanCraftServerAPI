using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System.Net.Mail;
using System.Net;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;
using JeanCraftLibrary;

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
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Password = HashPassword(user.Password),
                Image = user.Image,
                Status = user.Status,
            };
            return await _unitOfWork.UserRepository.RegisterUser(fileName, user);
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
    }
}

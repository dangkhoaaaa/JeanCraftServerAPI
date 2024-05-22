using JeanCraftServerAPI.Controllers;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Repositories;
using JeanCraftServerAPI.Util;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace JeanCraftServerAPI.Services.Interface
{
    public interface IUserService
    {
        Task<AccountDTO?> GetUserDTOByID(Guid userID);
        Task<Account?> GetUserByID(Guid userID);
        Task<Account?> GetUserByEmail(string email);
        Task<Account?> ChangePassword(ChangePWForm userDto);
        Task<Account> RegisterUser(string? fileName, Account user);
        Task<Account> CreateUserGoogle(GoogleLoginForm userDto);
        Task<Account?> UpdateUserProfile(Account user);
        Task<string> ResetPassWord(ResetPassWordRequest request);
    }
}

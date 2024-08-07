﻿using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using System.Linq.Expressions;

namespace JeanCraftLibrary.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<Account>
    {
        Task<Account> RegisterUser(string? fileName, Account user);
        Task<Account> CreateUserGoogle(Account user);
        Task<AccountDTO?> GetUserDTOByID(Guid userID);
        Task<Account?> GetUserByID(Guid userID);
        Task<Account?> GetUserByEmail(string email);
        Task<Account?> ChangePassword(Account user, string newPassword);
        Task<Account?> UpdateUser(Account user);
        Task<(IEnumerable<Account>, int)> GetAccountsAsync(string? search, int currentPage, int pageSize);
        Task<int> GetTotalAccountsCountAsync();
        Task<int> GetAccountCountByDateAsync(DateTime date);
    }
}

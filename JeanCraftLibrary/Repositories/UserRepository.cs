
using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

using Microsoft.VisualBasic;
using System;
using System.Data.Common;
using System.Drawing;

namespace JeanCraftLibrary.Repositories
{
    public class UserRepository : GenericRepository<Account>, IUserRepository
    {
        private readonly JeanCraftContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account?> ChangePassword(Account user, string newPassword)
        {
            user.Password = newPassword;
            //userOld.LastUpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Account> CreateUserGoogle(Account user)
        {
            await _context.Accounts.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Account?> GetUserByEmail(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AccountDTO?> GetUserDTOByID(Guid userID)
        {
            var account = await _context.Accounts.Include(a => a.Addresses)
            .FirstOrDefaultAsync(a => a.UserId == userID);

            if (account == null)
            {
                return null;
            }
            // Map Account to AccountDTO
            var accountDTO = _mapper.Map<AccountDTO>(account);
            //AddressDTO addressDTO = _mapper.Map<AddressDTO>(address);
            return accountDTO;
        }
        public async Task<Account> RegisterUser(string? fileName, Account user)
        {
            try
            {
                await _context.Accounts.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Account?> UpdateUser(Account user)
        {
            var userToUpdate = await _context.Accounts.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (userToUpdate != null)
            {
                userToUpdate.UserName = user.UserName;
                userToUpdate.Phonenumber = user.Phonenumber;
                userToUpdate.Email = user.Email;
                userToUpdate.Image = user.Image;
                userToUpdate.Password = user.Password;

                _context.Accounts.Update(userToUpdate);
                await _context.SaveChangesAsync();
                return userToUpdate;
            }
            return null;
        }

        public async Task<Account?> GetUserByID(Guid userID)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserId == userID);
        }
    }
}

using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

using Microsoft.VisualBasic;
using System;
using System.Data.Common;
using System.Drawing;
using System.Linq.Expressions;
using System.Linq;

namespace JeanCraftLibrary.Repositories
{
    public class UserRepository : GenericRepository<Account>, IUserRepository
    {
        private readonly JeanCraftContext _context;
        private readonly IMapper _mapper;

        public UserRepository(JeanCraftContext dbContext, IMapper mapper) : base(dbContext)
        {
            _context = dbContext;
            _mapper = mapper;
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
            var userToUpdate = await _context.Accounts
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (userToUpdate != null)
            {
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    userToUpdate.UserName = user.UserName;
                }

                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    userToUpdate.PhoneNumber = user.PhoneNumber;
                }

                if (!string.IsNullOrEmpty(user.Email))
                {
                    userToUpdate.Email = user.Email;
                }

                if (!string.IsNullOrEmpty(user.Image))
                {
                    userToUpdate.Image = user.Image;
                }

                if (!string.IsNullOrEmpty(user.Password))
                {
                    userToUpdate.Password = user.Password;
                }

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

        public async Task<(IEnumerable<Account>, int)> GetAccountsAsync(string? search, int currentPage, int pageSize)
        {
            var query = _context.Set<Account>().Include(a => a.Addresses).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.UserName.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var accounts = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return (accounts, totalCount);
        }

        public async Task<int> GetTotalAccountsCountAsync()
        {
            return await _context.Set<Account>().CountAsync();
        }

        public async Task<int> GetAccountCountByDateAsync(DateTime date)
        {
            var accountCount = await _context.Set<Account>()
                    .Where(o => o.InsDate.Date == date.Date)
                    .CountAsync();
            return accountCount;
        }
    }
}
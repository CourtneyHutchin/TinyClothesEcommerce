using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes
{
    public static class AccountDb
    {
        public static async Task<bool> IsUsernameAvailable(string username, StoreContext context)
        {
            bool isTaken =
                await (from acc in context.Accounts
                       where username == acc.Username
                       select acc).AnyAsync();

            return isTaken;
        }

        public static async Task<Account> Register(StoreContext context, Account acc)
        {
            await context.Accounts.AddAsync(acc);
            await context.SaveChangesAsync();
            return acc;
        }

        /// <summary>
        /// Returns the account of the user with the supplied login credentials.
        /// NUll is returned if there is no match
        /// </summary>
        /// <param name="login"></param>
        /// <param name="context"></param>
        public static async Task<Account> DoesUserMatch(LoginViewModel login, StoreContext context)
        {
            Account acc =
                await (from user in context.Accounts
                       where (user.Email == login.UsernameOrEmail ||
                             user.Username == login.UsernameOrEmail) &&
                             user.Password == login.Password
                       select user).SingleOrDefaultAsync();

            return acc;
        }
    }
}
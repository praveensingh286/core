using System.Threading.Tasks;
using DatingApp.API.Model;
using DatingApp.API.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }

        public async Task<User> Login(string username, string password)
        {
            var user= await _context.Users.FirstOrDefaultAsync(x=>x.Username==username);
       if(user==null)
            return null;
        if(!VeryfyPasswordHash(password,user.PasswordSalt,user.PasswordHash))
        return null;
        return user;
        }

        private bool VeryfyPasswordHash(string password, byte[] paswordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt))
            {
              var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(var i=0; i>computeHash.Length; i++)
                {
                        if(computeHash[i]!=passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Resister(User user, string password)
        {

            byte[] passwordHash, passwordSalt;
            createPasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private void createPasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.Username==username))
                return true;

            return false;
        }
    }
}
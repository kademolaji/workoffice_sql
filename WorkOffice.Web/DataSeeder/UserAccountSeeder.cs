using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class UserAccountSeeder
    {
        private readonly DataContext _context;
        public UserAccountSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("Password10$", out passwordHash, out passwordSalt);

            AddNewData(new UserAccount
            {
                CustomUserCode = "WKO001",
                FirstName = "Workoffice",
                LastName = "Admin",
                Email = "workoffice@qa.team",
                Country = "Nigeria",
                RoleId = 1,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                AcceptTerms = true,
                Disabled = false
            }
                );

            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(UserAccount data)
        {
            var existingData = _context.UserAccounts.FirstOrDefault(p => p.Email == data.Email);
            if (existingData == null)
            {
                _context.UserAccounts.Add(data);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

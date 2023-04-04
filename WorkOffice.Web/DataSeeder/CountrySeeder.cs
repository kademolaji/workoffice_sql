using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class CountrySeeder
    {
        private readonly DataContext _context;
        public CountrySeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new Country { CountryId = 1, Code = "NG", Name = "Nigeria", IsAfrica = true, ClientId = 1 });
            AddNewData(new Country { CountryId = 2, Code = "GH", Name = "Ghana", IsAfrica = true, ClientId = 1 });
            AddNewData(new Country { CountryId = 3, Code = "US", Name = "USA", IsAfrica = true, ClientId = 1 });
            AddNewData(new Country { CountryId = 4, Code = "UK", Name = "United Kingdom", IsAfrica = true, ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(Country data)
        {
            var existingData = _context.Countries.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.Countries.Add(data);
            }
        }
    }
}

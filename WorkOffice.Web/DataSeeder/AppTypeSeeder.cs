using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class AppTypeSeeder
    {
        private readonly DataContext _context;
        public AppTypeSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new AppType { AppTypeId = 1, Code = "F", Name = "FOLLOW UP APPOITMENT", ClientId = 1 });
            AddNewData(new AppType { AppTypeId = 2, Code = "N", Name = "NEW- APPOITMENT", ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(AppType data)
        {
            var existingData = _context.AppTypes.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.AppTypes.Add(data);
            }
        }
    }
}

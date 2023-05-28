using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class ConsultantSeeder
    {
        private readonly DataContext _context;
        public ConsultantSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new Consultant { ConsultantId = 1, Code = "CONSA", Name = "CONSULTANT -- A", ClientId = 1 });
            AddNewData(new Consultant { ConsultantId = 2, Code = "CONSB", Name = "CONSULTANT -- B",  ClientId = 1 });
            AddNewData(new Consultant { ConsultantId = 3, Code = "CONSC", Name = "CONSULTANT -- C",  ClientId = 1 });
            AddNewData(new Consultant { ConsultantId = 4, Code = "CONSD", Name = "CONSULTANT -- D",  ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(Consultant data)
        {
            var existingData = _context.Consultants.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.Consultants.Add(data);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class SpecialtySeeder
    {
        private readonly DataContext _context;
        public SpecialtySeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new Specialty { SpecialtyId = 1, Code = "A&E", Name = "Accident & Emergency",  ClientId = 1 });
            AddNewData(new Specialty { SpecialtyId = 2, Code = "ANAESTH", Name = "Anaesthetics",  ClientId = 1 });
            AddNewData(new Specialty { SpecialtyId = 3, Code = "B & S", Name = "Breast Surgery",  ClientId = 1 });
            AddNewData(new Specialty { SpecialtyId = 4, Code = "CARDIO", Name = "Cardiology",  ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(Specialty data)
        {
            var existingData = _context.Specialties.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.Specialties.Add(data);
            }
        }
    }
}

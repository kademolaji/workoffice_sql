using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Web.DataSeeder
{
   public class PathWayStatusSeeder
    {
        private readonly DataContext _context;
        public PathWayStatusSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            AddNewData(new PathWayStatus { PathWayStatusId = 1, Code = "10", Name = "First Activity prior to Treatment", AllowClosed = false, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 2, Code = "11", Name = "First Activity end Active Monitor", AllowClosed = false, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 3, Code = "12", Name = "First Activit new Consultant Referral", AllowClosed = false, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 4, Code = "20", Name = "Follow Up Activity prior to Treatment", AllowClosed = false, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 5, Code = "21", Name = "Transfer to Another Provider", AllowClosed = false, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 6, Code = "30", Name = "Start of first Treatment", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 7, Code = "31", Name = "Start Active Monitor - Patient", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 8, Code = "32", Name = "Start Active Monitor - Hospital", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 9, Code = "33", Name = "DNA(Did Not attend) first activity", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 10, Code = "34", Name = "Decision Not to treat", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 11, Code = "35", Name = "Patient Declined Treatment", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 12, Code = "36", Name = "Patient Died before Treatment", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 13, Code = "90", Name = "Activity following First treatment", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 14, Code = "91", Name = "Activity during Active Monitor", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 15, Code = "92", Name = "Pre-Referral Diagnostics Tests", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 16, Code = "98", Name = "Activity Not  Applicable To RTT Periods ( Mostly A&E)", AllowClosed = true, ClientId = 1 });
            AddNewData(new PathWayStatus { PathWayStatusId = 17, Code = "99", Name = "Activity not Applicable to RTT Not Known", AllowClosed = true, ClientId = 1 });
            _context.SaveChanges();
        }

        // since we run this seeder when the app starts
        // we should avoid adding duplicates, so check first
        // then add
        private void AddNewData(PathWayStatus data)
        {
            var existingData = _context.PathWayStatuses.FirstOrDefault(p => p.Name.ToLower().Trim() == data.Name.ToLower().Trim());
            if (existingData == null)
            {
                _context.PathWayStatuses.Add(data);
            }
        }
    }
}

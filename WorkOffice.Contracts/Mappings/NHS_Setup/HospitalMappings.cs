using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class HospitalMappings

    {
        public static T ToModel<T>(this Hospital entity) where T : HospitalViewModels, new()
        {
            return new T
            {
                HospitalId = entity.HospitalId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this HospitalViewModels entity) where T : Hospital, new()
        {
            return new T
            {
                HospitalId = entity.HospitalId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

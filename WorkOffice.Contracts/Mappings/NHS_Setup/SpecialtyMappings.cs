using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class SpecialtyMappings

    {
        public static T ToModel<T>(this Specialty entity) where T : SpecialtyViewModels, new()
        {
            return new T
            {
                SpecialtyId = entity.SpecialtyId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this SpecialtyViewModels entity) where T : Specialty, new()
        {
            return new T
            {
                SpecialtyId = entity.SpecialtyId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

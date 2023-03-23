using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class ConsultantMappings

    {
        public static T ToModel<T>(this Consultant entity) where T : ConsultantViewModels, new()
        {
            return new T
            {
                ConsultantId = entity.ConsultantId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this ConsultantViewModels entity) where T : Consultant, new()
        {
            return new T
            {
                ConsultantId = entity.ConsultantId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

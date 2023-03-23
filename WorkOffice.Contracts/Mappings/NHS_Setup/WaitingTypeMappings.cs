using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class WaitingTypeMappings

    {
        public static T ToModel<T>(this WaitingType entity) where T : WaitingTypeViewModels, new()
        {
            return new T
            {
                WaitingTypeId = entity.WaitingTypeId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this WaitingTypeViewModels entity) where T : WaitingType, new()
        {
            return new T
            {
                WaitingTypeId = entity.WaitingTypeId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

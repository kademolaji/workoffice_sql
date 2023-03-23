using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class WardMappings

    {
        public static T ToModel<T>(this Ward entity) where T : WardViewModels, new()
        {
            return new T
            {
                WardId = entity.WardId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this WardViewModels entity) where T : Ward, new()
        {
            return new T
            {
                WardId = entity.WardId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

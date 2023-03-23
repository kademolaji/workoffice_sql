using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class NHSActivityMappings

    {
        public static T ToModel<T>(this NHSActivity entity) where T : NHSActivityViewModels, new()
        {
            return new T
            {
                NHSActivityId = entity.NHSActivityId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this NHSActivityViewModels entity) where T : NHSActivity, new()
        {
            return new T
            {
                NHSActivityId = entity.NHSActivityId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

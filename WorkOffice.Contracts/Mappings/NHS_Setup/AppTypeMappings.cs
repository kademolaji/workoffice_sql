using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class AppTypeMappings

    {
        public static T ToModel<T>(this AppType entity) where T : AppTypeViewModels, new()
        {
            return new T
            {
                AppTypeId = entity.AppTypeId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this AppTypeViewModels entity) where T : AppType, new()
        {
            return new T
            {
                //AppTypeId = entity.AppTypeId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class PathwayStatusMappings

    {
        public static T ToModel<T>(this PathwayStatus entity) where T : PathwayStatusViewModels, new()
        {
            return new T
            {
                PathwayStatusId = entity.PathwayStatusId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this PathwayStatusViewModels entity) where T : PathwayStatus, new()
        {
            return new T
            {
                PathwayStatusId = entity.PathwayStatusId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

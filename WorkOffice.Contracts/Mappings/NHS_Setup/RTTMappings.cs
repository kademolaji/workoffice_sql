using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class RTTMappings

    {
        public static T ToModel<T>(this RTT entity) where T : RTTViewModels, new()
        {
            return new T
            {
                RTTId = entity.RTTId,
                Code = entity.Code,
                Name = entity.Name,

            };
        }

        public static T ToModel<T>(this RTTViewModels entity) where T : RTT, new()
        {
            return new T
            {
                RTTId = entity.RTTId,
                Code = entity.Code,
                Name = entity.Name,
 
            };
        }
    }
}

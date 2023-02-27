using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class LocationMappings

    {
        public static T ToModel<T>(this Location entity) where T : LocationModel, new()
        {
            return new T
            {
                LocationId = entity.LocationId,
                Name = entity.Name,
                Country = entity.Country,
                State = entity.State,
                City = entity.City,
                Address = entity.Address,
                ZipCode = entity.ZipCode,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Note = entity.Note,
                ClientId = entity.ClientId
            };
        }

        public static T ToModel<T>(this LocationModel entity) where T : Location, new()
        {
            return new T
            {
                LocationId = entity.LocationId,
                Name = entity.Name,
                Country = entity.Country,
                State = entity.State,
                City = entity.City,
                Address = entity.Address,
                ZipCode = entity.ZipCode,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Note = entity.Note,
                ClientId = entity.ClientId
            };
        }
    }
}

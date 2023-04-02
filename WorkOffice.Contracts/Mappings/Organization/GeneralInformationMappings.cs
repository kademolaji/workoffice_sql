using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class GeneralInformationMappings
    {
        public static T ToModel<T>(this GeneralInformation entity) where T : GeneralInformationModel, new()
        {
            return new T
            {
                GeneralInformationId = entity.GeneralInformationId,
                OrganisationName = entity.Organisationname,
                Taxid = entity.Taxid,
                Regno = entity.Regno,
                Phone = entity.Phone,
                Email = entity.Email,
                Fax = entity.Fax,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                Note = entity.Note,
                Zipcode = entity.Zipcode,
                Currency = entity.Currency,
                ImgLogo = entity.ImgLogo,
                Imgtype = entity.Imgtype,
                Ismulticompany = entity.Ismulticompany,
                Subsidiary_level = entity.Subsidiary_level,
                ClientId = entity.ClientId
            };
        }

        public static T ToModel<T>(this GeneralInformationModel entity) where T : GeneralInformation, new()
        {
            return new T
            {
                GeneralInformationId = entity.GeneralInformationId,
                Organisationname = entity.OrganisationName,
                Taxid = entity.Taxid,
                Regno = entity.Regno,
                Phone = entity.Phone,
                Email = entity.Email,
                Fax = entity.Fax,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                City = entity.City,
                State = entity.State,
                Country = entity.Country,
                Note = entity.Note,
                Zipcode = entity.Zipcode,
                Currency = entity.Currency,
                ImgLogo = entity.ImgLogo,
                Imgtype = entity.Imgtype,
                Ismulticompany = entity.Ismulticompany,
                Subsidiary_level = entity.Subsidiary_level,
                ClientId = entity.ClientId
            };
        }
    }
}

﻿
using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class CompanyStructureMappings
    {
        public static T ToModel<T>(this CompanyStructure entity) where T : CompanyStructureModel, new()
        {
            return new T
            {
                CompanyStructureId = entity.CompanyStructureId,
                Name = entity.Name,
                StructureTypeId = entity.StructureTypeID,
                Country = entity.Country,
                Parent = entity.Parent,
                Address = entity.Address,
                ContactEmail = entity.ContactEmail,
                ContactPhone = entity.ContactPhone,
                CompanyHead = entity.CompanyHead,
                ParentID = entity.ParentID,
                Company = entity.Company,
                ClientId = entity.ClientId
            };
        }

        public static T ToModel<T>(this CompanyStructureModel entity) where T : CompanyStructure, new()
        {
            return new T
            {
                CompanyStructureId =entity.CompanyStructureId,
                Name = entity.Name,
                StructureTypeID = entity.StructureTypeId,
                Country = entity.Country,
                Parent = entity.Parent,
                Address = entity.Address,
                ContactEmail = entity.ContactEmail,
                ContactPhone = entity.ContactPhone,
                CompanyHead = entity.CompanyHead,
                ParentID =entity.ParentID,
                Company = entity.Company,
                ClientId = entity.ClientId
            };
        }
    }
}

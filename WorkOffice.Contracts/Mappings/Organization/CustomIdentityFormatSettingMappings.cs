
using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class CustomIdentityFormatSettingMappings
    {
        public static T ToModel<T>(this CustomIdentityFormatSetting entity) where T : CustomIdentityFormatSettingModel, new()
        {
            return new T
            {
                CustomIdentityFormatSettingId = entity.CustomIdentityFormatSettingId,
                Prefix = entity.Prefix,
                Suffix = entity.Suffix,
                Digits = entity.Digits,
                Company = entity.Company,
                ClientId = entity.ClientId
            };
        }

        public static T ToModel<T>(this CustomIdentityFormatSettingModel entity) where T : CustomIdentityFormatSetting, new()
        {
            return new T
            {
                CustomIdentityFormatSettingId = entity.CustomIdentityFormatSettingId,
                Prefix = entity.Prefix,
                Suffix = entity.Suffix,
                Digits = entity.Digits,
                Company = entity.Company,
                ClientId = entity.ClientId
            };
        }
    }
}

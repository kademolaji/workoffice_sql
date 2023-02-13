﻿using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Domain.Entities.Account;

namespace WorkOffice.Contracts.Mappings
{
    public static class UserAccountMappings
    {
        public static T ToModel<T>(this User user) where T : AuthenticationResponse, new()
        {
            if (user == null)
                return null;

            return new T
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = Enum.GetName(typeof(RolesEnum), (RolesEnum)user.RoleId),
                ProfilePicture = user.ProfilePicture,
                IsVerified = user.IsVerified
            };
        }

        public static T ToModel<T>(this AuthenticationResponse user) where T : User, new()
        {
            if (user == null)
                return null;

            return new T
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
            };
        }
    }
}
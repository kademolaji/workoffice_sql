using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class UserAccountMappings
    {
        public static T ToModel<T>(this UserAccount user) where T : AuthenticationResponse, new()
        {
            if (user == null)
                return null;

            return new T
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRole = Enum.GetName(typeof(RolesEnum), (RolesEnum)user.RoleId),
                ProfilePicture = user.ProfilePicture,
                IsVerified = user.IsVerified
            };
        }

        public static T ToModel<T>(this AuthenticationResponse user) where T : UserAccount, new()
        {
            if (user == null)
                return null;

            return new T
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
            };
        }
    }

    public static class AuthenticationMappings
    {
        public static T ToModel<T>(this UserAccount user) where T : UserAccountModel, new()
        {
            if (user == null)
                return null;

            return new T
            {
                UserId = user.UserId,
                //UserName = user.UserName,
                //Status = user.Status,
                //Role = user.Role,
                //Name = user.Name,
                Email = user.Email,
                CustomUserCode = user.CustomUserCode,
                LastLogin = user.LastLogin,
                CurrentLogin = user.CurrentLogin,
                CanChangePassword = user.CanChangePassword,
                Accesslevel = user.Accesslevel
            };
        }

        public static T ToModel<T>(this UserAccountModel user) where T : UserAccount, new()
        {
            if (user == null)
                return null;

            return new T
            {
                UserId = user.UserId,
                //UserName = user.UserName,
                //Status = user.Status,
                //Role = user.Role,
                //Name = user.Name,
                Email = user.Email,
                //CustomUserCode = user.EmployeeId,
                LastLogin = user.LastLogin,
                CurrentLogin = user.CurrentLogin,
                CanChangePassword = user.CanChangePassword,
                Accesslevel = user.Accesslevel
            };
        }
    }
}

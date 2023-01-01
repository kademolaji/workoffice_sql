﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Contracts.ServicesContracts.Users
{
	public interface IUserAccountService
	{
		Task<ApiResponse<GetResponse<AuthenticationResponse>>> Login(string userName, string password, string ipAddress);
		Task<ApiResponse<GetResponse<AuthenticationResponse>>> Register(RegisterRequest model, string password, string origin, string ipAddress);
		Task<ApiResponse<CreateResponse>> UpdateUser(UpdateUserRequest model);
		Task<ApiResponse<CreateResponse>> ChangePassword(ChangePasswordModel model);
		Task<ApiResponse<CreateResponse>> VerifyEmail(string token, string origin);
		Task<ApiResponse<CreateResponse>> ResetPassword(ResetPasswordRequest model);
		Task<ApiResponse<CreateResponse>> ForgotPassword(ForgotPasswordRequest model, string origin);
		Task RevokeToken(string token, string ipAddress);
		Task<ApiResponse<GetResponse<AuthenticationResponse>>> RefreshToken(string token, string ipAddress);
		Task<ApiResponse<CreateResponse>> ValidateResetToken(ValidateResetTokenRequest model);
		Task<ApiResponse<GetResponse<UserAccountResponse>>> GetUserById(long userId);
		Task<ApiResponse<SearchReply<UsersListModel>>> GetAllUsers(SearchCall<SearchUserList> options);
		Task<ApiResponse<CreateResponse>> CreateAdminUser(CreateAdminUserModel model, string origin);
		Task<ApiResponse<CreateResponse>> DisableEnableUser(int userId, long loggedInUserId);
		Task<ApiResponse<GetResponse<AuthenticationResponse>>> GetUserAccountById(long userId);
	}
}

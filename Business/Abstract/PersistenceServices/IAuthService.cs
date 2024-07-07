using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.ViewModel.AccountVm;
using CodinaxProjectMvc.ViewModel.AuthVm;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Grpc.Core;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    /// <summary>
    /// Interface for authentication-related operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Performs user login asynchronously.
        /// </summary>
        /// <param name="loginVm">The view model containing user login credentials.</param>
        /// <returns>A task representing the asynchronous operation, returning the user roles upon successful login, or null if login fails.</returns>
        public Task<IEnumerable<string>?> LoginAsync(LoginVm loginVm);

        /// <summary>
        /// Allows an instructor to apply asynchronously.
        /// </summary>
        /// <param name="instructorApplyVm">The view model containing information for instructor application.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the application is successful, otherwise false.</returns>
        public Task<bool> InstructorApplyAsync(InstructorApplyVm instructorApplyVm);

        /// <summary>
        /// Allows a student to apply asynchronously.
        /// </summary>
        /// <param name="studentApplyVm">The view model containing information for student application.</param>
        /// <returns>A task representing the asynchronous operation, returning true if the application is successful, otherwise false.</returns>
        public Task<bool> StudentApplyAsync(StudentApplyVm studentApplyVm);

        /// <summary>
        /// Confirms email verification asynchronously.
        /// </summary>
        /// <param name="token">The token for email confirmation.</param>
        /// <param name="email">The email address to confirm.</param>
        /// <returns>A task representing the asynchronous operation, returning true if email confirmation is successful, otherwise false.</returns>
        public Task<bool> ConfirmMailAsync(string token, string email);

        /// <summary>
        /// Sets up password for a user asynchronously.
        /// </summary>
        /// <param name="passwordSetupVm">The view model containing information for setting up the password.</param>
        /// <returns>A task representing the asynchronous operation, returning true if password setup is successful, otherwise false.</returns>
        public Task<bool> SetupPasswordAsync(PasswordSetupVm passwordSetupVm);

        public Task<bool> SendForgotPasswordAsync(ForgotPasswordVm forgotPasswordVm);

        public Task<bool> ResetPasswordAsync(ResetPasswordVm resetPasswordVm);
    }


}

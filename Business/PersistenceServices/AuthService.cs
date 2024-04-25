using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel.AccountVm;
using CodinaxProjectMvc.ViewModel.AuthVm;
using CodinaxProjectMvc.ViewModel.InstructorVm;
using CodinaxProjectMvc.ViewModel.StudentVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager; 
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly CodinaxDbContext _db;
        private readonly IMailManager _mailManager;
        private readonly IReadRepository<Course> _readRepository;

        public AuthService(SignInManager<AppUser> signInManager, IActionContextAccessor actionContextAccessor, UserManager<AppUser> userManager, CodinaxDbContext db, IConfiguration configuration, IMailManager mailManager, IReadRepository<Course> readRepository)
        {
            _signInManager = signInManager;
            _actionContextAccessor = actionContextAccessor;
            _userManager = userManager;
            _db = db;
            _configuration = configuration;
            _mailManager = mailManager;
            _readRepository = readRepository;
        }

        public async Task<bool> ConfirmMailAsync(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            token = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return false;

            return true;
        }

        public async Task<bool> InstructorApplyAsync(InstructorApplyVm instructorApplyVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            DataAccess.Models.Instructor instructor = instructorApplyVm.FromInstructorApplyVm_ToInstructor();

            AppUser? user = user = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == instructor.Email);

            if (user != null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(InstructorApplyVm.EmailAddress), "This email already exists");
                return false;
            }

            IdentityResult result = await _userManager.CreateAsync(instructor);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError("", error.Description);
                }

                return false;
            }

            result = await _userManager.AddToRoleAsync(instructor, Roles.Instructor.ToString());

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError("", error.Description);
                }
                return false;

            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(instructor);

            await _mailManager.SendConfirmationMailAsync(token, instructor);

            return true;
        }

        public async Task<IEnumerable<string>?> LoginAsync(LoginVm loginVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return null;

            AppUser user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);

            if (user == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LoginVm.EmailAddress), "Email Address is not found");
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.IsRemember, true);

            if (!result.Succeeded)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(LoginVm.Password), "Invalid Login attempt");
                return null;
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> SetupPasswordAsync(PasswordSetupVm passwordSetupVm)
        {
            if(!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;

            AppUser user = await _userManager.FindByEmailAsync(passwordSetupVm.EmailAddress);

            string token = Uri.UnescapeDataString(passwordSetupVm.Token);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, passwordSetupVm.ConfirmPassword);

            if(!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> StudentApplyAsync(StudentApplyVm studentApplyVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
                return false;
            
            Course? course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == studentApplyVm.CourseId);

            if (course == null) 
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(StudentApplyVm.CourseId), "This course does not exists");
                return false;
            };

            Student student = studentApplyVm.FromStudentApplyVm_ToStudent();

            AppUser? user = user = await _db.Users
               .FirstOrDefaultAsync(x => x.Email == student.Email);

            if (user != null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(StudentApplyVm.EmailAddress), "This email is already exists");
                return false;
            }

            student.IsApproved = false;
            student.Courses = new List<Course>() { course };

            IdentityResult result = await _userManager.CreateAsync(student);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError("", error.Description);
                }

                return false;
            }

            result = await _userManager.AddToRoleAsync(student, Roles.Student.ToString());

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _actionContextAccessor.ActionContext.ModelState.AddModelError("", error.Description);
                }
                return false;

            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(student);

            await _mailManager.SendConfirmationMailAsync(token, student);

            return true;
        }
    }
}
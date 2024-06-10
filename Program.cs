using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Business.InfrastructureServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.Context;
using CodinaxProjectMvc.DataAccess.Abstract.Storages;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.DataAccess.Storages;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.Filters;
using CodinaxProjectMvc.Managers;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.Policies;
using CodinaxProjectMvc.Registrations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CodinaxDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
}).AddIdentity<AppUser, AppRole>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
}).AddDefaultTokenProviders()
    .AddUserStore<UserStore<AppUser, AppRole, CodinaxDbContext, Guid>>()
    .AddRoleStore<RoleStore<AppRole, CodinaxDbContext, Guid>>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 3000000000;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 3000000000;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 3000000000;
});

builder.Services.AddScoped<IAzureStorage, AzureStorage>();
builder.Services.AddScoped<IMailSender, MailSender>();

//builder.Services.AddProtectedBrowserStorage();

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(10);

    options.LoginPath = new PathString("/Auth/Login");
    options.LogoutPath = new PathString("/Auth/Logout");
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");

    options.SlidingExpiration = true;
});

builder.Services.AddTransient<IAuthorizationHandler, InstructorAuthorizationHandler>();
builder.Services.AddTransient<IAuthorizationHandler, StudentAuthorizationHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyConstants.AdminPolicy, policy => policy.RequireRole(Roles.Admin.ToString()));
    options.AddPolicy(PolicyConstants.InstructorPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new InstructorAuthorizationRequirement());
    });
    options.AddPolicy(PolicyConstants.StudentPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new StudentAuthorizationRequirement());
    });
    options.AddPolicy(PolicyConstants.AuthRequiredPolicy, policy => policy.RequireAuthenticatedUser());
    options.AddPolicy(PolicyConstants.NotAuthRequiredPolicy, policy => policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddScoped<IMailManager, MailManager>();
builder.Services.AddScoped<INotificationManager, NotificationManager>();
builder.Services.AddPersistenceServices();

builder.Services.AddScoped<PropertyAccessCourseFilter>();
builder.Services.AddScoped<EventViewFilter>();

var app = builder.Build();

using (var service = app.Services.CreateScope())
{
    var dbContext = service.ServiceProvider.GetRequiredService<CodinaxDbContext>();
    var _userManager = service.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var _roleManager = service.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    foreach (var item in Enum.GetValues(typeof(Roles)))
    {
        if (!_roleManager.RoleExistsAsync(item.ToString()).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new AppRole
            {
                Name = item.ToString()
            }).GetAwaiter().GetResult();
        }
    }

    if (_userManager.FindByEmailAsync("admin@mail.ru").GetAwaiter().GetResult() == null)
    {
        var user = new AppUser()
        {
            FirstName = "Admin",
            LastName = "Adminov",
            Email = "admin@mail.ru",
            UserName = "admin@mail.ru"
        };
        var result = _userManager.CreateAsync(user, "Admin123$").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
    }
}

app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.Run();

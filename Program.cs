using System.Net;
using App.ExtendMethods;
using AppMvc.Net.Data;
using AppMvc.Net.Menu;
using AppMvc.Net.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace AppMvc.Net;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //Add RazorPage
        builder.Services.AddRazorPages();

        builder.Services.AddSingleton<ProductService>();

        builder.Services.AddSingleton<PlanetService>();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            string connectString = builder.Configuration.GetConnectionString("AppMvcConnectionString");
            options.UseSqlServer(connectString);
        });

        builder.Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

        // Truy cập IdentityOptions
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Thiết lập về Password
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            // Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất

            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
            options.SignIn.RequireConfirmedAccount = true;

        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.AccessDeniedPath = "/pagedenied";
        });

        builder.Services.AddAuthentication()
                        .AddGoogle(options =>
                        {
                            var gconfig = builder.Configuration.GetSection("Authentication:Google");
                            options.ClientId = gconfig["ClientID"];
                            options.ClientSecret = gconfig["ClientSecret"];
                            options.CallbackPath = "/dang-nhap-tu-google";
                        })
                        .AddFacebook(options =>
                        {
                            var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
                            options.ClientId = fconfig["AppID"];
                            options.ClientSecret = fconfig["AppSecret"];
                            options.CallbackPath = "/dang-nhap-tu-facebook";
                        });

        //Dang ky dich vu gui email
        builder.Services.AddOptions();
        var mailsetting = builder.Configuration.GetSection("MailSettings");
        builder.Services.Configure<MailSettings>(mailsetting);
        builder.Services.AddTransient<IEmailSender, SendMailService>(); //dk dvu gui email co ten la emailsender

        builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddTransient<AdminSidebarService>();


        //DK dvu identityerrordescriber bang dvu appidentityerrordescriber do ta tu dinh nghia lai
        builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

        builder.Services.AddAuthorization(options => {
            options.AddPolicy("ViewManageMenu", builder => {
                builder.RequireAuthenticatedUser();
                builder.RequireRole(RoleName.Administrator);
            });
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.AddStatusCodePage(); //Tuy bien response loi tu 400-499

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapAreaControllerRoute(
                name: "product",
                pattern: "{controller}/{action=Index}/{id?}",
                areaName: "ProductManage"
            );

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapRazorPages();


        });

        app.Run();
    }
}

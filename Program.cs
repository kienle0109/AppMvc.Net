using System.Net;
using App.ExtendMethods;
using AppMvc.Net.Services;
using Microsoft.AspNetCore.Components.Web;

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

        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
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

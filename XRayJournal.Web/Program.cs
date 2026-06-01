using XRayJournal.Web.Components;
using Mapster;
using XRayJournal.Core;
using XRayJournal.Core.IRepositories;
using XRayJournal.DAL;
using XRayJournal.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace XRayJournal.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddDbContext<DataContext>();

            //Репозитории
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IXRayExamRepository, XRayExamRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INumberRepository, NumberRepository>();
            builder.Services.AddScoped<IRecordRepository, RecordRepository>();
            builder.Services.AddScoped<ICabinetRepository, CabinetRepository>();
            builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();

            //Сервисы
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<XRayExamService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<NumberService>();
            builder.Services.AddScoped<RecordService>();
            builder.Services.AddScoped<CabinetService>();

            TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
            builder.Services.AddMapster();

            //Аутентификация
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.Cookie.Name = "auth_token";
                        options.LoginPath = "/login";
                        options.Cookie.MaxAge = TimeSpan.FromMinutes(390);
                        options.AccessDeniedPath = "/access-denied"; // Что это значит?
                    });

            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            //----====!ВАЖНО!====----
            app.UseAuthentication(); // Сначала Аутентификация
            app.UseAuthorization(); // А потом Авторизация

            app.MapGet("/", () => Results.Redirect("/home"));

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}

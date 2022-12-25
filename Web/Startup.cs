using Business.Abstract;
using Business.Constants;
using DataAccess.Abstract;
using DataAccess.Contrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Identity;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllersWithViews();

            #region Identity

            services.AddScoped<IUserStore<DoctorUser>, DoctorUserStore>();
            services.AddScoped<IRoleStore<IdentityRole>, DoctorUserRoleStore>();
            services.AddScoped<UserManager<DoctorUser>, DoctorUserManager>();
            services.AddIdentity<DoctorUser, IdentityRole>(v =>
            {
                v.SignIn.RequireConfirmedAccount = true;
                v.SignIn.RequireConfirmedEmail = true;
                v.Password.RequiredLength = 5;
                v.User.RequireUniqueEmail = true;
                v.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                v.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            }).AddDefaultTokenProviders();
            services.AddAuthentication();
            services.ConfigureApplicationCookie(v =>
            {
                v.Cookie.Name = "GlaucoT.Cookie";
                v.LoginPath = "/Home/Index";
                v.ExpireTimeSpan = TimeSpan.FromDays(30);
                v.Cookie.SameSite = SameSiteMode.Strict;
                v.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                //v.AccessDeniedPath = "/Yardimci/Index";
            });

            services.AddDistributedMemoryCache();
            services.AddSession(v =>
            {
                v.Cookie.Name = "GlaucoT.Session";
                v.IdleTimeout = TimeSpan.FromMinutes(30);
                v.Cookie.IsEssential = true;
                v.Cookie.SameSite = SameSiteMode.Strict;
                v.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            #endregion

            services.AddSingleton<IDoctorService, DoctorManager>();
            services.AddSingleton<IDoctorDal, EFDoctorDal>();

            services.AddSingleton<IGlassRecordService, GlassRecordManager>();
            services.AddSingleton<IGlassRecordDal, EFGlassRecordDal>();

            services.AddSingleton<IMedicineRecordService, MedicineRecordManager>();
            services.AddSingleton<IMedicineRecordDal, EFMedicineRecordDal>();

            services.AddSingleton<IMedicineService, MedicineManager>();
            services.AddSingleton<IMedicineDal, EFMedicineDal>();

            services.AddSingleton<IPatientService, PatientManager>();
            services.AddSingleton<IPatientDal, EFPatientDal>();

            services.AddSingleton<IOTPService, OTPManager>();
            services.AddSingleton<IOTPDal, EFOTPDal>();

            services.AddSingleton<IGlassRecordService, GlassRecordManager>();
            services.AddSingleton<IGlassRecordDal, EFGlassRecordDal>();

            services.AddSingleton<IMedicineRecordService, MedicineRecordManager>();
            services.AddSingleton<IMedicineRecordDal, EFMedicineRecordDal>();

            services.AddSingleton<IMedicineService, MedicineManager>();
            services.AddSingleton<IMedicineDal, EFMedicineDal>();

            services.AddSingleton<IRegisterService, RegisterManager>();
            services.AddSingleton<ILoginService, LoginManager>();
            services.AddSingleton<IMobileHomeService, MobileHomeManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

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
            });
        }
    }
}
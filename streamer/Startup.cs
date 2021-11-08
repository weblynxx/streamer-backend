using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Serialization;
using streamer.ApiModel.settings;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.db.Database.Helpers;
using streamer.Extensions;
using streamer.Features.interfaces.services;
using streamer.Features.User.Login;
using streamer.Helpers;
using streamer.Security;
using streamer.Services;
using Stripe;
using Swashbuckle.AspNetCore.Swagger;

namespace streamer
{
    public class Startup
    {
        private IHostingEnvironment _appHost;

        public Startup(IConfiguration configuration, IHostingEnvironment appHost)
        {
            Configuration = configuration;
            _appHost = appHost;

            using (var db = new StreamerDbContext(null, new DbContextOptions<StreamerDbContext>(), Configuration))
            {
                //db.Database.EnsureCreated();
                db.Database.Migrate();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            StripeConfiguration.ApiKey = Crypto.DecryptString(appSettings.StripeSecretKey);

            var authCfg = Configuration.GetSection("Authentication").Get<AppConfiguration.Authentication>();
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authCfg.SecretKey));

            services.Configure<AuthSettings>(x => x.SecretKey = authCfg.SecretKey);

            services.AddEntityFrameworkNpgsql()
           .AddDbContext<StreamerDbContext>();

            services.AddIdentity<StreamerDm, IdentityRole<Guid>>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+äöüÄÖÜ";
            })
                .AddEntityFrameworkStores<StreamerDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<ITokenFactory, TokenFactory>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Inject IPrincipal
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // Ensure that User.Identity.Name is set correctly after login
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",

                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAutoMapper();

            services.AddMediatR();
            services.AddMediatorHandlers(Assembly.GetCallingAssembly());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            AddApplicationServices(services);


            services.AddOData();
            services.AddODataQueryFilter();

            services.AddCors();

            services.AddMvc(
                options =>
                {
                    //options.EnableEndpointRouting = false;
                    foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    {
                        outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                    }
                    foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    {
                        inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                    }
                }
                ).AddJsonOptions(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                    configureOptions.SaveToken = true;

                })
                .ConfigureExternalAuth(authCfg);

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(JwtConstants.RoleClaim, JwtConstants.ApiAccessClaim));
            });

            // add identity
            var builder = services.AddIdentityCore<StreamerDm>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            services.AddCors();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builderCors => builderCors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserResolveService, UserResolveService>();
            services.AddTransient<IAppVersionService, AppVersionService>();

            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<StreamerDm> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.SetTimeZoneInfo(TimeZoneInfo.Utc);
                routes.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                routes.MapODataServiceRoute("odata", "odata", GetEdmModel());

                // Workaround: https://github.com/OData/WebApi/issues/1175
                routes.EnableDependencyInjection();

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });



            });

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Time-Org V1");

            //});



        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();
            builder.EntitySet<StreamerDm>("Streamers");
            return builder.GetEdmModel();
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            //services.AddScoped<IUserRepository, UserRepository>();
            AddMediatr(services);
        }

        private static void AddMediatr(IServiceCollection services)
        {
            const string applicationAssemblyName = "streamer.Features";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            //services.AddSingleton<IAuthorizationHandler, CompanyDelete.DocumentAuthorizationHandler>();

            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            //TODO uncoment next line
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            //services.AddSingleton<CompanyAddEdit.Command>(new CompanyAddEdit.Command());
            services.AddSingleton<LoginCommand>(new LoginCommand());
            var th = new JwtTokenHandler();
            services.AddSingleton<IJwtTokenHandler>(th);
            services.AddSingleton<IJwtTokenValidator>(new JwtTokenValidator(th));

            //services.AddMediatR();
        }


    }

}


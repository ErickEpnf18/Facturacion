using Facturacion.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Reflection;
using System.Text;



namespace Facturacion
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
            // Add Services to the container.
            services.AddCors();
            services.AddSignalR();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddHttpContextAccessor();
  
            services.AddDbContext<AplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<NetUserAditional, IdentityRole>()

            //.AddEntityFrameworkStores<AplicationDbContext>()
            //.AddDefaultTokenProviders();

     //       services.Configure<IdentityOptions>(options =>
     //       {
     //           // Default Password settings.
     //           options.Password.RequireDigit = false;
     //           options.Password.RequireLowercase = false;
     //           options.Password.RequireNonAlphanumeric = false;
     //           options.Password.RequireUppercase = false;
     //           options.Password.RequiredLength = 8;

     //       });
     //       services.AddAuthorization(opciones =>
     //       {

     //           opciones.AddPolicy("AdminNes", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
     //                 "AdminEmpresaDueno", "AdminNessoft"));

     //           opciones.AddPolicy("EmpresaAdmin", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
     //     "AdminNessoft", "UsuarioEmpresa", "AdminEmpresa"));

     //           opciones.AddPolicy("UsariosNesEmpresaCreada", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
     //            "AdminEmpresaCreada", "UsuarioEmpresaCreada", "LocalEmpresaCreada", "LocalUsuarioEmpresaCreada"));




     //       });
     //       services.AddAuthentication(options =>
     //       {
     //           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     //           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     //           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
     //       })
     //.AddJwtBearer(options =>
     //{
     //    options.SaveToken = true;
     //    options.RequireHttpsMetadata = false;
     //    options.TokenValidationParameters = new TokenValidationParameters
     //    {
     //        ValidateIssuer = false,
     //        ValidateAudience = false,
     //        ValidateLifetime = true,
     //        ValidateIssuerSigningKey = true,
     //        IssuerSigningKey = new SymmetricSecurityKey(
     //     Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
     //        ClockSkew = TimeSpan.Zero
     //    };
     //});


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
       //     services.AddScoped<IInicializadorDB, InicializadorDB>();

            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "NesSoftFactApi",
            //        Version = "v1",
            //        Description = "Version de Api NesFAct Consumo de servicios realizados en .net core 6",
            //        TermsOfService = new Uri("https://nes-soft.te/terms"),
            //        Contact = new OpenApiContact
            //        {
            //            Name = "NesSoftFactApiDesarrollo",
            //            Email = "edwinwla13@hotmail.com",
            //            Url = new Uri("https://www.nes-soft.com/"),

            //        },

            //        License = new OpenApiLicense
            //        {
            //            Name = "Uso bajo supervicion",
            //            Url = new Uri("https://choosealicense.com/licenses/mit/"),
            //        }
            //    });

            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    options.IncludeXmlComments(xmlPath);
            //}
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiRest", Version = "v1" });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //IInicializadorDB dbInicial
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiRest v1"));
            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiRest v1"));


            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                return next.Invoke();
            });

            //app.UseExceptionHandler(builder =>
            //{
            //    builder.Run(async context =>
            //    {

            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        var error = context.Features.Get<IExceptionHandlerFeature>();

            //        if (error != null)
            //        {
            //            context.Response.AddApplicationError(error.Error.Message);
            //            await context.Response.WriteAsync(error.Error.Message);
            //        }
            //    });
            //});

            app.UseStaticFiles();

            app.UseSwagger();
            //dbInicial.Inicializar();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nessoft v1");

            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();


            });

        }
    }
}

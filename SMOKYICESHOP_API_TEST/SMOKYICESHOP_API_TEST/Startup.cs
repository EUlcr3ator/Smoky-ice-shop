using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMOKYICESHOP_API_TEST.DbContexts;
using SMOKYICESHOP_API_TEST.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using SMOKYICESHOP_API_TEST.Models;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.DtoService;

namespace SMOKYICESHOP_API_TEST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureAuthentication(services);
            ConfigureDbContexts(services);
            ConfigureModels(services);
            ConfigureCustomServices(services);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o => AddSwaggerDocumentation(o));
            services.AddCors(p => p.AddPolicy("corspolicy", build =>
            {
                build.WithOrigins("http://localhost:3000", "https://kliruk.github.io", "https://smoky-ice-shop.com").AllowAnyMethod().AllowAnyHeader();
            }));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corspolicy");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
        }

        private void ConfigureDbContexts(IServiceCollection services)
        {
            string conneciton = Configuration.GetConnectionString("Smoky_Ice_Shop_Host");

            services.AddDbContext<SmokyIceDbContext>(opt =>
                opt.UseSqlServer(conneciton));

            services.AddHttpContextAccessor();
            services.AddSingleton<UrlContext>();
        }

        private void ConfigureModels(IServiceCollection services)
        {
            services.AddTransient<CartrigesAndVaporizersModel>();
            services.AddTransient<CartsModel>();
            services.AddTransient<ClientsModel>();
            services.AddTransient<CoalsModel>();
            services.AddTransient<DeliveryMethodsModel>();
            services.AddTransient<ECigarettesModel>();
            services.AddTransient<GoodsModel>();
            services.AddTransient<HookahTobaccosModel>();
            services.AddTransient<ImagesModel>();
            services.AddTransient<LiquidsModel>();
            services.AddTransient<OrdersModel>();
            services.AddTransient<PaymentMethodsModel>();
            services.AddTransient<PodsModel>();
            services.AddTransient<PopularGoodsModel>();
            services.AddTransient<ProducersModel>();
            services.AddTransient<RecomendedGoodsModel>();
            services.AddTransient<UserModel>();
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddTransient<CartDtoService>();
            services.AddTransient<CartrigeDtoService>();
            services.AddTransient<CoalDtoService>();
            services.AddTransient<DefaultDtoService>();
            services.AddTransient<EcigaretteDtoService>();
            services.AddTransient<HookahTobaccoDtoService>();
            services.AddTransient<LiquidDtoService>();
            services.AddTransient<OrderDtoService>();
            services.AddTransient<PodDtoService>();

            services.AddTransient<AuthenticationService>();
            services.AddTransient<CategoriesService>();
            services.AddTransient<DiscountsService>();
            services.AddTransient<SearchService>();
            services.AddTransient<SendToBotService>();
            services.AddTransient<TokenService>();
        }

        private static void AddSwaggerDocumentation(SwaggerGenOptions o)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
    }
}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Constant;
using Models.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Text;
using Traibanhoa.Modules.BasketDetailModule;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Traibanhoa.Modules.BasketModule;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.BasketSubCateModule;
using Traibanhoa.Modules.BasketSubCateModule.Interface;
using Traibanhoa.Modules.CategoryModule;
using Traibanhoa.Modules.CategoryModule.Interface;
using Traibanhoa.Modules.CustomerModule;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.OrderModule;
using Traibanhoa.Modules.OrderModule.Interface;
using Traibanhoa.Modules.ProductModule;
using Traibanhoa.Modules.ProductModule.Interface;
using Traibanhoa.Modules.RequestBasketDetailModule;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Traibanhoa.Modules.RequestBasketModule;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Traibanhoa.Modules.SubCateModule;
using Traibanhoa.Modules.SubCateModule.Interface;
using Traibanhoa.Modules.TransactionModule;
using Traibanhoa.Modules.TransactionModule.Interface;
using Traibanhoa.Modules.TypeModule;
using Traibanhoa.Modules.TypeModule.Interface;
using Traibanhoa.Modules.UserModule;
using Traibanhoa.Modules.UserModule.Interface;

namespace Traibanhoa
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
            services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var errors = errorContext.ModelState.Values.SelectMany(e => e.Errors.Select(m => new
                    {
                        ErrorMessage = m.ErrorMessage
                    })).ToList();
                    var result = new
                    {
                        Errors = errors.Select(e => e.ErrorMessage).ToList()
                    };
                    return new BadRequestObjectResult(result);
                };
            });

            services
                 .AddControllers()
                 .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                 .AddNewtonsoftJson(x => x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Traibanhoa", Version = "v1" });
            });
            services.AddDbContext<TraibanhoaContext>(
                opt => opt.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            var secretKey = Configuration["AppSetting:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                  .AddJwtBearer(opt =>
                  {
                      opt.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = false,
                          ValidateAudience = false,
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                          ClockSkew = TimeSpan.Zero
                      };
                  });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.SetIsOriginAllowed(host => true)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });
            //Type Module
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<ITypeService, TypeService>();
            //Product Module
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            //User Module
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            //Basket Module
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            //BasketDetail Module
            services.AddScoped<IBasketDetailRepository, BasketDetailRepository>();
            //Customer Module
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            //Order Module
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            //Transaction Module
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            //RequestBasketDetail Module
            services.AddScoped<IRequestBasketDetailRepository, RequestBasketDetailRepository>();
            services.AddScoped<IRequestBasketDetailService, RequestBasketDetailService>();
            //RequestBasket Module
            services.AddScoped<IRequestBasketRepository, RequestBasketRepository>();
            services.AddScoped<IRequestBasketService, RequestBasketService>();
            //Basket SubCate Module
            services.AddScoped<IBasketSubCateRepository, BasketSubCateRepository>();
            //RequestBasket Module
            services.AddScoped<IRequestBasketRepository, RequestBasketRepository>();
            services.AddScoped<IRequestBasketService, RequestBasketService>();
            //Category Module
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            //Sub-Category Module
            services.AddScoped<ISubCateRepository, SubCateRepository>();
            services.AddScoped<ISubCateService, SubCateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Traibanhoa v1"));
            }
            app.UseSession();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

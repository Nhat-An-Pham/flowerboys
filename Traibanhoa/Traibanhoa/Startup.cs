using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models.Models;
using Traibanhoa.Modules.BasketDetailModule;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Traibanhoa.Modules.BasketModule;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.CartDetailModule;
using Traibanhoa.Modules.CartDetailModule.Interface;
using Traibanhoa.Modules.CartModule;
using Traibanhoa.Modules.CartModule.Interface;
using Traibanhoa.Modules.CategoryModule;
using Traibanhoa.Modules.CategoryModule.Interface;
using Traibanhoa.Modules.CustomerModule;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.OrderBasketDetailModule;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Traibanhoa.Modules.OrderModule;
using Traibanhoa.Modules.OrderModule.Interface;
using Traibanhoa.Modules.OrderProductDetailModule;
using Traibanhoa.Modules.OrderProductDetailModule.Interface;
using Traibanhoa.Modules.ProductModule;
using Traibanhoa.Modules.ProductModule.Interface;
using Traibanhoa.Modules.RequestBasketDetailModule;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Traibanhoa.Modules.RequestBasketModule;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Traibanhoa.Modules.SubCateModule;
using Traibanhoa.Modules.SubCateModule.Interface;
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Traibanhoa", Version = "v1" });
            });
            services.AddDbContext<TraibanhoaContext>(
                opt => opt.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                )
            );
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
            services.AddScoped<IBasketDetailService, BasketDetailService>();
            //Customer Module
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            //OrderBasketDetail Module
            services.AddScoped<IOrderBasketDetailRepository, OrderBasketDetailRepository>();
            services.AddScoped<IOrderBasketDetailService, OrderBasketDetailService>();
            //Order Module
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            //OrderProductDetail Module
            services.AddScoped<IOrderProductDetailRepository, OrderProductDetailRepository>();
            services.AddScoped<IOrderProductDetailService, OrderProductDetailService>();
            //RequestBasketDetail Module
            services.AddScoped<IRequestBasketDetailRepository, RequestBasketDetailRepository>();
            services.AddScoped<IRequestBasketDetailService, RequestBasketDetailService>();
            //RequestBasket Module
            services.AddScoped<IRequestBasketRepository, RequestBasketRepository>();
            services.AddScoped<IRequestBasketService, RequestBasketService>();
            //Cart Module
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            //Cart Detail Module
            services.AddScoped<ICartDetailRepository, CartDetailRepository>();
            services.AddScoped<ICartDetailService, CartDetailService>();
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

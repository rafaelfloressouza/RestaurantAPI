using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantAPI.Context;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;

namespace RestaurantAPI

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
            services.AddScoped<UserRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<DishRepository>();
            services.AddScoped<CookRepository>();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<ManagerRepository>();
            services.AddScoped<WaiterRepository>();
            services.AddScoped<IngredientRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<TransactionRepository>();
            services.AddScoped<MenuRepository>();
            services.AddScoped<ReviewRepository>();
            services.AddScoped<TableRepository>();
            services.AddScoped<Dish_IngredientRepository>();
            services.AddScoped<Ingredient_SupplierRepository>();
            services.AddScoped<Customer_TransactionRepository>();
            services.AddScoped<Order_DishRepository>();
            services.AddDbContext<AppDBContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Connection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

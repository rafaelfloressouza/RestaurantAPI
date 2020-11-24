using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Settingn up compound primary keys
            modelBuilder.Entity<Review>()
                .HasKey(r => new { r.User_ID, r.Review_ID });

            modelBuilder.Entity<Customer_Transaction>()
               .HasKey(ct => new { ct.User_ID, ct.Transaction_ID });

            modelBuilder.Entity<Dish_Ingredient>()
               .HasKey(di => new { di.Dish_ID, di.Ing_Name });

            modelBuilder.Entity<Ingredient_Supplier>()
               .HasKey(iss => new { iss.Ing_Name, iss.Supplier });

            modelBuilder.Entity<Order_Dish>()
             .HasKey(od => new { od.Order_ID, od.Dish_ID });

            modelBuilder.Entity<Order_Table>()
            .HasKey(ot => new { ot.Order_ID, ot.TableNo });

            modelBuilder.Entity<Order_Transaction>()
          .HasKey(otr => new { otr.Order_ID, otr.Transaction_ID });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Cook> Cook { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<Waiter> Waiter { get; set; }
        public DbSet<Customer_Transaction> Customer_Transaction { get; set; }
        public DbSet<Dish_Ingredient> Dish_Ingredient { get; set; }
        public DbSet<Ingredient_Supplier> Ingredient_Supplier { get; set; }
        public DbSet<In_Store_Order> In_Store_Order { get; set; }
        public DbSet<Online_Order> Online_Order { get; set; }
        public DbSet<Order_Dish> Order_Dish { get; set; }
        public DbSet<Order_Table> Order_Table { get; set; }
        public DbSet<Order_Transaction> Order_Transaction { get; set; }
    }
}
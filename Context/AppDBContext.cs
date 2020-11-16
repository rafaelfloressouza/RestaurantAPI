using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasKey(r => new { r.User_ID, r.Review_ID });
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
    }
}

using ETradeDataAccess.Configs;
using ETradeEntities.Entities;
using System.Data.Entity;

namespace ETradeDataAccess.Contexts
{
    public class ETradeContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public ETradeContext() : base(ETradeConfig.ConnectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(category => category.Products)
                .WithRequired(product => product.Category)
                .HasForeignKey(product => product.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(role => role.Users)
                .WithRequired(user => user.Role)
                .HasForeignKey(user => user.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}

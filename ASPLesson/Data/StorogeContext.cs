using ASPLesson.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPLesson.Data
{
	public class StorageContext : DbContext
	{
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<ProductGroup> ProductGroups { get; set; }
		public virtual DbSet<Storage> Storages { get; set; }

		private readonly IConfiguration _configuration;

		public StorageContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString = _configuration.GetConnectionString("db");
			optionsBuilder.UseSqlServer(connectionString).LogTo(Console.WriteLine);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductGroup>(entity =>
			{
				entity.HasKey(pg => pg.Id).HasName("product_group_pk");
				entity.ToTable("category");
				entity.Property(pg => pg.Name).HasColumnName("name").HasMaxLength(255);
			});
			modelBuilder.Entity<Product>(entity =>
			{
				entity.HasKey(p => p.Id).HasName("product_pk");
				entity.Property(p => p.Name).HasColumnName("name").HasMaxLength(255);
				entity.HasOne(p => p.ProductGroup).WithMany(pg => pg.Products).HasForeignKey(p => p.ProductGroupId);
			});
			modelBuilder.Entity<Storage>(entity =>
			{
				entity.HasKey(s => s.Id).HasName("storage_pk");
				entity.HasOne(s => s.Product).WithMany(p => p.Storages).HasForeignKey(s => s.ProductId);
			});
		}
	}
}

﻿using Microsoft.EntityFrameworkCore;

namespace shop.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Entity.User> Users { get; set; }
		public DbSet<Entity.Product> Products { get; set; }
		public DbSet<Entity.ProductGroup> ProductGroups { get; set; }
		public DbSet<Entity.Rate> Rates { get; set; }

		public DataContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("ASP121");
			modelBuilder.Entity<Entity.Rate>()	// Композитний ключ для таблиці Rates
						.HasKey(nameof(Entity.Rate.ProductId), nameof(Entity.Rate.UserId));

			modelBuilder.Entity<Entity.Product>()
						.HasMany(p => p.Rates)		 // Обов'язкова частина
						.WithOne(r => r.Product)	 // навігаційні властивості
						.HasPrincipalKey(p => p.Id)		   // Необов'язково - тільки
						.HasForeignKey(r => r.ProductId);  // якщо нестандартні назви полів
		}
	}
}

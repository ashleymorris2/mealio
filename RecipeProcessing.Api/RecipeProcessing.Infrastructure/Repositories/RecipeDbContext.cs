using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Repositories;

public class RecipeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Recipe>().OwnsMany(r => r.Ingredients, i =>
        {
            i.WithOwner().HasForeignKey("RecipeId");
            i.Property<int>("Id");
            i.HasKey("Id");
        });
        
        modelBuilder.Entity<Recipe>().OwnsMany(r => r.Instructions, i =>
        {
            i.WithOwner().HasForeignKey("RecipeId");
            i.Property<int>("Id");
            i.HasKey("Id");
        });

        modelBuilder.Entity<Recipe>().OwnsOne(r => r.NutritionPerServing);
    }
}
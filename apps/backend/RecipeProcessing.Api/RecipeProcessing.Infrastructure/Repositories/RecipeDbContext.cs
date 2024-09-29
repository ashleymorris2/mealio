using Microsoft.EntityFrameworkCore;
using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Repositories;

internal class RecipeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; init; }
    public DbSet<ImageHash> ImageHashes { get; set; }
    
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
        
        modelBuilder.Entity<ImageHash>()
            .HasOne(h => h.Recipe)
            .WithMany() 
            .HasForeignKey(h => h.RecipeId);
    }
}
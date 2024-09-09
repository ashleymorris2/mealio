using Microsoft.EntityFrameworkCore;
using RecipeProcessing.Core.Entities;

namespace RecipeProcessing.Infrastructure.Persistence;

public class RecipeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
}
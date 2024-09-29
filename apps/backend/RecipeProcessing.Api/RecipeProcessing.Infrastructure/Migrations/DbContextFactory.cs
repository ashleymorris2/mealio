using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RecipeProcessing.Infrastructure.Repositories;

namespace RecipeProcessing.Infrastructure
{
    internal class RecipeDbContextFactory : IDesignTimeDbContextFactory<RecipeDbContext>
    {
        public RecipeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RecipeDbContext>();

            var connectionString = "Host=localhost;Database=mealio_db_dev;Username=dev;Password=password";
            
            optionsBuilder.UseNpgsql(connectionString);

            return new RecipeDbContext(optionsBuilder.Options);
        }
    }
}
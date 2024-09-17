using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RecipeProcessing.Infrastructure.Repositories;

namespace RecipeProcessing.Infrastructure
{
    public partial class RecipeDbContextFactory : IDesignTimeDbContextFactory<RecipeDbContext>
    {
        [GeneratedRegex("(?<=\b[Hh]ost=)[^;]*")]
        private static partial Regex HostValueRegex();

        public RecipeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RecipeDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RecipeProcessing.Api"))
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = HostValueRegex()
                .Replace(
                    configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException(),
                    "localhost"
                );

            optionsBuilder.UseNpgsql(connectionString);

            return new RecipeDbContext(optionsBuilder.Options);
        }
    }
}
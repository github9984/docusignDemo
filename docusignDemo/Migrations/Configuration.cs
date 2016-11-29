using System.Data.Entity.Migrations;
using docusignDemo.Models;

namespace docusignDemo.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "docusignDemo.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
        }
    }
}
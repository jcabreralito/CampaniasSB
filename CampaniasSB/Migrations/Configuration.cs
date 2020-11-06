namespace CampaniasSB.Migrations
{
    using CampaniasSB.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CampaniasContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CampaniasSB.Models.CampaniasContext";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CampaniasContext context)
        {
        }
    }
}

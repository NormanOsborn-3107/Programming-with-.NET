using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Data;

namespace PRN232.LMS.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LMSDbContext>();
            db.Database.Migrate();
        }
    }
}

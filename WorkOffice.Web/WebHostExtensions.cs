using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkOffice.Domain.Helpers;
using WorkOffice.Web.DataSeeder;

namespace WorkOffice.Web
{
    public static class WebHostExtensions
    {
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                // now we have the DbContext. Run migrations
                context.Database.Migrate();

                // now that the database is up to date. Let's seed
                new AppTypeSeeder(context).SeedData();
                new ConsultantSeeder(context).SeedData();
                new HospitalSeeder(context).SeedData();
                new NHSActivitySeeder(context).SeedData();
                new PathWayStatusSeeder(context).SeedData();
                new RTTSeeder(context).SeedData();
                new SpecialtySeeder(context).SeedData();
                new WaitingTypeSeeder(context).SeedData();
                new WardSeeder(context).SeedData();
                new UserActivityParentSeeder(context).SeedData();
                new UserActivitiesSeeder(context).SeedData();
                new CountrySeeder(context).SeedData();
                new UserRoleDefinitionSeeder(context).SeedData();
                new UserAccountSeeder(context).SeedData();
                new UserRoleSeeder(context).SeedData();

            }

            return host;
        }
    }
}

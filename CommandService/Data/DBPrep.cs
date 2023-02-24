using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public static class DBPrep
    {
        public static void ApplyMigrations(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't apply migrations - " + ex.Message);
                }
            }
        }
    }
}
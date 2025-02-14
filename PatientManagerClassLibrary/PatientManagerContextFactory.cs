//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//namespace PatientManagerClassLibrary
//{
//    public class PatientManagerContextFactory : IDesignTimeDbContextFactory<PatientManagerContext>
//    {
//        public PatientManagerContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<PatientManagerContext>();

//            // Get the configuration from the appsettings.json file
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                .Build();

//            // Get the connection string from the configuration
//            var connectionString = configuration.GetConnectionString("Postgre");

//            // Configure the DbContext with the connection string
//            optionsBuilder.UseNpgsql(connectionString)
//                          .UseLazyLoadingProxies();

//            return new PatientManagerContext(optionsBuilder.Options);
//        }
//    }
//}

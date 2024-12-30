using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using System.Reflection;

public static class ConfigurationManager
{
    public static Configuration SetConfiguration()
    {
        var mapper = new ModelMapper();
        var config = new Configuration();

        // Add mappings from the current assembly
        mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

        // Configure database integration using Windows Authentication
        config.DataBaseIntegration(x =>
        {
            // Connection string for SQL Server with Windows Authentication
            x.ConnectionString = "Data Source=GITA002\\SQLEXPRESS;Initial Catalog=Shopsy;User Id=NewTest;Password=NewTest@1234;";

            // Using SqlClientDriver for SQL Server
            x.Driver<SqlClientDriver>();

            // Using the appropriate dialect for SQL Server 2012
            x.Dialect<MsSql2012Dialect>();

            // Optionally set more configurations like Connection Pooling, if necessary
            // x.ConnectionReleaseMode = ConnectionReleaseMode.OnClose; // Example

            // Use this setting if you're planning to enable multiple active result sets (MARS)
            // x.BatchSize = 10; // Example if batching is required
        });

        // Add assembly mapping (this is used to scan the current assembly for NHibernate mappings)
        config.AddAssembly(Assembly.GetExecutingAssembly());

        // Compile the model and add mappings
        config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        return config;
    }
    //public static string ConnectionString
    //{
    //    get
    //    {
    //        NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
    //        try
    //        {
    //            // Explicitly load the configuration file
    //            cfg.Configure("hibernate.cfg.xml");
    //            string connectionString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString);
    //            if (string.IsNullOrEmpty(connectionString))
    //            {
    //                throw new InvalidOperationException("Connection string is empty. Check your configuration.");
    //            }
    //            return connectionString;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("Error loading NHibernate configuration: " + ex.Message, ex);
    //        }
    //    }
    //}

}
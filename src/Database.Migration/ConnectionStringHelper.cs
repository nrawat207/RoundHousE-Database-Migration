using System;

namespace Database.Migration
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString(string connectionStringKey)
        {
            var connKey = string.Format("{0}_{1}", connectionStringKey, Environment.MachineName);
            var setting = System.Configuration.ConfigurationManager.ConnectionStrings[connKey];
            if (setting == null)
                throw new InvalidOperationException(string.Format("Connection string for key '{0}' is missing in config file.", connKey));
            return setting.ConnectionString;
        }
    }
}

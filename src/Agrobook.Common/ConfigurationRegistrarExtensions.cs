using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace System.Data.Entity
{
    public static class ConfigurationRegistrarExtensions
    {
        public static ConfigurationRegistrar Add<TEntityType>(this ConfigurationRegistrar registrar, EntityTypeConfiguration<TEntityType> entityTypeConfiguration)
            where TEntityType : class
        {
            registrar.Add<TEntityType>(entityTypeConfiguration);
            return registrar;
        }
    }
}

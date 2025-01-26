using Microsoft.EntityFrameworkCore;

namespace Persistence;
public static class ModelBuilderExtensions
{
    public static void ApplyConfigurationsWithInterface<TInterface>(this ModelBuilder modelBuilder)
    {
        IEnumerable<Type> configurations = typeof(TInterface).Assembly.GetTypes()
            .Where(t => typeof(TInterface).IsAssignableFrom(t) &&
                   t.IsClass &&
                  !t.IsAbstract &&
                   t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        foreach (Type configurationType in configurations)
        {
            dynamic? configurationInstance = Activator.CreateInstance(configurationType);
            if (configurationInstance != null)
                modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}

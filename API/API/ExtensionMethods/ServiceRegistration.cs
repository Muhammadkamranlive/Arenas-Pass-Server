using System.Reflection;

namespace API.ExtensionMethods
{
    public static class ServiceRegistration
    {
        public static void AddRepositoriesAndServices(this IServiceCollection services, Assembly servicesAssembly, Assembly repositoryAssembly)
        {
            RegisterDependencies(services, repositoryAssembly, "_Repo");
            RegisterDependencies(services, servicesAssembly, "_Service");
        }

        private static void RegisterDependencies(IServiceCollection services, Assembly assembly, string suffix)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(suffix))
                .ToList();

            foreach (var impl in types)
            {
                var interfaceType = impl.GetInterfaces().FirstOrDefault(i => i.Name == "I" + impl.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, impl);
                }
            }
        }

    }
}

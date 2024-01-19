namespace Jinget.Core.DiScanner.Internal;

internal class AttributeSelector(IEnumerable<Type> types) : ISelector
{
    private IEnumerable<Type> Types { get; } = types;

    void ISelector.Populate(IServiceCollection services, RegistrationStrategy? registrationStrategy)
    {
        var strategy = registrationStrategy ?? RegistrationStrategy.Append;

        foreach (var type in Types)
        {
            var attributes = type.GetCustomAttributes<ServiceDescriptorAttribute>().ToArray();

            // Check if the type has multiple attributes with same ServiceType.
            var duplicates = GetDuplicates(attributes);

            if (duplicates.Any())
            {
                throw new InvalidOperationException($@"Type ""{type.ToFriendlyName()}"" has multiple ServiceDescriptor attributes with the same service type.");
            }

            foreach (var attribute in attributes)
            {
                var serviceTypes = attribute.GetServiceTypes(type);

                foreach (var serviceType in serviceTypes)
                {
                    var descriptor = new ServiceDescriptor(serviceType, type, attribute.Lifetime);

                    strategy.Apply(services, descriptor);
                }
            }
        }
    }

    private static IEnumerable<ServiceDescriptorAttribute> GetDuplicates(IEnumerable<ServiceDescriptorAttribute> attributes) => attributes.GroupBy(s => s.ServiceType).SelectMany(grp => grp.Skip(1));
}

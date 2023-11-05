namespace DotNetWhy.Application.Infrastructure;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    public object Resolve(Type type) =>
        provider.GetService(type!);
}
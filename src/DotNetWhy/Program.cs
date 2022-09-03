using DotNetWhy.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection().AddServices();
var serviceProvider = serviceCollection.BuildServiceProvider();
var service = serviceProvider.GetRequiredService<IDotNetWhyService>();

service.Run(args);
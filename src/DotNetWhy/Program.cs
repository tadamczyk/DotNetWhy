using DotNetWhy.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
var serviceProvider = serviceCollection.AddServices().BuildServiceProvider();
var service = serviceProvider.GetService<IDotNetWhyService>();

service?.Run(args);
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Mapping;


namespace TaskFlow.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDI(this IServiceCollection services) 
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));


			services.AddAutoMapper(config =>
			{
				config.AddProfile<MappingProfile>();
			});


 
 
			return services;
		}
	}
}

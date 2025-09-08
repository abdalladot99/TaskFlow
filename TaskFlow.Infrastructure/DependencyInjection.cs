using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("MyConection")));
			 

			services.AddScoped(typeof(IRepository<RecurrenceType>), typeof(Repository<RecurrenceType>));
			services.AddScoped(typeof(IRepository<Notification>), typeof(Repository<Notification>));
			services.AddScoped(typeof(IRepository<TaskCollaborator>), typeof(Repository<TaskCollaborator>));

			services.AddScoped(typeof(ITaskCollaborator), typeof(TaskCollaboratorRepository));

			services.AddScoped(typeof(IRepository<AppTask>), typeof(TaskRepository));
			services.AddScoped(typeof(IRepository<Category>), typeof(CategoryRepository));
			services.AddScoped(typeof(IRepository<Priority>), typeof(PriorityRepository));
			services.AddScoped(typeof(IRepository<Status>), typeof(StatusRepository));
			
			services.AddScoped(typeof(IRefreshTokenRepository), typeof(RefreshTokenRepository));

			services.AddScoped<IFileStorageService, LocalFileStorageService>();

			services.AddScoped<IEmailSender, SmtpEmailSender>();

			services.AddHostedService<TaskReminderService>();

			services.AddScoped<IJwtService, JwtService>();

			services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

  
			return services;
		}
	}
}

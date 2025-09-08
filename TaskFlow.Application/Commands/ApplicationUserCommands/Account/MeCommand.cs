using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record MeCommand(string Id) : IRequest<ApplicationUser>;

	public class MeHandler : IRequestHandler<MeCommand, ApplicationUser>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public MeHandler( UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<ApplicationUser> Handle(MeCommand command, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(command.Id);
			if (user == null)
				return null;
			// Return only necessary user details
			return user;
		}

	}
}
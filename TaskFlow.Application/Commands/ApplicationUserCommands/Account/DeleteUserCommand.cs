using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.Commands.ApplicationUserCommands.Account
{
	public record DeleteUserCommand(string UserId) : IRequest<bool>;

	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
 			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user == null)
				return false;

 			var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
		}
	}

}

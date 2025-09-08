using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.DTOs.ApplicationUserDTO;
namespace TaskFlow.Application.Commands.ApplicationUserCommands.Auth
{
	public record RegisterCommand(RegisterUserDTO userDTO) : IRequest< (bool Success, List<string> Errors) >;


	public class RegisterHandler : IRequestHandler<RegisterCommand, (bool Success, List<string> Errors) >
	{
		private readonly UserManager<Core.Enitites.ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public RegisterHandler(UserManager<Core.Enitites.ApplicationUser> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task< (bool Success, List<string> Errors) > Handle(RegisterCommand command, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<Core.Enitites.ApplicationUser>(command.userDTO); 
			entity.UserName ??= command.userDTO.Email;
			entity.LastLogin = DateTime.UtcNow;
			entity.CreatedAt = DateTime.UtcNow;
			entity.LastUpdate = DateTime.UtcNow;
			IdentityResult result = await _userManager.CreateAsync(entity, command.userDTO.Password);
			if (result.Succeeded)
				return (true, new List<string>());
		 
			var errors = result.Errors.Select(e => e.Description).ToList(); 
			return (false,errors); 
	
		}
	}
}
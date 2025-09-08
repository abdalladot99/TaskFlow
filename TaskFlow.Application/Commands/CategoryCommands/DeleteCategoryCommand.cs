using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.Categorycommands
{

	public record DeleteCategoryCommand(string id) : IRequest<bool>;


	public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand,bool>
	{
		private readonly IRepository<Category> _categoryRepository;
 
		public DeleteCategoryHandler(IRepository<Category> CategoryRepository)
		{
			_categoryRepository = CategoryRepository;
 		}

		public async Task<bool> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
		{
			return await _categoryRepository.DeleteAsync(command.id);
 		}

	}




}


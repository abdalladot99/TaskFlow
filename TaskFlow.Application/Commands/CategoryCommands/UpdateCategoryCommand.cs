using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.Category;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.CategoryCommands
{
	public record UpdateCategoryCommand(string id, EditCategoryDTO Task) : IRequest<Category>;

	public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category>
	{
		private readonly IRepository<Category> _categoryRepository;
		private readonly IMapper _mapper;

		public UpdateCategoryHandler(IRepository<Category> CategoryRepository, IMapper mapper)
		{
			_categoryRepository = CategoryRepository;
			_mapper = mapper;
		}

		public async Task<Category> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
		{
 			if (command.Task == null)
			{
				throw new ArgumentNullException(nameof(command.Task), "Task cannot be null");
			}
			var entity = _mapper.Map<Category>(command.Task);

			var newEntity = await _categoryRepository.UpdateAsync(command.id,entity);
			return newEntity;
		}
	}


}
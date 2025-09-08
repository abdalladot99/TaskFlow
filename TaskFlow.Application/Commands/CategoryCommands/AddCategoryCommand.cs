using AutoMapper;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;
using TaskFlow.Application.DTOs.Category;
namespace TaskFlow.Application.Commands.CategoryCommands
{		

	public record AddCategoryCommand(AddCategoryDTO Task) : IRequest<Category>;

	public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, Category>
	{
		private readonly IRepository<Category> _categoryRepository;
		private readonly IMapper _mapper;

		public AddCategoryHandler(IRepository<Category> CategoryRepository, IMapper mapper)
		{
			_categoryRepository = CategoryRepository;
			_mapper = mapper;
		}

		public async Task<Category> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
		{
			if (command.Task == null)
			{
				throw new ArgumentNullException(nameof(command.Task), "Task cannot be null");
			}

			// 🟢 هنا هنعمل تحويل من DTO → Entity
			var entity = _mapper.Map<Category>(command.Task);

			// 🟢 نخزن الـ Entity في قاعدة البيانات
			await _categoryRepository.AddAsync(entity);

			// 🟢 نرجع الـ Entity بعد ما اتخزن
			return entity;
		}
	}


}

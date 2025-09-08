using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.CategoryQueries
{
 
	public record GetAllCategoryQueries() : IRequest<List<Category>>;


	public class GetAllCategoryHandler(IRepository<Category> CategoryRepository)
		: IRequestHandler<GetAllCategoryQueries, List<Category>>
	{
		public async Task<List<Category>> Handle(GetAllCategoryQueries request, CancellationToken cancellationToken)
		{
			var listCategory = await CategoryRepository.GetAllAsync();

			return listCategory;

		}
	}

}

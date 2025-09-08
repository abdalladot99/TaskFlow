using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.RecurrenceTypeQueries
{
	public record GetAllRecurrenceTypeQueries() : IRequest<List<RecurrenceType>>;


	public class GetAllRecurrenceTypeHandler
		: IRequestHandler<GetAllRecurrenceTypeQueries, List<RecurrenceType>>
	{
		private readonly IRepository<RecurrenceType> _recurrenceTypeRepository;

		public GetAllRecurrenceTypeHandler(IRepository<RecurrenceType> recurrenceTypeRepository)
		{
			_recurrenceTypeRepository = recurrenceTypeRepository;
		}

		public async Task<List<RecurrenceType>> Handle(GetAllRecurrenceTypeQueries request, CancellationToken cancellationToken)
		{
			var list = await _recurrenceTypeRepository.GetAllAsync();
			return list;

		}


	}
}

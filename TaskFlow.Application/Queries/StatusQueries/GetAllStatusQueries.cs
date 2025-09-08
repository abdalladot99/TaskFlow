using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Core.Enitites;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Queries.StatusQueries
{
 
	public record GetAllStatusQueries() : IRequest<List<Status>>;


	public class GetAllStatusHandler
		: IRequestHandler<GetAllStatusQueries, List<Status>>
	{
		private readonly IRepository<Status> _statusRepository;

		public GetAllStatusHandler(IRepository<Status> statusRepository)
		{
			_statusRepository = statusRepository;
		}

		public async Task<List<Status>> Handle(GetAllStatusQueries request, CancellationToken cancellationToken)
		{
			var listPriority = await _statusRepository.GetAllAsync();
			return listPriority; 
		}


	}
}

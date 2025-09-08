using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.DTOs.CategoryDTO
{
	public class DataCategoryDTO
	{
		public string Id { get; set; }   
		public string Name { get; set; }
		public ICollection<DataTaskDTO> Tasks { get; set; }

	}
}

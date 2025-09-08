using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDTO;
using TaskFlow.Core.Enitites;

namespace TaskFlow.Application.DTOs.PriorityDto
{
	public class DataPriorityDTO
	{
		public string Id { get; set; }
		public string Name { get; set; }    // مثلا: High - Medium - Low
		public int Level { get; set; }      // رقم نستخدمه للترتيب الفرعي مثلا High = 1 وهكذا

		public ICollection<DataTaskDTO> Tasks { get; set; }
	}
}

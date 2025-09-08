using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskDTO
{
	public class ViewTaskCollaboratorsMeDTO
	{
		public bool IsAccepted { get; set; }
		public List<BigDataViewTaskDTO> Tasks { get; set; }
	}
}

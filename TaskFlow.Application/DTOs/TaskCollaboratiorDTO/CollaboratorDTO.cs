using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.TaskCollaboratiorDTO
{
	public class CollaboratorDTO
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public bool IsAccepted { get; set; }
	}

}

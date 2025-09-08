using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ApplicationUserDTO
{
	public class ChangePasswordDto
	{
		[JsonIgnore]
		public string? Id { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
	}

}

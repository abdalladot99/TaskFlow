﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTOs.ApplicationUserDTO
{
	public class UpdateProfileDto
	{
		[JsonIgnore]
		public string? Id { get; set; }
		public string? UserName { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? ProfilePictureUrl { get; set; }
 
	}
}

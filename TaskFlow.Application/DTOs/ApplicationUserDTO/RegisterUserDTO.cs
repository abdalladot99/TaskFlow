using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskFlow.Application.DTOs.ApplicationUserDTO
{
	public class RegisterUserDTO
	{
		[Required]
 		public string FullName { get; set; }

		[Required]
		public string Password { get; set; } 
		[Required]
		[Compare("Password")]
		public string PasswordConfirmed { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string? ProfilePictureUrl { get; set; }

	}
}

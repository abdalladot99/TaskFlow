using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Core.Enitites
{
	public class RefreshToken
	{ 
		public int Id { get; set; }
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public bool IsRevoked { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}

}

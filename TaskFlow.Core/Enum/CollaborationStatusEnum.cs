using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Core.Enum
{
	public enum CollaborationStatusEnum
	{
		Pending = 1,    // في انتظار القبول
		Accepted = 2,   // مقبول
		Rejected = 3,   // مرفوض
		Removed = 4     // تمت إزالته
	}
}

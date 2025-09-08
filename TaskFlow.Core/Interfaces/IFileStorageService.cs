using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TaskFlow.Core.Interfaces
{
	public interface IFileStorageService
	{
		Task<string> SaveFileAsync(IFormFile file);
		Task<bool> DeleteFileAsync(string relativePath);

	}

}

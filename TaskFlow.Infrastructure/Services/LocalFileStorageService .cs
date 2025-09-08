using Microsoft.AspNetCore.Http;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Infrastructure.Services
{
	public class LocalFileStorageService : IFileStorageService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LocalFileStorageService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<string> SaveFileAsync(IFormFile file)
		{
			var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

			using (var stream = new FileStream(path, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var request = _httpContextAccessor.HttpContext.Request;
			return $"{request.Scheme}://{request.Host}/uploads/{fileName}";
		}

		public async Task<bool> DeleteFileAsync(string relativePath)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(relativePath));

			if (File.Exists(filePath))
			{
				await Task.Run(() => File.Delete(filePath));
				return true;
			}

			return false;
		}

	}

}

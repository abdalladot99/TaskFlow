using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.Features.UploadPicture
{
	public record UploadProfilePictureCommand(IFormFile File) : IRequest<string>;


	public class UploadProfilePictureHandler : IRequestHandler<UploadProfilePictureCommand, string>
	{
		private readonly IFileStorageService _fileStorage;

		public UploadProfilePictureHandler(IFileStorageService fileStorage)
		{
			_fileStorage = fileStorage;
		}

		public async Task<string> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
		{
			var url = await _fileStorage.SaveFileAsync(request.File);
			return url;
		}
	}



}

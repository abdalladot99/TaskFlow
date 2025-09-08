using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskFlow.Application.Commands.Features.UploadPicture;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Application.Commands.Features.DeletePicture
{
	public record DeleteProfilePictureCommand(string Path) :IRequest<bool>;

	public class DeleteProfilePictureHandler : IRequestHandler<DeleteProfilePictureCommand, bool>
	{
		private readonly IFileStorageService _fileStorage;

		public DeleteProfilePictureHandler(IFileStorageService fileStorage)
		{
			_fileStorage = fileStorage;
		}

		public async Task<bool> Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
		{
			var result = await _fileStorage.DeleteFileAsync(request.Path);
			return result;
		}
	}



}

using Booking.Application.Exceptions;
using Booking.Application.Repos;
using Booking.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Properties
{
	public sealed record DeletePropertyCommand(Guid PropertyId) : IRequest;

	public sealed class DeletePropertyHandler
		: IRequestHandler<DeletePropertyCommand>
	{
		private readonly IPropertyRepository _repository;
		private readonly ICurrentUser _currentUser;

		public DeletePropertyHandler(IPropertyRepository repository, ICurrentUser currentUser)
		{
			_repository = repository;
			_currentUser = currentUser;
		}

		public async Task Handle(DeletePropertyCommand request, CancellationToken ct)
		{
			var property = await _repository.GetByIdAsync(
				request.PropertyId,
				ct);

			if (property is null)
				throw new NotFoundException("Property not found");

			if (property.OwnerId != _currentUser.Id)
				throw new ForbiddenException("You are not the owner of this property");

			await _repository.DeleteAsync(property, ct);
		}
	}

}

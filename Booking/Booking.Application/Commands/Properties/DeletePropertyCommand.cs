using Booking.Application.Exceptions;
using Booking.Application.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Properties
{
	public sealed record DeletePropertyCommand(Guid PropertyId, Guid OwnerId) : IRequest;

	public sealed class DeletePropertyCommandHandler
		: IRequestHandler<DeletePropertyCommand>
	{
		private readonly IPropertyRepository _repository;

		public DeletePropertyCommandHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task Handle(
			DeletePropertyCommand request,
			CancellationToken cancellationToken)
		{
			var property = await _repository.GetByIdAsync(
				request.PropertyId,
				cancellationToken);

			if (property is null)
				throw new NotFoundException("Property not found");

			if (property.OwnerId != request.OwnerId)
				throw new ForbiddenException("You are not the owner of this property");

			await _repository.DeleteAsync(property, cancellationToken);
		}
	}

}

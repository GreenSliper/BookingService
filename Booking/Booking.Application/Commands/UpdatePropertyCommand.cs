using Booking.Application.Exceptions;
using Booking.Application.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands
{
	public sealed record UpdatePropertyCommand(Guid PropertyId, Guid OwnerId, 
		string Title, string Address) : IRequest;

	public sealed class UpdatePropertyCommandHandler
	: IRequestHandler<UpdatePropertyCommand>
	{
		private readonly IPropertyRepository _repository;

		public UpdatePropertyCommandHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task Handle(
			UpdatePropertyCommand request,
			CancellationToken cancellationToken)
		{
			var property = await _repository.GetByIdAsync(
				request.PropertyId,
				cancellationToken);

			if (property is null)
				throw new NotFoundException("Property not found");

			if (property.OwnerId != request.OwnerId)
				throw new ForbiddenException("You are not the owner of this property");

			property.Update(request.Title, request.Address);

			await _repository.UpdateAsync(property, cancellationToken);
		}
	}
}

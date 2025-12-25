using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Properties
{
	public sealed record UpdatePropertyCommand(Guid PropertyId, Guid OwnerId, 
		string Name, string Address, PropertyType Type) : IRequest<PropertyDto>;

	public sealed class UpdatePropertyCommandHandler
	: IRequestHandler<UpdatePropertyCommand, PropertyDto>
	{
		private readonly IPropertyRepository _repository;

		public UpdatePropertyCommandHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task<PropertyDto> Handle(
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

			property.Update(request.Name, request.Address, request.Type);

			await _repository.UpdateAsync(property, cancellationToken);
			return property.ToDto();
		}
	}
}

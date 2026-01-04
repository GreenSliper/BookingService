using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Application.Services;
using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands.Properties
{
	public sealed record UpdatePropertyCommand(Guid PropertyId, 
		string Name, string Address, PropertyType Type) : IRequest<PropertyDto>;

	public sealed class UpdatePropertyHandler
	: IRequestHandler<UpdatePropertyCommand, PropertyDto>
	{
		private readonly IPropertyRepository _repository;
		private readonly ICurrentUser _currentUser;

		public UpdatePropertyHandler(IPropertyRepository repository, ICurrentUser currentUser)
		{
			_repository = repository;
			_currentUser = currentUser;
		}

		public async Task<PropertyDto> Handle(UpdatePropertyCommand request, CancellationToken ct)
		{
			var property = await _repository.GetByIdAsync(
				request.PropertyId,
				ct);
			if (property is null)
				throw new NotFoundException("Property not found");
			if (property.OwnerId != _currentUser.Id)
				throw new ForbiddenException("You are not the owner of this property");

			property.Update(request.Name, request.Address, request.Type);

			await _repository.UpdateAsync(property, ct);
			return property.ToDto();
		}
	}
}

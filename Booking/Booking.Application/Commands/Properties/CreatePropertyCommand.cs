using Booking.Application.Dtos;
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
	public sealed record CreatePropertyCommand : IRequest<PropertyDto>
	{
		public required string Name { get; set; }
		public string Description { get; set; }
		public PropertyType Type { get; set; }
		public string Address { get; set; }
	}

	public class CreatePropertyHandler
	: IRequestHandler<CreatePropertyCommand, PropertyDto>
	{
		private readonly IPropertyRepository _repository;
		private readonly ICurrentUser _currentUser;

		public CreatePropertyHandler(IPropertyRepository repository, ICurrentUser currentUser)
		{
			_repository = repository;
			_currentUser = currentUser;
		}

		public async Task<PropertyDto> Handle(CreatePropertyCommand request, CancellationToken ct)
		{
			var property = Property.Create(
				_currentUser.Id,
				request.Name,
				request.Type,
				request.Address,
				request.Description);

			await _repository.AddAsync(property, ct);

			return property.ToDto();
		}
	}
}

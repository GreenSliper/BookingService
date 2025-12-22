using Booking.Application.Dtos;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Commands
{
	public class CreatePropertyCommand : IRequest<PropertyDto>
	{
		public Guid OwnerId { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public PropertyType Type { get; set; }
		public string Address { get; set; }
	}

	public class CreatePropertyCommandHandler
	: IRequestHandler<CreatePropertyCommand, PropertyDto>
	{
		private readonly IPropertyRepository _repository;

		public CreatePropertyCommandHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task<PropertyDto> Handle(
			CreatePropertyCommand request,
			CancellationToken cancellationToken)
		{
			var property = Property.Create(
				request.OwnerId,
				request.Title,
				request.Type,
				request.Address,
				request.Description);

			await _repository.AddAsync(property, cancellationToken);

			return property.ToDto();
		}
	}
}

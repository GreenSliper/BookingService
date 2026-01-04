using Booking.Application.Dtos;
using Booking.Application.Exceptions;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries.Properties
{
	public sealed record GetPropertyByIdQuery(Guid PropertyId) : IRequest<PropertyDto>;

	public sealed class GetPropertyByIdQueryHandler
	: IRequestHandler<GetPropertyByIdQuery, PropertyDto>
	{
		private readonly IPropertyRepository _repository;

		public GetPropertyByIdQueryHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task<PropertyDto> Handle(
			GetPropertyByIdQuery request,
			CancellationToken cancellationToken)
		{
			var property = await _repository.GetByIdAsync(
				request.PropertyId, cancellationToken);

			if (property == null)
				throw new NotFoundException($"Property with id {request.PropertyId} not found");

			return property.ToDto();
		}
	}
}

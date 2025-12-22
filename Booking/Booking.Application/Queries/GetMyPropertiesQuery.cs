using Booking.Application.Dtos;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries
{
	public sealed record GetMyPropertiesQuery(Guid UserId) : IRequest<IReadOnlyList<PropertyDto>>;

	public sealed class GetMyPropertiesQueryHandler
		: IRequestHandler<GetMyPropertiesQuery, IReadOnlyList<PropertyDto>>
	{
		private readonly IPropertyRepository _repository;

		public GetMyPropertiesQueryHandler(IPropertyRepository repository)
		{
			_repository = repository;
		}

		public async Task<IReadOnlyList<PropertyDto>> Handle(
			GetMyPropertiesQuery request,
			CancellationToken cancellationToken)
		{
			var properties = await _repository.GetByOwnerIdAsync(
				request.UserId,
				cancellationToken);

			return properties
				.Select(x => x.ToDto())
				.ToList();
		}
	}
}

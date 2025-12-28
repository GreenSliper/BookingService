using Booking.Application.Dtos;
using Booking.Application.Extensions;
using Booking.Application.Repos;
using Booking.Application.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Queries.Properties
{
	public sealed record GetMyPropertiesQuery() : IRequest<IReadOnlyList<PropertyDto>>;

	public sealed class GetMyPropertiesQueryHandler
		: IRequestHandler<GetMyPropertiesQuery, IReadOnlyList<PropertyDto>>
	{
		private readonly IPropertyRepository _repository;
		private readonly ICurrentUser _currentUser;

		public GetMyPropertiesQueryHandler(IPropertyRepository repository, ICurrentUser currentUser)
		{
			_repository = repository;
			_currentUser = currentUser;
		}

		public async Task<IReadOnlyList<PropertyDto>> Handle(
			GetMyPropertiesQuery request,
			CancellationToken cancellationToken)
		{
			var properties = await _repository.GetByOwnerIdAsync(
				_currentUser.Id,
				cancellationToken);

			return properties
				.Select(x => x.ToDto())
				.ToList();
		}
	}
}

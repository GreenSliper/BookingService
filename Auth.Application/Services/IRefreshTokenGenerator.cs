using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Services
{
	public interface IRefreshTokenGenerator
	{
		(RefreshToken entity, string token) Generate(Guid userId);
	}
}

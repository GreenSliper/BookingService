using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Exceptions
{
	public class EmailTakenException : Exception
	{
		public EmailTakenException(string email)
			: base($"The email '{email}' is already registered.")
		{
		}
	}
}

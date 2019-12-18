using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException()
			: base("User was not found")
		{
		}
	}
}

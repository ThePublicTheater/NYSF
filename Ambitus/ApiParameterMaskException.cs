using System;

namespace Ambitus
{
	public class ApiParameterMaskException : ApplicationException
	{
		public ApiParameterMaskException(string value)
			: base("Unable to mask API method parameter of value: " + value) { }
	}
}

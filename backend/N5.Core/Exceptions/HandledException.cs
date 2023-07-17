using System;
namespace N5.Core.Exceptions
{
	public class HandledException : Exception
	{
		public HandledException() : base()
		{
		}

		public HandledException(string message) : base(message)
		{

		}
	}
}


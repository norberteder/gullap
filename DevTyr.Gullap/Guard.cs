using System;

namespace DevTyr.Gullap
{
	public static class Guard
	{
		public static void NotNull (object obj, string argumentName, string message = null)
		{
			if (obj == null)
				throw new ArgumentNullException(argumentName, message);
		}

		public static void NotNullOrEmpty (string obj, string argumentName)
		{
			NotNull (obj, argumentName);

		    if (string.IsNullOrWhiteSpace(obj))
		        throw new ArgumentException(argumentName);
		}
	}
}

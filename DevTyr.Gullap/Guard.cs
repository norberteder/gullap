using System;

namespace DevTyr.Gullap
{
	public static class Guard
	{
		public static void NotNull (object obj, string argumentName)
		{
			if (obj == null)
				throw new ArgumentNullException(argumentName);
		}

		public static void NotNullOrEmpty (string obj, string argumentName)
		{
			NotNull (obj, argumentName);

		    if (string.IsNullOrWhiteSpace(obj))
		        throw new ArgumentException(argumentName);
		}
	}
}

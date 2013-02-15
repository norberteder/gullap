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

		public static void NotNullOrEmpty (object obj, string argumentName)
		{
			NotNull (obj, argumentName);
			if (obj is String) {
				String val = (String)obj;
				if (string.IsNullOrWhiteSpace(val))
					throw new ArgumentException(argumentName);
			}
		}
	}
}


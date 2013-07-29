using System.Collections.Generic;

namespace DevTyr.Gullap
{
	public class ConverterResult
	{
		public bool HasError { get; private set; }
		public string ErrorMessage { get; private set; }
		public List<string> SuccessMessages { get; private set; }

		public ConverterResult (bool hasError, string errorMessage, List<string> successMessages)
		{
			HasError = hasError;
			ErrorMessage = errorMessage;
			SuccessMessages = successMessages;
		}
	}
}


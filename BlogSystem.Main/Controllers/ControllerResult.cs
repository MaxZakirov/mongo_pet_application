using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Main.Controllers
{
	public enum ControllerResultStatus
	{
		Ok,
		Error
	}

	public struct ControllerResult
	{
		public ControllerResult(ControllerResultStatus resultStatus, string message = "")
		{
			ResultStatus = resultStatus;
			Message = message;
		}

		public ControllerResultStatus ResultStatus { get; }

		public string Message { get; }
	}
}

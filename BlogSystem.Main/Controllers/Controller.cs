using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Main.Controllers
{
	public abstract class Controller
	{
		protected ControllerResult Ok(string msg = "Success.")
		{
			return new ControllerResult(ControllerResultStatus.Ok, msg);
		}

		protected ControllerResult Error(string msg)
		{
			return new ControllerResult(ControllerResultStatus.Error, msg);
		}
	}
}

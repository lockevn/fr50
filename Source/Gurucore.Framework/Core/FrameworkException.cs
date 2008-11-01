using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core
{
	public class FrameworkException : Exception
	{
		public FrameworkException(string p_sMessage, Exception p_expInnerException)
			: base("[Gurucore Framework fatal exception] " + p_sMessage, p_expInnerException)
		{
		}
	}
}

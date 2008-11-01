using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Test
{
	public class AssertException : Exception
	{
		public AssertException(string p_sMessage)
			: base(p_sMessage)
		{
		}
	}
}

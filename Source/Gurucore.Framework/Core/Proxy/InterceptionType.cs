using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.Proxy
{
	public enum InterceptionType
	{
		BeforeCalling,
		AfterCallSuccess,
		AfterCallFailure,
		AfterReturned,
		CallWrapper
	}
}

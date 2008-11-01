﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.Proxy
{
	public class AfterCallFailureAttribute : InterceptAttribute
	{
		public AfterCallFailureAttribute(string p_sInterceptorClass, string p_sInterceptorAssembly)
			: base(p_sInterceptorClass, p_sInterceptorAssembly, InterceptionType.AfterCallFailure)
		{
		}
	}
}
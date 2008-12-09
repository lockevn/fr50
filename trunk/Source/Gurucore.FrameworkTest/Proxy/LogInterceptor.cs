﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class LogInterceptor : InterceptorBase
	{
		public override object Intercept(object p_oInstance, System.Reflection.MethodInfo p_oMethod, InterceptorType p_eInterceptorType, params object[] p_arrArg)
		{
			switch (p_eInterceptorType)
			{
				case InterceptorType.AfterCallSuccess:
					Console.WriteLine("Pass");
					break;
				case InterceptorType.AfterCallFailure:
					Console.WriteLine("Fail");
					break;
			}

			return null;
		}
	}
}

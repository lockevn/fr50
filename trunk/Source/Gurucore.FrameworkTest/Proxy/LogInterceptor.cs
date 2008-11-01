using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class LogInterceptor : InterceptorBase
	{
		public override object Intercept(object p_oInstance, System.Reflection.MethodInfo p_oMethod, InterceptionType p_eInterceptorType, params object[] p_arrArg)
		{
			switch (p_eInterceptorType)
			{
				case InterceptionType.AfterCallSuccess:
					//Console.WriteLine("Pass");
					break;
				case InterceptionType.AfterCallFailure:
					//Console.WriteLine("Fail");
					break;
			}

			return null;
		}
	}
}

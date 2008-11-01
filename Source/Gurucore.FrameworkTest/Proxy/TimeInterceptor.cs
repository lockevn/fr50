using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class TimeInterceptor : InterceptorBase
	{
		public override object Intercept(object p_oInstance, System.Reflection.MethodInfo p_oMethod, InterceptionType p_eInterceptorType,params object[] p_arrArg)
		{
			//Console.WriteLine("Now is " + DateTime.Now.ToShortTimeString());
			return null;
		}
	}
}

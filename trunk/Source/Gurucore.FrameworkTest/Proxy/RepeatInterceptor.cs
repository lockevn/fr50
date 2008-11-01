using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class RepeatInterceptor : InterceptorBase
	{
		public override object Intercept(object p_oInstance, System.Reflection.MethodInfo p_oMethod, InterceptionType p_eInterceptorType, params object[] p_arrArg)
		{
			if (p_eInterceptorType == InterceptionType.CallWrapper)
			{
				object oResult = null;

				//improvment need: parameterize repeat times
				for (int i = 0; i < 5; i++)
				{
					oResult = p_oMethod.Invoke(p_oInstance, p_arrArg);
				}

				return oResult;
			}
			else
			{
				return null;
			}
		}
	}
}

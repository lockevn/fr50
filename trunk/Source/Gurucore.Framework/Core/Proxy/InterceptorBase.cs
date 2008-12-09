using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace Gurucore.Framework.Core.Proxy
{
	public abstract class InterceptorBase
	{
		public abstract object Intercept(object p_oInstance, MethodInfo p_oMethod, InterceptorType p_eInterceptorType, params object[] p_arrArg);
	}
}

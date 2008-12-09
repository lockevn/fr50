using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.Proxy
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class CallWrapperAttribute : Attribute
	{
		private string m_sInterceptor;

		public string Interceptor
		{
			get { return m_sInterceptor; }
			set { m_sInterceptor = value; }
		}

		public CallWrapperAttribute(string p_sInterceptor)
		{
			m_sInterceptor = p_sInterceptor;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.Proxy
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AfterCallSuccessAttribute : Attribute
	{
		private string[] m_arrInterceptor;

		public string[] Interceptors
		{
			get { return m_arrInterceptor; }
			set { m_arrInterceptor = value; }
		}

		public AfterCallSuccessAttribute(params string[] p_arrInterceptors)
		{
			m_arrInterceptor = p_arrInterceptors;
		}
	}
}
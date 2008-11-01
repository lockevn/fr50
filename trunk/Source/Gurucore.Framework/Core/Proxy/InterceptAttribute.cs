using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.Proxy
{
	public class InterceptAttribute : Attribute
	{
		private string m_sInterceptorClass;

		public string InterceptorClass
		{
			get { return m_sInterceptorClass; }
			set { m_sInterceptorClass = value; }
		}

		private string m_sInterceptorAssembly;

		public string InterceptorAssembly
		{
			get { return m_sInterceptorAssembly; }
			set { m_sInterceptorAssembly = value; }
		}

		private InterceptionType m_eInterceptionType;

		public InterceptionType InterceptionType
		{
			get { return m_eInterceptionType; }
			set { m_eInterceptionType = value; }
		}

		public InterceptAttribute(string p_sInterceptorClass, string p_sInterceptorAssembly, InterceptionType p_eInterceptionType)
		{
			m_sInterceptorClass = p_sInterceptorClass;
			m_sInterceptorAssembly = p_sInterceptorAssembly;
			m_eInterceptionType = p_eInterceptionType;
		}
	}
}

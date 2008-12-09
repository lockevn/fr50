using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class SpokemanService
	{
		private string m_sName;

		public SpokemanService()
		{
		}

		public SpokemanService(string p_sName)
		{
			m_sName = p_sName;
		}

		[BeforeCalling(
			"Gurucore.FrameworkTest.Proxy.LogInterceptor@Gurucore.FrameworkTest.dll",
			"Gurucore.FrameworkTest.Proxy.TimeInterceptor@Gurucore.FrameworkTest.dll")]
		public virtual void SayHello()
		{
		}

		[BeforeCalling("Gurucore.FrameworkTest.Proxy.TimeInterceptor@Gurucore.FrameworkTest.dll")]
		[AfterReturned("Gurucore.FrameworkTest.Proxy.TimeInterceptor@Gurucore.FrameworkTest.dll")]
		[AfterCallSuccess("Gurucore.FrameworkTest.Proxy.LogInterceptor@Gurucore.FrameworkTest.dll")]
		public virtual bool Agree(string p_sQuestion)
		{
			return false;
		}

		[CallWrapper("Gurucore.FrameworkTest.Proxy.RepeatInterceptor@Gurucore.FrameworkTest.dll")]
		public virtual string GetName()
		{
			return m_sName;
		}

		public virtual string Name
		{
			[BeforeCalling("Gurucore.FrameworkTest.Proxy.TimeInterceptor@Gurucore.FrameworkTest.dll")]
			set
			{
				m_sName = value;
			}
			get
			{
				return m_sName;
			}
		}
	}
}

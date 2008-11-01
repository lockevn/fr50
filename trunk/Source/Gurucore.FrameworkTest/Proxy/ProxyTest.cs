using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.Proxy;
using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest.Proxy
{
	public class ProxyTest : TestSuiteBase
	{
		[TestCase]
		public void CreateProxy()
		{
			DynamicProxy oDynamicProxy = new DynamicProxy();
			SpokemanService svcSpokeman = oDynamicProxy.NewProxyInstance<SpokemanService>("Andy Wagner");

			svcSpokeman.SayHello();
			bool bAgree = svcSpokeman.Agree("Why are you so stupid?");
			string sName = svcSpokeman.GetName();

			this.AssertTrue(!sName.NullOrEmpty(), "Name is null");
		}
	}
}

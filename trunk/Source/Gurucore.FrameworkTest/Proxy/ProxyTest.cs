using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.JITGeneration;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.Activation;
using Gurucore.Framework.Core.Proxy;
using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest.Proxy
{
	public class ProxyTest : TestSuiteBase
	{
		[TestCase]
		public void GenerateProxy()
		{
			ProxyGenerator oGenerator = new ProxyGenerator(typeof(SpokemanService));

			MethodInfo oSayHelloMethod = typeof(SpokemanService).GetMethod("SayHello");
			oGenerator.AddMethodInterceptor(oSayHelloMethod, 
				"Gurucore.FrameworkTest.dll", 
				"Gurucore.FrameworkTest.Proxy.TimeInterceptor", 
				InterceptorType.BeforeCalling);
			oGenerator.AddMethodInterceptor(oSayHelloMethod, 
				"Gurucore.FrameworkTest.dll", 
				"Gurucore.FrameworkTest.Proxy.TimeInterceptor", 
				InterceptorType.AfterReturned);

			MethodInfo oAgreeMethod = typeof(SpokemanService).GetMethod("Agree");
			oGenerator.AddMethodInterceptor(oAgreeMethod, 
				"Gurucore.FrameworkTest.dll", 
				"Gurucore.FrameworkTest.Proxy.LogInterceptor", 
				InterceptorType.AfterCallSuccess);
			oGenerator.AddMethodInterceptor(oAgreeMethod,
				"Gurucore.FrameworkTest.dll",
				"Gurucore.FrameworkTest.Proxy.TimeInterceptor",
				InterceptorType.BeforeCalling);
			oGenerator.AddMethodInterceptor(oAgreeMethod,
				"Gurucore.FrameworkTest.dll",
				"Gurucore.FrameworkTest.Proxy.TimeInterceptor",
				InterceptorType.AfterReturned);

			MethodInfo oSayNameMethod = typeof(SpokemanService).GetMethod("GetName");
			oGenerator.AddMethodInterceptor(oSayNameMethod,
				"Gurucore.FrameworkTest.dll",
				"Gurucore.FrameworkTest.Proxy.RepeatInterceptor",
				InterceptorType.CallWrapper);

			PropertyInfo oNameProp = typeof(SpokemanService).GetProperty("Name");
			oGenerator.AddSetterInterceptor(oNameProp,
				"Gurucore.FrameworkTest.dll",
				"Gurucore.FrameworkTest.Proxy.TimeInterceptor",
				InterceptorType.BeforeCalling);

			Type oType = oGenerator.GetProxyClass();
			((SpokemanService)Activator.CreateInstance(oType)).GetName();
		}

		[TestCase]
		public void JITTest()
		{
			JITClassManager oJITClassManager = Application.GetInstance().GetGlobalSharedObject<JITClassManager>();

			StreamReader oFoo_cs = new StreamReader(@"D:\Projects\FR50\Source\Gurucore.FrameworkTest\Proxy\Foo.cs");
			StreamReader oBar_cs = new StreamReader(@"D:\Projects\FR50\Source\Gurucore.FrameworkTest\Proxy\Bar.cs");

			Activator.CreateInstance(oJITClassManager.GetRegisteredClass("Gurucore.FrameworkTest.Proxy.Foo", oFoo_cs.ReadToEnd(), true));
			oJITClassManager.GetRegisteredClass("Gurucore.FrameworkTest.Proxy.Bar", oBar_cs.ReadToEnd(), true);

			oFoo_cs.Close();
			oBar_cs.Close();
		}

		[TestCase]
		public void CreateProxy()
		{
			DynamicProxy oDynamicProxy = new DynamicProxy();
			SpokemanService svcSpokeman = oDynamicProxy.NewProxyInstance<SpokemanService>("Andy Wagner");

			svcSpokeman.Name = "Hehe";

			//bool bAgree = svcSpokeman.Agree("Why are you so stupid?");
			//string sName = svcSpokeman.GetName();

			//this.AssertTrue(!sName.NullOrEmpty(), "Name is null");
		}
	}
}

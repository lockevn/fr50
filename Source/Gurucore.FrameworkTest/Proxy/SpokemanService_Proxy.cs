using System;
using System.Reflection;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class SpokemanService_Proxy : Gurucore.FrameworkTest.Proxy.SpokemanService
	{
		public SpokemanService_Proxy()
			: base()
		{
		}
		public SpokemanService_Proxy(String p_sName)
			: base(p_sName)
		{
		}

		public override void SayHello()
		{
			Gurucore.FrameworkTest.Proxy.TimeInterceptor oBeforeCallingInterceptor = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();

			MethodInfo oMethod = this.GetType().GetMethod("SayHello");

			try
			{
				//before
				oBeforeCallingInterceptor.Intercept(this, oMethod, InterceptionType.BeforeCalling);

				base.SayHello();

				//after call success
			}
			catch (Exception ex)
			{
				//after call failure

				throw ex;
			}
			finally
			{
				//after returned
			}
		}
		public override Boolean Agree(String p_sQuestion)
		{
			Gurucore.FrameworkTest.Proxy.TimeInterceptor oBeforeCallingInterceptor = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();
			Gurucore.FrameworkTest.Proxy.TimeInterceptor oAfterReturnedInteceptor = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();
			Gurucore.FrameworkTest.Proxy.LogInterceptor oAfterCallSuccessInteceptor = new Gurucore.FrameworkTest.Proxy.LogInterceptor();
			Gurucore.FrameworkTest.Proxy.LogInterceptor oAfterCallFailureInteceptor = new Gurucore.FrameworkTest.Proxy.LogInterceptor();

			MethodInfo oMethod = this.GetType().GetMethod("Agree");

			Boolean oResult = default(Boolean);
			try
			{
				//before
				oBeforeCallingInterceptor.Intercept(this, oMethod, InterceptionType.BeforeCalling, p_sQuestion);

				oResult = base.Agree(p_sQuestion);

				//after call success
				oAfterCallSuccessInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallSuccess, p_sQuestion);
			}
			catch (Exception ex)
			{
				//after call failure
				oAfterCallFailureInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallFailure, p_sQuestion);

				throw ex;
			}
			finally
			{
				//after returned
				oAfterReturnedInteceptor.Intercept(this, oMethod, InterceptionType.AfterReturned, p_sQuestion);
			}
			return oResult;
		}
	}
}

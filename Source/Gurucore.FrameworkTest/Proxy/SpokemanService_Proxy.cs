using System;
using System.Reflection;

using Gurucore.Framework.Core.Proxy;

namespace Gurucore.FrameworkTest.Proxy
{
	public class SpokemanService_Proxy : SpokemanService
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


			MethodInfo oMethod = this.GetType().GetMethod("SayHello");


			try
			{

				Gurucore.FrameworkTest.Proxy.TimeInterceptor oBeforeCallingInterceptor1 = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();

				oBeforeCallingInterceptor1.Intercept(this, oMethod, InterceptorType.BeforeCalling);




				base.SayHello();



			}
			catch (Exception ex)
			{


				throw ex;
			}
			finally
			{

				Gurucore.FrameworkTest.Proxy.TimeInterceptor oAfterReturnedInterceptor1 = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();

				oAfterReturnedInterceptor1.Intercept(this, oMethod, InterceptorType.AfterReturned);


			}



		}

		public override Boolean Agree(String p_sQuestion)
		{

			Boolean result;


			MethodInfo oMethod = this.GetType().GetMethod("Agree");


			try
			{

				Gurucore.FrameworkTest.Proxy.TimeInterceptor oBeforeCallingInterceptor1 = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();

				oBeforeCallingInterceptor1.Intercept(this, oMethod, InterceptorType.BeforeCalling, p_sQuestion);




				result = base.Agree(p_sQuestion);



				Gurucore.FrameworkTest.Proxy.LogInterceptor oAfterCallSuccessInterceptor1 = new Gurucore.FrameworkTest.Proxy.LogInterceptor();

				oAfterCallSuccessInterceptor1.Intercept(this, oMethod, InterceptorType.BeforeCalling, p_sQuestion);


			}
			catch (Exception ex)
			{


				throw ex;
			}
			finally
			{

				Gurucore.FrameworkTest.Proxy.TimeInterceptor oAfterReturnedInterceptor1 = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();

				oAfterReturnedInterceptor1.Intercept(this, oMethod, InterceptorType.AfterReturned, p_sQuestion);


			}



			return result;

		}

		public override String GetName()
		{

			String result;


			MethodInfo oMethod = this.GetType().GetMethod("GetName");


			Gurucore.FrameworkTest.Proxy.RepeatInterceptor oCallWrapperInterceptor = new Gurucore.FrameworkTest.Proxy.RepeatInterceptor();


			result = (String)oCallWrapperInterceptor.Intercept(this, oMethod, InterceptorType.CallWrapper);





			return result;

		}



		public override string Name
		{
			get
			{
				MethodInfo oMethod = this.GetType().GetProperty("Name").GetAccessors()[0];
				String result;
				try
				{

					result = base.Name;

				}
				catch (Exception ex)
				{

					throw ex;
				}
				finally
				{

				}

				return result;
			}
			set
			{
				MethodInfo oMethod = this.GetType().GetProperty("Name").GetAccessors()[1];
				try
				{

					Gurucore.FrameworkTest.Proxy.TimeInterceptor oSetterBeforeCallingInterceptor1 = new Gurucore.FrameworkTest.Proxy.TimeInterceptor();
					oSetterBeforeCallingInterceptor1.Intercept(this, oMethod, InterceptorType.BeforeCalling);

					base.Name = value;

				}
				catch (Exception ex)
				{

					throw ex;
				}
				finally
				{

				}
			}
		}

	}
}

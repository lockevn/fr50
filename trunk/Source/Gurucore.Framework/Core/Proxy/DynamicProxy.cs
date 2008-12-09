using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Gurucore.Framework.Core.JITGeneration;

namespace Gurucore.Framework.Core.Proxy
{
	public class DynamicProxy
	{
		public Type GetProxyClass(Type p_oType)
		{
			ProxyGenerator oGenerator = new ProxyGenerator(p_oType);

			if (!oGenerator.IsGenerated())
			{
				//methods
				MethodInfo[] arrMethods = p_oType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
				foreach (MethodInfo oMethod in arrMethods)
				{
					if ((oMethod.Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName)
					{
						continue;
					}
					List<InterceptorInfo> arrInterceptors = AnalyzeMethod(oMethod);
					foreach (InterceptorInfo oInterceptor in arrInterceptors)
					{
						oGenerator.AddMethodInterceptor(oMethod, oInterceptor.Assembly, oInterceptor.Class, oInterceptor.Type);
					}
				}

				//properties
				PropertyInfo[] arrProperties = p_oType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
				foreach (PropertyInfo oProperty in arrProperties)
				{
					List<InterceptorInfo> arrInterceptors;

					MethodInfo[] arrAccessors = oProperty.GetAccessors();

					//Getter
					arrInterceptors = AnalyzeMethod(arrAccessors[0]);
					foreach (InterceptorInfo oInterceptor in arrInterceptors)
					{
						oGenerator.AddGetterInterceptor(oProperty, oInterceptor.Assembly, oInterceptor.Class, oInterceptor.Type);
					}

					//Setter
					arrInterceptors = AnalyzeMethod(arrAccessors[1]);
					foreach (InterceptorInfo oInterceptor in arrInterceptors)
					{
						oGenerator.AddSetterInterceptor(oProperty, oInterceptor.Assembly, oInterceptor.Class, oInterceptor.Type);
					}
				}
			}
			return oGenerator.GetProxyClass();
		}

		public object NewProxyInstance(Type p_oType, params object[] p_arrArg)
		{
			Type oGeneratedClass = this.GetProxyClass(p_oType);

			return Activator.CreateInstance(oGeneratedClass, p_arrArg);
		}

		public T NewProxyInstance<T>(params object[] p_arrArg)
		{
			return (T)this.NewProxyInstance(typeof(T), p_arrArg);
		}

		private List<InterceptorInfo> AnalyzeMethod(MethodInfo p_oMethod)
		{
			List<InterceptorInfo> arrResult = new List<InterceptorInfo>();

			object[] arrCallWrapperAttr = p_oMethod.GetCustomAttributes(typeof(CallWrapperAttribute), false);
			if (arrCallWrapperAttr.Length > 0)
			{
				string[] arrInterceptorPart = ((CallWrapperAttribute)arrCallWrapperAttr[0]).Interceptor.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
				InterceptorInfo oInterceptor = new InterceptorInfo();
				oInterceptor.Assembly = arrInterceptorPart[1];
				oInterceptor.Class = arrInterceptorPart[0];
				oInterceptor.Type = InterceptorType.CallWrapper;
				arrResult.Add(oInterceptor);
				return arrResult;
			}

			object[] arrBeforeCallingAttr = p_oMethod.GetCustomAttributes(typeof(BeforeCallingAttribute), false);
			if (arrBeforeCallingAttr.Length > 0)
			{
				string[] arrInterceptors = ((BeforeCallingAttribute)arrBeforeCallingAttr[0]).Interceptors;
				foreach (string sInterceptor in arrInterceptors)
				{
					string[] arrInterceptorPart = sInterceptor.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
					InterceptorInfo oInterceptor = new InterceptorInfo();
					oInterceptor.Assembly = arrInterceptorPart[1];
					oInterceptor.Class = arrInterceptorPart[0];
					oInterceptor.Type = InterceptorType.BeforeCalling;
					arrResult.Add(oInterceptor);
				}
			}

			object[] arrAfterCallSuccessAttr = p_oMethod.GetCustomAttributes(typeof(AfterCallSuccessAttribute), false);
			if (arrAfterCallSuccessAttr.Length > 0)
			{
				string[] arrInterceptors = ((AfterCallSuccessAttribute)arrAfterCallSuccessAttr[0]).Interceptors;
				foreach (string sInterceptor in arrInterceptors)
				{
					string[] arrInterceptorPart = sInterceptor.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
					InterceptorInfo oInterceptor = new InterceptorInfo();
					oInterceptor.Assembly = arrInterceptorPart[1];
					oInterceptor.Class = arrInterceptorPart[0];
					oInterceptor.Type = InterceptorType.AfterCallSuccess;
					arrResult.Add(oInterceptor);
				}
			}

			object[] arrAfterCallFailureAttr = p_oMethod.GetCustomAttributes(typeof(AfterCallFailureAttribute), false);
			if (arrAfterCallFailureAttr.Length > 0)
			{
				string[] arrInterceptors = ((AfterCallFailureAttribute)arrAfterCallFailureAttr[0]).Interceptors;
				foreach (string sInterceptor in arrInterceptors)
				{
					string[] arrInterceptorPart = sInterceptor.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
					InterceptorInfo oInterceptor = new InterceptorInfo();
					oInterceptor.Assembly = arrInterceptorPart[1];
					oInterceptor.Class = arrInterceptorPart[0];
					oInterceptor.Type = InterceptorType.AfterCallFailure;
					arrResult.Add(oInterceptor);
				}
			}

			object[] arrAfterReturnedAttr = p_oMethod.GetCustomAttributes(typeof(AfterReturnedAttribute), false);
			if (arrAfterReturnedAttr.Length > 0)
			{
				string[] arrInterceptors = ((AfterReturnedAttribute)arrAfterReturnedAttr[0]).Interceptors;
				foreach (string sInterceptor in arrInterceptors)
				{
					string[] arrInterceptorPart = sInterceptor.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
					InterceptorInfo oInterceptor = new InterceptorInfo();
					oInterceptor.Assembly = arrInterceptorPart[1];
					oInterceptor.Class = arrInterceptorPart[0];
					oInterceptor.Type = InterceptorType.AfterReturned;
					arrResult.Add(oInterceptor);
				}
			}

			return arrResult;
		}

		private struct InterceptorInfo
		{
			public string Assembly;
			public string Class;
			public InterceptorType Type;
		}
	}
}
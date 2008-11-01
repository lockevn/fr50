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
			return null;
		}

		public object NewProxyInstance(Type p_oType)
		{
			//generate proxy code
			return null;
		}

		public T NewProxyInstance<T>(params object[] p_arrArg)
		{
			string sCode = string.Empty;

			JITClassManager oJITClassManager = Application.GetInstance().GetGlobalSharedObject<JITClassManager>();
			string sClassName = typeof(T).FullName + "_Proxy";

			if (!oJITClassManager.IsRegistered(sClassName))
			{
				sCode =  GenerateClass(typeof(T));
			}
			Type oGeneratedClass = oJITClassManager.GetRegisteredClass(typeof(T).FullName + "_Proxy", sCode, true);

			return (T)Activator.CreateInstance(oGeneratedClass, p_arrArg);
		}

		private string GenerateClass(Type p_oType)
		{
			string sResult = string.Empty;
			StringBuilder sClassTemplate = new StringBuilder(
				"using System;\r\n").Append(
				"using System.Reflection;\r\n").Append(
				"\r\n").Append(
				"using Gurucore.Framework.Core.Proxy;\r\n").Append(
				"\r\n").Append(
				"namespace #namespace\r\n").Append(
				"{\r\n").Append(
				"	public sealed class #class_name_Proxy : #namespace.#class_name\r\n").Append(
				"	{\r\n").Append(
				"#constructor_list").Append(
				"		\r\n").Append(
				"#method_list").Append(
				"	}\r\n").Append(
				"}\r\n");

			string sClassName = p_oType.Name;
			string sNameSpace = p_oType.Namespace;
			
			//generate all constructors
			string sConstructorList = string.Empty;
			ConstructorInfo[] arrConstructor = p_oType.GetConstructors();
			foreach (ConstructorInfo oConstructor in arrConstructor)
			{
				sConstructorList += this.GenerateConstructor(oConstructor);
			}

			//generate all methods
			string sMethodList = string.Empty;
			MethodInfo[] arrMethod = p_oType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (MethodInfo oMethod in arrMethod)
			{
				sMethodList += this.GenerateMethod(oMethod);
			}

			sResult = sClassTemplate
				.Replace("#namespace", sNameSpace)
				.Replace("#class_name", sClassName)
				.Replace("#constructor_list", sConstructorList)
				.Replace("#method_list",sMethodList)
				.ToString();

			return sResult;
		}

		private string GenerateConstructor(ConstructorInfo p_oConstructor)
		{
			string sResult = string.Empty;
			StringBuilder sConstructorTemplate = new StringBuilder(
				"		public #class_name_Proxy(#param_pair_list) : base(#param_name_list)\r\n").Append(
				"		{\r\n").Append(
				"		}\r\n");

			string sClassName = p_oConstructor.DeclaringType.Name;
			StringBuilder sParamPairList = new StringBuilder();
			StringBuilder sParamNameList = new StringBuilder();

			ParameterInfo[] arrParameter = p_oConstructor.GetParameters();
			if (arrParameter.Length > 0)
			{
				foreach (ParameterInfo oParameter in arrParameter)
				{
					sParamPairList.Append(oParameter.ParameterType.Name).Append(" ").Append(oParameter.Name).Append(", ");
					sParamNameList.Append(oParameter.Name).Append(", ");
				}
				sParamPairList = sParamPairList.Remove(sParamPairList.Length - 2, 2);
				sParamNameList = sParamNameList.Remove(sParamNameList.Length - 2, 2);
			}

			sResult = sConstructorTemplate
				.Replace("#class_name", sClassName)
				.Replace("#param_pair_list", sParamPairList.ToString())
				.Replace("#param_name_list", sParamNameList.ToString())
				.ToString();

			return sResult;
		}

		private string GenerateMethod(MethodInfo p_oMethod)
		{
			object[] arrBeforeCallingAttr = p_oMethod.GetCustomAttributes(typeof(BeforeCallingAttribute), false);
			object[] arrAfterReturnedAttr = p_oMethod.GetCustomAttributes(typeof(AfterReturnedAttribute), false);
			object[] arrAfterCallSuccessAttr = p_oMethod.GetCustomAttributes(typeof(AfterCallSuccessAttribute), false);
			object[] arrAfterCallFailureAttr = p_oMethod.GetCustomAttributes(typeof(AfterCallFailureAttribute), false);

			if ((arrBeforeCallingAttr.Length > 0))
			{
				string sResult = string.Empty;
				StringBuilder sMethodTemplate = new StringBuilder(
					"		public override #return_type #method_name(#param_pair_list)\r\n").Append(
					"		{\r\n").Append(
					"#before_calling_interceptor_define").Append(
					"#after_returned_interceptor_define").Append(
					"#after_call_success_interceptor_define").Append(
					"#after_call_failure_interceptor_define").Append(
					"\r\n").Append(
					"			MethodInfo oMethod = this.GetType().GetMethod(\"#method_name\");\r\n").Append(
					"\r\n").Append(
					"#result_define").Append(
					"			try\r\n").Append(
					"			{\r\n").Append(
					"				//before\r\n").Append(
					"#before_calling_interceptor_call").Append(
					"\r\n").Append(
					"#call_statement").Append(
					"\r\n").Append(
					"				//after call success\r\n").Append(
					"#after_call_success_interceptor_call").Append(
					"			}\r\n").Append(
					"			catch (Exception ex)\r\n").Append(
					"			{\r\n").Append(
					"				//after call failure\r\n").Append(
					"#after_call_failure_interceptor_call").Append(
					"\r\n").Append(
					"				throw ex;\r\n").Append(
					"			}\r\n").Append(
					"			finally\r\n").Append(
					"			{\r\n").Append(
					"				//after returned\r\n").Append(
					"#after_returned_interceptor_call").Append(
					"			}\r\n").Append(
					"#return_statement").Append(
					"		}\r\n");

				bool bVoidMethod = p_oMethod.ReturnType == typeof(void);
				string sMethodName = p_oMethod.Name;
				string sReturnType = bVoidMethod ? "void" : p_oMethod.ReturnType.Name;
				string sReturnStatement = bVoidMethod ? "" : "			return oResult;";
				string sResultDefine = bVoidMethod ? 
					"" : 
					"			#return_type oResult = default(#return_type);\r\n".Replace("#return_type", sReturnType);

				StringBuilder sParamPairList = new StringBuilder();
				StringBuilder sParamNameList = new StringBuilder();

				ParameterInfo[] arrParameter = p_oMethod.GetParameters();
				if (arrParameter.Length > 0)
				{
					foreach (ParameterInfo oParameter in arrParameter)
					{
						sParamPairList.Append(oParameter.ParameterType.Name).Append(" ").Append(oParameter.Name).Append(", ");
						sParamNameList.Append(oParameter.Name).Append(", ");
					}
					sParamPairList = sParamPairList.Remove(sParamPairList.Length - 2, 2);
					sParamNameList = sParamNameList.Remove(sParamNameList.Length - 2, 2);
				}

				StringBuilder sCallStatement = bVoidMethod ?
					new StringBuilder("				base.#method_name(#param_name_list);\r\n")
						.Replace("#method_name", sMethodName)
						.Replace("#param_name_list", sParamNameList.ToString()) :
					new StringBuilder("				oResult = base.#method_name(#param_name_list);\r\n")
						.Replace("#method_name", sMethodName)
						.Replace("#param_name_list", sParamNameList.ToString());

				StringBuilder sBeforeCallingInterceptorDefine = null;
				StringBuilder sBeforeCallingInterceptorCall = null;
				if (arrBeforeCallingAttr.Length > 0)
				{
					BeforeCallingAttribute oAttr = (BeforeCallingAttribute)arrBeforeCallingAttr[0];
					sBeforeCallingInterceptorDefine = new StringBuilder("			#interceptor_class oBeforeCallingInterceptor = new #interceptor_class();\r\n")
						.Replace("#interceptor_class", oAttr.InterceptorClass);

					sBeforeCallingInterceptorCall = new StringBuilder("				oBeforeCallingInterceptor.Intercept(this, oMethod, InterceptionType.BeforeCalling#param_name_list);\r\n")
						.Replace("#param_name_list", sParamNameList.Length > 0 ? ", " + sParamNameList : "");
				}
				else
				{
					sBeforeCallingInterceptorDefine = new StringBuilder();
					sBeforeCallingInterceptorCall = new StringBuilder();
				}

				StringBuilder sAfterReturnedInterceptorDefine = null;
				StringBuilder sAfterReturnedInterceptorCall = null;
				if (arrAfterReturnedAttr.Length > 0)
				{
					AfterReturnedAttribute oAttr = (AfterReturnedAttribute)arrAfterReturnedAttr[0];
					sAfterReturnedInterceptorDefine = new StringBuilder("			#interceptor_class oAfterReturnedInteceptor = new #interceptor_class();\r\n")
						.Replace("#interceptor_class", oAttr.InterceptorClass);

					sAfterReturnedInterceptorCall = new StringBuilder("				oAfterReturnedInteceptor.Intercept(this, oMethod, InterceptionType.AfterReturned#param_name_list);\r\n")
						.Replace("#param_name_list", sParamNameList.Length > 0 ? ", " + sParamNameList : "");
				}
				else
				{
					sAfterReturnedInterceptorDefine = new StringBuilder();
					sAfterReturnedInterceptorCall = new StringBuilder();
				}

				StringBuilder sAfterCallSuccessInterceptorDefine = null;
				StringBuilder sAfterCallSuccessInterceptorCall = null;
				if (arrAfterCallSuccessAttr.Length > 0)
				{
					AfterCallSuccessAttribute oAttr = (AfterCallSuccessAttribute)arrAfterCallSuccessAttr[0];
					sAfterCallSuccessInterceptorDefine = new StringBuilder("			#interceptor_class oAfterCallSuccessInteceptor = new #interceptor_class();\r\n")
						.Replace("#interceptor_class", oAttr.InterceptorClass);

					sAfterCallSuccessInterceptorCall = new StringBuilder("				oAfterCallSuccessInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallSuccess#param_name_list);\r\n")
						.Replace("#param_name_list", sParamNameList.Length > 0 ? ", " + sParamNameList : "");
				}
				else
				{
					sAfterCallSuccessInterceptorDefine = new StringBuilder();
					sAfterCallSuccessInterceptorCall = new StringBuilder();
				}

				StringBuilder sAfterCallFailureInterceptorDefine = null;
				StringBuilder sAfterCallFailureInterceptorCall = null;
				if (arrAfterCallFailureAttr.Length > 0)
				{
					AfterCallFailureAttribute oAttr = (AfterCallFailureAttribute)arrAfterCallFailureAttr[0];
					sAfterCallFailureInterceptorDefine = new StringBuilder("			#interceptor_class oAfterCallFailureInteceptor = new #interceptor_class();\r\n")
						.Replace("#interceptor_class", oAttr.InterceptorClass);

					sAfterCallFailureInterceptorCall = new StringBuilder("				oAfterCallFailureInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallFailure#param_name_list);\r\n")
						.Replace("#param_name_list", sParamNameList.Length > 0 ? ", " + sParamNameList : "");
				}
				else
				{
					sAfterCallFailureInterceptorDefine = new StringBuilder();
					sAfterCallFailureInterceptorCall = new StringBuilder();
				}

				sResult = sMethodTemplate
					.Replace("#method_name", sMethodName)
					.Replace("#return_type", sReturnType)
					.Replace("#param_pair_list", sParamPairList.ToString())
					.Replace("#param_name_list", sParamNameList.ToString())
					.Replace("#result_define", sResultDefine)
					.Replace("#call_statement", sCallStatement.ToString())
					.Replace("#return_statement", sReturnStatement)
					.Replace("#before_calling_interceptor_define", sBeforeCallingInterceptorDefine.ToString())
					.Replace("#before_calling_interceptor_call", sBeforeCallingInterceptorCall.ToString())
					.Replace("#after_returned_interceptor_define", sAfterReturnedInterceptorDefine.ToString())
					.Replace("#after_returned_interceptor_call", sAfterReturnedInterceptorCall.ToString())
					.Replace("#after_call_success_interceptor_define", sAfterCallSuccessInterceptorDefine.ToString())
					.Replace("#after_call_success_interceptor_call", sAfterCallSuccessInterceptorCall.ToString())
					.Replace("#after_call_failure_interceptor_define", sAfterCallFailureInterceptorDefine.ToString())
					.Replace("#after_call_failure_interceptor_call", sAfterCallFailureInterceptorCall.ToString())
					.ToString();

				return sResult;
			}
			else
			{
				return null;
			}
		}
	}
}

/* class template

using System;
using System.Reflection;

using Gurucore.Framework.Core.Proxy;

namespace #namespace
{
	public class #class_name_Proxy : #classfullname
	{
		##constructor_list
		
		##method_list
	}
}
*/

/* constructor template
		public #class_name_Proxy(#paramtype #paramname) : base(#paramname)
		{
		}
*/

/* method template
		public override #return_type #method_name(#paramtype #paramname)
		{
			##if have before
			#interceptortype oBeforeCallingInteceptor = new #interceptortype();
			##endif
 
			##if have before
			#interceptortype oAfterReturnedInteceptor = new #interceptortype();
			##endif

			##if have before
			#interceptortype oAfterCallSuccessInteceptor = new #interceptortype();
			##endif
			
			##if have before
			#interceptortype oAfterCallFailureInteceptor = new #interceptortype();
			##endif

			MethodInfo oMethod = this.GetType().GetMethod("#method_name");

			#return_type oResult = default(#return_type);
			try
			{
				//before
				oBeforeCallingInteceptor.Intercept(this, oMethod, InterceptionType.BeforeCalling, #paramname);

				oResult = base.#method_name(#paramname);
				
				//after call success
				oAfterCallSuccessInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallSuccess, #paramname);
			}
			catch (Exception ex)
			{
				//after call failure
				oAfterCallFailureInteceptor.Intercept(this, oMethod, InterceptionType.AfterCallSuccess, #paramname);

				throw ex;
			}
			finally
			{
				//after returned
				oAfterReturnedInteceptor.Intercept(this, oMethod, InterceptionType.AfterReturned, #paramname);
			}
			return oResult;
		}
*/
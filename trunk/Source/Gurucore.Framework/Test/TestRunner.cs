using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gurucore.Framework.Test
{
	public class TestRunner
	{
		public TestRunner()
		{
		}

		public IEnumerable<TestResult> Run(TestSuiteBase p_oTestSuite)
		{
			Type oTestSuiteType = p_oTestSuite.GetType();
			MethodInfo[] arrTestMethod = oTestSuiteType.GetMethods();

			foreach (MethodInfo oTestMethod in arrTestMethod)
			{
				TestResult oResult = this.Run(oTestMethod, p_oTestSuite);
				if (oResult != null)
				{
					yield return oResult;
				}
			}
		}

		public TestResult Run(MethodInfo p_oTestMethod, object p_oTestSuite)
		{
			object[] arrTestCaseAttr = p_oTestMethod.GetCustomAttributes(typeof(TestCaseAttribute), true);
			if (arrTestCaseAttr.Length > 0)
			{
				bool bLoadTest = ((TestCaseAttribute)arrTestCaseAttr[0]).LoadTest;
				int nRepeat = ((TestCaseAttribute)arrTestCaseAttr[0]).Repeat;
				if (nRepeat == 0)
				{
					nRepeat = 1;
				}

				TestResult oTestResult = new TestResult(p_oTestMethod);

				//check if valid testcase
				if ((p_oTestMethod.ReturnType == typeof(void)) && (p_oTestMethod.GetParameters().Length == 0))
				{
					try
					{
						DateTime dtStart = DateTime.Now;
						for (int i = 0; i < nRepeat; i++)
						{
							p_oTestMethod.Invoke(p_oTestSuite, null);
						}
						if (bLoadTest)
						{
							oTestResult.Runtime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;
						}
						else
						{
							oTestResult.Runtime = 0.0;
						}
						oTestResult.Pass = true;
					}
					catch (Exception ex)
					{
						if (ex.InnerException is AssertException)
						{
							oTestResult.Pass = false;
							oTestResult.ErrorMessage = ex.InnerException.Message;
						}
						else
						{
							oTestResult.Fatal = true;
							oTestResult.ErrorMessage = ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
						}
					}
				}
				else
				{
					oTestResult.Fatal = true;
					oTestResult.ErrorMessage = "Invalid test method. It should be a void method with no parameter";
				}

				return oTestResult;
			}
			else
			{
				return null;
			}
		}
	}
}

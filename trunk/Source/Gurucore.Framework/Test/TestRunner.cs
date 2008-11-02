using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gurucore.Framework.Test
{
	public class TestRunner
	{
		TestSuiteBase m_oTestSuite;

		public TestRunner(TestSuiteBase p_oTestSuite)
		{
			m_oTestSuite = p_oTestSuite;
		}

		public IEnumerable<TestResult> Run()
		{
			Type oTestSuiteType = m_oTestSuite.GetType();
			MethodInfo[] arrTestMethod = oTestSuiteType.GetMethods();

			foreach (MethodInfo oTestMethod in arrTestMethod)
			{
				object[] arrTestCaseAttr = oTestMethod.GetCustomAttributes(typeof(TestCaseAttribute), true);
				if (arrTestCaseAttr.Length > 0)
				{
					bool bLoadTest = ((TestCaseAttribute)arrTestCaseAttr[0]).LoadTest;
					int nRepeat = ((TestCaseAttribute)arrTestCaseAttr[0]).Repeat;
					if (nRepeat == 0)
					{
						nRepeat = 1;
					}

					TestResult oTestResult = new TestResult(oTestMethod);

					//check if valid testcase
					if ((oTestMethod.ReturnType == typeof(void)) && (oTestMethod.GetParameters().Length == 0))
					{
						try
						{
							DateTime dtStart = DateTime.Now;
							for (int i = 0; i < nRepeat; i++)
							{
								oTestMethod.Invoke(m_oTestSuite, null);
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
								oTestResult.Message = ex.InnerException.Message;
							}
							else
							{
								oTestResult.Fatal = true;
								oTestResult.Message = ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
							}
						}
					}
					else
					{
						oTestResult.Fatal = true;
						oTestResult.Message = "Invalid test method. It should be a void method with no parameter";
					}

					yield return oTestResult;
				}
			}
		}
	}
}

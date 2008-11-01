using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gurucore.Framework.Test
{
	public class TestResult
	{
		MethodInfo m_oTestMethod;

		public MethodInfo TestMethod
		{
			get { return m_oTestMethod; }
			set { m_oTestMethod = value; }
		}

		bool m_bPass;

		public bool Pass
		{
			get { return m_bPass; }
			set { m_bPass = value; }
		}

		bool m_bFatal;

		public bool Fatal
		{
			get { return m_bFatal; }
			set { m_bFatal = value; }
		}

		string m_sMessage;

		public string Message
		{
			get { return m_sMessage; }
			set { m_sMessage = value; }
		}

		public TestResult(MethodInfo p_oTestMethod)
		{
			m_oTestMethod = p_oTestMethod;
		}
	}
}

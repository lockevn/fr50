using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gurucore.Framework.Test
{
	public class TestResult
	{
		private MethodInfo m_oTestMethod;

		public MethodInfo TestMethod
		{
			get { return m_oTestMethod; }
			set { m_oTestMethod = value; }
		}

		private bool m_bPass;

		public bool Pass
		{
			get { return m_bPass; }
			set { m_bPass = value; }
		}

		private bool m_bFatal;

		public bool Fatal
		{
			get { return m_bFatal; }
			set { m_bFatal = value; }
		}

		private string m_sMessage;

		public string Message
		{
			get { return m_sMessage; }
			set { m_sMessage = value; }
		}

		private double m_dblRuntime;

		public double Runtime
		{
			get { return m_dblRuntime; }
			set { m_dblRuntime = value; }
		}

		public TestResult(MethodInfo p_oTestMethod)
		{
			m_oTestMethod = p_oTestMethod;
		}
	}
}

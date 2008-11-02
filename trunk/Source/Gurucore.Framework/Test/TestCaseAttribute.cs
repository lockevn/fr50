using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Test
{
	[AttributeUsage(AttributeTargets.Method)]
	public class TestCaseAttribute : Attribute
	{
		private bool m_bLoadTest;

		public bool LoadTest
		{
			get { return m_bLoadTest; }
			set { m_bLoadTest = value; }
		}

		private int m_nRepeat;

		public int Repeat
		{
			get { return m_nRepeat; }
			set { m_nRepeat = value; }
		}
	}
}

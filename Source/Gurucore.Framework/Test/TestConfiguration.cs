using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Core.Configuration;

namespace Gurucore.Framework.Test
{
	[XmlClass("test")]
	public class TestConfiguration : ConfigurationBase
	{
		private List<string> m_arrTestTarget;

		[XmlSubSequence]
		public List<string> TestTarget
		{
			get { return m_arrTestTarget; }
			set { m_arrTestTarget = value; }
		}

		private bool m_bLogResult;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public bool LogResult
		{
			get { return m_bLogResult; }
			set { m_bLogResult = value; }
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.FrameworkTest.XmlBinding
{
	[XmClass]
	public class Language
	{
		private string m_sName;

		[XmlProperty(Type = XmlPropertyType.InnerText)]
		public string Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		private int m_nPercentage;

		[XmlProperty]
		public int Percentage
		{
			get { return m_nPercentage; }
			set { m_nPercentage = value; }
		}
	}
}

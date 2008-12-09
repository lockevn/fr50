using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.FrameworkTest.XmlBinding
{
	[XmlClass]
	public class City
	{
		private String m_sName;
		private int m_nPopulation;
		private bool m_bIsCapital;

		[XmlProperty(Type = XmlPropertyType.InnerText)]
		public String Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		[XmlProperty]
		public int Population
		{
			get { return m_nPopulation; }
			set { m_nPopulation = value; }
		}

		[XmlProperty]
		public bool IsCapital
		{
			get { return m_bIsCapital; }
			set { m_bIsCapital = value; }
		}
	}
}

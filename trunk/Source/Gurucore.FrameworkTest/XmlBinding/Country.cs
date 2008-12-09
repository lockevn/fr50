using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.FrameworkTest.XmlBinding
{
	[XmlClass]
	public class Country
	{
		private string m_sName;
		private Dictionary<string, City> m_arrCity;
		private Language m_oLanguage;

		[XmlProperty]
		public string Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public Language Language
		{
			get { return m_oLanguage; }
			set { m_oLanguage = value; }
		}

		[XmlSubSequence]
		public Dictionary<string, City> City
		{
			get { return m_arrCity; }
			set { m_arrCity = value; }
		}
	}
}

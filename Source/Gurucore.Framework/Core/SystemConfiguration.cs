using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Core.Configuration;

namespace Gurucore.Framework.Core
{
	[XmClass("system")]
	public class SystemConfiguration : ConfigurationBase
	{
		private string m_sParameter;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string Parameter
		{
			get { return m_sParameter; }
			set { m_sParameter = value; }
		}

		private string m_sDefaultLocale;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string DefaultLocale
		{
			get { return m_sDefaultLocale; }
			set { m_sDefaultLocale = value; }
		}

		private Dictionary<string, Capability> m_dicCapability;

		[XmlSubSequence]
		public Dictionary<string, Capability> Capability
		{
			get { return m_dicCapability; }
			set { m_dicCapability = value; }
		}
	}
}

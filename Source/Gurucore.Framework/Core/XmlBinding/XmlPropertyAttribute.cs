using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.XmlBinding
{
	[AttributeUsage(AttributeTargets.Property)]
	public class XmlPropertyAttribute : Attribute
	{
		private String m_sName;

		public String Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}

		private XmlPropertyType m_eType;

		public XmlPropertyType Type
		{
			get { return m_eType; }
			set { m_eType = value; }
		}

		public XmlPropertyAttribute() : this(null, XmlPropertyType.Attribute)
		{
		}

		public XmlPropertyAttribute(string p_sAttributeName, XmlPropertyType p_eType)
		{
			m_sName = p_sAttributeName;
			m_eType = p_eType;
		}
	}
}

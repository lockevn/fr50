using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.XmlBinding
{
	[AttributeUsage(AttributeTargets.Property)]
	public class XmlSubSequenceAttribute : Attribute
	{
		private string m_sElementName;
		
		public string ElementName
		{
			get { return m_sElementName; }
			set { m_sElementName = value; }
		}

		private string m_sHashKey;
		
		public string HashKey
		{
			get { return m_sHashKey; }
			set { m_sHashKey = value; }
		}

		public XmlSubSequenceAttribute()
			: this(null, null)
		{
		}

		public XmlSubSequenceAttribute(string p_sElementName, string p_sHashKey)
		{
			m_sElementName = p_sElementName;
			m_sHashKey = p_sHashKey;
		}
	}
}

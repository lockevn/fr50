using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.Core.XmlBinding
{
	public class XmClassAttribute : Attribute
	{
		private string m_sElementName;

		public string ElementName
		{
			get { return m_sElementName; }
			set { m_sElementName = value; }
		}

		public XmClassAttribute() : this(null)
		{
		}

		public XmClassAttribute(string p_sElementName)
		{
			m_sElementName = p_sElementName;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.DataAccess.DataSource
{
	[XmClass]
	public class ConnectionInfo
	{
		string m_sValue;

		[XmlProperty(Type = XmlPropertyType.InnerText)]
		public string Value
		{
			get { return m_sValue; }
			set { m_sValue = value; }
		}

		bool m_bEncrypted;

		[XmlProperty]
		public bool Encrypted
		{
			get { return m_bEncrypted; }
			set { m_bEncrypted = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Factory;
using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.DataAccess.DataSource
{
	[XmClass]
	public class DataSource : FactoryItemBase
	{
		private string m_sProvider;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string Provider
		{
			get { return m_sProvider; }
			set { m_sProvider = value; }
		}

		private string m_sTablePrefix;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string TablePrefix
		{
			get { return m_sTablePrefix; }
			set { m_sTablePrefix = value; }
		}

		private Dictionary<string, ConnectionInfo> m_dicConnectionInfo;

		[XmlSubSequence(HashKey = "key")]
		public Dictionary<string, ConnectionInfo> ConnectionInfo
		{
			get { return m_dicConnectionInfo; }
			set { m_dicConnectionInfo = value; }
		}
	}
}

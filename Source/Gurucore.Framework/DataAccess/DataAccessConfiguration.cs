using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Core.Configuration;

namespace Gurucore.Framework.DataAccess
{
	[XmlClass("data_access")]
	public class DataAccessConfiguration : ConfigurationBase
	{
		private string m_sSqlGeneratorAssembly;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string SqlGeneratorAssembly
		{
			get { return m_sSqlGeneratorAssembly; }
			set { m_sSqlGeneratorAssembly = value; }
		}

		private string m_sSqlGeneratorClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string SqlGeneratorClass
		{
			get { return m_sSqlGeneratorClass; }
			set { m_sSqlGeneratorClass = value; }
		}

		private string m_sTransformStrategy;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string TransformStrategy
		{
			get { return m_sTransformStrategy; }
			set { m_sTransformStrategy = value; }
		}

		private bool m_bSchemaSupport;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public bool SchemaSupport
		{
			get { return m_bSchemaSupport; }
			set { m_bSchemaSupport = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Core.Factory;

namespace Gurucore.Framework.DataAccess.DataProvider
{
	[XmlClass]
	public class DataProvider : FactoryItemBase
	{
		private string m_sAssembly;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string Assembly
		{
			get { return m_sAssembly; }
			set { m_sAssembly = value; }
		}

		private string m_sConnectionClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string ConnectionClass
		{
			get { return m_sConnectionClass; }
			set { m_sConnectionClass = value; }
		}

		private string m_sDataAdapterClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string DataAdapterClass
		{
			get { return m_sDataAdapterClass; }
			set { m_sDataAdapterClass = value; }
		}

		private string m_sCommandClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string CommandClass
		{
			get { return m_sCommandClass; }
			set { m_sCommandClass = value; }
		}

		private string m_sParameterClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string ParameterClass
		{
			get { return m_sParameterClass; }
			set { m_sParameterClass = value; }
		}

		private string m_sDataReaderClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string DataReaderClass
		{
			get { return m_sDataReaderClass; }
			set { m_sDataReaderClass = value; }
		}

		private string m_sTransactionClass;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string TransactionClass
		{
			get { return m_sTransactionClass; }
			set { m_sTransactionClass = value; }
		}

		private string m_sParameterForm;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string ParameterForm
		{
			get { return m_sParameterForm; }
			set { m_sParameterForm = value; }
		}

		private string m_sInlineParameterForm;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string InlineParameterForm
		{
			get { return m_sInlineParameterForm; }
			set { m_sInlineParameterForm = value; }
		}

		private string m_sUnicodeForm;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string UnicodeForm
		{
			get { return m_sUnicodeForm; }
			set { m_sUnicodeForm = value; }
		}

		private string m_sBooleanValues;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string BooleanValues
		{
			get { return m_sBooleanValues; }
			set { m_sBooleanValues = value; }
		}

		private string m_sLatestIdentityStatement;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string LatestIdentityStatement
		{
			get { return m_sLatestIdentityStatement; }
			set { m_sLatestIdentityStatement = value; }
		}

		private bool m_bAllowBatchQuery;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public bool AllowBatchQuery
		{
			get { return m_bAllowBatchQuery; }
			set { m_bAllowBatchQuery = value; }
		}

		private string m_sSelectTemplate;

		[XmlProperty(Type = XmlPropertyType.NestedElement)]
		public string SelectTemplate
		{
			get { return m_sSelectTemplate; }
			set { m_sSelectTemplate = value; }
		}
	}
}

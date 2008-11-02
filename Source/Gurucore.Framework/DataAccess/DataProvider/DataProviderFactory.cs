using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

using Gurucore.Framework.Core.Factory;
using Gurucore.Framework.Core.Activation;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.DataProvider
{
	public class DataProviderFactory : FactoryBase<DataProvider>
	{
		private string m_sDefaultDPAssembly;

		public DataProviderFactory(string p_sDescriptorFile)
			: base(p_sDescriptorFile)
		{
			const int URI_OFFSET = 8; // "file:///".Length;
			m_sDefaultDPAssembly = Assembly.GetAssembly(typeof(System.Data.IDbConnection)).CodeBase.Substring(URI_OFFSET);
		}

		public IDbConnection GetDbConnection(string p_sConnStr)
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetDbConnection(p_sConnStr, sDefaultSqlDS);

		}

		public IDbConnection GetDbConnection(string p_sConnStr, string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			object[] arrArgs = { p_sConnStr };
			DynamicActivator oActivator = Application.GetInstance().GetGlobalSharedObject<DynamicActivator>();
			object oInstance = oActivator.GetObject(oDataProvider.Assembly.NullOrEmpty() ? m_sDefaultDPAssembly : oDataProvider.Assembly, oDataProvider.ConnectionClass, arrArgs);
			return (IDbConnection)oInstance;
		}

		public IDbCommand GetDbCommand(IDbConnection p_oConn)
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetDbCommand(p_oConn, sDefaultSqlDS);
		}

		public IDbCommand GetDbCommand(IDbConnection p_oConn, string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			object[] arrArgs = { string.Empty, p_oConn };

			DynamicActivator oActivator = Application.GetInstance().GetGlobalSharedObject<DynamicActivator>();
			object oInstance = oActivator.GetObject(oDataProvider.Assembly.NullOrEmpty() ? m_sDefaultDPAssembly : oDataProvider.Assembly, oDataProvider.CommandClass, arrArgs);
			return (IDbCommand)oInstance;
		}

		public string GetInlineParameterForm()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetInlineParameterForm(sDefaultSqlDS);
		}

		public string GetInlineParameterForm(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.InlineParameterForm;
		}

		public string GetParameterForm()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetParameterForm(sDefaultSqlDS);
		}

		public string GetParameterForm(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.ParameterForm;
		}

		public string GetUnicodeForm()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetUnicodeForm(sDefaultSqlDS);
		}

		public string GetUnicodeForm(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.UnicodeForm;
		}

		public string GetBooleanValue()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetBooleanValues(sDefaultSqlDS);
		}

		public string GetBooleanValues(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.BooleanValues;
		}

		public string GetLatestIdentityStatement()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetLatestIdentityStatement(sDefaultSqlDS);
		}

		public string GetLatestIdentityStatement(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.LatestIdentityStatement;
		}

		public bool GetAllowBatchQuery()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetAllowBatchQuery(sDefaultSqlDS);
		}

		public bool GetAllowBatchQuery(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.AllowBatchQuery;
		}

		public string GetSelectTemplate()
		{
			string sDefaultSqlDS = this.GetFactoryAttribute("default");
			return this.GetSelectTemplate(sDefaultSqlDS);
		}

		public string GetSelectTemplate(string p_sDataProviderName)
		{
			DataProvider oDataProvider = (DataProvider)this.GetItem(p_sDataProviderName);
			return oDataProvider.SelectTemplate.Trim();
		}
	}
}

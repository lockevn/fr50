using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.DataSource;
using Gurucore.Framework.DataAccess.DataProvider;

namespace Gurucore.Framework.DataAccess
{
	public class DataAccessContext
	{
		private Stack<string> m_stkDataSource = new Stack<string>();
		private Dictionary<string, IDbTransaction> m_dicTransaction = new Dictionary<string, IDbTransaction>();
		private Dictionary<string, string> m_dicDefaultDataSource = new Dictionary<string, string>();
		private int m_nTransactionCount = 0;

		private DataAccessContext()
		{
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			List<string> arrDSInfo = oDSFactory.GetItemNames();
			foreach (string sDSName in arrDSInfo)
			{
				string[] arrDSNameElement = sDSName.Split('#');
				if (arrDSNameElement.Length == 1)
				{
					if (!m_dicDefaultDataSource.ContainsKey(string.Empty))
					{
						m_dicDefaultDataSource.Add(string.Empty, sDSName);
					}
				}
				else if (arrDSNameElement.Length == 2)
				{
					if (!m_dicDefaultDataSource.ContainsKey(arrDSNameElement[0]))
					{
						m_dicDefaultDataSource.Add(arrDSNameElement[0], sDSName);
					}
				}
			}
		}

		private string GetAssemblyID(object p_oCaller)
		{
			object[] arrAttr = p_oCaller.GetType().Assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
			if (arrAttr.Length == 1)
			{
				return ((System.Runtime.InteropServices.GuidAttribute)arrAttr[0]).Value;
			}
			else
			{
				return string.Empty;
			}
		}

		public void SetCurrentDataSource(object p_oCaller, string p_sDataSource)
		{
			string sAssemblyID = this.GetAssemblyID(p_oCaller);
			this.SetCurrentDataSource(sAssemblyID, p_sDataSource);
		}

		public void SetCurrentDataSource(string p_sAssemblyID, string p_sDataSource)
		{
			if (p_sDataSource == string.Empty)
			{
				//use module default
				m_stkDataSource.Push(m_dicDefaultDataSource[p_sAssemblyID]);
			}
			else
			{
				m_stkDataSource.Push(p_sAssemblyID + "#" + p_sDataSource);
			}
		}

		public string UnSetCurrentDataSource()
		{
			return m_stkDataSource.Pop();
		}

		public string GetCurrentDataSource(object p_oCaller)
		{
			return m_stkDataSource.Peek();
		}

		public void EnterTransaction()
		{
			m_nTransactionCount++;
		}

		public void LeaveTransaction(bool p_bCommit)
		{
			m_nTransactionCount--;
			if (m_nTransactionCount < 0)
			{
				throw new Exception("Inconsistency transaction stack");
			}

			if (p_bCommit)
			{
				CommitTransaction();
			}
			else
			{
				RollbackTransaction();
			}

			m_dicTransaction.Clear();
		}

		public IDbConnection GetConnection()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			if (m_nTransactionCount > 0)
			{
				if (null == m_dicTransaction[sCurrentDataSource])
				{
					DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
					string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
					string sConnStr = oDSFactory.GetConnectionString(sCurrentDataSource);

					DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
					IDbConnection oDbConn = oDPFactory.GetDbConnection(sConnStr, sProvider);

					oDbConn.Open();
					IDbTransaction oDbTran = oDbConn.BeginTransaction();

					m_dicTransaction.Add(sCurrentDataSource, oDbTran);
					return oDbConn;
				}
				else
				{
					IDbTransaction oDbTran = (IDbTransaction)m_dicTransaction[sCurrentDataSource];
					return oDbTran.Connection;
				}
			}
			else
			{
				DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
				string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
				string sConnStr = oDSFactory.GetConnectionString(sCurrentDataSource);

				DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
				IDbConnection oDbConn = oDPFactory.GetDbConnection(sConnStr, sProvider);

				oDbConn.Open();

				return oDbConn;
			}
		}

		public void ReturnConnection(IDbConnection p_oDbConn)
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			if (m_nTransactionCount > 0)
			{

			}
			else
			{
				if (p_oDbConn.State != ConnectionState.Closed)
				{
					p_oDbConn.Close();
				}
			}
		}

		private void CommitTransaction()
		{
			if (m_nTransactionCount == 0)
			{
				foreach (KeyValuePair<string, IDbTransaction> oEntry in m_dicTransaction)
				{
					IDbTransaction oDbTran = (IDbTransaction)oEntry.Value;
					IDbConnection oDbConn = oDbTran.Connection;
					oDbTran.Commit();
					if (oDbConn.State != ConnectionState.Closed)
					{
						oDbConn.Close();
					}
				}
			}
		}

		private void RollbackTransaction()
		{
			if (m_nTransactionCount == 0)
			{
				foreach (KeyValuePair<string, IDbTransaction> oEntry in m_dicTransaction)
				{
					IDbTransaction oDbTran = (IDbTransaction)oEntry.Value;
					IDbConnection oDbConn = oDbTran.Connection;
					oDbTran.Rollback();
					if (oDbConn.State != ConnectionState.Closed)
					{
						oDbConn.Close();
					}
				}
			}
		}

		public static DataAccessContext GetDataAccessContext()
		{
			DataAccessContext oDACtx = Application.GetInstance().GetThreadSharedObject<DataAccessContext>();

			if (oDACtx == null)
			{
				oDACtx = new DataAccessContext();
				Application.GetInstance().SetThreadSharedObject(oDACtx);
			}
	
			return oDACtx;
		}

		public IDbCommand GetDbCommand(IDbConnection p_oDbConn)
		{
			string sCurrentDataSource = m_stkDataSource.Peek();

			IDbCommand oDbCmd = p_oDbConn.CreateCommand();
			if (m_nTransactionCount > 0)
			{
				oDbCmd.Transaction = (IDbTransaction)m_dicTransaction[sCurrentDataSource];
			}
			return oDbCmd;
		}

		public string GetTablePrefix()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sTablePrefix = oDSFactory.GetTablePrefix(sCurrentDataSource);
			return sTablePrefix;
		}

		public string GetDateTimeFormat()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sDateTimeFormat = oDSFactory.GetDateTimeFormat(sCurrentDataSource);
			return sDateTimeFormat;
		}

		public string GetNumberFormat()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sNumberFormat = oDSFactory.GetNumberFormat(sCurrentDataSource);
			return sNumberFormat;
		}

		public string GetUnicodeForm()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetUnicodeForm(sProvider);
			return sReturn;
		}

		public string GetBooleanValues()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetBooleanValues(sProvider);
			return sReturn;
		}

		public string GetSelectTemplate()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetSelectTemplate(sProvider);
			return sReturn;
		}

		public string GetInlineParameterForm()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetInlineParameterForm(sProvider);
			return sReturn;
		}

		public string GetParameterForm()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetParameterForm(sProvider);
			return sReturn;
		}

		public string GetLatestIdentityStatement()
		{
			string sCurrentDataSource = m_stkDataSource.Peek();
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sProvider = oDSFactory.GetProvider(sCurrentDataSource);
			DataProviderFactory oDPFactory = Application.GetInstance().GetGlobalSharedObject<DataProviderFactory>();
			string sReturn = oDPFactory.GetLatestIdentityStatement(sProvider);
			return sReturn;
		}
	}
}

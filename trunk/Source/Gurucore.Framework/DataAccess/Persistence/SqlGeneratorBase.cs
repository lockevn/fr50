using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using Gurucore.Framework.Core.Util;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public abstract class SqlGeneratorBase : IExpressionMaker
	{
		public const string CACHE_KEY = "Gurucore.Framework.DataAccess.Persistence.SqlGenerator";

		public abstract void PrepareRecordSelectCommand<T>(IDbCommand p_oDbCmd, int p_nID, string[] p_arrColumn);

		protected void PrepareRecordSelectCommand(IDbCommand p_oDbCmd, string p_sTable, string p_sPrimaryKey, bool p_bAllowTablePrefix, int p_nID, string[] p_arrColumn)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = p_bAllowTablePrefix ? oDACtx.GetTablePrefix() : string.Empty;

			StringBuilder sTemplate = new StringBuilder("SELECT #column_list FROM #table WHERE #primary_key = #id ");

			StringBuilder sColumnList = new StringBuilder();
			foreach (string sColumn in p_arrColumn)
			{
				sColumnList.Append(sColumn).Append(", ");
			}
			sColumnList.Remove(sColumnList.Length - 2, 2);

			string sQuery = sTemplate
				.Replace("#column_list", sColumnList.ToString())
				.Replace("#table", sTablePrefix + p_sTable)
				.Replace("#primary_key", p_sPrimaryKey)
				.Replace("#id", p_nID.ToString()).ToString();

			p_oDbCmd.CommandType = CommandType.Text;
			p_oDbCmd.CommandText = sQuery;
		}

		public abstract void PrepareListSelectCommand<T>(IDbCommand p_oDbCmd, Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn);

		protected void PrepareListSelectCommand(IDbCommand p_oDbCmd, string p_sTable, string p_sPrimaryKey, bool p_bAllowTablePrefix, Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = p_bAllowTablePrefix ? oDACtx.GetTablePrefix() : string.Empty;
			
			StringBuilder sColumnList = new StringBuilder();

			foreach (string sColumn in p_arrColumn)
			{
				sColumnList.Append(sColumn).Append(", ");
			}
			sColumnList.Remove(sColumnList.Length - 2, 2);

			string sQuery = null;
			StringBuilder sTemplate = new StringBuilder();

			if ((p_nFirstRow == 0) && (p_nRowCount == 0))
			{
				sTemplate.Append("SELECT ")
					.Append(sColumnList)
					.Append(" FROM ")
					.Append(sTablePrefix + p_sTable);

				if (p_expFilter != null)
				{
					sTemplate.Append(" WHERE ")
						.Append(p_expFilter.ToExpressionString(this));
				}

				if (p_oOrder != null)
				{
					sTemplate.Append(" ORDER BY ")
						.Append(p_oOrder.ToExpressionString(this));
				}
			}
			else
			{
				//require both filter and order
				string sFilter = (p_expFilter == null) ? "1=1" : p_expFilter.ToExpressionString(this);
				string sOrder = (p_oOrder == null) ? p_sPrimaryKey + " ASC" : p_oOrder.ToExpressionString(this);

				sTemplate.Append(oDACtx.GetSelectTemplate())
					.Replace("#column_list", sColumnList.ToString())
					.Replace("#table", sTablePrefix + p_sTable)
					.Replace("#filter", sFilter)
					.Replace("#order", sOrder)
					.Replace("#first_row", p_nFirstRow.ToString())
					.Replace("#row_count", p_nRowCount.ToString());
			}

			sQuery = sTemplate.ToString();

			p_oDbCmd.CommandType = CommandType.Text;
			p_oDbCmd.CommandText = sQuery;
		}

		public abstract string[] PrepareInsertCommand<T>(IDbCommand p_oDbCmd, T p_dtoData);

		/// <summary>
		/// Prepare IDbCommand object for Insert query. Protected using
		/// </summary>
		/// <param name="p_oDbCmd">The IDbCommand object to prepare</param>
		/// <param name="p_sTable">Table name</param>
		/// <param name="p_sPrimaryKey">Primary key</param>
		/// <param name="p_bAllowTablePrefix">Use table prefix or not</param>
		/// <param name="p_arrColumn">Column list</param>
		/// <param name="p_arrValue">Value list</param>
		/// <returns>Return list of updated columns, including: ID column, auto generated column</returns>
		protected string[] PrepareInsertCommand(IDbCommand p_oDbCmd, string p_sTable, string p_sPrimaryKey, bool p_bAllowTablePrefix, string[] p_arrColumn, object[] p_arrValue)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = p_bAllowTablePrefix ? oDACtx.GetTablePrefix() : string.Empty;
			string sInlineParameterForm = oDACtx.GetInlineParameterForm();
			string sParameterForm = oDACtx.GetParameterForm();
			string sLatestIdentityStatement = oDACtx.GetLatestIdentityStatement();

			//check if batch query is not allowed
			//select all auto columns, not only primary key
			StringBuilder sTemplate = new StringBuilder("INSERT INTO #table (#column_list) VALUES (#value_list); SELECT #latest_identity AS #primary_key");

			StringBuilder sColumnList = new StringBuilder();
			StringBuilder sValueList = new StringBuilder();
			foreach (string sColumn in p_arrColumn)
			{
				sColumnList.Append(sColumn).Append(", ");
				sValueList.Append(sInlineParameterForm.Replace("name", sColumn)).Append(", ");
			}
			sColumnList.Remove(sColumnList.Length - 2, 2);
			sValueList.Remove(sValueList.Length - 2, 2);

			string sQuery = sTemplate
				.Replace("#column_list", sColumnList.ToString())
				.Replace("#value_list", sValueList.ToString())
				.Replace("#table", sTablePrefix + p_sTable)
				.Replace("#latest_identity", sLatestIdentityStatement)
				.Replace("#primary_key", p_sPrimaryKey)
				.ToString();

			p_oDbCmd.CommandType = CommandType.Text;
			p_oDbCmd.CommandText = sQuery;

			for (int i = 0; i < p_arrColumn.Length; i++)
			{
				IDbDataParameter oDbParam = p_oDbCmd.CreateParameter();
				oDbParam.ParameterName = sParameterForm.Replace("name", p_arrColumn[i]);
				oDbParam.Value = p_arrValue[i];
				p_oDbCmd.Parameters.Add(oDbParam);
			}

			return new string[] { p_sPrimaryKey };
		}

		public abstract string[] PrepareRecordUpdateCommand<T>(IDbCommand p_oDbCmd, T p_dtoData);

		protected string[] PrepareRecordUpdateCommand(IDbCommand p_oDbCmd, string p_sTable, string p_sPrimaryKey, int p_nID, bool p_bAllowTablePrefix, string[] p_arrColumn, object[] p_arrValue)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = p_bAllowTablePrefix ? oDACtx.GetTablePrefix() : string.Empty;
			string sInlineParameterForm = oDACtx.GetInlineParameterForm();
			string sParameterForm = oDACtx.GetParameterForm();

			//check if batch query is not allowed
			//select all auto columns, not only primary key
			StringBuilder sTemplate = new StringBuilder("UPDATE #table SET #update_list WHERE #primary_key = #id");

			StringBuilder sUpdateList = new StringBuilder();
			foreach (string sColumn in p_arrColumn)
			{
				sUpdateList.Append(sColumn).Append(" = ").Append(sInlineParameterForm.Replace("name", sColumn)).Append(", ");
			}
			sUpdateList.Remove(sUpdateList.Length - 2, 2);

			string sQuery = sTemplate
				.Replace("#update_list", sUpdateList.ToString())
				.Replace("#table", sTablePrefix + p_sTable)
				.Replace("#primary_key", p_sPrimaryKey)
				.Replace("#id", p_nID.ToString())
				.ToString();

			p_oDbCmd.CommandType = CommandType.Text;
			p_oDbCmd.CommandText = sQuery;

			for (int i = 0; i < p_arrColumn.Length; i++)
			{
				IDbDataParameter oDbParam = p_oDbCmd.CreateParameter();
				oDbParam.ParameterName = sParameterForm.Replace("name", p_arrColumn[i]);
				oDbParam.Value = p_arrValue[i];
				p_oDbCmd.Parameters.Add(oDbParam);
			}

			return new string[] { p_sPrimaryKey };
		}

		public string FormatStringConstant(string p_sInput)
		{
			//check for injection vulnerabilities here
			return DataAccessContext.GetDataAccessContext().GetUnicodeForm().Replace("value", p_sInput.Replace("'", "''"));
		}

		public string FormatDateTimeConstant(DateTime p_dtInput)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sDateTimeFormat = oDACtx.GetDateTimeFormat();
			if (sDateTimeFormat.NullOrEmpty())
			{
				sDateTimeFormat = "yyyy-MM-dd hh:mm:ss";
			}
			return DataAccessContext.GetDataAccessContext().GetUnicodeForm().Replace("value",p_dtInput.ToString(sDateTimeFormat));
		}

		public string FormatSingleConstant(float p_fInput)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sNumberFormat = oDACtx.GetNumberFormat();
			if (sNumberFormat.NullOrEmpty())
			{
				return p_fInput.ToString();
			}
			else
			{
				return p_fInput.ToString(new CultureInfo(sNumberFormat).NumberFormat);
			}
		}

		public string FormatDoubleConstant(double p_dblInput)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sNumberFormat = oDACtx.GetNumberFormat();
			if (sNumberFormat.NullOrEmpty())
			{
				return p_dblInput.ToString();
			}
			else
			{
				return p_dblInput.ToString(new CultureInfo(sNumberFormat).NumberFormat);
			}
		}

		public string FormatDecimalConstant(decimal p_decInput)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sNumberFormat = oDACtx.GetNumberFormat();
			if (sNumberFormat.NullOrEmpty())
			{
				return p_decInput.ToString();
			}
			else
			{
				return p_decInput.ToString(new CultureInfo(sNumberFormat).NumberFormat);
			}
		}

		public string FormatBooleanConstant(bool p_bInput)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sBooleanValues = oDACtx.GetBooleanValues();
			if (sBooleanValues.NullOrEmpty())
			{
				return p_bInput ? "1" : "0";
			}

			string[] arrBooleanValues = sBooleanValues.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			if (arrBooleanValues.Length != 2)
			{
				return p_bInput ? "1" : "0";
			}

			return p_bInput ? arrBooleanValues[0] : arrBooleanValues[1];
		}
	}
}

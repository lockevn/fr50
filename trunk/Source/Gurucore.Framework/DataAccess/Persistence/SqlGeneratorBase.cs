using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Gurucore.Framework.Core.Util;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public abstract class SqlGeneratorBase : IExpressionMaker
	{
		public const string CACHE_KEY = "Gurucore.Framework.DataAccess.Persistence.SqlGenerator";

		public abstract string GetRecordSelect<T>(int p_nID, string[] p_arrColumn);

		protected string GenerateRecordSelect(string p_sTable, string p_sPrimaryKey, bool p_bAllowTablePrefix, int p_nID, string[] p_arrColumn)
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

			return sQuery;
		}

		public abstract string GetListSelect<T>(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn);

		protected string GenerateListSelect(string p_sTable, string p_sPrimaryKey, bool p_bAllowTablePrefix, Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
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
			return sQuery;
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

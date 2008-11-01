using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class ReflectionSqlGenerator : SqlGeneratorBase, IExpressionMaker
	{
		public override string GetRecordSelect<T>(int p_nID, params string[] p_arrColumn) 
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo =  oTableInfoMgr.GetTableInfo(oType);

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = oDACtx.GetTablePrefix();

			StringBuilder sTemplate = new StringBuilder("SELECT #column_list FROM #table WHERE #primary_key = #id ");

			StringBuilder sColumnList = new StringBuilder();
			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				foreach (string sColumn in oTableInfo.Column)
				{
					sColumnList.Append(sColumn).Append(", ");
				}
			}
			else
			{
				foreach (string sField in p_arrColumn)
				{
					sColumnList.Append(sField).Append(", ");
				}
			}
			sColumnList.Remove(sColumnList.Length - 2, 2);

			string sQuery = sTemplate
				.Replace("#column_list", sColumnList.ToString())
				.Replace("#table", sTablePrefix + oTableInfo.TableName)
				.Replace("#primary_key", oTableInfo.PrimaryKey)
				.Replace("#id", p_nID.ToString()).ToString();

			return sQuery;
		}

		public override string GetListSelect<T>(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oType);

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			string sTablePrefix = oDACtx.GetTablePrefix();
			StringBuilder sTemplate = new StringBuilder(oDACtx.GetSelectTemplate());

			StringBuilder sColumnList = new StringBuilder();
			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				foreach (string sField in oTableInfo.Column)
				{
					sColumnList.Append(sField).Append(", ");
				}
			}
			else
			{
				foreach (string sField in p_arrColumn)
				{
					sColumnList.Append(sField).Append(", ");
				}
			}
			sColumnList.Remove(sColumnList.Length - 2, 2);

			string sQuery = sTemplate
				.Replace("#column_list", sColumnList.ToString())
				.Replace("#table", sTablePrefix + oTableInfo.TableName)
				.Replace("#filter", p_expFilter.ToExpressionString(this))
				.Replace("#order", p_oOrder.ToExpressionString(this))
				.Replace("#first_row", p_nFirstRow.ToString())
				.Replace("#row_count", p_nRowCount.ToString()).ToString();

			return sQuery;
		}

		public string FormatStringConstant(string p_sInput)
		{
			//check for injection vulnerabilities here
			return DataAccessContext.GetDataAccessContext().GetUnicodeForm().Replace("value", p_sInput.Replace("'", "''"));
		}

		public string FormatDateTimeConstant(DateTime p_dtInput)
		{
			return p_dtInput.F .ToString();
		}

		public string FormatSingleConstant(float p_fInput)
		{
			return p_fInput.ToString();
		}

		public string FormatDoubleConstant(double p_dblInput)
		{
			return p_dblInput.ToString();
		}

		public string FormatDecimalConstant(decimal p_decInput)
		{
			return p_decInput.ToString();
		}
	}
}

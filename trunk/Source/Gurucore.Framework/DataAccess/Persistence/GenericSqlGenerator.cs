using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class GenericSqlGenerator : SqlGeneratorBase
	{
		private string m_sGenericTable;
		private string m_sGenericPrimaryKey;

		public GenericSqlGenerator(string p_sGenericTable, string p_sGenericPrimaryKey)
		{
			m_sGenericTable = p_sGenericTable;
			m_sGenericPrimaryKey = p_sGenericPrimaryKey;
		}

		public override string GetRecordSelect<T>(int p_nID, string[] p_arrColumn)
		{
			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				throw new FrameworkException("Query a generic table need column list", null);
			}
			else
			{
				DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
				string sTablePrefix = oDACtx.GetTablePrefix();

				StringBuilder sTemplate = new StringBuilder("SELECT #column_list FROM #table WHERE #primary_key = #id ");

				StringBuilder sColumnList = new StringBuilder();
				foreach (string sColumn in p_arrColumn)
				{
					sColumnList.Append(sColumn).Append(", ");
				}
				sColumnList.Remove(sColumnList.Length - 2, 2);

				string sQuery = sTemplate
					.Replace("#column_list", sColumnList.ToString())
					.Replace("#table", sTablePrefix + m_sGenericTable)
					.Replace("#primary_key", m_sGenericPrimaryKey)
					.Replace("#id", p_nID.ToString()).ToString();

				return sQuery;

			}
		}

		public override string GetListSelect<T>(Gurucore.Framework.DataAccess.Persistence.QueryLanguage.Expression p_expFilter, Gurucore.Framework.DataAccess.Persistence.QueryLanguage.Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			throw new NotImplementedException();
		}
	}
}

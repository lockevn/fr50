using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class GenericSqlGenerator : SqlGeneratorBase
	{
		private string m_sTable;
		private string m_sPrimaryKey;

		public GenericSqlGenerator(string p_sGenericTable, string p_sGenericPrimaryKey)
		{
			this.m_sTable = p_sGenericTable;
			this.m_sPrimaryKey = p_sGenericPrimaryKey;
		}

		public override string GetRecordSelect<T>(int p_nID, string[] p_arrColumn)
		{
			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				return this.GenerateRecordSelect(m_sTable, m_sPrimaryKey, true, p_nID, new string[] {"*"});
			}
			else
			{
				return this.GenerateRecordSelect(m_sTable, m_sPrimaryKey, true, p_nID, p_arrColumn);
			}
		}

		public override string GetListSelect<T>(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				return this.GenerateListSelect(m_sTable, m_sPrimaryKey, true, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, new string[] {"*"});
			}
			else
			{
				return this.GenerateListSelect(m_sTable, m_sPrimaryKey, true, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, p_arrColumn);
			}
		}
	}
}

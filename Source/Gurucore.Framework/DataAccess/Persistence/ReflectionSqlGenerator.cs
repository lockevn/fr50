using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class ReflectionSqlGenerator : SqlGeneratorBase
	{
		public override string GetRecordSelect<T>(int p_nID, params string[] p_arrColumn) 
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo =  oTableInfoMgr.GetTableInfo(oType);

			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				return GenerateRecordSelect(oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_nID, oTableInfo.Column.ToArray());
			}
			else
			{
				return GenerateRecordSelect(oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_nID, p_arrColumn);
			}
		}

		public override string GetListSelect<T>(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oType);

			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				return GenerateListSelect(oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, oTableInfo.Column.ToArray());
			}
			else
			{
				return GenerateListSelect(oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, p_arrColumn);
			}
		}
	}
}

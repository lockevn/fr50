using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class ReflectionSqlGenerator : SqlGeneratorBase
	{
		public override void PrepareRecordSelectCommand<T>(IDbCommand p_oDbCmd, int p_nID, params string[] p_arrColumn) 
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo =  oTableInfoMgr.GetTableInfo(oType);

			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				PrepareRecordSelectCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_nID, oTableInfo.Column.ToArray());
			}
			else
			{
				PrepareRecordSelectCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_nID, p_arrColumn);
			}
		}

		public override void PrepareListSelectCommand<T>(IDbCommand p_oDbCmd, Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn)
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oType);

			if ((p_arrColumn == null) || (p_arrColumn.Length == 0))
			{
				PrepareListSelectCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, oTableInfo.Column.ToArray());
			}
			else
			{
				PrepareListSelectCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, p_arrColumn);
			}
		}

		public override string[] PrepareInsertCommand<T>(IDbCommand p_oDbCmd, T p_dtoData)
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oType);

			List<string> arrNotNullColumn = new List<string>();
			List<object> arrValue = new List<object>();

			if (p_dtoData is DTOBase)
			{
				DTOBase dto = (DTOBase)Convert.ChangeType(p_dtoData, typeof(T));
				foreach (string sColumn in oTableInfo.Column)
				{
					if (!dto.IsNull(sColumn))
					{
						arrNotNullColumn.Add(sColumn);
						arrValue.Add(oTableInfo.Property[sColumn].GetValue(p_dtoData, null));
					}
				}
			}
			else
			{
				foreach (string sColumn in oTableInfo.Column)
				{
					object oValue = oTableInfo.Property[sColumn].GetValue(p_dtoData, null);
					if (oValue != null)
					{
						arrNotNullColumn.Add(sColumn);
						arrValue.Add(oValue);
					}
				}
			}

			return this.PrepareInsertCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, oTableInfo.AllowTablePrefix, arrNotNullColumn.ToArray(), arrValue.ToArray());
		}

		public override string[] PrepareRecordUpdateCommand<T>(IDbCommand p_oDbCmd, T p_dtoData)
		{
			Type oType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oType);

			List<string> arrNotNullColumn = new List<string>();
			List<object> arrValue = new List<object>();

			if (p_dtoData is DTOBase)
			{
				DTOBase dto = (DTOBase)Convert.ChangeType(p_dtoData, typeof(T));
				foreach (string sColumn in oTableInfo.Column)
				{
					if (!dto.IsNull(sColumn))
					{
						arrNotNullColumn.Add(sColumn);
						arrValue.Add(oTableInfo.Property[sColumn].GetValue(p_dtoData, null));
					}
				}
			}
			else
			{
				foreach (string sColumn in oTableInfo.Column)
				{
					object oValue = oTableInfo.Property[sColumn].GetValue(p_dtoData, null);
					if (oValue != null)
					{
						arrNotNullColumn.Add(sColumn);
						arrValue.Add(oValue);
					}
				}
			}

			return this.PrepareRecordUpdateCommand(p_oDbCmd, oTableInfo.TableName, oTableInfo.PrimaryKey, 1, oTableInfo.AllowTablePrefix, arrNotNullColumn.ToArray(), arrValue.ToArray());
		}
	}
}

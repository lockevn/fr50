using System;
using System.Collections.Generic;
using System.Data;

using Gurucore.Framework.DataAccess.Persistence;

using Gurucore.FrameworkTest.TableMapper;

namespace Gurucore.FrameworkTest.TableMapper
{
	public sealed class Table_DTOMaker : DTOMakerBase
	{
		public Table_DTOMaker()
		{
		}

		public override T[] GetDTO<T>(IDataReader p_oReader, string[] p_arrColumn)
		{
			List<T> arrDTO = new List<T>();
			List<string> arrColumn = null;
			if (p_arrColumn != null)
			{
				arrColumn = new List<string>(p_arrColumn);
			}

			while (p_oReader.Read())
			{
				Table oDTO = new Table();

				if (((arrColumn == null) || (arrColumn.Count == 0) || (arrColumn.Contains("TABLE_CATALOG"))) && (p_oReader["TABLE_CATALOG"] != DBNull.Value))
				{
					oDTO.TableCatalog = (String)Convert.ChangeType(p_oReader["TABLE_CATALOG"], typeof(String));
				}
				if (((arrColumn == null) || (arrColumn.Count == 0) || (arrColumn.Contains("TABLE_SCHEMA"))) && (p_oReader["TABLE_SCHEMA"] != DBNull.Value))
				{
					oDTO.TableSchema = (String)Convert.ChangeType(p_oReader["TABLE_SCHEMA"], typeof(String));
				}
				if (((arrColumn == null) || (arrColumn.Count == 0) || (arrColumn.Contains("TABLE_NAME"))) && (p_oReader["TABLE_NAME"] != DBNull.Value))
				{
					oDTO.TableName = (String)Convert.ChangeType(p_oReader["TABLE_NAME"], typeof(String));
				}
				if (((arrColumn == null) || (arrColumn.Count == 0) || (arrColumn.Contains("TABLE_TYPE"))) && (p_oReader["TABLE_TYPE"] != DBNull.Value))
				{
					oDTO.TableType = (String)Convert.ChangeType(p_oReader["TABLE_TYPE"], typeof(String));
				}

				arrDTO.Add((T)Convert.ChangeType(oDTO, typeof(T)));
			}
			return arrDTO.ToArray();
		}
	}
}

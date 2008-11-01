using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class GenericDTOMaker : DTOMakerBase
	{
		private string m_sGenericTable;
		private string m_sGenericPrimaryKey;
		private string[] m_arrColumn;

		public GenericDTOMaker(string p_sGenericTable, string p_sGenericPrimaryKey, string[] p_arrColumn)
		{
			m_sGenericTable = p_sGenericTable;
			m_sGenericPrimaryKey = p_sGenericPrimaryKey;
			m_arrColumn = p_arrColumn;
		}

		public override T[] GetDTO<T>(System.Data.IDataReader p_oReader)
		{
			if ((m_arrColumn == null) || (m_arrColumn.Length == 0))
			{
				throw new FrameworkException("Query a generic table need column list", null);
			}
			else
			{
				List<T> arrDTO = new List<T>();

				while (p_oReader.Read())
				{
					GenericDTO dtoGeneric = new GenericDTO();

					foreach (string sColumn in m_arrColumn)
					{
						if (p_oReader[sColumn] != DBNull.Value)
						{
							dtoGeneric[sColumn] = p_oReader[sColumn];
						}
						else
						{
							dtoGeneric[sColumn] = null;
							dtoGeneric.SetSchema(sColumn, (Type)p_oReader.GetFieldType(p_oReader.GetOrdinal(sColumn)));
							dtoGeneric.SetNull(sColumn);
						}
					}
					arrDTO.Add((T)Convert.ChangeType(dtoGeneric, typeof(T)));
				}

				return arrDTO.ToArray();
			}
		}
	}
}

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

		public GenericDTOMaker(string p_sGenericTable, string p_sGenericPrimaryKey)
		{
			m_sGenericTable = p_sGenericTable;
			m_sGenericPrimaryKey = p_sGenericPrimaryKey;
		}

		public override T[] GetDTO<T>(System.Data.IDataReader p_oReader, string[] m_arrColumn)
		{
			List<T> arrDTO = new List<T>();

			List<string> arrColumn = null;

			if ((m_arrColumn == null) || (m_arrColumn.Length == 0))
			{
				arrColumn = new List<string>();
				int nColumnCount = p_oReader.FieldCount;
				for (int i = 0; i < nColumnCount; i++)
				{
					arrColumn.Add(p_oReader.GetName(i));
				}
			}
			else
			{
				arrColumn = new List<string>(m_arrColumn);
			}

			while (p_oReader.Read())
			{
				GenericDTO dtoGeneric = new GenericDTO();
				foreach (string sColumn in arrColumn)
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

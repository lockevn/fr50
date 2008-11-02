using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class ReflectionDTOMaker : DTOMakerBase
	{
		public override T[] GetDTO<T>(System.Data.IDataReader p_oReader, string[] p_arrColumn)
		{
			List<T> arrDTO = new List<T>();
			Type oDTOType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oDTOType);

			int nReadColumn = p_oReader.FieldCount;
			while (p_oReader.Read())
			{
				T oDTO = (T)Activator.CreateInstance(oDTOType);

				for (int i = 0; i < nReadColumn; i++)
				{
					string sColumn = p_oReader.GetName(i);
					PropertyInfo oProperty = oTableInfo.Property[sColumn];
					if (p_oReader[sColumn] != DBNull.Value)
					{
						if (oProperty.PropertyType == p_oReader.GetFieldType(i))
						{
							oProperty.SetValue(oDTO, p_oReader[i], null);
						}
						else
						{
							if (oProperty.PropertyType.IsPrimitive)
							{
								oProperty.SetValue(oDTO, Convert.ChangeType(p_oReader[i], oProperty.PropertyType), null);
							}
							else if (oProperty.PropertyType.IsGenericType && oProperty.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
							{
								oProperty.SetValue(oDTO, Convert.ChangeType(p_oReader[i], oProperty.PropertyType.GetGenericArguments()[0]), null);
							}
						}
					}
				}
				arrDTO.Add(oDTO);
			}

			return arrDTO.ToArray();
		}
	}
}

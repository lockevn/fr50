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
		public override T[] GetDTO<T>(System.Data.IDataReader p_oReader)
		{
			List<T> arrDTO = new List<T>();
			Type oDTOType = typeof(T);

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oDTOType);
			PropertyInfo[] arrProperty = oTableInfo.Property.Values.ToArray<PropertyInfo>();

			while (p_oReader.Read())
			{
				T oDTO = (T)Activator.CreateInstance(oDTOType);

				foreach (PropertyInfo oProperty in arrProperty)
				{
					if (p_oReader[oProperty.Name] != DBNull.Value)
					{
						oProperty.SetValue(oDTO, p_oReader[oProperty.Name], null);
					}
				}
				arrDTO.Add(oDTO);
			}

			return arrDTO.ToArray();
		}
	}
}

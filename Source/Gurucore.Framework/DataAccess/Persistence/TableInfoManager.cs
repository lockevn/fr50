using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.Util;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class TableInfoManager
	{
		private Dictionary<Type, TableInfo> m_dicTableInfo;

		public TableInfoManager()
		{
			m_dicTableInfo = new Dictionary<Type, TableInfo>();
		}

		public TableInfo GetTableInfo(Type p_oType)
		{
			if (m_dicTableInfo.ContainsKey(p_oType))
			{
				return m_dicTableInfo[p_oType];
			}
			else
			{
				lock (this)
				{
					if (m_dicTableInfo.ContainsKey(p_oType))
					{
						return m_dicTableInfo[p_oType];
					}
					else
					{
						object[] arrTableAttr = p_oType.GetCustomAttributes(typeof(MappedTableAttribute), true);

						string sTableName = ((MappedTableAttribute)arrTableAttr[0]).TableName;
						if (sTableName.NullOrEmpty())
						{
							if (p_oType.Name.EndsWith("DTO"))
							{
								sTableName = p_oType.Name.Substring(0, p_oType.Name.Length - 3);
							}
							else
							{
								sTableName = p_oType.Name;
							}
						}

						string sPrimaryKey =((MappedTableAttribute)arrTableAttr[0]).PrimaryKey;
						if (sPrimaryKey.NullOrEmpty())
						{
							sPrimaryKey = sTableName + "ID";
						}

						TableInfo oTableInfo = new TableInfo(sTableName, sPrimaryKey,
							((MappedTableAttribute)arrTableAttr[0]).Updatable,
							((MappedTableAttribute)arrTableAttr[0]).AllowTablePrefix);

						PropertyInfo[] arrProperty = p_oType.GetProperties();
						foreach (PropertyInfo oProperty in arrProperty)
						{
							object[] arrColumnAttr = oProperty.GetCustomAttributes(typeof(MappedColumnAttribute), true);
							if (arrColumnAttr.Length > 0)
							{
								MappedColumnAttribute oColumnAttr = (MappedColumnAttribute)arrColumnAttr[0];
								string sColumnName = oColumnAttr.ColumnName;
								if (sColumnName.NullOrEmpty())
								{
									sColumnName = oProperty.Name;
								}
								bool bIdentity = oColumnAttr.Identity;
								bool bReadOnly = false; //reserved
								oTableInfo.AddColumn(sColumnName, bIdentity, bReadOnly, oProperty);
							}
						}

						m_dicTableInfo.Add(p_oType, oTableInfo);
						return oTableInfo;
					}
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class TableInfo
	{
		private string m_sTableName;

		public string TableName
		{
			get { return m_sTableName; }
			set { m_sTableName = value; }
		}

		private string m_sPrimaryKey;

		public string PrimaryKey
		{
			get { return m_sPrimaryKey; }
			set { m_sPrimaryKey = value; }
		}

		private bool m_bUpdatable;

		public bool Updatable
		{
			get { return m_bUpdatable; }
			set { m_bUpdatable = value; }
		}

		private bool m_bAllowTablePrefix;

		public bool AllowTablePrefix
		{
			get { return m_bAllowTablePrefix; }
			set { m_bAllowTablePrefix = value; }
		}

		private List<string> m_arrColumn;

		public List<string> Column
		{
			get { return m_arrColumn; }
			set { m_arrColumn = value; }
		}

		private Dictionary<string, bool> m_dicIdentity;

		public Dictionary<string, bool> Identity
		{
			get { return m_dicIdentity; }
			set { m_dicIdentity = value; }
		}

		private Dictionary<string, bool> m_dicReadOnly;

		public Dictionary<string, bool> ReadOnly
		{
			get { return m_dicReadOnly; }
			set { m_dicReadOnly = value; }
		}

		private Dictionary<string, PropertyInfo> m_dicProperty;

		public Dictionary<string, PropertyInfo> Property
		{
			get { return m_dicProperty; }
			set { m_dicProperty = value; }
		}

		public TableInfo(string p_sTableName, string p_sPrimaryKey, bool p_bUpdatable, bool p_bAllowTablePrefix)
		{
			m_sTableName = p_sTableName;
			m_sPrimaryKey = p_sPrimaryKey;
			m_bUpdatable = p_bUpdatable;
			m_bAllowTablePrefix = p_bAllowTablePrefix;

			m_arrColumn = new List<string>();
			m_dicIdentity = new Dictionary<string, bool>();
			m_dicReadOnly = new Dictionary<string, bool>();
			m_dicProperty = new Dictionary<string, PropertyInfo>();
		}

		public void AddColumn(string p_sColumnName, bool p_bIdentity, bool p_bReadOnly, PropertyInfo p_oProperty)
		{
			m_arrColumn.Add(p_sColumnName);
			m_dicIdentity.Add(p_sColumnName, p_bIdentity);
			m_dicReadOnly.Add(p_sColumnName, p_bReadOnly);
			m_dicProperty.Add(p_sColumnName, p_oProperty);
		}
	}
}

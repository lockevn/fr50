using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MappedTableAttribute : ClonableAttribute
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

        public MappedTableAttribute(string p_sTableName, string p_sPrimaryKey, bool p_bUpdatable, bool p_bAllowTablePrefix)
        {
            m_sTableName = p_sTableName;
            m_sPrimaryKey = p_sPrimaryKey;
            m_bUpdatable = p_bUpdatable;
			m_bAllowTablePrefix = p_bAllowTablePrefix;
        }

		public MappedTableAttribute()
            : this(string.Empty, string.Empty, true, true)
        {
        }
	}
}

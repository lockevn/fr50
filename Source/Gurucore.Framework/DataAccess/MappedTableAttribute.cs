using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MappedTableAttribute : Attribute
	{
		private string m_sTableName;
		private string m_sPrimaryKey;
		private bool m_bUpdatable;		

		public string TableName
		{
			get
			{
				return m_sTableName;
			}
			set
			{
				m_sTableName = value;
			}
		}

		public string PrimaryKey
		{
			get
			{
				return m_sPrimaryKey;
			}
			set
			{
				m_sPrimaryKey = value;
			}
		}

		public bool Updatable
		{
			get
			{
				return m_bUpdatable;
			}
			set
			{
				m_bUpdatable = value;
			}
        }

        public MappedTableAttribute(string p_sTableName, string p_sPrimaryKey, bool p_bUpdatable)
        {
            m_sTableName = p_sTableName;
            m_sPrimaryKey = p_sPrimaryKey;
            m_bUpdatable = p_bUpdatable;
        }

		public MappedTableAttribute()
            : this(string.Empty, string.Empty, true)
        {
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MappedColumnAttribute : Attribute
	{
		private string m_sColumnName;

		public string ColumnName
		{
			get { return m_sColumnName;	}
			set { m_sColumnName = value; }
		}

		private bool m_bIdentity;

		public bool Identity
		{
			get { return m_bIdentity; }
			set { m_bIdentity = value; }
		}

		public MappedColumnAttribute()
		{
		}

        public MappedColumnAttribute(string p_sColumnName, bool p_bIdentity)
        {
            m_sColumnName = p_sColumnName;
			m_bIdentity = p_bIdentity;
        }
	}
}

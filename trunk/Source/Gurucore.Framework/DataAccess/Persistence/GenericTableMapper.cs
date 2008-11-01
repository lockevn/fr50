using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence
{
	//Non-generic implementation, use with GenericDTO
	public class GenericTableMapper : TableMapper<GenericDTO>
	{
		public GenericTableMapper(string p_sGenericTable)
			: this(p_sGenericTable, p_sGenericTable + "ID")
		{
		}

		public GenericTableMapper(string p_sGenericTable, string p_sGenericPrimaryKey)
		{
			this.m_sGenericTable = p_sGenericTable;
			this.m_sGenericPrimaryKey = p_sGenericPrimaryKey;
		}
	}

}

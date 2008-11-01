using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public abstract class SqlGeneratorBase
	{
		public const string CACHE_KEY = "Gurucore.Framework.DataAccess.Persistence.SqlGenerator";

		public abstract string GetRecordSelect<T>(int p_nID, string[] p_arrColumn);

		public abstract string GetListSelect<T>(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, string[] p_arrColumn);
	}
}

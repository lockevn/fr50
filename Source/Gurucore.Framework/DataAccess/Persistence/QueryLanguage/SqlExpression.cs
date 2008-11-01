using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class SqlExpression : Expression
	{
		private string m_sSql;

		public SqlExpression(string p_sSql)
		{
			m_sSql = p_sSql;
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return m_sSql;
		}
	}
}

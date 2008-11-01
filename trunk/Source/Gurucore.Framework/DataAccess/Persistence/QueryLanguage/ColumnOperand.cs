using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class ColumnOperand : IOperand
	{
		private string m_sColumnName;

		public ColumnOperand(string p_sColumnName)
		{
			m_sColumnName = p_sColumnName;
		}

		public string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return m_sColumnName;
		}
	}
}

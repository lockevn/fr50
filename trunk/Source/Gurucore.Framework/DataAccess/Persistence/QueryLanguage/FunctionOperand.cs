using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	[Obsolete("Don't use if you want to keep your application is DBMS independence. Please you Sql standard functions if possible")]
	public class FunctionOperand : OperandBase
	{
		private string m_sFunctionCall;
		public FunctionOperand(string p_sFunctionCall)
		{
			m_sFunctionCall = p_sFunctionCall;
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return m_sFunctionCall;
		}
	}
}

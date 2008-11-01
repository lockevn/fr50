using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	[Obsolete("Don't use if you want to keep your application is DBMS independence. Please you Sql standard functions if possible")]
	public class FunctionOperand : IOperand
	{
		private string m_sFunctionCall;
		public FunctionOperand(string p_sFunctionCall)
		{
			m_sFunctionCall = p_sFunctionCall;
		}

		public string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return m_sFunctionCall;
		}
	}
}

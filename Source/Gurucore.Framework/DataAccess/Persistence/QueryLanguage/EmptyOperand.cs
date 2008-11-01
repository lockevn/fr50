using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{ 
	public class EmptyOperand : IOperand
	{
		public EmptyOperand()
		{
		}

		public string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return string.Empty;
		}
	}
}

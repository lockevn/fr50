using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{ 
	public class EmptyOperand : OperandBase
	{
		public EmptyOperand()
		{
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			return string.Empty;
		}
	}
}

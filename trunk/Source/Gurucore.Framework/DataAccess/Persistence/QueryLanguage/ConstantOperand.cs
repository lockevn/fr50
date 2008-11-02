using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class ConstantOperand : OperandBase
	{
		private object m_oValue;

		public ConstantOperand(object p_oValue)
		{
			m_oValue = p_oValue;
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			string sValue = string.Empty;
			if (m_oValue is DateTime)
			{
				DateTime dtValue = (DateTime)m_oValue;
				sValue = p_oExpressionMaker.FormatDateTimeConstant(dtValue);
			}
			else if (m_oValue is bool)
			{
				bool bValue = (bool)m_oValue;
				sValue = p_oExpressionMaker.FormatBooleanConstant(bValue);
			}
			else if (m_oValue is Single)
			{
				float fValue = (float)m_oValue;
				sValue = p_oExpressionMaker.FormatSingleConstant(fValue);
			}
			else if (m_oValue is Double)
			{
				double dblValue = (double)m_oValue;
				sValue = p_oExpressionMaker.FormatDoubleConstant(dblValue);
			}
			else if (m_oValue is Decimal)
			{
				decimal bValue = (decimal)m_oValue;
				sValue = p_oExpressionMaker.FormatDecimalConstant(bValue);
			}
			else
			{
				sValue = p_oExpressionMaker.FormatStringConstant(m_oValue.ToString());
			}

			return sValue;
		}
	}
}

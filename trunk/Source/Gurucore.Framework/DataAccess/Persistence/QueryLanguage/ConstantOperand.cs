using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class ConstantOperand : IOperand
	{
		private object m_oValue;
		private bool m_bSingleQuoted;

		public ConstantOperand(object p_oValue)
		{
			m_oValue = p_oValue;
			//m_bSingleQuoted = !((p_oValue is int) || (p_oValue is Int16) || (p_oValue is Int64) || (p_oValue is bool) || (p_oValue is double)
			//	|| (p_oValue is float) || (p_oValue is short) || (p_oValue is decimal));
			m_bSingleQuoted = (p_oValue is string) || (p_oValue is DateTime);
		}

		public string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			string sValue = string.Empty;
			if (m_oValue is DateTime)
			{
				DateTime dtValue = (DateTime)m_oValue;
				sValue = dtValue.ToString("yyyy-MM-dd hh:mm:ss");
			}
			else if (m_oValue is bool)
			{
				sValue = ((bool)m_oValue) ? "1" : "0";
			}
			else if (m_oValue is Decimal || m_oValue is Double || m_oValue is float || m_oValue is Single)
			{
				System.Globalization.CultureInfo ci = ci = new System.Globalization.CultureInfo("en-US");
				decimal decValue = Convert.ToDecimal(m_oValue);
				sValue = decValue.ToString("F", ci);
			}
			else
			{
				sValue = m_oValue.ToString();
			}

			if (m_bSingleQuoted)
			{
				return p_oExpressionMaker.FormatStringConstant(sValue);//"'" + m_oValue.ToString().Replace("'","''") + "'";
			}
			else
			{
				return sValue;
			}
		}
	}
}

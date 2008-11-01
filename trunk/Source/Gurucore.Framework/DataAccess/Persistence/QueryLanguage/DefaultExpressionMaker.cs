using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class DefaultExpressionMaker : IExpressionMaker
	{
		public string FormatStringConstant(string p_sInput)
		{
			return p_sInput;
		}

		public string FormatDateTimeConstant(DateTime p_dtInput)
		{
			return p_dtInput.ToString();
		}

		public string FormatSingleConstant(float p_fInput)
		{
			return p_fInput.ToString();
		}

		public string FormatDoubleConstant(double p_dblInput)
		{
			return p_dblInput.ToString();
		}

		public string FormatDecimalConstant(decimal p_decInput)
		{
			return p_decInput.ToString();
		}
	}
}

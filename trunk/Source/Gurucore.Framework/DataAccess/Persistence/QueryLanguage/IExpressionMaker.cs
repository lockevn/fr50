using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public interface IExpressionMaker
	{
		string FormatStringConstant(string p_sInput);
		string FormatDateTimeConstant(DateTime p_dtInput);
		string FormatSingleConstant(float p_fInput);
		string FormatDoubleConstant(double p_dblInput);
		string FormatDecimalConstant(decimal p_decInput);
		string FormatBooleanConstant(bool p_bInput);
	}
}

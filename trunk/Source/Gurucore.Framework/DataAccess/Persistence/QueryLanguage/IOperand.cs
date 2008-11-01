using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public interface IOperand
	{
		string ToExpressionString(IExpressionMaker p_oExpressionMaker);
	}
}

using System;

using System.Collections;
using System.Collections.Generic;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class SetOperand : OperandBase
	{
		object[] m_arrElements;

		public SetOperand(ArrayList p_arrElements)
		{
			m_arrElements = p_arrElements.ToArray();
		}

		public SetOperand(params object[] p_arrElements)
		{
			m_arrElements = p_arrElements;
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			string sSet = "(";
			foreach (object oElement in m_arrElements)
			{
				sSet += oElement.ToString() + ",";
			}
			sSet = sSet.Substring(0,sSet.Length - 1) + ")";
			return sSet;
		}
	}
}

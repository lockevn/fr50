using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class Expression : OperandBase
	{
		private OperandBase m_oFirstOperand, m_oSecondOperand;
		private Operator m_oOperator;

		protected Expression()
		{
		}

		public Expression(OperandBase p_oFirstOperand, Operator p_oOperator, OperandBase p_oSecondOperand)
		{
			if (p_oOperator.IsUnary())
			{
				throw new FrameworkException("Unary operator can have only 1 operand", null);
			}
			else
			{
				m_oFirstOperand = p_oFirstOperand;
				m_oOperator = p_oOperator;
				m_oSecondOperand = p_oSecondOperand;
			}
		}

		public Expression(Operator p_oOperator, OperandBase p_oSecondOperand)
		{
			if (!p_oOperator.IsUnary())
			{
				throw new FrameworkException("Binary operator must have 2 operands", null);
			}
			else
			{
				m_oOperator = p_oOperator;
				m_oSecondOperand = p_oSecondOperand;
			}
		}

		public Expression(OperandBase p_oFirstOperand, Operator p_oOperator)
		{
			if (p_oOperator.IsUnary())
			{
				throw new FrameworkException("Binary operator must have 2 operands", null);
			}
			else
			{
				m_oFirstOperand = p_oFirstOperand;
				m_oOperator = p_oOperator;
				m_oSecondOperand = new EmptyOperand();
			}
		}

		public override string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			if (m_oOperator.IsUnary())
			{
				return "(" + m_oOperator.ToString() + " " + m_oSecondOperand.ToExpressionString(p_oExpressionMaker) + ")";
			}
			else
			{
				return "(" + m_oFirstOperand.ToExpressionString(p_oExpressionMaker) + " " + m_oOperator.ToString() + " " + m_oSecondOperand.ToExpressionString(p_oExpressionMaker) + ")";
			}
		}

		public override string ToString()
		{
			return ToExpressionString(new DefaultExpressionMaker());
		}
	}
}
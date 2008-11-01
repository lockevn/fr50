using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class Expression : IOperand
	{
		private IOperand m_oFirstOperand, m_oSecondOperand;
		private Operator m_oOperator;

		protected Expression()
		{
		}

		public Expression(IOperand p_oFirstOperand, Operator p_oOperator, IOperand p_oSecondOperand)
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

		public Expression(Operator p_oOperator, IOperand p_oSecondOperand)
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

		public Expression(IOperand p_oFirstOperand, Operator p_oOperator)
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

		public Expression And(Expression p_expSecond)
		{
			return new Expression(this, Operator.And, p_expSecond);
		}

		public Expression Or(Expression p_expSecond)
		{
			return new Expression(this, Operator.Or, p_expSecond);
		}

		public Expression Xor(Expression p_expSecond)
		{
			return new Expression(this, Operator.Xor, p_expSecond);
		}

		public Expression Not()
		{
			return new Expression(Operator.Not, this);
		}

		public Expression Add(Expression p_expSecond)
		{
			return new Expression(this, Operator.Add, p_expSecond);
		}

		public Expression Sub(Expression p_expSecond)
		{
			return new Expression(this, Operator.Sub, p_expSecond);
		}

		public Expression Mul(Expression p_expSecond)
		{
			return new Expression(this, Operator.Mul, p_expSecond);
		}

		public Expression Div(Expression p_expSecond)
		{
			return new Expression(this, Operator.Div, p_expSecond);
		}

		public virtual string ToExpressionString(IExpressionMaker p_oExpressionMaker)
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
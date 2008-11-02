using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public abstract class OperandBase
	{
		public abstract string ToExpressionString(IExpressionMaker p_oExpressionMaker);
	
		public Expression And(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.And, p_oSecondOperand);
		}

		public Expression Or(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Or, p_oSecondOperand);
		}

		public Expression Xor(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Xor, p_oSecondOperand);
		}

		public Expression Not()
		{
			return new Expression(Operator.Not, this);
		}

		public Expression Add(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Add, p_oSecondOperand);
		}

		public Expression Sub(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Sub, p_oSecondOperand);
		}

		public Expression Mul(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Mul, p_oSecondOperand);
		}

		public Expression Div(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Div, p_oSecondOperand);
		}

		public Expression Like(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Like, p_oSecondOperand);
		}

		public Expression In(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.In, p_oSecondOperand);
		}

		public Expression NotIn(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.NotIn, p_oSecondOperand);
		}

		public Expression Between(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Between, p_oSecondOperand);
		}

		public Expression Gt(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Gt, p_oSecondOperand);
		}

		public Expression Lt(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Lt, p_oSecondOperand);
		}

		public Expression Eq(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Eq, p_oSecondOperand);
		}

		public Expression Neq(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Neq, p_oSecondOperand);
		}

		public Expression Gte(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Gte, p_oSecondOperand);
		}

		public Expression Lte(OperandBase p_oSecondOperand)
		{
			return new Expression(this, Operator.Lte, p_oSecondOperand);
		}

		public Expression IsNull()
		{
			return new Expression(this, Operator.IsNull, new EmptyOperand());
		}

		public Expression IsNotNull()
		{
			return new Expression(this, Operator.IsNotNull, new EmptyOperand());
		}
	}
}

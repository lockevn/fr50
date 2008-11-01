using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class Order
	{
		private Expression m_oExpression;
		private SortType m_nSort;

		public Order(Expression p_oExpression, SortType p_nSort)
		{
			m_oExpression = p_oExpression;
			m_nSort = p_nSort;
		}

		public Order ReversedOrder
		{
			get
			{
				if (m_nSort == SortType.Ascending)
				{
					return new Order(m_oExpression, SortType.Descending);
				}
				else
				{
					return new Order(m_oExpression, SortType.Ascending);
				}
			}
		}

		public string ToExpressionString(IExpressionMaker p_oExpressionMaker)
		{
			string sResult = string.Empty;

			switch (m_nSort)
			{
				case SortType.Ascending:
					sResult = m_oExpression.ToExpressionString(p_oExpressionMaker) + " ASC";
					break;
				case SortType.Descending:
					sResult = m_oExpression.ToExpressionString(p_oExpressionMaker) + " DESC";
					break;
			}

			return sResult;
		}

		public Expression Expression
		{
			get
			{
				return m_oExpression;
			}
		}

		public string Sort
		{
			get
			{
				if (m_nSort > 0)
				{
					return "ASC";
				}
				else
				{
					return "DESC";
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Gurucore.Framework.Test
{
	public abstract class TestSuiteBase
	{
		protected void AssertTrue(bool p_bCondition, string p_sMessage)
		{
			if (p_bCondition == false)
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertNull(object p_oObject, string p_sMessage)
		{
			if (p_oObject != null)
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertNotNull(object p_oObject, string p_sMessage)
		{
			if (p_oObject == null)
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertEqual(object p_oObject, object p_oAnother, string p_sMessage)
		{
			if (!p_oObject.Equals(p_oAnother))
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertNotEqual(object p_oObject, object p_oAnother, string p_sMessage)
		{
			if (p_oObject.Equals(p_oAnother))
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertTheSame(object p_oObject, object p_oAnother, string p_sMessage)
		{
			if (!(object.ReferenceEquals(p_oObject, p_oAnother)))
			{
				throw new AssertException(p_sMessage);
			}
		}

		protected void AssertDifferent(object p_oObject, object p_oAnother, string p_sMessage)
		{
			if (object.ReferenceEquals(p_oObject, p_oAnother))
			{
				throw new AssertException(p_sMessage);
			}
		}
	}
}

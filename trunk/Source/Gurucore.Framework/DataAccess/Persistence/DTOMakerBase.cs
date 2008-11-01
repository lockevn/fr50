using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public abstract class DTOMakerBase
	{
		public abstract T[] GetDTO<T>(IDataReader p_oReader);
	}
}

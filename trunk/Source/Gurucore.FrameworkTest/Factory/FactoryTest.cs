using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.DataAccess.DataSource;
using Gurucore.Framework.DataAccess.DataProvider;

using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest.Factory
{
	public class FactoryTest : TestSuiteBase
	{
		[TestCase]
		public void LoadFactory()
		{
			DataSourceFactory oDSFactory = new DataSourceFactory("Config/System/DataSource.xml");

			DataProviderFactory oDPFactory = new DataProviderFactory("Config/System/DataProvider.xml");
		}
	}
}

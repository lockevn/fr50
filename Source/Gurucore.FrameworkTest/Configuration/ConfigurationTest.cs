using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.Configuration;

using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest.Configuration
{
	public class ConfigurationTest : TestSuiteBase
	{
		[TestCase]
		public void LoadConfig()
		{
			ConfigurationLoader oConfigLoader = new ConfigurationLoader("Configuration/Config4Test.xml");
			List<ConfigurationBase> arrConfig = oConfigLoader.Load();
		}
	}
}

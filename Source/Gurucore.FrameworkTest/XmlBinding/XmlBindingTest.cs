using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest.XmlBinding
{
	public class XmlBindingTest : TestSuiteBase
	{
		[TestCase]
		public void BindTheWorld()
		{
			XmlBinder<World> oBinder = new XmlBinder<World>();
			World oWorld = oBinder.Load("XmlBinding/Asia.xml");

			oBinder.Unload(oWorld, "XmlBinding/Asia2.xml");
		}
	}
}

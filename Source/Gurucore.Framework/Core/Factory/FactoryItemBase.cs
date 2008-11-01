using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.Core.Factory
{
	public class FactoryItemBase
	{
		private string m_sName;

		[XmlProperty("name", XmlPropertyType.Attribute)]
		public string Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.Core
{
	[XmlClass]
	public class Capability
	{
		private string m_sAssembly;

		[XmlProperty]
		public string Assembly
		{
			get { return m_sAssembly; }
			set { m_sAssembly = value; }
		}

		private string m_sClass;

		[XmlProperty]
		public string Class
		{
			get { return m_sClass; }
			set { m_sClass = value; }
		}

		private string m_sName;

		[XmlProperty(Type = XmlPropertyType.InnerText)]
		public string Name
		{
			get { return m_sName; }
			set { m_sName = value; }
		}
	}
}

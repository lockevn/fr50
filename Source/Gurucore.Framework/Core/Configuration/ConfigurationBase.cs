using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.XmlBinding;
using Gurucore.Framework.Core.Util;

namespace Gurucore.Framework.Core.Configuration
{
	public abstract class ConfigurationBase
	{
		public ConfigurationBase Copy()
		{
			return (ConfigurationBase)(this.DeepClone());
		}

		private string m_sClass;

		[XmlProperty]
		public string Class
		{
			get { return m_sClass; }
			set { m_sClass = value; }
		}

		private string m_sAssembly;

		[XmlProperty]
		public string Assembly
		{
			get { return m_sAssembly; }
			set { m_sAssembly = value; }
		}
	}
}

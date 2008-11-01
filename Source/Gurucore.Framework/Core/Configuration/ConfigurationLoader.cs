using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

using Gurucore.Framework.Core.Activation;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.Core.Configuration
{
	public class ConfigurationLoader
	{
		private string m_sFileName;

		public ConfigurationLoader(string p_sFileName)
		{
			m_sFileName = p_sFileName;
		}

		public List<ConfigurationBase> Load()
		{
			List<ConfigurationBase> arrResult = new List<ConfigurationBase>();

			XmlTextReader oXmlReader = new XmlTextReader(m_sFileName);
			XmlDocument oXmlDoc = new XmlDocument();
			oXmlDoc.Load(oXmlReader);
			oXmlReader.Close();

			foreach (XmlElement oElement in oXmlDoc.DocumentElement.ChildNodes)
			{
				//cannot use Dynamic activation because it is not loaded

				string sClass = oElement.GetAttribute("class").Trim();
				string sAssembly = oElement.GetAttribute("assembly").Trim();

				DynamicActivator oDynActivator = Application.GetInstance().GetGlobalSharedObject<DynamicActivator>();

				if (sAssembly.NullOrEmpty())
				{
					sAssembly = oDynActivator.GetExecutingAssemblyPath();
				}
				Type oConfigType = oDynActivator.GetType(sAssembly, sClass);

				XmlBinder<object> oXmlBinder = new XmlBinder<object>(oElement, oConfigType);

				ConfigurationBase oConfig = (ConfigurationBase)oXmlBinder.Load();

				arrResult.Add(oConfig);
			}

			return arrResult;
		}
	}
}

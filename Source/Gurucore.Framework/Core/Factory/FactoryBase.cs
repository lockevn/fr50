using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.Framework.Core.Factory
{
	public abstract class FactoryBase<T> where T : FactoryItemBase
	{
		private string m_sDescriptorFile;

		protected string m_sMergeKey;

		private Dictionary<string, T> m_dicItem;
		private Dictionary<string, string> m_dicFactoryAttribute;

		public FactoryBase()
		{
		}

		public FactoryBase(string p_sDescriptorFile) : this(p_sDescriptorFile, null)
		{
		}

		public FactoryBase(string p_sDescriptorFile, string p_sMergeKey)
		{
			m_sDescriptorFile = p_sDescriptorFile;
			m_sMergeKey = p_sMergeKey;
			m_dicItem = new Dictionary<string, T>();
			m_dicFactoryAttribute = new Dictionary<string, string>();
			Load();
		}

		private void Load()
		{
			XmlTextReader oReader = null;

			//get factory name, factory item name
			string sItemName = typeof(T).Name.CamelToLower();

			oReader = new XmlTextReader(m_sDescriptorFile);
			XmlDocument oXmlDoc = new XmlDocument();
			oXmlDoc.Load(oReader);

			//load all factory attributes
			foreach (XmlAttribute oXmlAttr in oXmlDoc.DocumentElement.Attributes)
			{
				m_dicFactoryAttribute.Add(oXmlAttr.Name, oXmlAttr.Value);
			}

			XmlNodeList arrItemNodes = oXmlDoc.DocumentElement.GetElementsByTagName(sItemName);

			foreach (XmlNode oItemNode in arrItemNodes)
			{
				XmlBinder<T> oXmlBinder = new XmlBinder<T>();
				T oItem = oXmlBinder.Load((XmlElement)oItemNode);

				if (m_sMergeKey.NullOrEmpty())
				{
					AddItem(oItem.Name, oItem);
				}
				else
				{
					AddItem(this.m_dicFactoryAttribute[m_sMergeKey] + "#" + oItem.Name, oItem);
				}
			}

			oReader.Close();
		}

		protected void AddItem(string p_sKey, T p_oItem)
		{
			if (m_dicItem.ContainsKey(p_sKey))
			{
				m_dicItem[p_sKey] = p_oItem;
			}
			else
			{
				m_dicItem.Add(p_sKey, p_oItem);
			}
		}

		protected T GetItem(string p_sName)
		{
			T oItem = m_dicItem[p_sName];
			return oItem;
		}

		protected T GetItem(string p_sMergeValue, string p_sName)
		{
			T oItem = m_dicItem[p_sMergeValue + "#" + p_sName];
			return oItem;
		}

		public List<string> GetItemNames()
		{
			List<string> arrItemNames = new List<string>();
			foreach (KeyValuePair<string, T> oEntry in m_dicItem)
			{
				arrItemNames.Add(oEntry.Key);
			}
			return arrItemNames;
		}

		protected string GetFactoryAttribute(string p_sName)
		{
			return m_dicFactoryAttribute[p_sName].ToString();
		}

		public void Merge(FactoryBase<T> p_oOther)
		{
			Type tThis = this.GetType();
			Type tOther = p_oOther.GetType();
			if (tThis != tOther)
			{
				throw new Exception("Factory type mismatch");
			}

			foreach (KeyValuePair<string, string> oEntry in p_oOther.m_dicFactoryAttribute)
			{
				if ((!this.m_dicFactoryAttribute.ContainsKey(oEntry.Key)) && (oEntry.Key.ToString() != m_sMergeKey))
				{
					this.m_dicFactoryAttribute.Add(oEntry.Key, oEntry.Value);
				}
			}

			foreach (KeyValuePair<string, T> oEntry in p_oOther.m_dicItem)
			{
				AddItem(oEntry.Key, oEntry.Value);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

using Gurucore.Framework.Core.Util;

namespace Gurucore.Framework.Core.XmlBinding
{
	public class XmlBinder<T> where T : class
	{
		private XmlElement m_oRootElement;
		private Type m_oRootType;

		public XmlBinder(string p_sFileName)
		{
			XmlTextReader oXmlReader = new XmlTextReader(p_sFileName);
			XmlDocument oXmlDoc = new XmlDocument();
			oXmlDoc.Load(oXmlReader);
			m_oRootElement = oXmlDoc.DocumentElement;
			oXmlReader.Close();
		}

		public XmlBinder(XmlElement p_oRootElement)
		{
			m_oRootElement = p_oRootElement;
		}

		//For non-generic use, when T is object
		public XmlBinder(string p_sFileName, Type p_oRootType) 
			: this(p_sFileName)
		{
			m_oRootType = p_oRootType;
		}

		public XmlBinder(XmlElement p_oRootElement, Type p_oRootType)
			: this(p_oRootElement)
		{
			m_oRootType = p_oRootType;
		}

		public virtual T Load()
		{
			if (typeof(T) == typeof(object))
			{
				//non-generic use
				return UnMarshal(m_oRootElement, m_oRootType) as T;
			}
			else
			{
				//generic use
				return UnMarshal(m_oRootElement, typeof(T)) as T;
			}
		}

		protected virtual object UnMarshal(XmlElement p_oXmlElement, Type p_oType)
		{
			object[] arrClassAttr = p_oType.GetCustomAttributes(typeof(XmClassAttribute), false);
			if (arrClassAttr.Length > 0)
			{
				object oInstance = Activator.CreateInstance(p_oType);
				string sElementName = ((XmClassAttribute)arrClassAttr[0]).ElementName;
				if (sElementName.NullOrEmpty())
				{
					sElementName = p_oType.Name.CamelToLower();
				}

				if (sElementName == p_oXmlElement.Name)
				{
					PropertyInfo[] arrProp = p_oType.GetProperties();
					foreach (PropertyInfo oProp in arrProp)
					{
						object[] arrPropAttr = oProp.GetCustomAttributes(typeof(XmlPropertyAttribute), false);
						if (arrPropAttr.Length > 0)
						{
							XmlPropertyType eType = ((XmlPropertyAttribute)arrPropAttr[0]).Type;
							string sAttributeName = ((XmlPropertyAttribute)arrPropAttr[0]).Name;
							if (sAttributeName.NullOrEmpty())
							{
								sAttributeName = oProp.Name.CamelToLower();
							}

							string sValue = null;
							switch (eType)
							{
								case XmlPropertyType.Attribute:
									sValue = p_oXmlElement.GetAttribute(sAttributeName);
									if (!((sValue.NullOrEmpty()) && (oProp.PropertyType != typeof(string))))
									{
										oProp.SetValue(oInstance, Convert.ChangeType(sValue, oProp.PropertyType), null);
									}
									break;
								case XmlPropertyType.NestedElement:
									XmlElement oNestedElement = (XmlElement)p_oXmlElement.GetElementsByTagName(sAttributeName)[0];
									object oNestedObject = UnMarshal(oNestedElement, oProp.PropertyType);
									oProp.SetValue(oInstance, oNestedObject, null);
									break;
								case XmlPropertyType.InnerText:
									sValue = p_oXmlElement.InnerText;
									oProp.SetValue(oInstance, Convert.ChangeType(sValue, oProp.PropertyType), null);
									break;
							}

							//recursive needed
						}
						else
						{
							arrPropAttr = oProp.GetCustomAttributes(typeof(XmlSubSequenceAttribute), false);
							if (arrPropAttr.Length > 0)
							{
								string sNestedElementName = ((XmlSubSequenceAttribute)arrPropAttr[0]).ElementName;
								if (sNestedElementName.NullOrEmpty())
								{
									sNestedElementName = oProp.Name.CamelToLower();
								}
								XmlNodeList arrNestedElement = p_oXmlElement.GetElementsByTagName(sNestedElementName);

								object oValue = Activator.CreateInstance(oProp.PropertyType);

								//if is dictionary
								bool bIsDictionary = oProp.PropertyType.FullName.StartsWith("System.Collections.Generic.Dictionary`2");

								foreach (XmlElement oNestedElement in arrNestedElement)
								{
									object oNestedObject;
									if (bIsDictionary)
									{
										string sHashKey = ((XmlSubSequenceAttribute)arrPropAttr[0]).HashKey;
										string sHashKeyValue;
										oNestedObject = UnMarshal(oNestedElement, oProp.PropertyType.GetGenericArguments()[1]);
										if (sHashKey.NullOrEmpty())
										{
											//use inner text as key
											sHashKeyValue = oNestedElement.InnerText;
										}
										else
										{
											//attribute or nested nested element's inner
											if ((sHashKeyValue = oNestedElement.GetAttribute(sHashKey)) == null)
											{
												XmlNodeList arrNestedElementChildren = oNestedElement.GetElementsByTagName(sHashKey);
												if (arrNestedElementChildren.Count > 0)
												{
													sHashKeyValue = arrNestedElementChildren[0].InnerText;
												}
												else
												{
													continue;
												}
											}
										}
										oProp.PropertyType.GetMethod("Add").Invoke(oValue, new object[] { sHashKeyValue, oNestedObject });
									}
									else
									{
										oNestedObject = UnMarshal(oNestedElement, oProp.PropertyType.GetGenericArguments()[0]);
										oProp.PropertyType.GetMethod("Add").Invoke(oValue, new object[] { oNestedObject });
									}
								}
								oProp.SetValue(oInstance,oValue,null);
							}
							else
							{
								continue;
							}
						}
					}
					return oInstance;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return Convert.ChangeType(p_oXmlElement.InnerText, p_oType);
			}
		}
	}
}

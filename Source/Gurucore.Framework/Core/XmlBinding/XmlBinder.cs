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
		private Type m_oRootType;

		public XmlBinder()
		{
		}

		public XmlBinder(Type p_oRootType) 
		{
			m_oRootType = p_oRootType;
		}

		public virtual T Load(string p_sFileName)
		{			
			XmlTextReader oXmlReader = new XmlTextReader(p_sFileName);
			XmlDocument oXmlDoc = new XmlDocument();
			oXmlDoc.Load(oXmlReader);
			XmlElement oRootElement = oXmlDoc.DocumentElement;
			oXmlReader.Close();

			return this.Load(oRootElement);
		}

		public virtual T Load(XmlElement p_oRootElement)
		{
			if (typeof(T) == typeof(object))
			{
				//non-generic use
				return UnMarshal(p_oRootElement, m_oRootType) as T;
			}
			else
			{
				//generic use
				return UnMarshal(p_oRootElement, typeof(T)) as T;
			}
		}

		protected virtual object UnMarshal(XmlElement p_oXmlElement, Type p_oType)
		{
			object[] arrClassAttr = p_oType.GetCustomAttributes(typeof(XmlClassAttribute), false);
			if (arrClassAttr.Length > 0)
			{
				object oInstance = Activator.CreateInstance(p_oType);
				string sElementName = ((XmlClassAttribute)arrClassAttr[0]).ElementName;
				if (sElementName.NullOrEmpty())
				{
					sElementName = p_oType.Name.CamelToLower();
				}

				if (sElementName == p_oXmlElement.Name)
				{
					PropertyInfo[] arrProp = p_oType.GetProperties();
					foreach (PropertyInfo oProp in arrProp)
					{
						object[] arrPropAttr = oProp.GetCustomAttributes(typeof(XmlPropertyAttribute), true);
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

		public virtual XmlElement Unload(T p_oObject, string p_sFileName)
		{
			XmlDocument oXmlDoc = new XmlDocument();

			XmlNode oDeclareNode = oXmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
			oXmlDoc.AppendChild(oDeclareNode);

			XmlElement oRootElement = this.Unload(p_oObject, oXmlDoc);
			oXmlDoc.AppendChild(oRootElement);

			oXmlDoc.Save(p_sFileName);
			return oRootElement;
		}

		public virtual XmlElement Unload(T p_oObject, XmlDocument p_oXmlDoc)
		{
			if (typeof(T) == typeof(object))
			{
				//non-generic use
				return Marshal(p_oObject, m_oRootType, p_oXmlDoc);
			}
			else
			{
				//generic use
				return Marshal(p_oObject, typeof(T), p_oXmlDoc);
			}
		}

		protected XmlElement Marshal(object p_oObject, Type p_oType, XmlDocument p_oXmlDoc)
		{
			object[] arrXmlClassAttr = p_oType.GetCustomAttributes(typeof(XmlClassAttribute), true);
			if (arrXmlClassAttr.Length > 0)
			{
				string sElementName = ((XmlClassAttribute)arrXmlClassAttr[0]).ElementName;
				if (sElementName.NullOrEmpty())
				{
					sElementName = p_oType.Name.CamelToLower();
				}
				XmlElement oElement = p_oXmlDoc.CreateElement(sElementName);

				PropertyInfo[] arrProperty = p_oType.GetProperties();
				foreach (PropertyInfo oProperty in arrProperty)
				{
					object[] arrXmlPropAttr = oProperty.GetCustomAttributes(typeof(XmlPropertyAttribute), true);
					object[] arrXmlSubSeqAttr = oProperty.GetCustomAttributes(typeof(XmlSubSequenceAttribute), true);
					if (arrXmlPropAttr.Length > 0)
					{
						XmlPropertyAttribute oPropAttr = (XmlPropertyAttribute)arrXmlPropAttr[0];
						string sPropName = oPropAttr.Name;
						if (sPropName.NullOrEmpty())
						{
							sPropName = oProperty.Name.CamelToLower();
						}
						switch (oPropAttr.Type)
						{
							case XmlPropertyType.Attribute:
								oElement.SetAttribute(sPropName, oProperty.GetValue(p_oObject, null).ToString());
								break;
							case XmlPropertyType.InnerText:
								oElement.InnerText = oProperty.GetValue(p_oObject, null).ToString();
								break;
							case XmlPropertyType.NestedElement:
								object[] arrXmlClassAttrSub = oProperty.PropertyType.GetCustomAttributes(typeof(XmlClassAttribute), true);
								if (arrXmlClassAttrSub.Length > 0)
								{
									oElement.AppendChild(this.Marshal(oProperty.GetValue(p_oObject, null), oProperty.PropertyType, p_oXmlDoc));
								}
								else
								{
									XmlElement oSubElement = p_oXmlDoc.CreateElement(sPropName);
									oSubElement.InnerText = oProperty.GetValue(p_oObject, null).ToString();
									oElement.AppendChild(oSubElement);
								}
								break;
						}
					}
					else if (arrXmlSubSeqAttr.Length > 0)
					{
						object oPropValue = oProperty.GetValue(p_oObject, null);
						if ((oProperty.PropertyType.IsGenericType) && (oProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))) //List<>
						{
							int nCount = (int)oProperty.PropertyType.GetProperty("Count").GetValue(oPropValue, null);
							MethodInfo oGetItemMethod = oProperty.PropertyType.GetMethod("get_Item");
							for (int nIndex = 0; nIndex < nCount; nIndex++)
							{
								object oItem = oGetItemMethod.Invoke(oPropValue, new object[] { nIndex });
								//check for primitive subsequence
								//recursive marshal
								oElement.AppendChild(this.Marshal(oItem, oItem.GetType() ,p_oXmlDoc));
							}
						}
						else if ((oProperty.PropertyType.IsGenericType) && (oProperty.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))) //Dictionary
						{
							object oEnumerator = oProperty.PropertyType.GetMethod("GetEnumerator").Invoke(oPropValue, null);
							MethodInfo oNextMethod = oEnumerator.GetType().GetMethod("MoveNext");
							PropertyInfo oCurrentProperty = oEnumerator.GetType().GetProperty("Current");

							while ((bool)oNextMethod.Invoke(oEnumerator, null))
							{
								object oItem = oCurrentProperty.GetValue(oEnumerator, null);
								object oKey = oItem.GetType().GetProperty("Key").GetValue(oItem, null);
								object oValue = oItem.GetType().GetProperty("Value").GetValue(oItem, null);
								oElement.AppendChild(this.Marshal(oValue, oValue.GetType(), p_oXmlDoc));
							}
						}
					}
				}

				return oElement;
			}
			else
			{
				return null;
			}
		}
	}
}

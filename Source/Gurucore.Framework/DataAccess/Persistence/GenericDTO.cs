using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Gurucore.Framework.Core;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class GenericDTO : DTOBase
	{
		private const string SCHEMA_MAP = "__schema_map__";

		private Dictionary<string, object> m_dicColumn;

		private Dictionary<string, Type> m_dicSchema;

		public GenericDTO() : base(1)
		{
			//m_dicIsNull = new Dictionary<string, bool>();
			m_dicColumn = new Dictionary<string, object>();
			m_dicSchema = new Dictionary<string, Type>();
		}

		public object this[string index]
		{
			get
			{
				return m_dicColumn[index];
			}
			set
			{
				if (value != null)
				{
					if (!m_dicColumn.ContainsKey(index))
					{
						//create schema
						if (!m_dicSchema.ContainsKey(index))
						{
							m_dicSchema.Add(index, value.GetType());
						}
						else
						{
							//check against schema
							if (!m_dicSchema[index].Equals(value.GetType()))
							{
								throw new FrameworkException("Column type mismatch: " + index, null);
							}
						}

						m_dicColumn.Add(index, value);
						//m_dicIsNull.Add(index, false);
					}
					else
					{
						//check against schema
						if (!m_dicSchema[index].Equals(value.GetType()))
						{
							throw new FrameworkException("Column type mismatch: " + index, null);
						}

						m_dicColumn[index] = value;
						//this.SetNotNull(index);
					}
				}
				else
				{
					m_dicColumn[index] = null;
					//m_dicIsNull[index] = true;
					m_dicSchema[index] = typeof(int);//default as int
				}
			}
		}

		public override void SetNull(string p_sColumn)
		{
			m_dicColumn[p_sColumn] = null;
		}

		protected override void SetNotNull(string p_sColumn)
		{
			//do nothing
		}

		public override bool IsNull(string p_sColumn)
		{
			return m_dicColumn[p_sColumn] == null;
		}

		internal void SetSchema(string p_sColumn, Type p_sDataType)
		{
			m_dicSchema[p_sColumn] = p_sDataType;
		}

		public override void ReadXml(System.Xml.XmlReader p_oReader)
		{
			p_oReader.ReadStartElement();

			this.SchemaSerializeString = p_oReader.ReadElementString(GenericDTO.SCHEMA_MAP);
			
			foreach (string sColumn in this.m_dicSchema.Keys)
			{
				try
				{
					string sValue = p_oReader.ReadElementString(sColumn);
					m_dicColumn[sColumn] = Convert.ChangeType(sValue, m_dicSchema[sColumn]);
				}
				catch
				{
					//it means column has null value
					m_dicColumn[sColumn] = null;
				}
			}

			p_oReader.ReadEndElement();
		}

		public override void WriteXml(System.Xml.XmlWriter p_oWriter)
		{
			//write the schema
			p_oWriter.WriteElementString(GenericDTO.SCHEMA_MAP, this.SchemaSerializeString);

			foreach (string sColumn in this.m_dicColumn.Keys)
			{
				if (m_dicColumn[sColumn] != null)
				{
					string sValue = m_dicColumn[sColumn].ToString();
					p_oWriter.WriteElementString(sColumn, sValue);
				}
			}
		}

		private string SchemaSerializeString
		{
			get
			{
				string sSerializeString = string.Empty;
				foreach (KeyValuePair<string, Type> oEntry in m_dicSchema)
				{
					sSerializeString += oEntry.Key + ":" + oEntry.Value.FullName + "#";
				}
				return sSerializeString;
			}

			set
			{
				Dictionary<string, Type> dicNew = new Dictionary<string, Type>();
				Assembly oSystemAssembly = typeof(string).Assembly;

				string[] arrSchemaType = value.Split(new string[] {"#"}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string sSchemaType in arrSchemaType)
				{
					string[] arrSchemaPart = sSchemaType.Split(new string[] {":"}, StringSplitOptions.RemoveEmptyEntries);
					dicNew.Add(arrSchemaPart[0], oSystemAssembly.GetType(arrSchemaPart[1]));
				}
				this.m_dicSchema = dicNew;
			}
		}
	}
}

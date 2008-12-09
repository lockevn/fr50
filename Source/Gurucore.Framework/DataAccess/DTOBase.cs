using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence;

namespace Gurucore.Framework.DataAccess
{
	public abstract class DTOBase : IXmlSerializable
	{
		private const string NULL_MAP = "__null_map__";
		private Dictionary<string, bool> m_dicIsNull;

		protected Dictionary<string, DTOColumnState> m_dicColumnState;

		public DTOBase()
		{
			m_dicIsNull = new Dictionary<string, bool>();

			Type oInstanceType = this.GetType();
			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oInstanceType);

			foreach (string sColumn in oTableInfo.Column)
			{
				m_dicIsNull.Add(sColumn, true);
			}
		}

		protected DTOBase(int p_nDummy)
		{
		}

		private object m_oDTOTag;

		public object DTOTag
		{
			get { return m_oDTOTag; }
			set { m_oDTOTag = value; }
		}

		private string IsNullSerializeString
		{
			get
			{
				string sSerializeString = string.Empty;
				foreach (KeyValuePair<string, bool> oEntry in m_dicIsNull)
				{
					if ((bool)oEntry.Value)
					{
						sSerializeString += "1";
					}
					else
					{
						sSerializeString += "0";
					}
				}
				return sSerializeString;
			}

			set
			{
				Dictionary<string, bool> dicNew = new Dictionary<string, bool>();
				int nIdx = 0;
				foreach (KeyValuePair<string, bool> oEntry in this.m_dicIsNull)
				{
					if (value[nIdx] == '0')
					{
						dicNew.Add(oEntry.Key, false);
					}
					else
					{
						dicNew.Add(oEntry.Key, true);
					}
					nIdx++;
				}
				this.m_dicIsNull = dicNew;
			}
		}

		public virtual bool IsNull(string p_sColumn)
		{
			return m_dicIsNull[p_sColumn];
		}

		public virtual void SetNull(string p_sColumn)
		{
			m_dicIsNull[p_sColumn] = true;
		}

		protected virtual void SetNotNull(string p_sColumn)
		{
			m_dicIsNull[p_sColumn] = false;
		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public virtual void ReadXml(System.Xml.XmlReader p_oReader)
		{
			Type oInstanceType = this.GetType();
			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oInstanceType);

			p_oReader.ReadStartElement();
			foreach (string sColumn in oTableInfo.Column)
			{
				try
				{
					string sValue = p_oReader.ReadElementString(sColumn);
					oTableInfo.Property[sColumn].SetValue(this, Convert.ChangeType(sValue, oTableInfo.Property[sColumn].PropertyType), null);
				}
				catch
				{
					// column had null value and was not serialized
					if (!oTableInfo.Property[sColumn].PropertyType.IsPrimitive)
					{
						oTableInfo.Property[sColumn].SetValue(this, null, null);
					}
				}
			}

			this.IsNullSerializeString = p_oReader.ReadElementString(DTOBase.NULL_MAP);

			p_oReader.ReadEndElement();
		}

		public virtual void WriteXml(System.Xml.XmlWriter p_oWriter)
		{
			Type oInstanceType = this.GetType();
			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(oInstanceType);

			//write properties
			foreach (string sColumn in oTableInfo.Column)
			{
				object oValue = oTableInfo.Property[sColumn].GetValue(this, null);
				if (oValue != null)
				{
					p_oWriter.WriteElementString(sColumn, oValue.ToString());
				}
			}

			//write is null string
			p_oWriter.WriteElementString(DTOBase.NULL_MAP, this.IsNullSerializeString);
		}
	}
}

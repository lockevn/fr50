using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.JITGeneration;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class DTOMakerClassGenerator
	{
		private Type m_oDTOType;		

		public DTOMakerClassGenerator(Type p_oDTOType)
		{
			m_oDTOType = p_oDTOType;
		}

		public DTOMakerBase GetDTOMaker()
		{
			JITClassManager oJITClassMgr = Application.GetInstance().GetGlobalSharedObject<JITClassManager>();
			string sClassName = m_oDTOType.FullName + "_DTOMaker";
			string sGeneratedCode = string.Empty;

			if (!oJITClassMgr.IsRegistered(sClassName))
			{
				sGeneratedCode = this.GenerateCode();
			}
			Type oType = oJITClassMgr.GetRegisteredClass(sClassName, sGeneratedCode, true);

			return (DTOMakerBase)Activator.CreateInstance(oType);
		}

		private string GenerateCode()
		{
			Dictionary<string, string> m_dicNullableMap = new Dictionary<string, string>();
			m_dicNullableMap.Add("Byte", "byte");
			m_dicNullableMap.Add("SByte", "sbyte");
			m_dicNullableMap.Add("Int32", "int");
			m_dicNullableMap.Add("UInt32", "uint");
			m_dicNullableMap.Add("Int16", "short");
			m_dicNullableMap.Add("UInt16", "ushort");
			m_dicNullableMap.Add("Int64", "long");
			m_dicNullableMap.Add("UInt64", "ulong");
			m_dicNullableMap.Add("Single", "float");
			m_dicNullableMap.Add("Double", "double");
			m_dicNullableMap.Add("Char", "char");
			m_dicNullableMap.Add("Boolean", "bool");
			m_dicNullableMap.Add("Decimal", "decimal");

			TableInfoManager oTableInfoMgr = Application.GetInstance().GetGlobalSharedObject<TableInfoManager>();
			TableInfo oTableInfo = oTableInfoMgr.GetTableInfo(m_oDTOType);

			StringBuilder oCodeBuilder = new StringBuilder();
			string sUniqueNumber = (new Random()).Next().ToString();

			oCodeBuilder.Append("using System;\r\n");
			oCodeBuilder.Append("using System.Collections.Generic;\r\n");
			oCodeBuilder.Append("using System.Data;\r\n");
			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("using Gurucore.Framework.DataAccess.Persistence;\r\n");
			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("using #dto_namespace;\r\n");
			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("namespace #dto_namespace\r\n");
			oCodeBuilder.Append("{\r\n");
			oCodeBuilder.Append("	public sealed class #dto_class_DTOMaker : DTOMakerBase\r\n");
			oCodeBuilder.Append("	{\r\n");
			oCodeBuilder.Append("		public #dto_class_DTOMaker()\r\n");
			oCodeBuilder.Append("		{\r\n");
			oCodeBuilder.Append("		}\r\n");
			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("		public override T[] GetDTO<T>(IDataReader p_oReader, string[] p_arrColumn)\r\n");
			oCodeBuilder.Append("		{\r\n");
			oCodeBuilder.Append("			List<T> arrDTO = new List<T>();\r\n");
			oCodeBuilder.Append("			List<string> arrColumn = null;\r\n");
			oCodeBuilder.Append("			if (p_arrColumn != null)\r\n");
			oCodeBuilder.Append("			{\r\n");
			oCodeBuilder.Append("				arrColumn = new List<string>(p_arrColumn);\r\n");
			oCodeBuilder.Append("			}\r\n");
			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("			while (p_oReader.Read())\r\n");
			oCodeBuilder.Append("			{\r\n");
			oCodeBuilder.Append("				#dto_class oDTO = new #dto_class();\r\n");
			oCodeBuilder.Append("\r\n");

			foreach (string sColumn in oTableInfo.Column)
			{
				string sPropertyType = null;
				if (oTableInfo.Property[sColumn].PropertyType.IsGenericType)
				{
					//Nullable
					sPropertyType = m_dicNullableMap[oTableInfo.Property[sColumn].PropertyType.GetGenericArguments()[0].Name];
				}
				else
				{
					sPropertyType = oTableInfo.Property[sColumn].PropertyType.Name;
				}
				oCodeBuilder.Append("				if (((arrColumn == null) || (arrColumn.Count == 0) || (arrColumn.Contains(\"").Append(sColumn).Append("\"))) && (p_oReader[\"").Append(sColumn).Append("\"] != DBNull.Value))\r\n");
				oCodeBuilder.Append("				{\r\n");
				oCodeBuilder.Append("					oDTO.").Append(oTableInfo.Property[sColumn].Name).Append(" = (").Append(sPropertyType).Append(")Convert.ChangeType(p_oReader[\"").Append(sColumn).Append("\"],typeof(").Append(sPropertyType).Append("));\r\n");
				oCodeBuilder.Append("				}\r\n");
			}

			oCodeBuilder.Append("\r\n");
			oCodeBuilder.Append("				arrDTO.Add((T)Convert.ChangeType(oDTO, typeof(T)));\r\n");
			oCodeBuilder.Append("			}\r\n");
			oCodeBuilder.Append("			return arrDTO.ToArray();\r\n");
			oCodeBuilder.Append("		}\r\n");
			oCodeBuilder.Append("	}\r\n");
			oCodeBuilder.Append("}\r\n");

			oCodeBuilder.Replace("#dto_namespace", m_oDTOType.Namespace);
			oCodeBuilder.Replace("#dto_class", m_oDTOType.Name);
			oCodeBuilder.Replace("#unique_number", sUniqueNumber);

			//System.Diagnostics.Debug.Write(oCodeBuilder);

			return oCodeBuilder.ToString();
		}
	}
}
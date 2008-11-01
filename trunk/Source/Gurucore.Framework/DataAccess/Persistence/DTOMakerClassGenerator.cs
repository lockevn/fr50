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
			string sClassName = "Gurucore.Framework.DataAccess.Persistence." + m_oDTOType.Name + "_DTOMaker";
			string sGeneratedCode = string.Empty;
			if (!oJITClassMgr.IsRegistered(sClassName))
			{
				sGeneratedCode = GenerateCode();
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

			StringBuilder sGeneratedCode = new StringBuilder();

			sGeneratedCode.Append("using System;\r\n");
			sGeneratedCode.Append("using System.Collections.Generic;\r\n");
			sGeneratedCode.Append("using System.Data;\r\n");
			sGeneratedCode.Append("\r\n");
			sGeneratedCode.Append("using #dto_namespace;\r\n");
			sGeneratedCode.Append("\r\n");
			sGeneratedCode.Append("namespace Gurucore.Framework.DataAccess.Persistence\r\n");
			sGeneratedCode.Append("{\r\n");
			sGeneratedCode.Append("	public sealed class #dto_class_DTOMaker : DTOMakerBase\r\n");
			sGeneratedCode.Append("	{\r\n");
			sGeneratedCode.Append("		public #dto_class_DTOMaker()\r\n");
			sGeneratedCode.Append("		{\r\n");
			sGeneratedCode.Append("		}\r\n");
			sGeneratedCode.Append("\r\n");
			sGeneratedCode.Append("		public override T[] GetDTO<T>(IDataReader p_oReader)\r\n");
			sGeneratedCode.Append("		{\r\n");
			sGeneratedCode.Append("			List<T> arrDTO = new List<T>();\r\n");
			sGeneratedCode.Append("\r\n");
			sGeneratedCode.Append("			while (p_oReader.Read())\r\n");
			sGeneratedCode.Append("			{\r\n");
			sGeneratedCode.Append("				#dto_class oDTO = new #dto_class();\r\n");
			sGeneratedCode.Append("\r\n");

			foreach (string sColumn in oTableInfo.Column)
			{
				string sPropertyType = null;
				if (oTableInfo.Property[sColumn].PropertyType.IsGenericType)
				{
					//Nullable
					sPropertyType = m_dicNullableMap[oTableInfo.Property[sColumn].PropertyType.GetGenericArguments()[0].Name] + "?";
				}
				else
				{
					sPropertyType = oTableInfo.Property[sColumn].PropertyType.Name;
				}
				sGeneratedCode.Append("				if (p_oReader[\"").Append(sColumn).Append("\"] != DBNull.Value)\r\n");
				sGeneratedCode.Append("				{\r\n");
				sGeneratedCode.Append("					oDTO.").Append(sColumn).Append(" = (").Append(sPropertyType).Append(")p_oReader[\"").Append(sColumn).Append("\"];\r\n");
				sGeneratedCode.Append("				}\r\n");
			}

			sGeneratedCode.Append("\r\n");
			sGeneratedCode.Append("				arrDTO.Add((T)Convert.ChangeType(oDTO, typeof(T)));\r\n");
			sGeneratedCode.Append("			}\r\n");
			sGeneratedCode.Append("			return arrDTO.ToArray();\r\n");
			sGeneratedCode.Append("		}\r\n");
			sGeneratedCode.Append("	}\r\n");
			sGeneratedCode.Append("}\r\n");

			sGeneratedCode.Replace("#dto_namespace", m_oDTOType.Namespace);
			sGeneratedCode.Replace("#dto_class", m_oDTOType.Name);

			return sGeneratedCode.ToString();
		}
	}
}

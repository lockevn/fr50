using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.DataAccess;

namespace Gurucore.DTOGenerator.DTO
{
	[MappedTable(TableName = "INFORMATION_SCHEMA.TABLES", AllowTablePrefix = false, Updatable = false)]
	public class Table
	{
		public const string TABLE_CATALOG = "TABLE_CATALOG";
		public const string TABLE_SCHEMA = "TABLE_SCHEMA";
		public const string TABLE_NAME = "TABLE_NAME";
		public const string TABLE_TYPE = "TABLE_TYPE";

		private string m_sTableCatalog;

		[MappedColumn(ColumnName = TABLE_CATALOG)]
		public string TableCatalog
		{
			get { return m_sTableCatalog; }
			set { m_sTableCatalog = value; }
		}

		private string m_sTableSchema;

		[MappedColumn(ColumnName = TABLE_SCHEMA)]
		public string TableSchema
		{
			get { return m_sTableSchema; }
			set { m_sTableSchema = value; }
		}

		private string m_sTableName;

		[MappedColumn(ColumnName = TABLE_NAME)]
		public string TableName
		{
			get { return m_sTableName; }
			set { m_sTableName = value; }
		}

		private string m_sTableType;

		[MappedColumn(ColumnName = TABLE_TYPE)]
		public string TableType
		{
			get { return m_sTableType; }
			set { m_sTableType = value; }
		}
	}
}

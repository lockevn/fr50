using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.DataAccess;
using Gurucore.Framework.DataAccess.Persistence;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;
using Gurucore.Framework.DataAccess.DataSource;

using Gurucore.DTOGenerator.DTO;

namespace Gurucore.DTOGenerator.Business
{
	public class SchemaService
	{
		public string[] GetAllDataSource()
		{
			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			List<string> arrDSNameWithGUID = oDSFactory.GetItemNames();

			List<string> arrDSName = new List<string>();
			foreach (string sDSName in arrDSNameWithGUID)
			{
				arrDSName.Add(sDSName.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[1]);
			}

			return arrDSName.ToArray();
		}

		public ClassInfo[] GetAllObjects(string p_sDataSource, bool p_bGetView)
		{
			DTOGeneratorConfiguration oDTOGenCfg = Application.GetInstance().GetGlobalSharedObject<DTOGeneratorConfiguration>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			oDACtx.SetCurrentDataSource(this, p_sDataSource);

			DataSourceFactory oDSFactory = Application.GetInstance().GetGlobalSharedObject<DataSourceFactory>();
			string sDSFull = oDACtx.GetCurrentDataSource(this);
			string sProvider = oDSFactory.GetProvider(sDSFull);
			string sTablePrefix = oDSFactory.GetTablePrefix(sDSFull);

			TableMapper<Table> tblTable = new TableMapper<Table>();
			TableMapper<Column> tblColumn = new TableMapper<Column>();	
			TableMapper<TableConstraint> tblTableConstraint = new TableMapper<TableConstraint>();
			TableMapper<KeyColumnUsage> tblKeyColumnUsage = new TableMapper<KeyColumnUsage>();

			Expression expTableFilter = new ColumnOperand(Table.TABLE_SCHEMA).Eq(new ConstantOperand(p_sDataSource));
			Table[] arrTables = tblTable.Select(expTableFilter);
			
			List<ClassInfo> arrObjects = new List<ClassInfo>();
			foreach (Table dtoTable in arrTables)
			{
				ClassInfo oObject = new ClassInfo();
				oObject.TableName = dtoTable.TableName;
				if (oObject.TableName.StartsWith(sTablePrefix))
				{
					oObject.TableName = oObject.TableName.Substring(sTablePrefix.Length);
				}
				oObject.IsTable = dtoTable.TableType == "BASE TABLE";
				oObject.Namespace = oDTOGenCfg.GeneratedNamespace;

				//get all columns
				Expression expColumnFilter = 
					new ColumnOperand(Column.TABLE_SCHEMA).Eq(new ConstantOperand(dtoTable.TableSchema)).And(
					new ColumnOperand(Column.TABLE_NAME).Eq(new ConstantOperand(dtoTable.TableName)));

				Column[] arrColumns = tblColumn.Select(expColumnFilter);

				//get primary key constraints
				Expression expConstraintFilter =
					new ColumnOperand(TableConstraint.TABLE_SCHEMA).Eq(new ConstantOperand(dtoTable.TableSchema)).And(
					new ColumnOperand(TableConstraint.TABLE_NAME).Eq(new ConstantOperand(dtoTable.TableName)).And(
					new ColumnOperand(TableConstraint.CONSTRAINT_TYPE).Eq(new ConstantOperand("PRIMARY KEY"))));

				TableConstraint[] arrConstraints = tblTableConstraint.Select(expConstraintFilter);

				//get primary key column
				if (arrConstraints.Length > 0)
				{
					Expression expKeyColumnUsage = 
						new ColumnOperand(KeyColumnUsage.CONSTRAINT_NAME).Eq(new ConstantOperand(arrConstraints[0].ConstraintName)).And(
						new ColumnOperand(KeyColumnUsage.TABLE_NAME).Eq(new ConstantOperand(arrConstraints[0].TableName)).And(
						new ColumnOperand(KeyColumnUsage.TABLE_SCHEMA).Eq(new ConstantOperand(arrConstraints[0].TableSchema))));
					KeyColumnUsage[] arrKeys = tblKeyColumnUsage.Select(expKeyColumnUsage);
					if (arrKeys.Length > 0)
					{
						oObject.PrimaryKey = arrKeys[0].ColumnName;
						oObject.NoConventionPK = (oObject.PrimaryKey != oObject.TableName + "ID");
					}
				}

				foreach (Column dtoColumn in arrColumns)
				{
					PropertyInfo oProperty = new PropertyInfo();
					oProperty.ColumnName = oProperty.PropertyName = dtoColumn.ColumnName;
					oProperty.PropertyType = oDTOGenCfg.TypeMapping[sProvider].Type[dtoColumn.DataType].CSharpType;
					oProperty.VariableName = "m_" + oDTOGenCfg.TypeMapping[sProvider].Type[dtoColumn.DataType].Prefix + dtoColumn.ColumnName;
					oProperty.ConstantName = oProperty.ColumnName.CamelToLower().ToUpper();

					//check if column is primary key
					oProperty.IsIdentity = (oObject.PrimaryKey == oProperty.ColumnName);

					oObject.Properties.Add(oProperty);
				}
				
				arrObjects.Add(oObject);
			}
			
			oDACtx.UnSetCurrentDataSource();
			return arrObjects.ToArray();
		}
	}
}

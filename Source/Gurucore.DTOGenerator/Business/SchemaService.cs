using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
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

		public Table[] GetAllTable(string p_sDataSource, bool p_bGetView)
		{
			TableMapper<Table> oTableMapper = new TableMapper<Table>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource(this, p_sDataSource);

			Expression expFilter = new ColumnOperand(Table.TABLE_SCHEMA).Eq(new ConstantOperand("fr50"));

			Table[] arrTable = oTableMapper.Select(expFilter);

			oDACtx.UnSetCurrentDataSource();

			return arrTable;
		}

		public string GetTablePrefix(string p_sDataSource)
		{
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();
			oDACtx.SetCurrentDataSource(this, p_sDataSource);
			return oDACtx.GetTablePrefix();
			oDACtx.UnSetCurrentDataSource();
		}
	}
}

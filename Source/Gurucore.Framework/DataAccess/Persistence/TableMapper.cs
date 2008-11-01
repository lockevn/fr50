using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using Gurucore.Framework.Core;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class TableMapper<T>
	{
		protected string m_sGenericTable;
		protected string m_sGenericPrimaryKey;

		public TableMapper()
		{
		}

		public T GetObject(int p_nObjectID, params string[] p_arrColumn)
		{
			//get current data access context
			DataAccessConfiguration oDACfg = Application.GetInstance().GetGlobalSharedObject<DataAccessConfiguration>();
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			//get Sql generator
			SqlGeneratorBase oSqlGenerator = null;
			if (m_sGenericTable != null)
			{
				oSqlGenerator = new GenericSqlGenerator(m_sGenericTable, m_sGenericPrimaryKey);
			}
			else
			{
				oSqlGenerator = Application.GetInstance().GetGlobalSharedObject<SqlGeneratorBase>(SqlGeneratorBase.CACHE_KEY);
			}

			//obtain DTOMaker
			DTOMakerBase oDTOMaker = null;
			if (m_sGenericTable != null)
			{
				oDTOMaker = new GenericDTOMaker(m_sGenericTable, m_sGenericPrimaryKey, p_arrColumn);
			}
			else
			{
				switch (oDACfg.TransformStrategy)
				{
					case "JIT":
						DTOMakerClassGenerator oGenerator = new DTOMakerClassGenerator(typeof(T));
						oDTOMaker = oGenerator.GetDTOMaker();
						break;
					default:
						oDTOMaker = new ReflectionDTOMaker();
						break;
				}
			}

			//generate query
			string sSql = oSqlGenerator.GetRecordSelect<T>(p_nObjectID, p_arrColumn);

			//obtain connection
			IDbConnection oDbConn = oDACtx.GetConnection();

			//do query
			IDbCommand oDbCmd = oDACtx.GetDbCommand(oDbConn);
			oDbCmd.CommandText = sSql;
			oDbCmd.CommandType = CommandType.Text;
			IDataReader oDataReader = oDbCmd.ExecuteReader();

			//convert IDbDataReader into DTO
			T[] arrDTOs = oDTOMaker.GetDTO<T>(oDataReader);
			oDataReader.Close();
			oDACtx.ReturnConnection(oDbConn);

			//return
			if (arrDTOs.Length != 1)
			{
				return default(T);
			}
			else
			{
				return arrDTOs[0];
			}
		}
		
		public T[] GetObjects(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, params string[] p_arrColumn)
		{
			//get current data access context
			DataAccessConfiguration oDACfg = Application.GetInstance().GetGlobalSharedObject<DataAccessConfiguration>();
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			//get Sql generator
			SqlGeneratorBase oSqlGenerator = Application.GetInstance().GetGlobalSharedObject<SqlGeneratorBase>(SqlGeneratorBase.CACHE_KEY);

			//obtain DTOMaker
			DTOMakerBase oDTOMaker = null;
			switch (oDACfg.TransformStrategy)
			{
				case "JIT":
					DTOMakerClassGenerator oGenerator = new DTOMakerClassGenerator(typeof(T));
					oDTOMaker = oGenerator.GetDTOMaker();
					break;
				default:
					oDTOMaker = new ReflectionDTOMaker();
					break;
			}

			//generate query
			string sSql = oSqlGenerator.GetListSelect<T>(p_expFilter, p_oOrder, p_nFirstRow, p_nRowCount, p_arrColumn);

			//obtain connection
			IDbConnection oDbConn = oDACtx.GetConnection();

			//do query
			IDbCommand oDbCmd = oDACtx.GetDbCommand(oDbConn);
			oDbCmd.CommandText = sSql;
			oDbCmd.CommandType = CommandType.Text;
			IDataReader oDataReader = oDbCmd.ExecuteReader();

			//convert IDbDataReader into DTO
			T[] arrDTOs = oDTOMaker.GetDTO<T>(oDataReader);
			oDataReader.Close();
			oDACtx.ReturnConnection(oDbConn);

			//return
			return arrDTOs;
		}

		public T AddObject(T p_oObject)
		{
			return default(T);
		}

		public int DeleteObject(int p_nObjectID)
		{
			return 0;
		}

		public int DeleteObjects(Expression p_oCriteria)
		{
			return 0;
		}

		public int DeleteObject(T p_oObject)
		{
			return 0;
		}

		public int DeleteObjects(T[] p_arrObject)
		{
			return 0;
		}

		public int SaveObject(T p_oDTO)
		{
			return 0;
		}

		public int SaveObjects(T[] p_arrObject)
		{
			return 0;
		}

		public object GetAggregateValue(Expression p_expTarget, Expression p_oCriteria, Aggregation p_oAggregation)
		{
			return null;
		}
	}
}

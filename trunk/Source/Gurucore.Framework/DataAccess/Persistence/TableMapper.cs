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

		//SELECT column1, column2,... FROM table WHERE tableID = #id
		public T Select(int p_nID, params string[] p_arrColumn)
		{
			//get current data access context
			DataAccessConfiguration oDACfg = Application.GetInstance().GetGlobalSharedObject<DataAccessConfiguration>();
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			//get SqlGenerator
			SqlGeneratorBase oSqlGenerator = GetSqlGenerator();

			//get DTOMaker
			DTOMakerBase oDTOMaker = GetDTOMaker(oDACfg.TransformStrategy, p_arrColumn);

			//generate query
			string sSql = oSqlGenerator.GetRecordSelect<T>(p_nID, p_arrColumn);

			//obtain connection
			IDbConnection oDbConn = oDACtx.GetConnection();

			//do query
			IDbCommand oDbCmd = oDACtx.GetDbCommand(oDbConn);
			oDbCmd.CommandText = sSql;
			oDbCmd.CommandType = CommandType.Text;
			IDataReader oDataReader = oDbCmd.ExecuteReader();

			//convert IDbDataReader into DTO
			T[] arrDTOs = oDTOMaker.GetDTO<T>(oDataReader, p_arrColumn);
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

		//NOTHING
		public T[] Select(params string[] p_arrColumn)
		{
			return this.Select(null, null, 0, 0, p_arrColumn);
		}

		//LIMIT
		public T[] Select(int p_nFirstRow, int p_nRowCount, params string[] p_arrColumn)
		{
			return this.Select(null, null, p_nFirstRow, p_nRowCount, p_arrColumn);
		}

		//ORDER
		public T[] Select(Order p_oOrder, params string[] p_arrColumn)
		{
			return this.Select(null, p_oOrder, 0, 0, p_arrColumn);
		}

		//ORDER + LIMIT
		public T[] Select(Order p_oOrder, int p_nFirstRow, int p_nRowCount, params string[] p_arrColumn)
		{
			return this.Select(null, p_oOrder, p_nFirstRow, p_nRowCount, p_arrColumn);
		}

		//WHERE
		public T[] Select(Expression p_expFilter, params string[] p_arrColumn)
		{
			return this.Select(p_expFilter, null, 0, 0, p_arrColumn);
		}

		//WHERE + LIMIT
		public T[] Select(Expression p_expFilter, int p_nFirstRow, int p_nRowCount, params string[] p_arrColumn)
		{
			return this.Select(p_expFilter, null, p_nFirstRow, p_nRowCount, p_arrColumn);
		}

		//WHERE + ORDER
		public T[] Select(Expression p_expFilter, Order p_oOrder, params string[] p_arrColumn)
		{
			return this.Select(p_expFilter, p_oOrder, 0, 0, p_arrColumn);
		}

		//WHERE + ORDER + LIMIT
		public T[] Select(Expression p_expFilter, Order p_oOrder, int p_nFirstRow, int p_nRowCount, params string[] p_arrColumn)
		{
			//get current data access context
			DataAccessConfiguration oDACfg = Application.GetInstance().GetGlobalSharedObject<DataAccessConfiguration>();
			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			//get SqlGenerator
			SqlGeneratorBase oSqlGenerator = GetSqlGenerator();

			//get DTOMaker
			DTOMakerBase oDTOMaker = GetDTOMaker(oDACfg.TransformStrategy, p_arrColumn);

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
			T[] arrDTOs = oDTOMaker.GetDTO<T>(oDataReader, p_arrColumn);
			oDataReader.Close();
			oDACtx.ReturnConnection(oDbConn);

			//return
			return arrDTOs;
		}

		public T Insert(T p_oObject)
		{
			return default(T);
		}

		public int Delete(int p_nObjectID)
		{
			return 0;
		}

		public int Delete(Expression p_oCriteria)
		{
			return 0;
		}

		public int Delete(T p_oObject)
		{
			return 0;
		}

		public int Delete(T[] p_arrObject)
		{
			return 0;
		}

		public int Update(T p_oDTO)
		{
			return 0;
		}

		public int Update(T[] p_arrObject)
		{
			return 0;
		}

		public object Aggregate(Expression p_expTarget, Expression p_oCriteria, Aggregation p_oAggregation)
		{
			return null;
		}

		private SqlGeneratorBase GetSqlGenerator()
		{
			SqlGeneratorBase oSqlGenerator = null;
			if (m_sGenericTable != null)
			{
				oSqlGenerator = new GenericSqlGenerator(m_sGenericTable, m_sGenericPrimaryKey);
			}
			else
			{
				oSqlGenerator = Application.GetInstance().GetGlobalSharedObject<SqlGeneratorBase>(SqlGeneratorBase.CACHE_KEY);
			}
			return oSqlGenerator;
		}

		private DTOMakerBase GetDTOMaker(string p_sTransformStrategy, string[] p_arrColumn)
		{
			DTOMakerBase oDTOMaker = null;
			if (m_sGenericTable != null)
			{
				oDTOMaker = new GenericDTOMaker(m_sGenericTable, m_sGenericPrimaryKey);
			}
			else
			{
				switch (p_sTransformStrategy)
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
			return oDTOMaker;
		}
	}
}

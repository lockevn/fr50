using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Gurucore.Framework.Test;
using Gurucore.Framework.DataAccess.Persistence;
using Gurucore.Framework.DataAccess;
using Gurucore.Framework.DataAccess.Persistence.QueryLanguage;

namespace Gurucore.FrameworkTest.TableMapper
{
	public class TableMapperTest : TestSuiteBase
	{
		[TestCase]
		public void DTOSerialization()
		{
			XmlSerializer oSerializer = new XmlSerializer(typeof(AutoDTO));
			TextWriter oWriter = new StreamWriter("bmw.xml");

			AutoDTO oBMW = new AutoDTO();
			oBMW.Brand = "BMW";
			oBMW.Age = 4;
			oSerializer.Serialize(oWriter, oBMW);
			oWriter.Close();

			AutoDTO oAnotherBMW;
			TextReader oReader = new StreamReader("bmw.xml");
			oAnotherBMW = (AutoDTO)oSerializer.Deserialize(oReader);
			oReader.Close();

			this.AssertTrue(oAnotherBMW.IsNull(AutoDTO.AUTO_ID), "Null state serialization fail");
		}

		[TestCase]
		public void GenericDTOSerialization()
		{
			XmlSerializer oSerializer = new XmlSerializer(typeof(GenericDTO));
			TextWriter oWriter = new StreamWriter("audi.xml");

			GenericDTO oAudi = new GenericDTO();
			oAudi["AutoID"] = 0;
			oAudi.SetNull("AutoID");
			oAudi["Brand"] = "Audi";
			oAudi["Age"] = 1;
			oSerializer.Serialize(oWriter, oAudi);
			oWriter.Close();

			GenericDTO oAnotherAudi; 
			TextReader oReader = new StreamReader("audi.xml");
			oAnotherAudi = (GenericDTO)oSerializer.Deserialize(oReader);
			oReader.Close();

			this.AssertTrue(oAnotherAudi.IsNull(AutoDTO.AUTO_ID), "Null state serialization fail");
		}

		[TestCase]
		public void GetInformationSchema()
		{
			TableMapper<Table> oTableMapper = new TableMapper<Table>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			Expression expFilter = new ColumnOperand(Table.TABLE_SCHEMA).Eq(new ConstantOperand("fr50"));
				
			Table[] arrTable = oTableMapper.Select(expFilter);

			oDACtx.UnSetCurrentDataSource();
		}

		[TestCase]
		public void GetObjectUsingDTO()
		{
			TableMapper<AutoDTO> oTableMapper = new TableMapper<AutoDTO>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			int nAutoID = 2;
			AutoDTO dtoAuto = null;
			DateTime dtStart = DateTime.Now;
			dtoAuto = oTableMapper.Select(nAutoID, AutoDTO.AUTO_ID, AutoDTO.BRAND, AutoDTO.SERIES, AutoDTO.IS_LUXURY);
			int nTime = DateTime.Now.Subtract(dtStart).Milliseconds;

			oDACtx.UnSetCurrentDataSource();

			this.AssertTrue(dtoAuto.AutoID == nAutoID, "Query was not correct");
			this.AssertTrue(!dtoAuto.IsNull(AutoDTO.BRAND), "Query was not correct. Brand should not be null");
			this.AssertTrue(dtoAuto.IsNull(AutoDTO.SERIES), "Query was not correct. Series should be null");
		}

		[TestCase]
		public void GetObjectUsingPONO()
		{
			TableMapper<Auto> oTableMapper = new TableMapper<Auto>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			int nAutoID = 5;
			Auto oAuto = null;
			DateTime dtStart = DateTime.Now;
			oAuto = oTableMapper.Select(nAutoID);
			int nTime = DateTime.Now.Subtract(dtStart).Milliseconds;

			oDACtx.UnSetCurrentDataSource();

			this.AssertEqual(oAuto.AutoID, nAutoID, "Query was not correct");
			this.AssertNotNull(oAuto.Brand, "Query was not correct. Brand should not be null");
			this.AssertNull(oAuto.Series, "Query was not correct. Series should be null");
		}

		[TestCase]
		public void GetObjectUsingGenericDTO()
		{
			GenericTableMapper oTableMapper = new GenericTableMapper("Auto");

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			int nAutoID = 5;
			GenericDTO dtoAuto = null;
			//dtoAuto = oTableMapper.Select(nAutoID, "AutoID", "Brand", "Age", "Series");
			dtoAuto = oTableMapper.Select(nAutoID); //test SELECT *

			oDACtx.UnSetCurrentDataSource();

			this.AssertTrue((int)dtoAuto["AutoID"] == nAutoID, "Query was not correct");
			this.AssertTrue(!dtoAuto.IsNull("Brand"), "Query was not correct. Brand should not be null");
			this.AssertTrue(dtoAuto.IsNull("Series"), "Query was not correct. Series should be null");
			this.AssertEqual(dtoAuto["IsLuxury"], 1UL, "Query was not correct. This auto should be luxury");
		}

		[TestCase(LoadTest = true)]
		public void GetObjectsUsingDTO()
		{
			TableMapper<AutoDTO> oTableMapper = new TableMapper<AutoDTO>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			AutoDTO[] arrAuto;
			DateTime dtStart = DateTime.Now;

			Expression expFilter =	new Expression(new ColumnOperand(AutoDTO.BRAND), Operator.Neq, new ConstantOperand("BMW")).Or(
									new Expression(new ColumnOperand(AutoDTO.EXPIRE_DATE), Operator.Gt, new ConstantOperand(DateTime.Now))).Or(
									new Expression(new ColumnOperand(AutoDTO.BRAND), Operator.IsNull));

			//arrAuto = oTableMapper.Select(expFilter, new Order(AutoDTO.AUTO_ID, SortType.Ascending), 0, 100);
			arrAuto = oTableMapper.Select(0, 100000); //SELECT WITH no order, no filter

			double dblTime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;

			oDACtx.UnSetCurrentDataSource();
		}

		[TestCase(LoadTest = true)]
		public void GetObjectsUsingPONO()
		{
			TableMapper<Auto> oTableMapper = new TableMapper<Auto>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			Auto[] arrAuto;
			DateTime dtStart = DateTime.Now;

			Expression expFilter = new Expression(new ColumnOperand(Auto.BRAND), Operator.Neq, new ConstantOperand("BMW")).Or(
									new Expression(new ColumnOperand(Auto.EXPIRE_DATE), Operator.Gt, new ConstantOperand(DateTime.Now))).Or(
									new Expression(new ColumnOperand(Auto.BRAND), Operator.IsNull));

			//arrAuto = oTableMapper.Select(expFilter, new Order(Auto.AUTO_ID, SortType.Ascending), 0, 100);
			arrAuto = oTableMapper.Select(0, 100000); //SELECT WITH no order, no filter
			double dblTime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;

			oDACtx.UnSetCurrentDataSource();
		}

		[TestCase(LoadTest = true)]
		public void GetObjectsUsingGenericDTO()
		{
			GenericTableMapper oTableMapper = new GenericTableMapper("Auto");

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			GenericDTO[] arrAuto;
			DateTime dtStart = DateTime.Now;

			Expression expFilter = new Expression(new ColumnOperand("Brand"), Operator.Neq, new ConstantOperand("BMW")).Or(
									new Expression(new ColumnOperand("ExpireDate"), Operator.Gt, new ConstantOperand(DateTime.Now))).Or(
									new Expression(new ColumnOperand("Brand"), Operator.IsNull));

			//arrAuto = oTableMapper.Select(expFilter, new Order("AutoID", SortType.Ascending), 0, 100, "AutoID", "Brand", "Age", "Series");
			//arrAuto = oTableMapper.Select(expFilter, new Order("AutoID", SortType.Ascending), 0, 100); //SELECT *
			arrAuto = oTableMapper.Select(0, 100000); //SELECT WITH no order, no filter

			double dblTime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;

			oDACtx.UnSetCurrentDataSource();
		}
	}
}

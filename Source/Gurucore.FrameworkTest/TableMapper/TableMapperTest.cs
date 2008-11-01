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
		public void GetObjectUsingDTO()
		{
			TableMapper<AutoDTO> oTableMapper = new TableMapper<AutoDTO>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			int nAutoID = 2;
			AutoDTO dtoAuto = null;
			DateTime dtStart = DateTime.Now;
			dtoAuto = oTableMapper.GetObject(nAutoID);
			int nTime = DateTime.Now.Subtract(dtStart).Milliseconds;

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

			int nAutoID = 2;
			Auto dtoAuto = null;
			DateTime dtStart = DateTime.Now;
			dtoAuto = oTableMapper.GetObject(nAutoID);
			int nTime = DateTime.Now.Subtract(dtStart).Milliseconds;

			this.AssertEqual(dtoAuto.AutoID, nAutoID, "Query was not correct");
			this.AssertNotNull(dtoAuto.Brand, "Query was not correct. Brand should not be null");
			this.AssertNull(dtoAuto.Series, "Query was not correct. Series should be null");
		}

		[TestCase]
		public void GetObjectUsingGenericDTO()
		{
			GenericTableMapper oTableMapper = new GenericTableMapper("Auto");

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			int nAutoID = 2;
			GenericDTO dtoAuto = null;
			DateTime dtStart = DateTime.Now;
			dtoAuto = oTableMapper.GetObject(nAutoID, "AutoID", "Brand", "Age", "Series");
			int nTime = DateTime.Now.Subtract(dtStart).Milliseconds;

			this.AssertTrue((int)dtoAuto["AutoID"] == nAutoID, "Query was not correct");
			this.AssertTrue(!dtoAuto.IsNull("Brand"), "Query was not correct. Brand should not be null");
			this.AssertTrue(dtoAuto.IsNull("Series"), "Query was not correct. Series should be null");
		}

		[TestCase]
		public void GetObjectsUsingDTO()
		{
			TableMapper<AutoDTO> oTableMapper = new TableMapper<AutoDTO>();

			DataAccessContext oDACtx = DataAccessContext.GetDataAccessContext();

			oDACtx.SetCurrentDataSource("012d80be-8b2c-4904-b4f1-1637d6c91a3c", "Test");

			AutoDTO[] arrAuto;
			DateTime dtStart = DateTime.Now;
			arrAuto = oTableMapper.GetObjects(new SqlExpression("1=1"), new Order(new SqlExpression("AutoID"), SortType.Ascending), 0, 10);
			double dblTime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;

			this.AssertTrue(arrAuto.Length == 10, "Query was not correct");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;

using Gurucore.FrameworkTest.TableMapper;

namespace Gurucore.Framework.DataAccess.Persistence
{
	public class AutoDTO_DTOMaker : DTOMakerBase
	{
		public AutoDTO_DTOMaker()
		{
		}

		public override T[] GetDTO<T>(IDataReader p_oReader)
		{
			List<T> arrDTO = new List<T>();

			while (p_oReader.Read())
			{
				AutoDTO oDTO = new AutoDTO();

				oDTO.AutoID = (Int32)p_oReader["AutoID"];
				oDTO.Brand = (String)p_oReader["Brand"];
				oDTO.Age = (Int32)p_oReader["Age"];

				arrDTO.Add((T)Convert.ChangeType(oDTO, typeof(T)));
			}
			return arrDTO.ToArray();
		}
	}
}

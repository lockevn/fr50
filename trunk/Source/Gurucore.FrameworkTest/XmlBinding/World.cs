using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.FrameworkTest.XmlBinding
{
	[XmlClass]
	public class World
	{
		private List<Country> m_arrCountry;

		[XmlSubSequence]
		public List<Country> Country
		{
			get { return m_arrCountry; }
			set { m_arrCountry = value; }
		}

		/*private List<int> m_arrImportantYear;

		public List<int> ImportantYear
		{
			get { return m_arrImportantYear; }
			set { m_arrImportantYear = value; }
		}*/

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core.XmlBinding;

namespace Gurucore.FrameworkTest.XmlBinding
{
	[XmClass]
	public class World
	{
		private List<Country> m_arrCountry;

		[XmlSubSequence]
		public List<Country> Country
		{
			get { return m_arrCountry; }
			set { m_arrCountry = value; }
		}

	}
}

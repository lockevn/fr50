﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.DataAccess;

namespace Gurucore.FrameworkTest.TableMapper
{
	[MappedTable]
	public class Auto
	{
		public const string AUTO_ID = "AutoId";
		public const string BRAND = "Brand";
		public const string AGE = "Age";

		private int? m_nAutoID;

		[MappedColumn]
		public int? AutoID
		{
			get { return m_nAutoID; }
			set { m_nAutoID = value; }
		}

		private string m_sBrand;

		[MappedColumn]
		public string Brand
		{
			get { return m_sBrand; }
			set { m_sBrand = value; }
		}

		private int? m_nAge;

		[MappedColumn]
		public int? Age
		{
			get { return m_nAge; }
			set { m_nAge = value; }
		}

		private string m_sSeries;

		[MappedColumn]
		public string Series
		{
			get { return m_sSeries; }
			set { m_sSeries = value; }
		}
	}
}

﻿using System;

using Gurucore.Framework.DataAccess;

namespace Gurucore.FrameworkTest.TableMapper
{
	[MappedTable]
	public class AutoDTO : DTOBase
	{
		public const string AUTO_ID = "AutoID";
		public const string BRAND = "Brand";
		public const string AGE = "Age";
		public const string SERIES = "Series";
		public const string CYLINDER = "Cylinder";
		public const string EXPIRE_DATE = "ExpireDate";
		public const string IS_LUXURY = "IsLuxury";

		private int m_nAutoID;

		[MappedColumn(Identity = true)]
		public int AutoID
		{
			get { return m_nAutoID; }
			set 
			{
				this.SetNotNull(AutoDTO.AUTO_ID);
				m_nAutoID = value; 
			}
		}

		private string m_sBrand;

		[MappedColumn]
		public string Brand
		{
			get { return m_sBrand; }
			set 
			{
				this.SetNotNull(AutoDTO.BRAND);
				m_sBrand = value;
			}
		}

		private int m_nAge;

		[MappedColumn]
		public int Age
		{
			get { return m_nAge; }
			set 
			{
				this.SetNotNull(AutoDTO.AGE);
				m_nAge = value;
			}
		}

		private string m_sSeries;

		[MappedColumn]
		public string Series
		{
			get { return m_sSeries; }
			set
			{
				this.SetNotNull(AutoDTO.SERIES);
				m_sSeries = value;
			}
		}

		private double m_dblCylinder;

		[MappedColumn]
		public double Cylinder
		{
			get { return m_dblCylinder; }
			set 
			{
				this.SetNotNull(AutoDTO.CYLINDER);
				m_dblCylinder = value;
			}
		}

		private DateTime m_dtExpireDate;

		[MappedColumn]
		public DateTime ExpireDate
		{
			get { return m_dtExpireDate; }
			set 
			{
				this.SetNotNull(AutoDTO.EXPIRE_DATE);
				m_dtExpireDate = value;
			}
		}

		private bool m_bIsLuxury;

		[MappedColumn]
		public bool IsLuxury
		{
			get { return m_bIsLuxury; }
			set
			{
				this.SetNotNull(AutoDTO.IS_LUXURY);
				m_bIsLuxury = value;
			}
		}
	}
}

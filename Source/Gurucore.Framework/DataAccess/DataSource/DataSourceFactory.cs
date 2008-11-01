using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.Factory;

namespace Gurucore.Framework.DataAccess.DataSource
{
	public class DataSourceFactory : FactoryBase<DataSource>
	{
		public DataSourceFactory()
		{
		}

		public DataSourceFactory(string p_sDescriptorFile)
			: base(p_sDescriptorFile, "caller_assembly_id")
		{			
		}

		public string GetProvider(string p_sDataSource)
		{
			if (null == p_sDataSource)
			{
				p_sDataSource = this.GetFactoryAttribute("default");
			}
			DataSource oDSInfo = (DataSource)this.GetItem(p_sDataSource);
			return oDSInfo.Provider;
		}

		public string GetTablePrefix(string p_sDataSource)
		{
			if (null == p_sDataSource)
			{
				p_sDataSource = this.GetFactoryAttribute("default");
			}
			DataSource oDSInfo = (DataSource)this.GetItem(p_sDataSource);
			return oDSInfo.TablePrefix;
		}

		public string GetConnectionString(string p_sDataSource)
		{
			if (null == p_sDataSource)
			{
				p_sDataSource = this.GetFactoryAttribute("default");
			}
			DataSource oDSInfo = (DataSource)this.GetItem(p_sDataSource);
			string sConnStr = string.Empty;
			foreach (KeyValuePair<string, ConnectionInfo> oEntry in oDSInfo.ConnectionInfo)
			{
				if (oEntry.Value.Encrypted)
				{
					sConnStr += oEntry.Key + "=" + oEntry.Value.Value.Decrypt() + ";";
				}
				else
				{
					sConnStr += oEntry.Key + "=" + oEntry.Value.Value + ";";
				}
			}

			return sConnStr;
		}
	}
}

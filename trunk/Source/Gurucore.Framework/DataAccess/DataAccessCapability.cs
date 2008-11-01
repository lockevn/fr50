using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.Activation;
using Gurucore.Framework.Core.Configuration;
using Gurucore.Framework.DataAccess.DataSource;
using Gurucore.Framework.DataAccess.DataProvider;
using Gurucore.Framework.DataAccess.Persistence;

namespace Gurucore.Framework.DataAccess
{
	public class DataAccessCapability : CapabilityBase
	{
		public override void Initialize(params object[] p_arrParam)
		{
			string sRootDir = Application.GetInstance().WorkingDirectory;
			string sConfigDir = sRootDir + Path.DirectorySeparatorChar + "Config";

			DataAccessConfiguration oDACfg = Application.GetInstance().GetGlobalSharedObject<DataAccessConfiguration>();

			//create table info manager
			TableInfoManager oTableInfoMgr = new TableInfoManager();
			Application.GetInstance().AddGlobalSharedObject(oTableInfoMgr);

			//load Sql generator
			SqlGeneratorBase oSqlGenerator;
			if (oDACfg.SqlGeneratorAssembly.NullOrEmpty())
			{
				oSqlGenerator = new ReflectionSqlGenerator();
				Application.GetInstance().AddGlobalSharedObject(SqlGeneratorBase.CACHE_KEY, oSqlGenerator);
			}
			else
			{
				DynamicActivator oDynActivator = Application.GetInstance().GetGlobalSharedObject<DynamicActivator>();
				oSqlGenerator = (SqlGeneratorBase)(oDynActivator.GetObject(oDACfg.SqlGeneratorAssembly, oDACfg.SqlGeneratorClass));
				Application.GetInstance().AddGlobalSharedObject(SqlGeneratorBase.CACHE_KEY, oSqlGenerator);
			}

			//load dataprovider factory
			DataProviderFactory oDPFactory = new DataProviderFactory(
				sConfigDir + Path.DirectorySeparatorChar + "System" + Path.DirectorySeparatorChar + "DataProvider.xml");
			Application.GetInstance().AddGlobalSharedObject(oDPFactory);

			//load system's datasource factory
			DataSourceFactory oDSFactory = new DataSourceFactory(
				sConfigDir + Path.DirectorySeparatorChar + "System" + Path.DirectorySeparatorChar + "DataSource.xml");
			//load all module's datasource factory
			DirectoryInfo oDirInfo = new DirectoryInfo(sConfigDir);
			FileInfo[] arrFile = oDirInfo.GetFiles("DataSource.xml", SearchOption.AllDirectories);
			foreach (FileInfo oFile in arrFile)
			{
				if (!oFile.Directory.Name.EndsWith("System"))
				{
					DataSourceFactory oModuleDSFactory = new DataSourceFactory(oFile.FullName);
					oDSFactory.Merge(oModuleDSFactory);
				}
			}

			Application.GetInstance().AddGlobalSharedObject(oDSFactory);			
		}
	}
}

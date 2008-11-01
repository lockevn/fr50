using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.IO;

using Gurucore.Framework.Core.Configuration;
using Gurucore.Framework.Core.Util;
using Gurucore.Framework.Core.JITGeneration;
using Gurucore.Framework.Core.Activation;

namespace Gurucore.Framework.Core
{
	public sealed class Application
	{
		private const string THREAD_SHARED_OBJECTS_SLOT_NAME = "ThreadSharedObjectSlotName";
		private Dictionary<string, object> m_dicGlobalSharedObjects;
		
		private static Application m_oInstance;
		private static object m_oLockInstance = new object();

		private string m_sWorkingDirectory;

		public string WorkingDirectory
		{
			get { return m_sWorkingDirectory; }
			set { m_sWorkingDirectory = value; }
		}

		private Application()
		{
			m_dicGlobalSharedObjects = new Dictionary<string, object>();
		}

		public static Application GetInstance()
		{
			if (m_oInstance == null)
			{
				lock (m_oLockInstance)
				{
					m_oInstance = new Application();
				}
			}
			return m_oInstance;
		}

		public void AddGlobalSharedObject(string p_sName, object p_oObject)
		{
			lock (m_oLockInstance)
			{
				m_dicGlobalSharedObjects.Add(p_sName, p_oObject);
			}
		}

		public void AddGlobalSharedObject(object p_oObject)
		{
			this.AddGlobalSharedObject(p_oObject.GetType().FullName, p_oObject);
		}

		public object GetGlobalSharedObject(string p_sName)
		{
			if (m_dicGlobalSharedObjects.ContainsKey(p_sName))
			{
				return m_dicGlobalSharedObjects[p_sName];
			}
			else
			{
				return null;
			}
		}

		public T GetGlobalSharedObject<T>(string p_sName)
		{
			object oResult = this.GetGlobalSharedObject(p_sName);
			if (oResult != null)
			{
				return (T)oResult;
			}
			else
			{
				return default(T);
			}
		}

		public T GetGlobalSharedObject<T>()
		{
			return this.GetGlobalSharedObject<T>(typeof(T).FullName);
		}

		public void AddThreadSharedObject(string p_sName, object p_oObject)
		{
			LocalDataStoreSlot oSlot = Thread.GetNamedDataSlot(THREAD_SHARED_OBJECTS_SLOT_NAME);
			Dictionary<string, object> dicThreadSharedObjects;
			if (oSlot != null)
			{
				dicThreadSharedObjects = (Dictionary<string, object>)Thread.GetData(oSlot);
			}
			else
			{
				dicThreadSharedObjects = new Dictionary<string, object>();
				oSlot = Thread.AllocateNamedDataSlot(THREAD_SHARED_OBJECTS_SLOT_NAME);
				Thread.SetData(oSlot, dicThreadSharedObjects);
			}
			if (!dicThreadSharedObjects.ContainsKey(p_sName))
			{
				dicThreadSharedObjects.Add(p_sName, p_oObject);
			}
			else
			{
				dicThreadSharedObjects[p_sName] = p_oObject;
			}
		}

		public void AddThreadSharedObject(object p_oObject)
		{
			this.AddThreadSharedObject(p_oObject.GetType().FullName, p_oObject);
		}

		public object GetThreadSharedObject(string p_sName)
		{
			LocalDataStoreSlot oSlot = Thread.GetNamedDataSlot(THREAD_SHARED_OBJECTS_SLOT_NAME);
			if (oSlot == null)
			{
				oSlot = Thread.AllocateNamedDataSlot(THREAD_SHARED_OBJECTS_SLOT_NAME);
			}

			Dictionary<string, object> dicThreadSharedObjects = (Dictionary<string, object>)Thread.GetData(oSlot);
			if (dicThreadSharedObjects != null)
			{
				if (dicThreadSharedObjects.ContainsKey(p_sName))
				{
					return dicThreadSharedObjects[p_sName];
				}
				else
				{
					return null;
				}
			}
			else
			{
				dicThreadSharedObjects = new Dictionary<string, object>();
				Thread.SetData(oSlot, dicThreadSharedObjects);
				return null;
			}
		}

		public T GetThreadSharedObject<T>(string p_sName)
		{
			object oResult = this.GetThreadSharedObject(p_sName);
			if (oResult != null)
			{
				return (T)oResult;
			}
			else
			{
				return default(T);
			}
		}

		public T GetThreadSharedObject<T>()
		{
			return this.GetThreadSharedObject<T>(typeof(T).FullName);
		}

		public void Start(string p_sWorkingDirectory, params object[] p_arrParam)
		{
			const int URI_OFFSET = 8; // "file:///".Length;

			m_sWorkingDirectory = p_sWorkingDirectory;

			//create Dynamic Activator
			DynamicActivator oDynActivator = new DynamicActivator();
			this.AddGlobalSharedObject(oDynActivator);

			//create JITClassManager
			JITClassManager oJITClassManager = new JITClassManager();
			this.AddGlobalSharedObject(oJITClassManager);

			//load application config
			string sCodeBase = Assembly.GetExecutingAssembly().CodeBase;
			FileInfo oCodeBaseFile = new FileInfo(sCodeBase.Substring(URI_OFFSET, sCodeBase.Length - URI_OFFSET));
			string sConfigFilePath;

			//find config file in assembly directory
			DirectoryInfo oBinDir = new DirectoryInfo(oCodeBaseFile.Directory.FullName);
			DirectoryInfo oConfigDir = new DirectoryInfo(oCodeBaseFile.Directory.FullName + "/Config/System/");
			if (oConfigDir.Exists && (oConfigDir.GetFiles("Config.xml").Length > 0))
			{
			}
			else
			{
				//find config file in assembly directory 's parent
				oConfigDir = new DirectoryInfo(oCodeBaseFile.Directory.Parent.FullName + "/Config/System/");
				if (oConfigDir.Exists && (oConfigDir.GetFiles("Config.xml").Length > 0))
				{
				}
				else
				{
					//find config file in assembly directory 's parent 's parent
					oConfigDir = new DirectoryInfo(oCodeBaseFile.Directory.Parent.Parent.FullName + "/Config/System/");
					if (oConfigDir.Exists && (oConfigDir.GetFiles("Config.xml").Length > 0))
					{

					}
					else
					{
						//find config file in working directory
						oConfigDir = new DirectoryInfo("./Config/System/");
						if (oConfigDir.Exists && (oConfigDir.GetFiles("Config.xml").Length == 0))
						{
							//fatal
							throw new Exception("GURUCORE Framework error: could not find application config file");
						}
					}
				}
			}

			sConfigFilePath = oConfigDir.GetFiles("Config.xml")[0].FullName;
			ConfigurationLoader oConfigLoader = new ConfigurationLoader(sConfigFilePath);
			List<ConfigurationBase> arrConfig = oConfigLoader.Load();
			foreach (ConfigurationBase oConfig in arrConfig)
			{
				m_oInstance.AddGlobalSharedObject(oConfig);
			}

			//initialize capability
			SystemConfiguration oSysCfg = this.GetGlobalSharedObject<SystemConfiguration>();
			foreach (KeyValuePair<string, Capability> oItem in oSysCfg.Capability)
			{
				CapabilityBase oCapability = (CapabilityBase)oDynActivator.GetObject(oItem.Value.Assembly, oItem.Value.Class);
				oCapability.Initialize();
			}
		}

		public void Stop()
		{
		}
	}
}

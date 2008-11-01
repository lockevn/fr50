using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

using Gurucore.Framework.Core.Util;

namespace Gurucore.Framework.Core.Activation
{
	public class DynamicActivator
	{
		private const int URI_OFFSET = 8; //"file:///".Length;

		private Dictionary<string, Assembly> m_dicAssembly;

		public DynamicActivator()
		{
			m_dicAssembly = new Dictionary<string, Assembly>();
		}

		public Assembly GetAssembly(string p_sAssembly)
		{
			if (p_sAssembly.NullOrEmpty())
			{
				p_sAssembly = this.GetEntryAssemblyPath();
			}

			Assembly oAssembly = null;

			//check if given full path
			if (!Path.IsPathRooted(p_sAssembly))
			{
				//first, find the assembly
				string sCLRFolder = RuntimeEnvironment.GetRuntimeDirectory();
				FileInfo oCurrentAssemblyFile = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(URI_OFFSET));
				string sCurrentFolder = oCurrentAssemblyFile.DirectoryName + Path.DirectorySeparatorChar;

				DirectoryInfo oDirInfo;
				oDirInfo = new DirectoryInfo(sCurrentFolder);
				if (oDirInfo.GetFiles(p_sAssembly).Length == 0)
				{
					oDirInfo = new DirectoryInfo(sCLRFolder);
					if (oDirInfo.GetFiles(p_sAssembly).Length == 0)
					{
						throw new Exception();
					}
					else
					{
						p_sAssembly = sCLRFolder + p_sAssembly;
					}
				}
				else
				{
					p_sAssembly = sCurrentFolder + p_sAssembly;
				}
			}
			if (m_dicAssembly.ContainsKey(p_sAssembly))
			{
				oAssembly = m_dicAssembly[p_sAssembly];
			}
			else
			{
				oAssembly = Assembly.LoadFrom(p_sAssembly);
				m_dicAssembly.Add(p_sAssembly, oAssembly);
			}

			return oAssembly;
		}

		public Type GetType(string p_sAssembly, string p_sTypeName)
		{
			Assembly oAssembly = this.GetAssembly(p_sAssembly);
			Type oType = oAssembly.GetType(p_sTypeName);
			return oType;
		}

		public Type GetType(string p_sTypeName)
		{
			return this.GetType(string.Empty, p_sTypeName);
		}

		public object GetObject(string p_sAssembly, string p_sTypeName, params object[] p_arrParam)
		{
			Type oType = this.GetType(p_sAssembly, p_sTypeName);
			return Activator.CreateInstance(oType, p_arrParam);
		}

		public string GetEntryAssemblyPath()
		{
			return Assembly.GetEntryAssembly().CodeBase.Substring(URI_OFFSET);
		}

		public string GetExecutingAssemblyPath()
		{
			return Assembly.GetExecutingAssembly().CodeBase.Substring(URI_OFFSET);
		}

		public string GetCallingAssemblyPath()
		{
			return Assembly.GetCallingAssembly().CodeBase.Substring(URI_OFFSET);
		}
	}
}

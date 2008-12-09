using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

using Microsoft.CSharp;

namespace Gurucore.Framework.Core.JITGeneration
{
	public class JITClassManager
	{
		private Dictionary<string, Type> m_dicClass;

		public JITClassManager()
		{
			m_dicClass = new Dictionary<string, Type>();
		}

		public bool IsRegistered(string p_sClassName)
		{
			return m_dicClass.ContainsKey(p_sClassName);
		}

		public Type GetRegisteredClass(string p_sClassName, string p_sCode, bool p_bReferAllLoadedAssembly)
		{
			if (m_dicClass.ContainsKey(p_sClassName))
			{
				return m_dicClass[p_sClassName];
			}
			else
			{
				lock (this)
				{
					if (m_dicClass.ContainsKey(p_sClassName))
					{
						return m_dicClass[p_sClassName];
					}
					else
					{
						CSharpCodeProvider oProvider = new CSharpCodeProvider();

						CompilerParameters oParameters = new CompilerParameters();
						oParameters.GenerateInMemory = true;

						const int URI_OFFSET = 8;// "file:///".Length;

						if (p_bReferAllLoadedAssembly)
						{
							Assembly[] arrLoadedAssembly = AppDomain.CurrentDomain.GetAssemblies();
							foreach (Assembly oReferAssembly in arrLoadedAssembly)
							{
								oParameters.ReferencedAssemblies.Add(oReferAssembly.CodeBase.Substring(URI_OFFSET));

							}
						}
						else
						{
							//refer to System assembly
							oParameters.ReferencedAssemblies.Add("System.dll");

							//refer to Executing assembly (Gurucore.Framework)
							oParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().CodeBase.Substring(URI_OFFSET));

							//refer to Calling assembly
							oParameters.ReferencedAssemblies.Add(Assembly.GetCallingAssembly().CodeBase.Substring(URI_OFFSET));

							//refer to Entry assembly
							oParameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().CodeBase.Substring(URI_OFFSET));

							oParameters.ReferencedAssemblies.Add(this.GetType().Assembly.CodeBase.Substring(URI_OFFSET));
						}

						oParameters.GenerateInMemory = false;

						CompilerResults oResults = oProvider.CompileAssemblyFromSource(oParameters, p_sCode);
						Assembly oAssembly = oResults.CompiledAssembly;
						Type oType = oAssembly.GetType(p_sClassName);

						m_dicClass.Add(p_sClassName, oType);

						return oType;
					}
				}
			}
		}
	}
}

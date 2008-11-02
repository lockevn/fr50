using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

using Gurucore.Framework.Core;
using Gurucore.Framework.Core.Activation;
using Gurucore.Framework.Test;

namespace Gurucore.FrameworkTest
{
	public class Program
	{
		[DllImport("kernel32.dll")]
		public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(uint nStdHandle);

		[StructLayout(LayoutKind.Sequential)]
		private struct COORD
		{
			public short X;
			public short Y;
		}
		[StructLayout(LayoutKind.Sequential)]
		private struct SMALL_RECT
		{
			public short Left;
			public short Top;
			public short Right;
			public short Bottom;
		}
		[StructLayout(LayoutKind.Sequential)]
		private struct CONSOLE_SCREEN_BUFFER_INFO
		{
			public COORD Size;
			public COORD CursorPosition;
			public short Attributes;
			public SMALL_RECT Window;
			public COORD MaximumWindowSize;
		}

		[DllImport("kernel32.dll")]
		private static extern bool SetConsoleDisplayMode(IntPtr hConsole, int mode);
	
		[DllImport("kernel32.dll")]
		private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsole, out CONSOLE_SCREEN_BUFFER_INFO info);
		
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleWindowInfo(IntPtr hConsole, bool absolute, ref SMALL_RECT rect);

		static void Main(string[] args)
		{
			/*string[] arrColumn = { "AutoID", "Brand", "Age", "Series", "ExpireDate", "Cylinder", "IsLuxury"};
			List<string> lstColumn = new List<string>();

			DateTime dtStart = DateTime.Now;
			for (int i = 0; i < 100000; i++)
			{
				lstColumn.Contains("Series");
			}
			double dblTime = DateTime.Now.Subtract(dtStart).TotalMilliseconds;*/

			Application.GetInstance().Start(Environment.CurrentDirectory);

			TestConfiguration oTestConfig = Application.GetInstance().GetGlobalSharedObject<TestConfiguration>();

			DynamicActivator oDynActivator = Application.GetInstance().GetGlobalSharedObject<DynamicActivator>();

			uint STD_OUTPUT_HANDLE = 0xfffffff5;
			IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
			CONSOLE_SCREEN_BUFFER_INFO info;
			GetConsoleScreenBufferInfo(hConsole, out info);
			
			SMALL_RECT rc;
			rc.Left = rc.Top = 0;
			rc.Right = (short)(Math.Min(info.MaximumWindowSize.X, info.Size.X) - 1);
			rc.Bottom = (short)(Math.Min(info.MaximumWindowSize.Y, info.Size.Y) - 1);
			SetConsoleWindowInfo(hConsole, true, ref rc);
			SetConsoleDisplayMode(hConsole, 1);
			
			SetConsoleTextAttribute(hConsole, 15);
	
			Console.WriteLine("Gurucore Framework v5.0 Console Test Runner\r\n");
			foreach (string sTestTarget in oTestConfig.TestTarget)
			{
				Assembly oAssembly = oDynActivator.GetAssembly(sTestTarget);

				Type[] arrType = oAssembly.GetTypes();
				foreach (Type oType in arrType)
				{
					if (oType.BaseType == typeof(TestSuiteBase))
					{
						Console.WriteLine();
						SetConsoleTextAttribute(hConsole, 240);
						Console.Write(">>>");
						SetConsoleTextAttribute(hConsole, 15);
						Console.WriteLine(" " + oType.Name + " ...\r\n");
						TestSuiteBase oTestSuite = (TestSuiteBase)Activator.CreateInstance(oType);
						TestRunner oTestRunner = new TestRunner(oTestSuite);

						foreach (TestResult oTestResult in oTestRunner.Run())
						{
							if (oTestResult.Pass)
							{
								SetConsoleTextAttribute(hConsole, 10);
								Console.Write("[PASS]");
								SetConsoleTextAttribute(hConsole, 15);
								Console.Write(" " + oTestResult.TestMethod.Name + " ");
								if (oTestResult.Runtime > 0.0)
								{
									SetConsoleTextAttribute(hConsole, 14);
									Console.Write("[" + oTestResult.Runtime + "(ms)]");
									SetConsoleTextAttribute(hConsole, 15);
								}
								Console.WriteLine();
								Console.WriteLine();
							}
							else
							{
								if (oTestResult.Fatal)
								{
									SetConsoleTextAttribute(hConsole, 206);
									Console.Write("[FATAL EXCEPTION]");
									SetConsoleTextAttribute(hConsole, 15);
									Console.Write(" " + oTestResult.TestMethod.Name + " ");
									Console.WriteLine();
									SetConsoleTextAttribute(hConsole, 8);
									Console.WriteLine(oTestResult.Message);
									SetConsoleTextAttribute(hConsole, 15);
									Console.WriteLine();
								}
								else
								{
									SetConsoleTextAttribute(hConsole, 12);
									Console.Write("[FAIL]");
									SetConsoleTextAttribute(hConsole, 15);
									Console.Write(" " + oTestResult.TestMethod.Name + " ");
									Console.WriteLine();
									SetConsoleTextAttribute(hConsole, 8);
									Console.WriteLine(oTestResult.Message);
									SetConsoleTextAttribute(hConsole, 15);
									Console.WriteLine();
								}
							}
						}
						Console.WriteLine("------------------------------------------------------------------");
					}
				}
			}
			Console.WriteLine("\r\nPress <ENTER> to return...");
			Console.ReadLine();

			Application.GetInstance().Stop();
		}
	}
}

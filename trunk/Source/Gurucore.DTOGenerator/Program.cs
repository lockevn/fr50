using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gurucore.DTOGenerator
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Gurucore.Framework.Core.Application.GetInstance().Start(Environment.CurrentDirectory);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

			Gurucore.Framework.Core.Application.GetInstance().Stop();
		}
	}
}

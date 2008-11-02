using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Gurucore.DTOGenerator.DTO;
using Gurucore.DTOGenerator.Business;

namespace Gurucore.DTOGenerator
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			SchemaService svcSchema = new SchemaService();
			string[] arrDataSource = svcSchema.GetAllDataSource();

			foreach (string sDataSource in arrDataSource)
			{
				TreeNode oDataSourceNode = new TreeNode(sDataSource);
				tvwDBObject.Nodes.Add(oDataSourceNode);

				Table[] arrTable = svcSchema.GetAllTable(sDataSource, true);
				string sTablePrefix = svcSchema.GetTablePrefix(sDataSource);
				foreach (Table dtoTable in arrTable)
				{
					string sTableName = dtoTable.TableName;
					if (sTableName.StartsWith(sTablePrefix))
					{
						sTableName = sTableName.Substring(sTablePrefix.Length);
					}
					TreeNode oTableNode = new TreeNode(sTableName);
					oTableNode.Tag = dtoTable;
					oDataSourceNode.Nodes.Add(oTableNode);
				}
			}

			tvwDBObject.ExpandAll();
		}
	}
}

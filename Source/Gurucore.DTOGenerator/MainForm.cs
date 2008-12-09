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
		private Dictionary<string, TabPage> m_dicTabPages;
		private Dictionary<string, CodeViewer> m_dicCodeViewers;

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
				oDataSourceNode.ImageIndex = oDataSourceNode.SelectedImageIndex = 2;
				tvwDBObject.Nodes.Add(oDataSourceNode);

				ClassInfo[] arrObjects = svcSchema.GetAllObjects(sDataSource, true);
				foreach (ClassInfo oObject in arrObjects)
				{
					TreeNode oTableNode = new TreeNode(oObject.TableName);
					oTableNode.Tag = oObject;
					oTableNode.ImageIndex = oTableNode.SelectedImageIndex = 7;
					oDataSourceNode.Nodes.Add(oTableNode);
				}
			}

			tvwDBObject.ExpandAll();
			cboTemplate.SelectedIndex = 1;

			m_dicTabPages = new Dictionary<string, TabPage>();
			m_dicCodeViewers = new Dictionary<string, CodeViewer>();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{

		}

		private void tvwDBObject_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if ((e.Node.Tag != null) && (e.Node.Tag is ClassInfo))
			{
				GenerateCode(e.Node);
			}
			else
			{
				foreach (TreeNode oNode in e.Node.Nodes)
				{
					GenerateCode(oNode);
				}
			}
		}

		private void GenerateCode(TreeNode p_oNode)
		{
			TemplateService svcTemplate = new TemplateService();
			string sWorkingDirectory = Gurucore.Framework.Core.Application.GetInstance().WorkingDirectory;
			string sGeneratedCode = string.Empty;
			if (cboTemplate.SelectedIndex == 0)
			{
				sGeneratedCode = svcTemplate.TemplateWeave(sWorkingDirectory + @"\Template\DTO.tpl", (ClassInfo)p_oNode.Tag);
			}
			else
			{
				sGeneratedCode = svcTemplate.TemplateWeave(sWorkingDirectory + @"\Template\PONO.tpl", (ClassInfo)p_oNode.Tag);
			}

			DisplayGeneratedCode(((ClassInfo)p_oNode.Tag).TableName, sGeneratedCode);
		}

		private void DisplayGeneratedCode(string p_sTableName, string p_sGeneratedCode)
		{
			if (m_dicTabPages.ContainsKey(p_sTableName))
			{
				tabGeneratedCode.SelectedTab = m_dicTabPages[p_sTableName];
				m_dicCodeViewers[p_sTableName].Text = p_sGeneratedCode;
			}
			else
			{
				CodeViewer oCodeViewer = new CodeViewer();
				oCodeViewer.Code = p_sGeneratedCode;

				TabPage oTabPage = new TabPage();
				oTabPage.Text = p_sTableName + ".cs";
				oTabPage.Controls.Add(oCodeViewer);
				oCodeViewer.Dock = DockStyle.Fill;
				tabGeneratedCode.TabPages.Add(oTabPage);
				oTabPage.Select();
				
				m_dicTabPages.Add(p_sTableName, oTabPage);
				m_dicCodeViewers.Add(p_sTableName, oCodeViewer);

			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			DTOGeneratorConfiguration oDTOGenCfg = Gurucore.Framework.Core.Application.GetInstance().GetGlobalSharedObject<DTOGeneratorConfiguration>();
			foreach (KeyValuePair<string, CodeViewer> oEntry in m_dicCodeViewers)
			{
				string sFileName = oDTOGenCfg.SaveTo + System.IO.Path.DirectorySeparatorChar + m_dicTabPages[oEntry.Key].Text;
				string sCode = oEntry.Value.Code;
				System.IO.StreamWriter oWriter = new System.IO.StreamWriter(sFileName);
				oWriter.Write(sCode);
				oWriter.Close();
			}
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			Options frmOption = new Options();
			frmOption.ShowDialog(this);
		}
	}
}

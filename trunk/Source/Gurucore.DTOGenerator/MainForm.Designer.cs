namespace Gurucore.DTOGenerator
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mnuMainMenu = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.tbrMain = new System.Windows.Forms.ToolStrip();
			this.btnGenerate = new System.Windows.Forms.ToolStripButton();
			this.sbrMain = new System.Windows.Forms.StatusStrip();
			this.pnlMain = new System.Windows.Forms.SplitContainer();
			this.tvwDBObject = new System.Windows.Forms.TreeView();
			this.tabGeneratedCode = new System.Windows.Forms.TabControl();
			this.mnuMainMenu.SuspendLayout();
			this.tbrMain.SuspendLayout();
			this.pnlMain.Panel1.SuspendLayout();
			this.pnlMain.Panel2.SuspendLayout();
			this.pnlMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMainMenu
			// 
			this.mnuMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuHelp});
			this.mnuMainMenu.Location = new System.Drawing.Point(0, 0);
			this.mnuMainMenu.Name = "mnuMainMenu";
			this.mnuMainMenu.Size = new System.Drawing.Size(792, 24);
			this.mnuMainMenu.TabIndex = 1;
			this.mnuMainMenu.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(35, 20);
			this.mnuFile.Text = "&File";
			// 
			// mnuExit
			// 
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(92, 22);
			this.mnuExit.Text = "E&xit";
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(40, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(103, 22);
			this.mnuAbout.Text = "About";
			// 
			// tbrMain
			// 
			this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGenerate});
			this.tbrMain.Location = new System.Drawing.Point(0, 24);
			this.tbrMain.Name = "tbrMain";
			this.tbrMain.Size = new System.Drawing.Size(792, 25);
			this.tbrMain.TabIndex = 2;
			this.tbrMain.Text = "toolStrip1";
			// 
			// btnGenerate
			// 
			this.btnGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
			this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(23, 22);
			this.btnGenerate.Text = "toolStripButton1";
			// 
			// sbrMain
			// 
			this.sbrMain.Location = new System.Drawing.Point(0, 551);
			this.sbrMain.Name = "sbrMain";
			this.sbrMain.Size = new System.Drawing.Size(792, 22);
			this.sbrMain.TabIndex = 3;
			this.sbrMain.Text = "statusStrip1";
			// 
			// pnlMain
			// 
			this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMain.Location = new System.Drawing.Point(0, 49);
			this.pnlMain.Name = "pnlMain";
			// 
			// pnlMain.Panel1
			// 
			this.pnlMain.Panel1.Controls.Add(this.tvwDBObject);
			// 
			// pnlMain.Panel2
			// 
			this.pnlMain.Panel2.Controls.Add(this.tabGeneratedCode);
			this.pnlMain.Size = new System.Drawing.Size(792, 502);
			this.pnlMain.SplitterDistance = 196;
			this.pnlMain.TabIndex = 4;
			// 
			// tvwDBObject
			// 
			this.tvwDBObject.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwDBObject.Location = new System.Drawing.Point(0, 0);
			this.tvwDBObject.Name = "tvwDBObject";
			this.tvwDBObject.Size = new System.Drawing.Size(196, 502);
			this.tvwDBObject.TabIndex = 0;
			// 
			// tabGeneratedCode
			// 
			this.tabGeneratedCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabGeneratedCode.Location = new System.Drawing.Point(0, 0);
			this.tabGeneratedCode.Name = "tabGeneratedCode";
			this.tabGeneratedCode.SelectedIndex = 0;
			this.tabGeneratedCode.Size = new System.Drawing.Size(592, 502);
			this.tabGeneratedCode.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(792, 573);
			this.Controls.Add(this.pnlMain);
			this.Controls.Add(this.sbrMain);
			this.Controls.Add(this.tbrMain);
			this.Controls.Add(this.mnuMainMenu);
			this.MainMenuStrip = this.mnuMainMenu;
			this.Name = "MainForm";
			this.Text = "Gurucore DTO Generator (version 5.0)";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mnuMainMenu.ResumeLayout(false);
			this.mnuMainMenu.PerformLayout();
			this.tbrMain.ResumeLayout(false);
			this.tbrMain.PerformLayout();
			this.pnlMain.Panel1.ResumeLayout(false);
			this.pnlMain.Panel2.ResumeLayout(false);
			this.pnlMain.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mnuMainMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStrip tbrMain;
		private System.Windows.Forms.StatusStrip sbrMain;
		private System.Windows.Forms.SplitContainer pnlMain;
		private System.Windows.Forms.TreeView tvwDBObject;
		private System.Windows.Forms.TabControl tabGeneratedCode;
		private System.Windows.Forms.ToolStripButton btnGenerate;
	}
}


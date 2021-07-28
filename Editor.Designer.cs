namespace AMMEdit
{
    partial class Editor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.openAMMFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.currentAMMFile = new System.Windows.Forms.Label();
            this.rawBinaryOutput = new System.Windows.Forms.TextBox();
            this.saveAMMFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonEditProps = new System.Windows.Forms.Button();
            this.openDATFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAM1AXSFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAM2ANIFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAXSFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openANIDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mapFileBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapFileBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(49, 60);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(322, 511);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // openAMMFileDialog
            // 
            this.openAMMFileDialog.DefaultExt = "amm";
            this.openAMMFileDialog.FileName = "openFileDialog1";
            this.openAMMFileDialog.Filter = "Army Men 1 Maps|*.amm";
            // 
            // currentAMMFile
            // 
            this.currentAMMFile.AutoSize = true;
            this.currentAMMFile.Location = new System.Drawing.Point(46, 587);
            this.currentAMMFile.Name = "currentAMMFile";
            this.currentAMMFile.Size = new System.Drawing.Size(131, 13);
            this.currentAMMFile.TabIndex = 2;
            this.currentAMMFile.Text = "Select a AMM file to begin";
            // 
            // rawBinaryOutput
            // 
            this.rawBinaryOutput.CausesValidation = false;
            this.rawBinaryOutput.Location = new System.Drawing.Point(491, 48);
            this.rawBinaryOutput.MaxLength = 99999999;
            this.rawBinaryOutput.Multiline = true;
            this.rawBinaryOutput.Name = "rawBinaryOutput";
            this.rawBinaryOutput.ReadOnly = true;
            this.rawBinaryOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawBinaryOutput.Size = new System.Drawing.Size(494, 583);
            this.rawBinaryOutput.TabIndex = 3;
            // 
            // saveAMMFileDialog
            // 
            this.saveAMMFileDialog.DefaultExt = "AMM";
            this.saveAMMFileDialog.Filter = "Army Men 1 Maps|*.amm";
            // 
            // buttonEditProps
            // 
            this.buttonEditProps.Enabled = false;
            this.buttonEditProps.Location = new System.Drawing.Point(377, 164);
            this.buttonEditProps.Name = "buttonEditProps";
            this.buttonEditProps.Size = new System.Drawing.Size(108, 23);
            this.buttonEditProps.TabIndex = 5;
            this.buttonEditProps.Text = "Edit properties";
            this.buttonEditProps.UseVisualStyleBackColor = true;
            this.buttonEditProps.Click += new System.EventHandler(this.button3_Click);
            // 
            // openDATFileDialog
            // 
            this.openDATFileDialog.DefaultExt = "dat";
            this.openDATFileDialog.Filter = "Army Men Data|*.dat";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(13, 633);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(27, 13);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Idle.";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(885, 633);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.helpToolStripMenuItem,
            this.objectsToolStripMenuItem,
            this.animationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(997, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openToolStripMenuItem.Text = "&Open AMM...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(185, 6);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Enabled = false;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // objectsToolStripMenuItem
            // 
            this.objectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.viewToolStripMenuItem});
            this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
            this.objectsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.objectsToolStripMenuItem.Text = "&Objects";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Enabled = false;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // animationsToolStripMenuItem
            // 
            this.animationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAM1AXSFileToolStripMenuItem,
            this.loadAM2ANIFileToolStripMenuItem});
            this.animationsToolStripMenuItem.Name = "animationsToolStripMenuItem";
            this.animationsToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.animationsToolStripMenuItem.Text = "&Animations";
            // 
            // loadAM1AXSFileToolStripMenuItem
            // 
            this.loadAM1AXSFileToolStripMenuItem.Name = "loadAM1AXSFileToolStripMenuItem";
            this.loadAM1AXSFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadAM1AXSFileToolStripMenuItem.Text = "Load AM1 AXS file...";
            this.loadAM1AXSFileToolStripMenuItem.Click += new System.EventHandler(this.loadAM1AXSFileToolStripMenuItem_Click);
            // 
            // loadAM2ANIFileToolStripMenuItem
            // 
            this.loadAM2ANIFileToolStripMenuItem.Name = "loadAM2ANIFileToolStripMenuItem";
            this.loadAM2ANIFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadAM2ANIFileToolStripMenuItem.Text = "Load AM2+ ANI file...";
            this.loadAM2ANIFileToolStripMenuItem.Click += new System.EventHandler(this.loadAM2ANIFileToolStripMenuItem_Click);
            // 
            // openAXSFileDialog
            // 
            this.openAXSFileDialog.DefaultExt = "axs";
            this.openAXSFileDialog.Filter = "Army Men 1 Animation|*.axs";
            // 
            // openANIDialog1
            // 
            this.openANIDialog1.DefaultExt = "ani";
            this.openANIDialog1.Filter = "Army Men 2 Animation|*.ani";
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem1.Text = "Load DAT file...";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // mapFileBindingSource
            // 
            this.mapFileBindingSource.DataSource = typeof(AMMEdit.amm.MapFile);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 658);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.buttonEditProps);
            this.Controls.Add(this.rawBinaryOutput);
            this.Controls.Add(this.currentAMMFile);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Editor";
            this.Text = "3DO Army Men Editor";
            this.Load += new System.EventHandler(this.Editor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapFileBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.BindingSource mapFileBindingSource;
        private System.Windows.Forms.OpenFileDialog openAMMFileDialog;
        private System.Windows.Forms.Label currentAMMFile;
        private System.Windows.Forms.TextBox rawBinaryOutput;
        private System.Windows.Forms.SaveFileDialog saveAMMFileDialog;
        private System.Windows.Forms.Button buttonEditProps;
        private System.Windows.Forms.OpenFileDialog openDATFileDialog;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAM1AXSFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openAXSFileDialog;
        private System.Windows.Forms.ToolStripMenuItem loadAM2ANIFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openANIDialog1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
    }
}


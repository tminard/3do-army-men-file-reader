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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openAMMFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.currentAMMFile = new System.Windows.Forms.Label();
            this.rawBinaryOutput = new System.Windows.Forms.TextBox();
            this.mapFileBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mapFileBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(49, 60);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(322, 446);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(386, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open AMM file...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.currentAMMFile.Location = new System.Drawing.Point(49, 26);
            this.currentAMMFile.Name = "currentAMMFile";
            this.currentAMMFile.Size = new System.Drawing.Size(131, 13);
            this.currentAMMFile.TabIndex = 2;
            this.currentAMMFile.Text = "Select a AMM file to begin";
            // 
            // rawBinaryOutput
            // 
            this.rawBinaryOutput.CausesValidation = false;
            this.rawBinaryOutput.Location = new System.Drawing.Point(491, 60);
            this.rawBinaryOutput.MaxLength = 99999999;
            this.rawBinaryOutput.Multiline = true;
            this.rawBinaryOutput.Name = "rawBinaryOutput";
            this.rawBinaryOutput.ReadOnly = true;
            this.rawBinaryOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rawBinaryOutput.Size = new System.Drawing.Size(794, 446);
            this.rawBinaryOutput.TabIndex = 3;
            // 
            // mapFileBindingSource
            // 
            this.mapFileBindingSource.DataSource = typeof(AMMEdit.amm.MapFile);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 662);
            this.Controls.Add(this.rawBinaryOutput);
            this.Controls.Add(this.currentAMMFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Name = "Editor";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mapFileBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.BindingSource mapFileBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openAMMFileDialog;
        private System.Windows.Forms.Label currentAMMFile;
        private System.Windows.Forms.TextBox rawBinaryOutput;
    }
}


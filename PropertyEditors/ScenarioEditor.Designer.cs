namespace AMMEdit.PropertyEditors
{
    partial class ScenarioEditor
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
            this.scenarioList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listGreenFractions = new System.Windows.Forms.ListBox();
            this.listTanFractions = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listGreyFractions = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBlueFractions = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioList
            // 
            this.scenarioList.FormattingEnabled = true;
            this.scenarioList.Location = new System.Drawing.Point(12, 12);
            this.scenarioList.Name = "scenarioList";
            this.scenarioList.Size = new System.Drawing.Size(194, 680);
            this.scenarioList.TabIndex = 0;
            this.scenarioList.SelectedIndexChanged += new System.EventHandler(this.scenarioList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(375, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scenario Name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(222, 12);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(147, 20);
            this.textBox1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listGreenFractions);
            this.groupBox1.Location = new System.Drawing.Point(222, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 311);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Green";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listTanFractions);
            this.groupBox2.Location = new System.Drawing.Point(575, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 311);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tan";
            // 
            // listGreenFractions
            // 
            this.listGreenFractions.FormattingEnabled = true;
            this.listGreenFractions.Location = new System.Drawing.Point(7, 20);
            this.listGreenFractions.Name = "listGreenFractions";
            this.listGreenFractions.Size = new System.Drawing.Size(301, 277);
            this.listGreenFractions.TabIndex = 0;
            // 
            // listTanFractions
            // 
            this.listTanFractions.FormattingEnabled = true;
            this.listTanFractions.Location = new System.Drawing.Point(7, 17);
            this.listTanFractions.Name = "listTanFractions";
            this.listTanFractions.Size = new System.Drawing.Size(301, 277);
            this.listTanFractions.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listGreyFractions);
            this.groupBox3.Location = new System.Drawing.Point(575, 371);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(314, 311);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Grey";
            // 
            // listGreyFractions
            // 
            this.listGreyFractions.FormattingEnabled = true;
            this.listGreyFractions.Location = new System.Drawing.Point(7, 17);
            this.listGreyFractions.Name = "listGreyFractions";
            this.listGreyFractions.Size = new System.Drawing.Size(301, 277);
            this.listGreyFractions.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBlueFractions);
            this.groupBox4.Location = new System.Drawing.Point(222, 371);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(314, 311);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Blue";
            // 
            // listBlueFractions
            // 
            this.listBlueFractions.FormattingEnabled = true;
            this.listBlueFractions.Location = new System.Drawing.Point(6, 17);
            this.listBlueFractions.Name = "listBlueFractions";
            this.listBlueFractions.Size = new System.Drawing.Size(301, 277);
            this.listBlueFractions.TabIndex = 0;
            // 
            // ScenarioEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(905, 709);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scenarioList);
            this.Name = "ScenarioEditor";
            this.Text = "Scenario Editor";
            this.Load += new System.EventHandler(this.ScenarioEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox scenarioList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listGreenFractions;
        private System.Windows.Forms.ListBox listTanFractions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listGreyFractions;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox listBlueFractions;
    }
}
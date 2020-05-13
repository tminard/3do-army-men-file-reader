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
            this.listGreenFractions = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listTanFractions = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listGreyFractions = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBlueFractions = new System.Windows.Forms.ListBox();
            this.propertyGridFractionUnit = new System.Windows.Forms.PropertyGrid();
            this.buttonAddGreenUnit = new System.Windows.Forms.Button();
            this.buttonRemoveGreenUnit = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRemoveTanUnit = new System.Windows.Forms.Button();
            this.buttonAddTanUnit = new System.Windows.Forms.Button();
            this.buttonRemoveBlueUnit = new System.Windows.Forms.Button();
            this.buttonAddBlueUnit = new System.Windows.Forms.Button();
            this.buttonRemoveGreyUnit = new System.Windows.Forms.Button();
            this.buttonAddGreyUnit = new System.Windows.Forms.Button();
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
            this.scenarioList.Size = new System.Drawing.Size(194, 173);
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
            this.groupBox1.Controls.Add(this.buttonRemoveGreenUnit);
            this.groupBox1.Controls.Add(this.buttonAddGreenUnit);
            this.groupBox1.Controls.Add(this.listGreenFractions);
            this.groupBox1.Location = new System.Drawing.Point(222, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 311);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Green";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // listGreenFractions
            // 
            this.listGreenFractions.FormattingEnabled = true;
            this.listGreenFractions.Location = new System.Drawing.Point(7, 20);
            this.listGreenFractions.Name = "listGreenFractions";
            this.listGreenFractions.Size = new System.Drawing.Size(301, 251);
            this.listGreenFractions.TabIndex = 0;
            this.listGreenFractions.SelectedIndexChanged += new System.EventHandler(this.listGreenFractions_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRemoveTanUnit);
            this.groupBox2.Controls.Add(this.buttonAddTanUnit);
            this.groupBox2.Controls.Add(this.listTanFractions);
            this.groupBox2.Location = new System.Drawing.Point(575, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 311);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tan";
            // 
            // listTanFractions
            // 
            this.listTanFractions.FormattingEnabled = true;
            this.listTanFractions.Location = new System.Drawing.Point(7, 17);
            this.listTanFractions.Name = "listTanFractions";
            this.listTanFractions.Size = new System.Drawing.Size(301, 251);
            this.listTanFractions.TabIndex = 1;
            this.listTanFractions.SelectedIndexChanged += new System.EventHandler(this.listTanFractions_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonRemoveGreyUnit);
            this.groupBox3.Controls.Add(this.listGreyFractions);
            this.groupBox3.Controls.Add(this.buttonAddGreyUnit);
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
            this.listGreyFractions.Size = new System.Drawing.Size(301, 251);
            this.listGreyFractions.TabIndex = 1;
            this.listGreyFractions.SelectedIndexChanged += new System.EventHandler(this.listGreyFractions_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonRemoveBlueUnit);
            this.groupBox4.Controls.Add(this.buttonAddBlueUnit);
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
            this.listBlueFractions.Size = new System.Drawing.Size(301, 251);
            this.listBlueFractions.TabIndex = 0;
            this.listBlueFractions.SelectedIndexChanged += new System.EventHandler(this.listBlueFractions_SelectedIndexChanged);
            // 
            // propertyGridFractionUnit
            // 
            this.propertyGridFractionUnit.Location = new System.Drawing.Point(895, 54);
            this.propertyGridFractionUnit.Name = "propertyGridFractionUnit";
            this.propertyGridFractionUnit.Size = new System.Drawing.Size(297, 628);
            this.propertyGridFractionUnit.TabIndex = 7;
            // 
            // buttonAddGreenUnit
            // 
            this.buttonAddGreenUnit.Location = new System.Drawing.Point(7, 278);
            this.buttonAddGreenUnit.Name = "buttonAddGreenUnit";
            this.buttonAddGreenUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonAddGreenUnit.TabIndex = 1;
            this.buttonAddGreenUnit.Text = "Add +";
            this.buttonAddGreenUnit.UseVisualStyleBackColor = true;
            this.buttonAddGreenUnit.Click += new System.EventHandler(this.buttonAddGreenUnit_Click);
            // 
            // buttonRemoveGreenUnit
            // 
            this.buttonRemoveGreenUnit.Enabled = false;
            this.buttonRemoveGreenUnit.Location = new System.Drawing.Point(232, 278);
            this.buttonRemoveGreenUnit.Name = "buttonRemoveGreenUnit";
            this.buttonRemoveGreenUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveGreenUnit.TabIndex = 2;
            this.buttonRemoveGreenUnit.Text = "Remove Unit";
            this.buttonRemoveGreenUnit.UseVisualStyleBackColor = true;
            this.buttonRemoveGreenUnit.Click += new System.EventHandler(this.buttonRemoveGreenUnit_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(1084, 688);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(108, 35);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Save and Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveTanUnit
            // 
            this.buttonRemoveTanUnit.Enabled = false;
            this.buttonRemoveTanUnit.Location = new System.Drawing.Point(232, 278);
            this.buttonRemoveTanUnit.Name = "buttonRemoveTanUnit";
            this.buttonRemoveTanUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveTanUnit.TabIndex = 4;
            this.buttonRemoveTanUnit.Text = "Remove Unit";
            this.buttonRemoveTanUnit.UseVisualStyleBackColor = true;
            this.buttonRemoveTanUnit.Click += new System.EventHandler(this.buttonRemoveTanUnit_Click);
            // 
            // buttonAddTanUnit
            // 
            this.buttonAddTanUnit.Location = new System.Drawing.Point(7, 278);
            this.buttonAddTanUnit.Name = "buttonAddTanUnit";
            this.buttonAddTanUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonAddTanUnit.TabIndex = 3;
            this.buttonAddTanUnit.Text = "Add +";
            this.buttonAddTanUnit.UseVisualStyleBackColor = true;
            this.buttonAddTanUnit.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonRemoveBlueUnit
            // 
            this.buttonRemoveBlueUnit.Enabled = false;
            this.buttonRemoveBlueUnit.Location = new System.Drawing.Point(232, 274);
            this.buttonRemoveBlueUnit.Name = "buttonRemoveBlueUnit";
            this.buttonRemoveBlueUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveBlueUnit.TabIndex = 4;
            this.buttonRemoveBlueUnit.Text = "Remove Unit";
            this.buttonRemoveBlueUnit.UseVisualStyleBackColor = true;
            this.buttonRemoveBlueUnit.Click += new System.EventHandler(this.buttonRemoveBlueUnit_Click);
            // 
            // buttonAddBlueUnit
            // 
            this.buttonAddBlueUnit.Location = new System.Drawing.Point(7, 274);
            this.buttonAddBlueUnit.Name = "buttonAddBlueUnit";
            this.buttonAddBlueUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonAddBlueUnit.TabIndex = 3;
            this.buttonAddBlueUnit.Text = "Add +";
            this.buttonAddBlueUnit.UseVisualStyleBackColor = true;
            this.buttonAddBlueUnit.Click += new System.EventHandler(this.buttonAddBlueUnit_Click);
            // 
            // buttonRemoveGreyUnit
            // 
            this.buttonRemoveGreyUnit.Enabled = false;
            this.buttonRemoveGreyUnit.Location = new System.Drawing.Point(232, 274);
            this.buttonRemoveGreyUnit.Name = "buttonRemoveGreyUnit";
            this.buttonRemoveGreyUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveGreyUnit.TabIndex = 10;
            this.buttonRemoveGreyUnit.Text = "Remove Unit";
            this.buttonRemoveGreyUnit.UseVisualStyleBackColor = true;
            this.buttonRemoveGreyUnit.Click += new System.EventHandler(this.buttonRemoveGreyUnit_Click);
            // 
            // buttonAddGreyUnit
            // 
            this.buttonAddGreyUnit.Location = new System.Drawing.Point(7, 274);
            this.buttonAddGreyUnit.Name = "buttonAddGreyUnit";
            this.buttonAddGreyUnit.Size = new System.Drawing.Size(75, 23);
            this.buttonAddGreyUnit.TabIndex = 9;
            this.buttonAddGreyUnit.Text = "Add +";
            this.buttonAddGreyUnit.UseVisualStyleBackColor = true;
            this.buttonAddGreyUnit.Click += new System.EventHandler(this.buttonAddGreyUnit_Click);
            // 
            // ScenarioEditor
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1215, 735);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.propertyGridFractionUnit);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scenarioList);
            this.MaximizeBox = false;
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
        private System.Windows.Forms.PropertyGrid propertyGridFractionUnit;
        private System.Windows.Forms.Button buttonAddGreenUnit;
        private System.Windows.Forms.Button buttonRemoveGreenUnit;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRemoveTanUnit;
        private System.Windows.Forms.Button buttonAddTanUnit;
        private System.Windows.Forms.Button buttonRemoveGreyUnit;
        private System.Windows.Forms.Button buttonAddGreyUnit;
        private System.Windows.Forms.Button buttonRemoveBlueUnit;
        private System.Windows.Forms.Button buttonAddBlueUnit;
    }
}
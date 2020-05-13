namespace AMMEdit.PropertyEditors.dialogs
{
    partial class AddUnit
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
            this.inputTypeClass = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyGridValues = new System.Windows.Forms.PropertyGrid();
            this.buttonSaveUnit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputTypeClass
            // 
            this.inputTypeClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputTypeClass.FormattingEnabled = true;
            this.inputTypeClass.Location = new System.Drawing.Point(72, 34);
            this.inputTypeClass.Name = "inputTypeClass";
            this.inputTypeClass.Size = new System.Drawing.Size(216, 21);
            this.inputTypeClass.TabIndex = 0;
            this.inputTypeClass.SelectedIndexChanged += new System.EventHandler(this.inputTypeClass_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Unit Type";
            // 
            // propertyGridValues
            // 
            this.propertyGridValues.Location = new System.Drawing.Point(16, 77);
            this.propertyGridValues.Name = "propertyGridValues";
            this.propertyGridValues.Size = new System.Drawing.Size(336, 313);
            this.propertyGridValues.TabIndex = 3;
            // 
            // buttonSaveUnit
            // 
            this.buttonSaveUnit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSaveUnit.Location = new System.Drawing.Point(277, 396);
            this.buttonSaveUnit.Name = "buttonSaveUnit";
            this.buttonSaveUnit.Size = new System.Drawing.Size(75, 33);
            this.buttonSaveUnit.TabIndex = 4;
            this.buttonSaveUnit.Text = "Add";
            this.buttonSaveUnit.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(16, 396);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 32);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // AddUnit
            // 
            this.AcceptButton = this.buttonSaveUnit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(364, 441);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSaveUnit);
            this.Controls.Add(this.propertyGridValues);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputTypeClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddUnit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Unit";
            this.Load += new System.EventHandler(this.AddUnit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox inputTypeClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid propertyGridValues;
        private System.Windows.Forms.Button buttonSaveUnit;
        private System.Windows.Forms.Button button1;
    }
}

namespace AMMEdit.PropertyEditors.dialogs
{
    partial class EditPlaceableObjectProperties
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSaveUnit = new System.Windows.Forms.Button();
            this.propertyGridValues = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(11, 375);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(98, 32);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSaveUnit
            // 
            this.buttonSaveUnit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSaveUnit.Location = new System.Drawing.Point(272, 375);
            this.buttonSaveUnit.Name = "buttonSaveUnit";
            this.buttonSaveUnit.Size = new System.Drawing.Size(75, 33);
            this.buttonSaveUnit.TabIndex = 7;
            this.buttonSaveUnit.Text = "Save";
            this.buttonSaveUnit.UseVisualStyleBackColor = true;
            // 
            // propertyGridValues
            // 
            this.propertyGridValues.Location = new System.Drawing.Point(11, 25);
            this.propertyGridValues.Name = "propertyGridValues";
            this.propertyGridValues.Size = new System.Drawing.Size(336, 344);
            this.propertyGridValues.TabIndex = 6;
            // 
            // EditPlaceableObjectProperties
            // 
            this.AcceptButton = this.buttonSaveUnit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(359, 416);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSaveUnit);
            this.Controls.Add(this.propertyGridValues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPlaceableObjectProperties";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Placeable Properties";
            this.Load += new System.EventHandler(this.EditPlaceableObjectProperties_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSaveUnit;
        private System.Windows.Forms.PropertyGrid propertyGridValues;
    }
}
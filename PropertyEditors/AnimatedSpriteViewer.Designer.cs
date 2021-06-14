
namespace AMMEdit.PropertyEditors
{
    partial class AnimatedSpriteViewer
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.spriteViewBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.spriteViewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(377, 875);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(399, 289);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(256, 598);
            this.propertyGrid1.TabIndex = 10;
            // 
            // spriteViewBox
            // 
            this.spriteViewBox.Location = new System.Drawing.Point(399, 12);
            this.spriteViewBox.Name = "spriteViewBox";
            this.spriteViewBox.Size = new System.Drawing.Size(256, 256);
            this.spriteViewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.spriteViewBox.TabIndex = 9;
            this.spriteViewBox.TabStop = false;
            // 
            // AnimatedSpriteViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 901);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.spriteViewBox);
            this.Controls.Add(this.listBox1);
            this.Name = "AnimatedSpriteViewer";
            this.Text = "AnimatedSpriteViewer";
            this.Load += new System.EventHandler(this.AnimatedSpriteViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spriteViewBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PictureBox spriteViewBox;
    }
}
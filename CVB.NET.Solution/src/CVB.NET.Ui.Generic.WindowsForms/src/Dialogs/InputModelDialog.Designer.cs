using CVB.NET.Ui.Generic.WindowsForms.Controls;

namespace CVB.NET.Ui.Generic.WindowsForms.Dialogs
{
    partial class InputModelDialog
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
            this.inputModelControl1 = new InputModelControl();
            this.SuspendLayout();
            // 
            // inputModelControl1
            // 
            this.inputModelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputModelControl1.ControlMargin = ((uint)(0u));
            this.inputModelControl1.Location = new System.Drawing.Point(12, 12);
            this.inputModelControl1.Name = "inputModelControl1";
            this.inputModelControl1.Size = new System.Drawing.Size(260, 238);
            this.inputModelControl1.TabIndex = 0;
            // 
            // InputModelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.inputModelControl1);
            this.Name = "InputModelDialog";
            this.Text = "InputModelDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private InputModelControl inputModelControl1;
    }
}
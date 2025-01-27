namespace CVB.NET.Ui.WindowsForms.Controls
{
    partial class EditableList
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddTextBox = new System.Windows.Forms.TextBox();
            this.ValueListBox = new System.Windows.Forms.ListBox();
            this.RaiseIndexButton = new System.Windows.Forms.Button();
            this.LowerIndexButton = new System.Windows.Forms.Button();
            this.ItemIndexBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(0, 0);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(71, 23);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(79, 0);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(71, 23);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddTextBox
            // 
            this.AddTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddTextBox.Location = new System.Drawing.Point(0, 29);
            this.AddTextBox.Name = "AddTextBox";
            this.AddTextBox.Size = new System.Drawing.Size(151, 20);
            this.AddTextBox.TabIndex = 2;
            // 
            // ValueListBox
            // 
            this.ValueListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueListBox.FormattingEnabled = true;
            this.ValueListBox.Location = new System.Drawing.Point(0, 52);
            this.ValueListBox.Name = "ValueListBox";
            this.ValueListBox.Size = new System.Drawing.Size(95, 82);
            this.ValueListBox.TabIndex = 3;
            this.ValueListBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // RaiseIndexButton
            // 
            this.RaiseIndexButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RaiseIndexButton.Location = new System.Drawing.Point(101, 55);
            this.RaiseIndexButton.Name = "RaiseIndexButton";
            this.RaiseIndexButton.Size = new System.Drawing.Size(50, 23);
            this.RaiseIndexButton.TabIndex = 4;
            this.RaiseIndexButton.Text = "button1";
            this.RaiseIndexButton.UseVisualStyleBackColor = true;
            this.RaiseIndexButton.Click += new System.EventHandler(this.RaiseIndexButton_Click);
            // 
            // LowerIndexButton
            // 
            this.LowerIndexButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LowerIndexButton.Location = new System.Drawing.Point(101, 110);
            this.LowerIndexButton.Name = "LowerIndexButton";
            this.LowerIndexButton.Size = new System.Drawing.Size(50, 23);
            this.LowerIndexButton.TabIndex = 5;
            this.LowerIndexButton.Text = "button2";
            this.LowerIndexButton.UseVisualStyleBackColor = true;
            this.LowerIndexButton.Click += new System.EventHandler(this.LowerIndexButton_Click);
            // 
            // ItemIndexBox
            // 
            this.ItemIndexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemIndexBox.Enabled = false;
            this.ItemIndexBox.Location = new System.Drawing.Point(101, 84);
            this.ItemIndexBox.Name = "ItemIndexBox";
            this.ItemIndexBox.Size = new System.Drawing.Size(50, 20);
            this.ItemIndexBox.TabIndex = 6;
            // 
            // EditableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ItemIndexBox);
            this.Controls.Add(this.LowerIndexButton);
            this.Controls.Add(this.RaiseIndexButton);
            this.Controls.Add(this.ValueListBox);
            this.Controls.Add(this.AddTextBox);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.MinimumSize = new System.Drawing.Size(151, 140);
            this.Name = "EditableList";
            this.Size = new System.Drawing.Size(151, 140);
            this.Load += new System.EventHandler(this.EditableList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.TextBox AddTextBox;
        private System.Windows.Forms.ListBox ValueListBox;
        private System.Windows.Forms.Button RaiseIndexButton;
        private System.Windows.Forms.Button LowerIndexButton;
        private System.Windows.Forms.TextBox ItemIndexBox;
    }
}

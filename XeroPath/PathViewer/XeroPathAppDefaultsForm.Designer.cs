namespace PathViewer
{
    partial class XeroPathAppDefaultsForm
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
            this.m_cancel = new System.Windows.Forms.Button();
            this.m_ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_undostack = new System.Windows.Forms.NumericUpDown();
            this.m_units = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_undostack)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cancel
            // 
            this.m_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancel.Location = new System.Drawing.Point(283, 89);
            this.m_cancel.Name = "m_cancel";
            this.m_cancel.Size = new System.Drawing.Size(75, 32);
            this.m_cancel.TabIndex = 0;
            this.m_cancel.Text = "Cancel";
            this.m_cancel.UseVisualStyleBackColor = true;
            // 
            // m_ok
            // 
            this.m_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_ok.Location = new System.Drawing.Point(202, 89);
            this.m_ok.Name = "m_ok";
            this.m_ok.Size = new System.Drawing.Size(75, 32);
            this.m_ok.TabIndex = 1;
            this.m_ok.Text = "OK";
            this.m_ok.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Undo Stack Depth:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Default Units:";
            // 
            // m_undostack
            // 
            this.m_undostack.Location = new System.Drawing.Point(164, 8);
            this.m_undostack.Name = "m_undostack";
            this.m_undostack.Size = new System.Drawing.Size(191, 26);
            this.m_undostack.TabIndex = 4;
            // 
            // m_units
            // 
            this.m_units.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_units.FormattingEnabled = true;
            this.m_units.Location = new System.Drawing.Point(163, 45);
            this.m_units.Name = "m_units";
            this.m_units.Size = new System.Drawing.Size(191, 28);
            this.m_units.TabIndex = 5;
            // 
            // XeroPathAppDefaultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 133);
            this.Controls.Add(this.m_units);
            this.Controls.Add(this.m_undostack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_ok);
            this.Controls.Add(this.m_cancel);
            this.Name = "XeroPathAppDefaultsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XeroPathAppDefaultsForm";
            ((System.ComponentModel.ISupportInitialize)(this.m_undostack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_cancel;
        private System.Windows.Forms.Button m_ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_undostack;
        private System.Windows.Forms.ComboBox m_units;
    }
}
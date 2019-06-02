namespace DataPlotter
{
    partial class DataViewer
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
            this.datalist_ = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // datalist_
            // 
            this.datalist_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datalist_.FullRowSelect = true;
            this.datalist_.Location = new System.Drawing.Point(0, 0);
            this.datalist_.Name = "datalist_";
            this.datalist_.Size = new System.Drawing.Size(800, 450);
            this.datalist_.TabIndex = 0;
            this.datalist_.UseCompatibleStateImageBehavior = false;
            this.datalist_.View = System.Windows.Forms.View.Details;
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.datalist_);
            this.Name = "DataViewer";
            this.Text = "DataViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView datalist_;
    }
}
namespace DataPlotter
{
    partial class RobotGrapher
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotwindows_ = new DataPlotter.PlotWindow();
            this.split1_ = new System.Windows.Forms.SplitContainer();
            this.split2_ = new System.Windows.Forms.SplitContainer();
            this.sessions_ = new System.Windows.Forms.ListView();
            this.variables_ = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split1_)).BeginInit();
            this.split1_.Panel1.SuspendLayout();
            this.split1_.Panel2.SuspendLayout();
            this.split1_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split2_)).BeginInit();
            this.split2_.Panel1.SuspendLayout();
            this.split2_.Panel2.SuspendLayout();
            this.split2_.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.graphToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1916, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSessionToolStripMenuItem,
            this.loadSessionToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSessionToolStripMenuItem
            // 
            this.saveSessionToolStripMenuItem.Name = "saveSessionToolStripMenuItem";
            this.saveSessionToolStripMenuItem.Size = new System.Drawing.Size(266, 30);
            this.saveSessionToolStripMenuItem.Text = "Save Configuration ...";
            this.saveSessionToolStripMenuItem.Click += new System.EventHandler(this.SaveConfigMenuItemHandler);
            // 
            // loadSessionToolStripMenuItem
            // 
            this.loadSessionToolStripMenuItem.Name = "loadSessionToolStripMenuItem";
            this.loadSessionToolStripMenuItem.Size = new System.Drawing.Size(266, 30);
            this.loadSessionToolStripMenuItem.Text = "Load Configuration ...";
            this.loadSessionToolStripMenuItem.Click += new System.EventHandler(this.LoadConfigMenuItemHandler);
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Size = new System.Drawing.Size(72, 29);
            this.graphToolStripMenuItem.Text = "Graph";
            // 
            // createNewToolStripMenuItem
            // 
            this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
            this.createNewToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.createNewToolStripMenuItem.Text = "New";
            this.createNewToolStripMenuItem.Click += new System.EventHandler(this.CreateNewGraphHandler);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteMenuItemHandler);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.HelpAboutMenuHandler);
            // 
            // plotwindows_
            // 
            this.plotwindows_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotwindows_.Location = new System.Drawing.Point(0, 0);
            this.plotwindows_.Name = "plotwindows_";
            this.plotwindows_.Size = new System.Drawing.Size(1550, 989);
            this.plotwindows_.TabIndex = 0;
            // 
            // split1_
            // 
            this.split1_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split1_.Location = new System.Drawing.Point(0, 33);
            this.split1_.Name = "split1_";
            // 
            // split1_.Panel1
            // 
            this.split1_.Panel1.Controls.Add(this.split2_);
            // 
            // split1_.Panel2
            // 
            this.split1_.Panel2.Controls.Add(this.plotwindows_);
            this.split1_.Size = new System.Drawing.Size(1916, 989);
            this.split1_.SplitterDistance = 362;
            this.split1_.TabIndex = 0;
            // 
            // split2_
            // 
            this.split2_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split2_.Location = new System.Drawing.Point(0, 0);
            this.split2_.Name = "split2_";
            this.split2_.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split2_.Panel1
            // 
            this.split2_.Panel1.Controls.Add(this.sessions_);
            // 
            // split2_.Panel2
            // 
            this.split2_.Panel2.Controls.Add(this.variables_);
            this.split2_.Size = new System.Drawing.Size(362, 989);
            this.split2_.SplitterDistance = 135;
            this.split2_.TabIndex = 0;
            // 
            // sessions_
            // 
            this.sessions_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessions_.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessions_.Location = new System.Drawing.Point(0, 0);
            this.sessions_.MultiSelect = false;
            this.sessions_.Name = "sessions_";
            this.sessions_.Size = new System.Drawing.Size(362, 135);
            this.sessions_.TabIndex = 0;
            this.sessions_.UseCompatibleStateImageBehavior = false;
            this.sessions_.View = System.Windows.Forms.View.List;
            // 
            // variables_
            // 
            this.variables_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variables_.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variables_.Location = new System.Drawing.Point(0, 0);
            this.variables_.Name = "variables_";
            this.variables_.Size = new System.Drawing.Size(362, 850);
            this.variables_.TabIndex = 1;
            this.variables_.UseCompatibleStateImageBehavior = false;
            this.variables_.View = System.Windows.Forms.View.List;
            // 
            // RobotGrapher
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1916, 1022);
            this.Controls.Add(this.split1_);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RobotGrapher";
            this.Text = "Robot Data Grapher";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.split1_.Panel1.ResumeLayout(false);
            this.split1_.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split1_)).EndInit();
            this.split1_.ResumeLayout(false);
            this.split2_.Panel1.ResumeLayout(false);
            this.split2_.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split2_)).EndInit();
            this.split2_.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private PlotWindow plotwindows_;
        private System.Windows.Forms.SplitContainer split1_;
        private System.Windows.Forms.SplitContainer split2_;
        private System.Windows.Forms.ListView sessions_;
        private System.Windows.Forms.ListView variables_;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}


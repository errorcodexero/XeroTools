namespace PathViewer
{
    partial class PathViewerForm
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
            this.m_split = new System.Windows.Forms.SplitContainer();
            this.m_vertical = new System.Windows.Forms.SplitContainer();
            this.m_field_tab = new System.Windows.Forms.TabControl();
            this.m_field_tab_page = new System.Windows.Forms.TabPage();
            this.m_field = new PathViewer.PathFieldView();
            this.m_detailed_tab_page = new System.Windows.Forms.TabPage();
            this.m_detailed = new PathViewer.RobotFieldView();
            this.m_bottom_tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_plot = new PathViewer.RobotPlotViewer();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_logger_window = new System.Windows.Forms.TextBox();
            this.m_flow = new System.Windows.Forms.FlowLayoutPanel();
            this.m_right_one = new System.Windows.Forms.GroupBox();
            this.m_pathfile_tree = new System.Windows.Forms.TreeView();
            this.m_right_two = new System.Windows.Forms.GroupBox();
            this.m_robot_view = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_right_three = new System.Windows.Forms.GroupBox();
            this.m_path_view = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_right_four = new System.Windows.Forms.GroupBox();
            this.m_waypoint_view = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_pos_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_status_strip = new System.Windows.Forms.StatusStrip();
            this.m_running_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_misc_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_undo_stack_usage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_file_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.generatePathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatePathsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.recentlFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_edit_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.insertWaypointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.newPathGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_games_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.m_generators_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menu = new System.Windows.Forms.MenuStrip();
            this.m_play_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPathGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.completePathFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_help_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.m_split)).BeginInit();
            this.m_split.Panel1.SuspendLayout();
            this.m_split.Panel2.SuspendLayout();
            this.m_split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_vertical)).BeginInit();
            this.m_vertical.Panel1.SuspendLayout();
            this.m_vertical.Panel2.SuspendLayout();
            this.m_vertical.SuspendLayout();
            this.m_field_tab.SuspendLayout();
            this.m_field_tab_page.SuspendLayout();
            this.m_detailed_tab_page.SuspendLayout();
            this.m_bottom_tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.m_flow.SuspendLayout();
            this.m_right_one.SuspendLayout();
            this.m_right_two.SuspendLayout();
            this.m_right_three.SuspendLayout();
            this.m_right_four.SuspendLayout();
            this.m_status_strip.SuspendLayout();
            this.m_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_split
            // 
            this.m_split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_split.Location = new System.Drawing.Point(0, 33);
            this.m_split.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_split.Name = "m_split";
            // 
            // m_split.Panel1
            // 
            this.m_split.Panel1.Controls.Add(this.m_vertical);
            // 
            // m_split.Panel2
            // 
            this.m_split.Panel2.Controls.Add(this.m_flow);
            this.m_split.Size = new System.Drawing.Size(1764, 988);
            this.m_split.SplitterDistance = 1362;
            this.m_split.TabIndex = 3;
            // 
            // m_vertical
            // 
            this.m_vertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_vertical.Location = new System.Drawing.Point(0, 0);
            this.m_vertical.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_vertical.Name = "m_vertical";
            this.m_vertical.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // m_vertical.Panel1
            // 
            this.m_vertical.Panel1.Controls.Add(this.m_field_tab);
            // 
            // m_vertical.Panel2
            // 
            this.m_vertical.Panel2.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.m_vertical.Panel2.Controls.Add(this.m_bottom_tab);
            this.m_vertical.Size = new System.Drawing.Size(1362, 988);
            this.m_vertical.SplitterDistance = 581;
            this.m_vertical.SplitterWidth = 5;
            this.m_vertical.TabIndex = 0;
            // 
            // m_field_tab
            // 
            this.m_field_tab.Controls.Add(this.m_field_tab_page);
            this.m_field_tab.Controls.Add(this.m_detailed_tab_page);
            this.m_field_tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_field_tab.Location = new System.Drawing.Point(0, 0);
            this.m_field_tab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_field_tab.Name = "m_field_tab";
            this.m_field_tab.SelectedIndex = 0;
            this.m_field_tab.Size = new System.Drawing.Size(1362, 581);
            this.m_field_tab.TabIndex = 0;
            // 
            // m_field_tab_page
            // 
            this.m_field_tab_page.Controls.Add(this.m_field);
            this.m_field_tab_page.Location = new System.Drawing.Point(4, 29);
            this.m_field_tab_page.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_field_tab_page.Name = "m_field_tab_page";
            this.m_field_tab_page.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_field_tab_page.Size = new System.Drawing.Size(1354, 548);
            this.m_field_tab_page.TabIndex = 0;
            this.m_field_tab_page.Text = "Path Editor";
            this.m_field_tab_page.UseVisualStyleBackColor = true;
            // 
            // m_field
            // 
            this.m_field.DisplayedPath = null;
            this.m_field.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_field.FieldGame = null;
            this.m_field.File = null;
            this.m_field.HighlightPoint = null;
            this.m_field.Location = new System.Drawing.Point(3, 2);
            this.m_field.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_field.Name = "m_field";
            this.m_field.SelectedWaypoint = null;
            this.m_field.Size = new System.Drawing.Size(1348, 544);
            this.m_field.TabIndex = 2;
            this.m_field.Units = "inches";
            // 
            // m_detailed_tab_page
            // 
            this.m_detailed_tab_page.Controls.Add(this.m_detailed);
            this.m_detailed_tab_page.Location = new System.Drawing.Point(4, 29);
            this.m_detailed_tab_page.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_detailed_tab_page.Name = "m_detailed_tab_page";
            this.m_detailed_tab_page.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_detailed_tab_page.Size = new System.Drawing.Size(1354, 548);
            this.m_detailed_tab_page.TabIndex = 1;
            this.m_detailed_tab_page.Text = "Robot View";
            this.m_detailed_tab_page.UseVisualStyleBackColor = true;
            // 
            // m_detailed
            // 
            this.m_detailed.DisplayedPath = null;
            this.m_detailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_detailed.FieldGame = null;
            this.m_detailed.Location = new System.Drawing.Point(3, 2);
            this.m_detailed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_detailed.Name = "m_detailed";
            this.m_detailed.Robot = null;
            this.m_detailed.Size = new System.Drawing.Size(1348, 544);
            this.m_detailed.TabIndex = 0;
            this.m_detailed.Time = 0D;
            // 
            // m_bottom_tab
            // 
            this.m_bottom_tab.Controls.Add(this.tabPage1);
            this.m_bottom_tab.Controls.Add(this.tabPage2);
            this.m_bottom_tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_bottom_tab.Location = new System.Drawing.Point(0, 0);
            this.m_bottom_tab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_bottom_tab.Name = "m_bottom_tab";
            this.m_bottom_tab.SelectedIndex = 0;
            this.m_bottom_tab.Size = new System.Drawing.Size(1362, 402);
            this.m_bottom_tab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_plot);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1354, 369);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Plots";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_plot
            // 
            this.m_plot.AutoScroll = true;
            this.m_plot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_plot.Location = new System.Drawing.Point(3, 2);
            this.m_plot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_plot.Name = "m_plot";
            this.m_plot.Path = null;
            this.m_plot.Size = new System.Drawing.Size(1348, 365);
            this.m_plot.TabIndex = 0;
            this.m_plot.Time = 0D;
            this.m_plot.Units = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_logger_window);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1354, 369);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Messages";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_logger_window
            // 
            this.m_logger_window.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_logger_window.Location = new System.Drawing.Point(3, 2);
            this.m_logger_window.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_logger_window.Multiline = true;
            this.m_logger_window.Name = "m_logger_window";
            this.m_logger_window.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_logger_window.Size = new System.Drawing.Size(1348, 365);
            this.m_logger_window.TabIndex = 0;
            // 
            // m_flow
            // 
            this.m_flow.Controls.Add(this.m_right_one);
            this.m_flow.Controls.Add(this.m_right_two);
            this.m_flow.Controls.Add(this.m_right_three);
            this.m_flow.Controls.Add(this.m_right_four);
            this.m_flow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_flow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_flow.Location = new System.Drawing.Point(0, 0);
            this.m_flow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_flow.Name = "m_flow";
            this.m_flow.Size = new System.Drawing.Size(398, 988);
            this.m_flow.TabIndex = 3;
            // 
            // m_right_one
            // 
            this.m_right_one.Controls.Add(this.m_pathfile_tree);
            this.m_right_one.Location = new System.Drawing.Point(3, 2);
            this.m_right_one.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_one.Name = "m_right_one";
            this.m_right_one.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_one.Size = new System.Drawing.Size(395, 95);
            this.m_right_one.TabIndex = 0;
            this.m_right_one.TabStop = false;
            this.m_right_one.Text = "Paths";
            // 
            // m_pathfile_tree
            // 
            this.m_pathfile_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_pathfile_tree.FullRowSelect = true;
            this.m_pathfile_tree.HideSelection = false;
            this.m_pathfile_tree.Location = new System.Drawing.Point(3, 21);
            this.m_pathfile_tree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_pathfile_tree.Name = "m_pathfile_tree";
            this.m_pathfile_tree.Size = new System.Drawing.Size(389, 72);
            this.m_pathfile_tree.TabIndex = 0;
            // 
            // m_right_two
            // 
            this.m_right_two.Controls.Add(this.m_robot_view);
            this.m_right_two.Location = new System.Drawing.Point(3, 101);
            this.m_right_two.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_two.Name = "m_right_two";
            this.m_right_two.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_two.Size = new System.Drawing.Size(395, 140);
            this.m_right_two.TabIndex = 0;
            this.m_right_two.TabStop = false;
            this.m_right_two.Text = "Robot Parameters";
            // 
            // m_robot_view
            // 
            this.m_robot_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.m_robot_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_robot_view.FullRowSelect = true;
            this.m_robot_view.Location = new System.Drawing.Point(3, 21);
            this.m_robot_view.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_robot_view.Name = "m_robot_view";
            this.m_robot_view.Size = new System.Drawing.Size(389, 117);
            this.m_robot_view.TabIndex = 2;
            this.m_robot_view.UseCompatibleStateImageBehavior = false;
            this.m_robot_view.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 150;
            // 
            // m_right_three
            // 
            this.m_right_three.Controls.Add(this.m_path_view);
            this.m_right_three.Location = new System.Drawing.Point(3, 245);
            this.m_right_three.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_three.Name = "m_right_three";
            this.m_right_three.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_three.Size = new System.Drawing.Size(395, 116);
            this.m_right_three.TabIndex = 1;
            this.m_right_three.TabStop = false;
            this.m_right_three.Text = "Selected Path";
            // 
            // m_path_view
            // 
            this.m_path_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.m_path_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_path_view.FullRowSelect = true;
            this.m_path_view.Location = new System.Drawing.Point(3, 21);
            this.m_path_view.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_path_view.Name = "m_path_view";
            this.m_path_view.Size = new System.Drawing.Size(389, 93);
            this.m_path_view.TabIndex = 0;
            this.m_path_view.UseCompatibleStateImageBehavior = false;
            this.m_path_view.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Value";
            this.columnHeader6.Width = 150;
            // 
            // m_right_four
            // 
            this.m_right_four.Controls.Add(this.m_waypoint_view);
            this.m_right_four.Location = new System.Drawing.Point(3, 365);
            this.m_right_four.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_four.Name = "m_right_four";
            this.m_right_four.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_right_four.Size = new System.Drawing.Size(395, 248);
            this.m_right_four.TabIndex = 0;
            this.m_right_four.TabStop = false;
            this.m_right_four.Text = "Selected Waypoint";
            // 
            // m_waypoint_view
            // 
            this.m_waypoint_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.m_waypoint_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_waypoint_view.FullRowSelect = true;
            this.m_waypoint_view.Location = new System.Drawing.Point(3, 21);
            this.m_waypoint_view.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_waypoint_view.Name = "m_waypoint_view";
            this.m_waypoint_view.Size = new System.Drawing.Size(389, 225);
            this.m_waypoint_view.TabIndex = 0;
            this.m_waypoint_view.UseCompatibleStateImageBehavior = false;
            this.m_waypoint_view.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 150;
            // 
            // m_pos_status
            // 
            this.m_pos_status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_pos_status.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.m_pos_status.Name = "m_pos_status";
            this.m_pos_status.Size = new System.Drawing.Size(117, 29);
            this.m_pos_status.Text = "Position: 0, 0";
            this.m_pos_status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_status_strip
            // 
            this.m_status_strip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.m_status_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_pos_status,
            this.m_running_status,
            this.m_misc_status,
            this.m_undo_stack_usage,
            this.toolStripStatusLabel1});
            this.m_status_strip.Location = new System.Drawing.Point(0, 1021);
            this.m_status_strip.Name = "m_status_strip";
            this.m_status_strip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.m_status_strip.Size = new System.Drawing.Size(1764, 34);
            this.m_status_strip.TabIndex = 0;
            this.m_status_strip.Text = "statusStrip1";
            // 
            // m_running_status
            // 
            this.m_running_status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_running_status.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.m_running_status.Name = "m_running_status";
            this.m_running_status.Size = new System.Drawing.Size(140, 29);
            this.m_running_status.Text = "Generation: Idle";
            this.m_running_status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_misc_status
            // 
            this.m_misc_status.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_misc_status.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.m_misc_status.Name = "m_misc_status";
            this.m_misc_status.Size = new System.Drawing.Size(1348, 29);
            this.m_misc_status.Spring = true;
            this.m_misc_status.Text = "Selected Group: None, Selected Path: None";
            // 
            // m_undo_stack_usage
            // 
            this.m_undo_stack_usage.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.m_undo_stack_usage.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.m_undo_stack_usage.Name = "m_undo_stack_usage";
            this.m_undo_stack_usage.Size = new System.Drawing.Size(125, 29);
            this.m_undo_stack_usage.Text = "Undo Stack: 0";
            this.m_undo_stack_usage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(17, 29);
            this.toolStripStatusLabel1.Text = " ";
            // 
            // m_file_menu
            // 
            this.m_file_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.loadToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.generatePathsToolStripMenuItem,
            this.generatePathsToolStripMenuItem1,
            this.toolStripSeparator6,
            this.recentlFilesToolStripMenuItem});
            this.m_file_menu.Name = "m_file_menu";
            this.m_file_menu.Size = new System.Drawing.Size(50, 29);
            this.m_file_menu.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(260, 30);
            this.toolStripMenuItem2.Text = "New";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.NewMenuItemEventHandler);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(260, 30);
            this.loadToolStripMenuItem.Text = "Open ...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(257, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(260, 30);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(260, 30);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(260, 30);
            this.toolStripMenuItem1.Text = "Close";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.CloseMenuItemEventHandler);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(257, 6);
            // 
            // generatePathsToolStripMenuItem
            // 
            this.generatePathsToolStripMenuItem.Name = "generatePathsToolStripMenuItem";
            this.generatePathsToolStripMenuItem.Size = new System.Drawing.Size(260, 30);
            this.generatePathsToolStripMenuItem.Text = "Generate Paths As  ...";
            this.generatePathsToolStripMenuItem.Click += new System.EventHandler(this.GeneratePathAsMenuItem);
            // 
            // generatePathsToolStripMenuItem1
            // 
            this.generatePathsToolStripMenuItem1.Name = "generatePathsToolStripMenuItem1";
            this.generatePathsToolStripMenuItem1.Size = new System.Drawing.Size(260, 30);
            this.generatePathsToolStripMenuItem1.Text = "Generate Paths";
            this.generatePathsToolStripMenuItem1.Click += new System.EventHandler(this.GeneratePathMenuItem);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(257, 6);
            // 
            // recentlFilesToolStripMenuItem
            // 
            this.recentlFilesToolStripMenuItem.Name = "recentlFilesToolStripMenuItem";
            this.recentlFilesToolStripMenuItem.Size = new System.Drawing.Size(260, 30);
            this.recentlFilesToolStripMenuItem.Text = "Recent Files";
            // 
            // m_edit_menu
            // 
            this.m_edit_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertWaypointToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripSeparator7,
            this.newPathGroupToolStripMenuItem,
            this.newPathToolStripMenuItem,
            this.toolStripSeparator2,
            this.undoToolStripMenuItem,
            this.toolStripSeparator5,
            this.preferencesToolStripMenuItem});
            this.m_edit_menu.Name = "m_edit_menu";
            this.m_edit_menu.Size = new System.Drawing.Size(54, 29);
            this.m_edit_menu.Text = "Edit";
            // 
            // insertWaypointToolStripMenuItem
            // 
            this.insertWaypointToolStripMenuItem.Name = "insertWaypointToolStripMenuItem";
            this.insertWaypointToolStripMenuItem.Size = new System.Drawing.Size(227, 30);
            this.insertWaypointToolStripMenuItem.Text = "Insert Waypoint";
            this.insertWaypointToolStripMenuItem.Click += new System.EventHandler(this.InsertWaypointHandler);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(227, 30);
            this.toolStripMenuItem3.Text = "Delete Waypoint";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.DeleteWaypointHandler);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(224, 6);
            // 
            // newPathGroupToolStripMenuItem
            // 
            this.newPathGroupToolStripMenuItem.Name = "newPathGroupToolStripMenuItem";
            this.newPathGroupToolStripMenuItem.Size = new System.Drawing.Size(227, 30);
            this.newPathGroupToolStripMenuItem.Text = "Add Path Group";
            this.newPathGroupToolStripMenuItem.Click += new System.EventHandler(this.NewPathGroupToolStripMenuItem_Click);
            // 
            // newPathToolStripMenuItem
            // 
            this.newPathToolStripMenuItem.Name = "newPathToolStripMenuItem";
            this.newPathToolStripMenuItem.Size = new System.Drawing.Size(227, 30);
            this.newPathToolStripMenuItem.Text = "Add Path";
            this.newPathToolStripMenuItem.Click += new System.EventHandler(this.NewPathToolStripMenuItemHandler);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(224, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(227, 30);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoMenuItem);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(224, 6);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(227, 30);
            this.preferencesToolStripMenuItem.Text = "Preferences ...";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.EditPreferencesHandler);
            // 
            // m_games_menu
            // 
            this.m_games_menu.Name = "m_games_menu";
            this.m_games_menu.Size = new System.Drawing.Size(78, 29);
            this.m_games_menu.Text = "Games";
            // 
            // m_generators_menu
            // 
            this.m_generators_menu.Name = "m_generators_menu";
            this.m_generators_menu.Size = new System.Drawing.Size(110, 29);
            this.m_generators_menu.Text = "Generators";
            // 
            // m_menu
            // 
            this.m_menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.m_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_file_menu,
            this.m_edit_menu,
            this.m_play_menu,
            this.m_games_menu,
            this.m_generators_menu,
            this.m_help_menu});
            this.m_menu.Location = new System.Drawing.Point(0, 0);
            this.m_menu.Name = "m_menu";
            this.m_menu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.m_menu.Size = new System.Drawing.Size(1764, 33);
            this.m_menu.TabIndex = 1;
            this.m_menu.Text = "MainMenu";
            // 
            // m_play_menu
            // 
            this.m_play_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentPathGroupToolStripMenuItem,
            this.groupToolStripMenuItem,
            this.completePathFileToolStripMenuItem});
            this.m_play_menu.Name = "m_play_menu";
            this.m_play_menu.Size = new System.Drawing.Size(73, 29);
            this.m_play_menu.Text = "Demo";
            // 
            // currentPathGroupToolStripMenuItem
            // 
            this.currentPathGroupToolStripMenuItem.Name = "currentPathGroupToolStripMenuItem";
            this.currentPathGroupToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.currentPathGroupToolStripMenuItem.Text = "Path";
            this.currentPathGroupToolStripMenuItem.Click += new System.EventHandler(this.PatyMenuPathDemo);
            // 
            // groupToolStripMenuItem
            // 
            this.groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            this.groupToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.groupToolStripMenuItem.Text = "Group";
            this.groupToolStripMenuItem.Click += new System.EventHandler(this.PlayMenuGroupDemo);
            // 
            // completePathFileToolStripMenuItem
            // 
            this.completePathFileToolStripMenuItem.Name = "completePathFileToolStripMenuItem";
            this.completePathFileToolStripMenuItem.Size = new System.Drawing.Size(146, 30);
            this.completePathFileToolStripMenuItem.Text = "File";
            this.completePathFileToolStripMenuItem.Click += new System.EventHandler(this.PlayMenuFileDemo);
            // 
            // m_help_menu
            // 
            this.m_help_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.documentationToolStripMenuItem});
            this.m_help_menu.Name = "m_help_menu";
            this.m_help_menu.Size = new System.Drawing.Size(61, 29);
            this.m_help_menu.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(219, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutMenuItemHandler);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(219, 30);
            this.documentationToolStripMenuItem.Text = "Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.DocumentationMenuItemHandler);
            // 
            // PathViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1764, 1055);
            this.Controls.Add(this.m_split);
            this.Controls.Add(this.m_status_strip);
            this.Controls.Add(this.m_menu);
            this.MainMenuStrip = this.m_menu;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PathViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Path Editor";
            this.m_split.Panel1.ResumeLayout(false);
            this.m_split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_split)).EndInit();
            this.m_split.ResumeLayout(false);
            this.m_vertical.Panel1.ResumeLayout(false);
            this.m_vertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_vertical)).EndInit();
            this.m_vertical.ResumeLayout(false);
            this.m_field_tab.ResumeLayout(false);
            this.m_field_tab_page.ResumeLayout(false);
            this.m_detailed_tab_page.ResumeLayout(false);
            this.m_bottom_tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.m_flow.ResumeLayout(false);
            this.m_right_one.ResumeLayout(false);
            this.m_right_two.ResumeLayout(false);
            this.m_right_three.ResumeLayout(false);
            this.m_right_four.ResumeLayout(false);
            this.m_status_strip.ResumeLayout(false);
            this.m_status_strip.PerformLayout();
            this.m_menu.ResumeLayout(false);
            this.m_menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PathFieldView m_field;
        private System.Windows.Forms.SplitContainer m_split;
        private System.Windows.Forms.SplitContainer m_vertical;
        private System.Windows.Forms.ListView m_waypoint_view;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripStatusLabel m_pos_status;
        private System.Windows.Forms.StatusStrip m_status_strip;
        private System.Windows.Forms.TreeView m_pathfile_tree;
        private System.Windows.Forms.ListView m_robot_view;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox m_right_one;
        private System.Windows.Forms.GroupBox m_right_two;
        private System.Windows.Forms.GroupBox m_right_four;
        private RobotPlotViewer m_plot;
        private System.Windows.Forms.TabControl m_bottom_tab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox m_logger_window;
        private System.Windows.Forms.GroupBox m_right_three;
        private System.Windows.Forms.ListView m_path_view;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.FlowLayoutPanel m_flow;
        private System.Windows.Forms.TabControl m_field_tab;
        private System.Windows.Forms.TabPage m_field_tab_page;
        private System.Windows.Forms.TabPage m_detailed_tab_page;
        private RobotFieldView m_detailed;
        private System.Windows.Forms.ToolStripStatusLabel m_running_status;
        private System.Windows.Forms.ToolStripMenuItem m_file_menu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem generatePathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePathsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem m_edit_menu;
        private System.Windows.Forms.ToolStripMenuItem newPathGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_games_menu;
        private System.Windows.Forms.ToolStripMenuItem m_generators_menu;
        private System.Windows.Forms.MenuStrip m_menu;
        private System.Windows.Forms.ToolStripStatusLabel m_misc_status;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertWaypointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem m_play_menu;
        private System.Windows.Forms.ToolStripMenuItem currentPathGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem completePathFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_help_menu;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel m_undo_stack_usage;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem recentlFilesToolStripMenuItem;
    }
}


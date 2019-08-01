using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using XeroMath;

namespace PathViewer
{
    public partial class PathViewerForm : Form
    {

        #region constant names for window items
        private const string StartAngleLabel = "Start Angle";
        private const string MaxVelocityLabel = "Max Velocity";
        private const string MaxAccelerationLabel = "Max Acceleration";
        private const string MaxJerkLabel = "Max Jerk";
        private const string EndAngleLabel = "End Angle";
        private const string RotationStartDelayLabel = "Rotation Start Delay";
        private const string RotationEndDelayLabel = "Rotation End Delay";
        private const string TypeLabel = "Type";
        private const string UnitsLabel = "Units";
        private const string AngleUnitsLabel = "Angles";
        private const string WidthLabel = "Width";
        private const string LengthLabel = "Length";
        private const string TimestampLabel = "Timestep";
        private const string XLabel = "X";
        private const string YLabel = "Y";
        private const string HeadingLabel = "Heading";
        private const string VelocityLabel = "Velocity";
        private string[] SupportedDriveBaseTypes = new string[] { "tank", "swerve" };
        #endregion

        #region private member variables

        /// <summary>
        /// Defaults for the program
        /// </summary>
        private XeroPathAppDefaults m_defaults;

        /// <summary>
        /// Demo all paths
        /// </summary>
        private DemoMode m_demo_mode;

        /// <summary>
        /// The set of games that are available
        /// </summary>
        private GameManager m_games;

        /// <summary>
        /// The current game
        /// </summary>
        private Game m_current_game;

        /// <summary>
        /// THe set of path generators that are available
        /// </summary>
        private GeneratorManager m_generators;

        /// <summary>
        /// The current path file we are editing
        /// </summary>
        private PathFile m_file;

        /// <summary>
        /// The selected path
        /// </summary>
        private RobotPath m_selected_path;

        /// <summary>
        /// The selected path group, used for demoing  a complete path
        /// </summary>
        private PathGroup m_selected_group;

        /// <summary>
        /// The control for editing path group names and path names in place
        /// </summary>
        private TextBox m_text_editor;

        /// <summary>
        /// The control for editing robot params related selectable items
        /// </summary>
        private ComboBox m_combo_editor;

        /// <summary>
        /// The node we are currently editing in the path group and path tree control
        /// </summary>
        private TreeNode m_editing_pathtree;

        /// <summary>
        /// The list view item we are editing in the waypoint window or robot params window
        /// </summary>
        private ListViewItem m_waypoint_editing;

        /// <summary>
        /// The robot view item we are editing
        /// </summary>
        private ListViewItem m_robot_param_editing;

        /// <summary>
        /// The path view item we are editing
        /// </summary>
        private ListViewItem m_path_view_editing;

        /// <summary>
        /// The currently selected generator
        /// </summary>
        private PathGenerator m_generator;

        /// <summary>
        /// The message logger for the application
        /// </summary>
        private Logger m_logger;

        /// <summary>
        /// If true, igonre the lost focus event on the editing text control
        /// </summary>
        private bool m_ignore_lost_focus;

        /// <summary>
        /// The old selected node during a context menu event on the path tree view
        /// </summary>
        private TreeNode m_old_node;

        /// <summary>
        /// The undo stack
        /// </summary>
        private List<UndoState> m_undo_stack;

        /// <summary>
        /// The length of the undo stack
        /// </summary>
        private int m_undo_length;

        /// <summary>
        /// The number of the undo state generated
        /// </summary>
        private int m_undo_serial;

        /// <summary>
        /// The timer for automatically showing the robot path
        /// </summary>
        private System.Windows.Forms.Timer m_timer;

        /// <summary>
        /// The set of recent files
        /// </summary>
        private RecentFiles m_recent;

        #endregion

        #region private delegates
        private delegate void HighlightDelegate(RobotPath path, WayPoint pt);
        private delegate void UpdateJobStatusDelegate(PathGenerationStateChangeEvent state);
        private delegate void UpdatePathWindowDelegate();
        private delegate void SetTimeDelegate(double t, bool user);
        #endregion

        #region private enumerations
        private enum DemoMode
        {
            File,
            Group,
            Path,
            None
        };

        #endregion

        #region public constructor
        public PathViewerForm()
        {
            m_logger = new Logger();
            m_games = new GameManager();
            m_generators = new GeneratorManager();

            m_defaults = ReadDefaults();
            m_recent = ReadRecent();

            if (m_games.Count == 0)
            {
                MessageBox.Show("No games are installed - cannot start path generator program", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_logger.LogMessage(Logger.MessageType.Error, "no games are installed");
                throw new Exception("No games installed");
            }

            if (m_generators.Count == 0)
            {
                MessageBox.Show("No path generators are installed - cannot start path generator program", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_logger.LogMessage(Logger.MessageType.Error, "no generators are installed");
                throw new Exception("No generators installed");
            }

            m_file = new PathFile();
            m_file.ConvertUnits(m_defaults.DefaultLengthUnits);
            m_file.Robot.AngleUnits = m_defaults.DefaultAngleUnits;
            Text = "Path Editor - Unsaved";

            m_editing_pathtree = null;
            m_text_editor = null;

            InitializeComponent();

            m_split.Layout += M_split_Layout;
            m_vertical.Layout += M_vertical_Layout;
            m_flow.Layout += M_flow_Layout;

            m_field.File = m_file;
            m_field.WaypointSelected += WayPointSelectedOrChanged;
            m_field.WaypointChanged += WayPointSelectedOrChanged;
            m_field.FieldMouseMoved += FieldMouseMoved;
            m_field.PreviewKeyDown += FieldKeyPreview;
            m_field.KeyDown += FieldKeyDown;

            m_plot.Name = "VelocityProfileWindow";
            m_plot.TimeCursorMoved += PlotWindowTimeChanged;
            m_plot.Time = 0.0;

            m_pathfile_tree.DoubleClick += DoPathTreeDoubleClick;
            m_pathfile_tree.MouseUp += PathTreeMouseUp;
            m_pathfile_tree.AfterSelect += PathTreeSelectionChanged;
            m_pathfile_tree.ItemDrag += PathTreePathDrag;
            m_pathfile_tree.DragEnter += PathTreeDragEnter;
            m_pathfile_tree.DragDrop += PathTreeDragDrop;
            m_pathfile_tree.AllowDrop = true;

            m_waypoint_view.DoubleClick += WaypointDoubleClick;
            m_robot_view.DoubleClick += RobotParamDoubleClick;
            m_path_view.DoubleClick += PathViewDoubleClick;

            m_ignore_lost_focus = false;
            m_undo_stack = new List<UndoState>();
            m_undo_serial = 0;
            m_undo_length = m_defaults.UndoStackSize;

            PopulateGameMenu();
            PopulateGeneratorMenu();
            SetUnits();
            UpdateRobotWindow();
            UpdateRecentMenu();

            m_detailed.Robot = m_file.Robot;
            m_detailed.FieldMouseMoved += FieldMouseMoved;
            m_logger.OutputAvailable += LoggerOutputAvailable;

            OutputCopyright();

            m_timer = null;
            m_demo_mode = DemoMode.None;

            m_play_menu.DropDownOpening += PlayMenuBeforeOpen;
            m_edit_menu.DropDownOpening += EditMenuBeforeOpen;
            m_file_menu.DropDownOpening += FileMenuBeforeOpen;

            m_undo_stack_usage.DoubleClickEnabled = true;
            m_undo_stack_usage.DoubleClick += UndoUsageDoubleClickHandler;
        }


        private void UndoUsageDoubleClickHandler(object sender, EventArgs e)
        {
            if (m_undo_stack.Count > 0)
            {
                List<string> diff = new List<string>();
                PathFileDiff.Diff(m_undo_stack[m_undo_stack.Count - 1].File, m_file, diff);
                m_logger.LogMessage(Logger.MessageType.Debug, "=========================== DIFF ======================================");
                foreach (string str in diff)
                    m_logger.LogMessage(Logger.MessageType.Debug, str);
            }
        }

        private XeroPathAppDefaults ReadDefaults()
        {
            XeroPathAppDefaults defaults = null;

            string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XeroPath", "defaults.json");
            if (File.Exists(appdata))
            {
                string json = File.ReadAllText(appdata);
                try
                {
                    defaults = JsonConvert.DeserializeObject<XeroPathAppDefaults>(json);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    defaults = null;
                }
            }

            if (defaults == null || defaults.DefaultLengthUnits == null)
            {
                defaults = new XeroPathAppDefaults();
                defaults.DefaultLengthUnits = "inches";
                defaults.UndoStackSize = 20;
            }

            return defaults;
        }

        private void StartTimer()
        {
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Interval = 100;
            m_timer.Tick += RobotFieldTimerTick;
            m_timer.Enabled = true;
            SetTime(0.0);
        }

        private void StopTimer()
        {
            if (m_timer != null)
            {
                m_timer.Tick -= RobotFieldTimerTick;
                m_timer.Enabled = false;
                m_timer = null;
                SetTime(0.0);
            }
            
        }

        private bool NextGroupInFile()
        {
            bool ret = false;
            int index;

            if (m_selected_group == null)
                index = 0;
            else
            {
                index = Array.IndexOf(m_file.Groups, m_selected_group);
                if (index == m_file.Groups.Length - 1)
                {
                    ret = true;
                    index = 0;
                }
                else
                    index++;
            }

            SetPath(null);
            SetGroup(m_file.Groups[index]);
            NextPathInGroup();

            return ret;
        }

        private bool NextPathInGroup()
        {
            Debug.Assert(m_selected_group != null);

            int index;
            bool ret = false;

            if (m_selected_path == null)
                index = 0;
            else
            {
                index = Array.IndexOf(m_selected_group.Paths, m_selected_path);
                if (index == m_selected_group.Paths.Length - 1)
                {
                    index = 0;
                    ret = true;
                }
                else
                    index++;
            }

            SetPath(m_selected_group.Paths[index]);
            SetTime(0.0);

            return ret;
        }

        private bool StepPathDemo()
        {
            Debug.Assert(m_selected_path != null);

            double time = m_detailed.Time + 0.1;
            try
            {
                if (time > m_selected_path.TotalTime)
                    return true;
            }
            catch (NoSegmentsException)
            {
                return false;
            }

            SetTime(time);
            return false;
        }

        private void RobotFieldTimerTick(object sender, EventArgs e)
        {
            if (m_demo_mode == DemoMode.None)
            {
                StopTimer();
            }
            else
            {
                if (StepPathDemo())
                {
                    if (m_demo_mode == DemoMode.Path)
                    {
                        SetTime(0.0);
                    }
                    else
                    {
                        //
                        // Group mode or complete file mode
                        //
                        if (NextPathInGroup())
                        {
                            //
                            // Note, NextPathInGroup sets up to repeat the group if we are at the
                            // end of the last path.  We only have to take action if we are in
                            // file mode.
                            //
                            if (m_demo_mode == DemoMode.File)
                            {
                                //
                                // If we are at the last group, NextGroupInFile repeats from the start
                                // of the file.
                                //
                                NextGroupInFile();
                            }
                        }
                    }
                }
            }
        }

        void SetTime(double time, bool user = false)
        {
            if (InvokeRequired)
                Invoke(new SetTimeDelegate(SetTime), new object[] { time , user});
            else
            {
                m_detailed.Time = time;
                if (!user)
                    m_plot.Time = time;
            }
        }

        void SetPath(RobotPath path)
        {
            if (path != m_selected_path)
            {
                m_selected_path = path;
                m_field.HighlightPoint = null;
                m_plot.Path = path;
                m_field.DisplayedPath = path;
                m_detailed.DisplayedPath = path;
                SetTime(0.0);

                if (path == null)
                {
                    m_misc_status.Text = "Selected Group: None, Selected Path: None";
                }
                else
                {
                    PathGroup group = m_file.FindGroupByPath(path);
                    m_misc_status.Text = "Selected Group: " + group.Name + ", Selected Path: " + path.Name;
                }
            }

            if ((m_demo_mode == DemoMode.Group || m_demo_mode == DemoMode.File) && m_selected_path != null)
            {
                Debug.Assert(m_selected_group != null);
                Debug.Assert(m_selected_path != null);

                foreach(TreeNode tnode in m_pathfile_tree.Nodes)
                {
                    if (tnode.Text == m_selected_group.Name)
                    {
                        foreach(TreeNode pnode in tnode.Nodes)
                        {
                            if (pnode.Text == m_selected_path.Name)
                            {
                                m_pathfile_tree.SelectedNode = pnode;
                                pnode.EnsureVisible();
                                tnode.EnsureVisible();
                            }
                        }
                    }
                }
            }
        }

        void SetGroup(PathGroup gr)
        {
            if (m_selected_group != gr)
            {
                m_selected_group = gr;
                if (gr == null)
                    m_misc_status.Text = "Selected Group: None, Selected Path: None";
                else
                    m_misc_status.Text = "Selected Group: " + gr.Name;
            }
        }

        void SetUnits()
        {
            m_plot.Units = m_file.Robot.LengthUnits;
            m_current_game.Units = m_file.Robot.LengthUnits;
            m_field.Units = m_file.Robot.LengthUnits;
        }

        private void M_flow_Layout(object sender, LayoutEventArgs e)
        {
            m_right_one.Width = m_flow.Width - m_flow.Margin.Left - m_flow.Margin.Right;
            m_right_two.Width = m_flow.Width - m_flow.Margin.Left - m_flow.Margin.Right;
            m_right_three.Width = m_flow.Width - m_flow.Margin.Left - m_flow.Margin.Right;
            m_right_four.Width = m_flow.Width - m_flow.Margin.Left - m_flow.Margin.Right;

            int height = m_flow.Height - m_flow.Margin.Top - m_flow.Margin.Bottom;
            height -= m_right_one.Margin.Top + m_right_one.Margin.Bottom;
            height -= m_right_two.Margin.Top + m_right_two.Margin.Bottom;
            height -= m_right_three.Margin.Top + m_right_three.Margin.Bottom;
            height -= m_right_four.Margin.Top + m_right_four.Margin.Bottom;

            m_right_one.Height = (int)(height * 0.40);
            m_right_two.Height = (int)(height * 0.20);
            m_right_three.Height = (int)(height * 0.20);
            m_right_four.Height = (int)(height * 0.20);
        }

        private void M_vertical_Layout(object sender, LayoutEventArgs e)
        {
            m_vertical.SplitterDistance = m_vertical.Height * 5 / 8;
        }

        private void M_split_Layout(object sender, LayoutEventArgs e)
        {
        }
        #endregion

        #region event handlers for the logger window
        private void LoggerOutputAvailable(object sender, LoggerOutputEventArgs e)
        {
            if (e.MessageType != Logger.MessageType.Info)
                m_logger_window.Text += e.MessageType.ToString() + ": " + e.MessageText + "\r\n";
            else
                m_logger_window.Text += e.MessageText + "\r\n";

            m_logger_window.SelectionStart = m_logger_window.Text.Length;
            m_logger_window.ScrollToCaret();
        }
        #endregion

        #region event handlers for the plot child control

        private void PlotWindowTimeChanged(object sender, TimeCursorMovedArgs e)
        {
            SetTime(e.Time, true);

            Nullable<PointF> wpt = FindPointAtTime(m_selected_path, e.Time);
            if (wpt == null)
                return;

            m_field.HighlightPoint = wpt;
            m_field.SelectedWaypoint = null;
        }
        #endregion

        #region event handlers for the field child control
        private void FieldKeyPreview(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                e.IsInputKey = true;
        }

        private void FieldKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.Insert &&
                e.KeyCode != Keys.Up && e.KeyCode != Keys.Down &&
                e.KeyCode != Keys.Left && e.KeyCode != Keys.Right &&
                e.KeyCode != Keys.Add && e.KeyCode != Keys.Subtract &&
                e.KeyCode != Keys.PageDown && e.KeyCode != Keys.PageUp &&
                e.KeyCode != Keys.Z)
                return;

            e.Handled = true;

            if (e.KeyCode == Keys.Z)
            {
                if (e.Control)
                    PopUndoStack();

                return;
            }

            bool gen = false;

            if (e.KeyCode == Keys.Delete)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to delete the waypoint", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (m_selected_path.RemovePoint(m_field.SelectedWaypoint))
                    {
                        gen = true;
                        m_file.IsDirty = true;
                        m_field.SelectedWaypoint = null;
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete the first or last waypoint in a path", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to insert a new waypoint", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    WayPoint pt = m_selected_path.InsertPoint(m_field.SelectedWaypoint, m_selected_path.MaxVelocity);
                    if (pt != null)
                    {
                        gen = true;
                        m_file.IsDirty = true;
                        m_field.SelectedWaypoint = pt;
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.Y += UnitConverter.Convert(0.1, "inches", m_file.Robot.LengthUnits);
                    else
                        m_field.SelectedWaypoint.Y += UnitConverter.Convert(1.0, "inches", m_file.Robot.LengthUnits);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.Y -= UnitConverter.Convert(0.1, "inches", m_file.Robot.LengthUnits);
                    else
                        m_field.SelectedWaypoint.Y -= UnitConverter.Convert(1.0, "inches", m_file.Robot.LengthUnits);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.X -= UnitConverter.Convert(0.1, "inches", m_file.Robot.LengthUnits);
                    else
                        m_field.SelectedWaypoint.X -= UnitConverter.Convert(1.0, "inches", m_file.Robot.LengthUnits);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.X += UnitConverter.Convert(0.1, "inches", m_file.Robot.LengthUnits);
                    else
                        m_field.SelectedWaypoint.X += UnitConverter.Convert(1.0, "inches", m_file.Robot.LengthUnits);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }
            else if (e.KeyCode == Keys.Add || e.KeyCode == Keys.PageUp)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.Heading = XeroUtils.BoundDegrees(m_field.SelectedWaypoint.Heading + 0.5);
                    else
                        m_field.SelectedWaypoint.Heading = XeroUtils.BoundDegrees(m_field.SelectedWaypoint.Heading + 5.0);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.PageDown)
            {
                if (m_field.SelectedWaypoint == null)
                    MessageBox.Show("Must select a waypoint to move it", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (e.Shift)
                        m_field.SelectedWaypoint.Heading = XeroUtils.BoundDegrees(m_field.SelectedWaypoint.Heading - 0.5);
                    else
                        m_field.SelectedWaypoint.Heading = XeroUtils.BoundDegrees(m_field.SelectedWaypoint.Heading - 5.0);

                    m_file.IsDirty = true;
                    gen = true;
                }
            }

            if (gen)
                UpdateCurrentPath();
        }

        private void FieldMouseMoved(object sender, FieldMouseMovedArgs e)
        {
            string pos = "Position: " + e.WorldPoint.X.ToString("F1") + "," + e.WorldPoint.Y.ToString("F1");
            //pos += "   " + e.WindowPoint.X.ToString("F1") + "," + e.WindowPoint.Y.ToString("F1");
            //pos += "   " + e.BackPoint.X.ToString("F1") + "," + e.BackPoint.Y.ToString("F1");
            m_pos_status.Text = pos;
        }

        private void WayPointSelectedOrChanged(object sender, WaypointEventArgs e)
        {
            if (e.Path != null)
            {
                if (e.Reason == WaypointEventArgs.ReasonType.Selected)
                    HighlightTime(e.Path, e.Point);

                if (e.Reason == WaypointEventArgs.ReasonType.StartChange)
                    PushUndoStack();

                if (e.Reason == WaypointEventArgs.ReasonType.Changing)
                    GenerateSplines(e.Path);

                if (e.Reason == WaypointEventArgs.ReasonType.EndChange)
                {
                    GenerateSplines(e.Path);
                    GenerateSegments(e.Path);
                    HighlightTime(e.Path, e.Point);
                }
            }

            if (e.Point != null)
                UpdateWaypointPropertyWindow();

            if (e.Reason == WaypointEventArgs.ReasonType.Unselected)
            {
                m_plot.Time = 0.0;
                m_waypoint_view.Items.Clear();
            }
        }
        #endregion

        #region event handlers for the path tree control

        private void PathTreeDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = m_pathfile_tree.PointToClient(new Point(e.X, e.Y));
                TreeNode drag = e.Data.GetData("System.Windows.Forms.TreeNode") as TreeNode;
                TreeNode dest = m_pathfile_tree.GetNodeAt(pt);
                if (dest.Parent != null)
                    dest = dest.Parent;

                //
                // Need to move the paths between the path groups
                //
                PathGroup destgroup = m_file.FindGroupByName(dest.Text);
                PathGroup srcgroup = m_file.FindGroupByName(drag.Parent.Text);
                RobotPath path = srcgroup.FindPathByName(drag.Text);

                //
                // Remove from the source group
                //
                srcgroup.RemovePath(drag.Text);

                //
                // Add to the destination group
                //
                if (destgroup.FindPathByName(path.Name) != null)
                {
                    string newname = GetCopyPathName(destgroup, path);
                    path.Name = newname;
                    drag.Text = newname;
                }
                destgroup.AddPath(path);

                //
                // Need to move the nodes in the tree
                //
                drag.Parent.Nodes.Remove(drag);
                dest.Nodes.Add(drag);
                m_file.IsDirty = true;
            }
        }

        private void PathTreeDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void PathTreePathDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void PathTreeSelectionChanged(object sender, TreeViewEventArgs e)
        {
            if (m_demo_mode == DemoMode.None)
            {
                if (e.Node.Parent == null)
                {
                    // A group is selected
                    SetPath(null);
                    SetGroup(m_file.FindGroupByName(e.Node.Text));
                }
                else
                {
                    RobotPath path = m_file.FindPathByName(e.Node.Parent.Text, e.Node.Text);
                    SetGroup(null);
                    SetPath(path);
                }
                m_field.SelectedWaypoint = null;
                UpdatePathWindow();
            }
        }

        private void PathTreeMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (m_old_node != null)
                return;

            Point pt = new Point(e.X, e.Y);
            TreeNode tn = m_pathfile_tree.GetNodeAt(pt);
            if (tn == null)
                return;

            m_old_node = m_pathfile_tree.SelectedNode;
            m_pathfile_tree.SelectedNode = tn;

            ContextMenu menu = new ContextMenu();
            MenuItem mi;

            if (tn.Parent == null)
            {
                mi = new MenuItem("Add Path Group", NewPathGroupToolStripMenuItem_Click);
                menu.MenuItems.Add(mi);

                mi = new MenuItem("Delete Path Group", RemovePathGroupToolStripMenuItem_Click);
                menu.MenuItems.Add(mi);
            }

            mi = new MenuItem("Add Path", NewPathToolStripMenuItemHandler);
            menu.MenuItems.Add(mi);

            if (tn.Parent != null)
            {
                mi = new MenuItem("Copy Path", CopyPathToolStripMenuItemHandler);
                menu.MenuItems.Add(mi);

                mi = new MenuItem("Delete Path", RemovePathToolStripMenuItem_Click);
                menu.MenuItems.Add(mi);
            }

            menu.Show(m_pathfile_tree, pt);

            m_pathfile_tree.SelectedNode = m_old_node;
            m_old_node = null;
        }
        private void PathTreeCheckBoxChanged(object sender, TreeViewEventArgs e)
        {

        }

        private void DoPathTreeDoubleClick(object sender, EventArgs e)
        {
            TreeNode tn = m_pathfile_tree.SelectedNode;
            if (tn != null)
            {
                m_ignore_lost_focus = false;
                EditGroupOrPathName(tn);
            }
        }
        #endregion

        #region event handlers for the path view window

        private void PathViewDoubleClick(object sender, EventArgs e)
        {
            if (m_text_editor != null || m_path_view.SelectedItems.Count != 1)
                return;

            if (m_timer != null)
                m_timer.Enabled = false;

            ListViewItem item = m_path_view.SelectedItems[0];

            if (item.Text == "Total Time")
                return;

            m_ignore_lost_focus = false;
            m_path_view_editing = item;
            Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);

            m_text_editor = new TextBox();
            m_text_editor.Text = item.SubItems[1].Text;
            m_text_editor.Bounds = b;
            m_text_editor.Parent = m_path_view;
            m_text_editor.Enabled = true;
            m_text_editor.Visible = true;
            m_text_editor.LostFocus += FinishedEditingProperty;
            m_text_editor.PreviewKeyDown += PreviewEditorKeyProperty;
            m_text_editor.Focus();
        }
        #endregion

        #region event handlers for the robot params window

        private void SetComboInitialValue(string value)
        {
            for(int i = 0; i < m_combo_editor.Items.Count; i++)
            {
                string str = m_combo_editor.Items[i] as string;
                if (str != null && str == value)
                {
                    m_combo_editor.SelectedIndex = i;
                    break;
                }
            }
        }
        private void RobotParamDoubleClick(object sender, EventArgs e)
        {
            if (m_text_editor != null || m_robot_view.SelectedItems.Count != 1 || m_combo_editor != null)
                return;

            if (m_timer != null)
                m_timer.Enabled = false;

            ListViewItem item = m_robot_view.SelectedItems[0];
            m_robot_param_editing = item;

            if (item.Text == TypeLabel)
            {
                Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);
                m_ignore_lost_focus = false;
                m_combo_editor = new ComboBox();
                foreach (string sitem in SupportedDriveBaseTypes)
                    m_combo_editor.Items.Add(sitem);
                SetComboInitialValue(m_file.Robot.DriveType);
                m_combo_editor.DropDownStyle = ComboBoxStyle.DropDownList;
                m_combo_editor.Enabled = true;
                m_combo_editor.Visible = true;
                m_combo_editor.Bounds = b;
                m_combo_editor.Parent = m_robot_view;
                m_combo_editor.LostFocus += FinishedEditingProperty;
                m_combo_editor.SelectedIndexChanged += SelectedComboItemChanged;
                m_combo_editor.Focus();
            }
            else if (item.Text == AngleUnitsLabel)
            {
                List<string> supported = new List<string>() { "degrees", "radians" };

                Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);
                m_ignore_lost_focus = false;
                m_combo_editor = new ComboBox();
                foreach (string sitem in supported)
                    m_combo_editor.Items.Add(sitem);
                SetComboInitialValue(m_file.Robot.AngleUnits);
                m_combo_editor.DropDownStyle = ComboBoxStyle.DropDownList;
                m_combo_editor.Enabled = true;
                m_combo_editor.Visible = true;
                m_combo_editor.Bounds = b;
                m_combo_editor.Parent = m_robot_view;
                m_combo_editor.LostFocus += FinishedEditingProperty;
                m_combo_editor.SelectedIndexChanged += SelectedComboItemChanged;
                m_combo_editor.Focus();
            }
            else if (item.Text == UnitsLabel)
            {
                List<string> supported = UnitConverter.SupportedUnits;

                Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);
                m_ignore_lost_focus = false;
                m_combo_editor = new ComboBox();
                foreach (string sitem in supported)
                    m_combo_editor.Items.Add(sitem);
                SetComboInitialValue(m_file.Robot.LengthUnits);
                m_combo_editor.DropDownStyle = ComboBoxStyle.DropDownList;
                m_combo_editor.Enabled = true;
                m_combo_editor.Visible = true;
                m_combo_editor.Bounds = b;
                m_combo_editor.Parent = m_robot_view;
                m_combo_editor.LostFocus += FinishedEditingProperty;
                m_combo_editor.SelectedIndexChanged += SelectedComboItemChanged;
                m_combo_editor.Focus();
            }
            else
            {

                Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);
                m_ignore_lost_focus = false;
                m_text_editor = new TextBox();
                m_text_editor.Text = item.SubItems[1].Text;
                m_text_editor.Bounds = b;
                m_text_editor.Parent = m_robot_view;
                m_text_editor.Enabled = true;
                m_text_editor.Visible = true;
                m_text_editor.LostFocus += FinishedEditingProperty;
                m_text_editor.PreviewKeyDown += PreviewEditorKeyProperty;
                m_text_editor.Focus();
            }
        }

        private void SelectedComboItemChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region event handlers for the waypoints window
        private void WaypointDoubleClick(object sender, EventArgs e)
        {
            if (m_text_editor != null || m_waypoint_view.SelectedItems.Count != 1 || m_field.SelectedWaypoint == null)
                return;

            ListViewItem item = m_waypoint_view.SelectedItems[0];
            if (item.Text == "Group" || item.Text == "Path")
                return;

            if (m_timer != null)
                m_timer.Enabled = false;

            m_waypoint_editing = item;
            Rectangle b = new Rectangle(item.SubItems[1].Bounds.Left, item.SubItems[1].Bounds.Top, item.SubItems[1].Bounds.Width, item.SubItems[1].Bounds.Height);
            m_ignore_lost_focus = false;
            m_text_editor = new TextBox();
            m_text_editor.Text = item.SubItems[1].Text;
            m_text_editor.Bounds = b;
            m_text_editor.Parent = m_waypoint_view;
            m_text_editor.Enabled = true;
            m_text_editor.Visible = true;
            m_text_editor.LostFocus += FinishedEditingProperty;
            m_text_editor.PreviewKeyDown += PreviewEditorKeyProperty;
            m_text_editor.Focus();
        }
        #endregion

        #region event handlers for the top level form

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (m_file.IsDirty)
            {
                if (MessageBox.Show("The paths file has been changed.  Closing now will discard these changes.  Do you really want to quit?",
                        "Really Quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel && m_generator != null)
                m_generator.Stop();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
        }

        private void PreviewEditorKeyProperty(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Cancel || e.KeyData == Keys.Escape)
            {
                StopEditing();
            }
            else if (e.KeyData == Keys.Return)
            {
                FinishedEditingProperty(sender, EventArgs.Empty);
            }
        }

        private void FinishedEditingProperty(object sender, EventArgs args)
        {
            bool stop = true;

            if (m_ignore_lost_focus)
                return;

            if (m_robot_param_editing != null)
                stop = FinishedEditingRobotParam();
            else if (m_waypoint_editing != null)
                stop = FinishedEditingWaypoint();
            else if (m_path_view_editing != null)
                stop = FinishedEditingPathView();

            if (stop)
            {
                m_ignore_lost_focus = true;
                StopEditing();
            }
            else
            {
                m_ignore_lost_focus = false;
                m_text_editor.Focus();
            }
        }


        private void PreviewEditorKeyPathTree(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Cancel || e.KeyData == Keys.Escape)
            {
                StopEditing();
            }
            else if (e.KeyData == Keys.Return)
            {
                FinishedEditingNamePathTree(sender, EventArgs.Empty);
            }
        }

        private void FinishedEditingNamePathTree(object sender, EventArgs e)
        {
            string name = m_text_editor.Text;

            if (m_editing_pathtree.Parent == null)
            {
                PushUndoStack();
                if (m_file.RenameGroup(m_editing_pathtree.Text, m_text_editor.Text))
                {
                    m_editing_pathtree.Text = m_text_editor.Text;
                    StopEditing();
                }
            }
            else
            {
                PushUndoStack();
                if (m_file.RenamePath(m_editing_pathtree.Parent.Text, m_editing_pathtree.Text, m_text_editor.Text))
                {
                    m_editing_pathtree.Text = m_text_editor.Text;
                    StopEditing();
                }
            }
        }
        #endregion

        #region event handlers for the menus

        private void AboutMenuItemHandler(object sender, EventArgs e)
        {
            XeroPathAbout about = new XeroPathAbout();
            about.ShowDialog();
        }

        private void DocumentationMenuItemHandler(object sender, EventArgs e)
        {
            string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XeroPath.pdf");
            System.Diagnostics.Process.Start(app);
        }

        private void FileMenuBeforeOpen(object sender, EventArgs e)
        {
            (m_file_menu.DropDownItems[8] as ToolStripMenuItem).Enabled = m_file.AllSegmentsReady;
            (m_file_menu.DropDownItems[9] as ToolStripMenuItem).Enabled = m_file.AllSegmentsReady;
        }

        private void EditMenuBeforeOpen(object sender, EventArgs e)
        {
            if (m_field.SelectedWaypoint != null)
            {
                (m_edit_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = true;
                (m_edit_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = true;
            }
            else
            {
                (m_edit_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = false;
                (m_edit_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = false;
            }

            if (m_selected_group != null)
                (m_edit_menu.DropDownItems[4] as ToolStripMenuItem).Enabled = true;
            else
                (m_edit_menu.DropDownItems[4] as ToolStripMenuItem).Enabled = false;

            if (m_undo_stack.Count != 0)
                (m_edit_menu.DropDownItems[6] as ToolStripMenuItem).Enabled = true;
            else
                (m_edit_menu.DropDownItems[6] as ToolStripMenuItem).Enabled = false;
        }

        private void PlayMenuBeforeOpen(object sender, EventArgs e)
        {
            switch(m_demo_mode)
            {
                case DemoMode.File:
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Checked = true;
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Enabled = true;

                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = false;

                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = false;
                    break;

                case DemoMode.Group:
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Enabled = false;

                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Checked = true;
                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = true;

                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = false;
                    break;

                case DemoMode.Path:
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Enabled = false ;

                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Checked = false;
                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = false;

                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Checked = true;
                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = true;
                    break;

                case DemoMode.None:

                    (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Checked = false;
                    if (m_file.TotalPaths > 0)
                        (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Enabled = true;
                    else
                        (m_play_menu.DropDownItems[2] as ToolStripMenuItem).Enabled = false;


                    (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Checked = false;
                    if (m_selected_group != null)
                        (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = true;
                    else
                        (m_play_menu.DropDownItems[1] as ToolStripMenuItem).Enabled = false;

                    (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Checked = false;
                    if (m_selected_path != null)
                        (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = true;
                    else
                        (m_play_menu.DropDownItems[0] as ToolStripMenuItem).Enabled = false;

                    break;
            }
        }

        private void PlayMenuFileDemo(object sender, EventArgs e)
        {
            if (m_demo_mode == DemoMode.None)
            {
                m_demo_mode = DemoMode.File;
                m_field_tab.SelectedTab = m_detailed_tab_page;
                SetPath(null);
                SetGroup(null);
                NextGroupInFile();
                SetTime(0.0);
                StartTimer();
            }
            else
            {
                m_demo_mode = DemoMode.None;
                StopTimer();
                SetGroup(null);
                SetPath(null);
                SetTime(0.0);
            }
        }

        private void PlayMenuGroupDemo(object sender, EventArgs e)
        {
            if (m_demo_mode == DemoMode.None)
            {
                m_demo_mode = DemoMode.Group;
                m_field_tab.SelectedTab = m_detailed_tab_page;
                SetPath(null);
                NextPathInGroup();
                SetTime(0.0);
                StartTimer();
            }
            else
            {
                m_demo_mode = DemoMode.None;
                StopTimer();
                SetPath(null);
                SetTime(0.0);
            }
        }

        private void PatyMenuPathDemo(object sender, EventArgs e)
        {
            if (m_demo_mode == DemoMode.None)
            {
                m_demo_mode = DemoMode.Path;
                m_field_tab.SelectedTab = m_detailed_tab_page;
                SetTime(0.0);
                StartTimer();
            }
            else
            {
                m_demo_mode = DemoMode.None;
                StopTimer();
                SetTime(0.0);
            }
        }


        private void EditPreferencesHandler(object sender, EventArgs e)
        {
            XeroPathAppDefaultsForm form = new XeroPathAppDefaultsForm(m_defaults);
            if (form.ShowDialog() == DialogResult.OK)
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (!Directory.Exists(appdata))
                {
                    MessageBox.Show("Cannot save preferences, appdata directory '" + appdata + "' does not exist", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                appdata = Path.Combine(appdata, "XeroPath");
                if (!Directory.Exists(appdata))
                {
                    DirectoryInfo info;

                    try
                    {
                        info = Directory.CreateDirectory(appdata);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot save preferences, cannot create directory '" + appdata + "' - " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                appdata = Path.Combine(appdata, "defaults.json");
                string json = JsonConvert.SerializeObject(m_defaults);
                try
                {
                    File.WriteAllText(appdata, json);

                    UpdatePreferences();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not write preferences file - " + ex.Message, "Error Saving File",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void InsertWaypointHandler(object sender, EventArgs e)
        {
            if (m_field.SelectedWaypoint == null)
            {
                MessageBox.Show("Must select a waypoint to insert a new waypoint", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WayPoint pt = m_selected_path.InsertPoint(m_field.SelectedWaypoint, m_selected_path.MaxVelocity);
            if (pt != null)
            {
                m_file.IsDirty = true;
                m_field.SelectedWaypoint = pt;
                UpdateCurrentPath();
            }
        }

        private void DeleteWaypointHandler(object sender, EventArgs e)
        {
            if (m_field.SelectedWaypoint == null)
            {
                MessageBox.Show("Must select a waypoint to delete the waypoint", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_selected_path.RemovePoint(m_field.SelectedWaypoint))
            {
                m_file.IsDirty = true;
                m_field.SelectedWaypoint = null;
                UpdateCurrentPath();
            }
        }

        private void NewMenuItemEventHandler(object sender, EventArgs e)
        {
            CloseMenuItemEventHandler(sender, e);
        }

        private void CloseMenuItemEventHandler(object sender, EventArgs e)
        {
            if (m_file.IsDirty)
            {
                DialogResult dr = MessageBox.Show("You have unsaved changes. Are you sure you want to close this path file", "Really Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                    return;
            }

            m_file = new PathFile();
            m_field.File = m_file;
            m_field.SelectedWaypoint = null;

            SetPath(null);
            UpdateRobotWindow();
            UpdateWaypointPropertyWindow();
            UpdatePathWindow();
            UpdatePathTree();
        }

        private string GetRelativeIfPossible(string destdir, string jsonfile)
        {
            string result = PathUtils.GetRelativePath(jsonfile, destdir);
            if (result != null)
                return result;

            return destdir;
        }

        private void GeneratePathAsMenuItem(object sender, EventArgs e)
        {
            if (m_file.IsDirty)
            {
                DialogResult dr = MessageBox.Show("You have unsaved changes. Do you want to save before generating the paths", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.Yes)
                    SaveAsToolStripMenuItem_Click(sender, e);
            }

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(m_file.PathName))
                dialog.SelectedPath = Path.GetDirectoryName(m_file.PathName);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_file.PathDirectory = GetRelativeIfPossible(dialog.SelectedPath, m_file.PathName);
                m_file.IsDirty = true;

                DialogResult dr = MessageBox.Show("You have unsaved changes. Do you want to save before generating the paths", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.Yes)
                    SaveToolStripMenuItem_Click(sender, e);

                GeneratePaths();
            }
        }

        private void GeneratePathMenuItem(object sender, EventArgs e)
        {
            if (m_file.IsDirty)
            {
                DialogResult dr = MessageBox.Show("You have unsaved changes. Do you want to save before generating the paths", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.Yes)
                    SaveToolStripMenuItem_Click(sender, e);
            }

            if (string.IsNullOrEmpty(m_file.PathDirectory))
                GeneratePathAsMenuItem(sender, e);
            else
            {
                GeneratePaths();
            }
        }

        private void UndoMenuItem(object sender, EventArgs e)
        {
            PopUndoStack();
        }

        private void GeneratorSelected(object sender, EventArgs args)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            //
            // Uncheck all generators
            //
            ToolStrip strip = item.GetCurrentParent();
            if (strip != null)
            {
                foreach (var vitem in strip.Items)
                {
                    ToolStripMenuItem mitem = vitem as ToolStripMenuItem;
                    if (mitem != null)
                        mitem.Checked = false;
                }
            }

            //
            // Check the one generator selected
            //
            item.Checked = true;

            //
            // Initialize the generator
            //
            InitializeGenerator(item.Tag as PathGenerator);
        }

        private void GameSelected(object sender, EventArgs args)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            //
            // Uncheck all games
            //
            ToolStrip strip = item.GetCurrentParent();
            if (strip != null)
            {
                foreach (var vitem in strip.Items)
                {
                    ToolStripMenuItem mitem = vitem as ToolStripMenuItem;
                    if (mitem != null)
                        mitem.Checked = false;
                }
            }

            item.Checked = true;
            InitializeGame(item.Tag as Game);
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_file != null && m_file.IsDirty)
            {
                DialogResult result = MessageBox.Show(
                    "The current path file has unsaved changes.  Do you want to save this file first?",
                    "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (!SaveChanges())
                    {
                        MessageBox.Show("Could not save changes, open operation aborted", "Open Aborted",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                    return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open a Path File";
            dialog.Filter = "Path Files (*.json)|*.json|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string json = File.ReadAllText(dialog.FileName);
                try
                {
                    m_file = JsonConvert.DeserializeObject<PathFile>(json);
                    if (string.IsNullOrEmpty(m_file.Robot.AngleUnits))
                        m_file.Robot.AngleUnits = "Degrees" ;

                    SetUnits();
                }
                catch(Newtonsoft.Json.JsonSerializationException ex)
                {
                    string msg = "Cannot load path file '" + dialog.FileName + "' - invalid file contents - " + ex.Message;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                m_file.PathName = dialog.FileName;
                m_field.File = m_file;
                m_detailed.Robot = m_file.Robot;
                Text = "Path Editor - " + m_file.PathName;
                m_undo_stack = new List<UndoState>();
                GenerateAllSplines();
                UpdatePathTree();
                UpdateRobotWindow();
                UpdateRecentFileList(dialog.FileName);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save a Path File";
            dialog.Filter = "Path Files (*.json)|*.json|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_file.PathName = dialog.FileName;
                Text = "Path Editor - " + m_file.PathName;
                SaveChanges();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_file.PathName))
                SaveAsToolStripMenuItem_Click(sender, e);
            else
                SaveChanges();
        }

        private void NewPathGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PushUndoStack();

            string newname = GetNewPathGroupName();
            m_file.AddPathGroup(newname);

            TreeNode node = new TreeNode(newname);
            m_pathfile_tree.Nodes.Add(node);
            m_pathfile_tree.SelectedNode = node;
        }

        private void NewPathToolStripMenuItemHandler(object sender, EventArgs e)
        {
            TreeNode node = m_pathfile_tree.SelectedNode;
            if (node == null)
                return;

            if (node.Parent != null)
                node = node.Parent;

            PushUndoStack();
            string newname = GetNewPathName(node);
            TreeNode nnode = new TreeNode(newname);
            node.Nodes.Add(nnode);
            m_pathfile_tree.SelectedNode = nnode;
            m_file.AddPath(node.Text, newname);
            PathTreeSelectionChanged(m_pathfile_tree, new TreeViewEventArgs(nnode));
            GenerateAllSplines();
        }

        private string GetCopyPathName(PathGroup parent, RobotPath tocopy)
        {
            string name = tocopy.Name + "-Copy";
            int i = 0;

            do
            {
                if (parent.FindPathByName(name) == null)
                    break;

                i++;
                name = tocopy.Name + "-Copy-" + i.ToString();

            } while (true);

            return name;
        }

        private void CopyPathToolStripMenuItemHandler(object sender, EventArgs e)
        {
            TreeNode node = m_pathfile_tree.SelectedNode;
            if (node == null)
                return;

            PushUndoStack();

            string grname = node.Parent.Text;
            PathGroup parent = m_file.FindGroupByName(grname);
            if (parent == null)
                return;

            RobotPath tocopy = parent.FindPathByName(node.Text);
            RobotPath path = new RobotPath(tocopy);
            path.Name = GetCopyPathName(parent, tocopy);
            parent.AddPath(path);

            TreeNode newnode = new TreeNode(path.Name);
            node.Parent.Nodes.Add(newnode);

            GenerateSplines(path);
            GenerateSegments(path);
        }

        private void RemovePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = m_pathfile_tree.SelectedNode;
            if (node == null)
                return;

            PushUndoStack();

            string grname = node.Parent.Text;
            node.Parent.Nodes.Remove(node);
            RobotPath path = m_file.FindPathByName(grname, node.Text);
            if (m_field.DisplayedPath == path)
                m_field.DisplayedPath = null;

            if (m_detailed.DisplayedPath == path)
                m_detailed.DisplayedPath = null;

            m_file.RemovePath(grname, node.Text);
        }

        private void RemovePathGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = m_pathfile_tree.SelectedNode;
            if (node == null)
                return;

            m_pathfile_tree.Nodes.Remove(node);
            PushUndoStack();
            m_file.RemoveGroup(node.Text);
        }
        #endregion

        #region private methods

        #region undo stack
        private void PushUndoStack()
        {
            PathFile pf = new PathFile(m_file);
            UndoState st = new UndoState(m_undo_serial++, pf);

            if (m_demo_mode == DemoMode.None)
            {
                if (m_selected_group != null)
                    st.SelectedGroup = m_selected_group.Name;

                if (m_selected_path != null && m_pathfile_tree.SelectedNode != null && m_pathfile_tree.SelectedNode.Parent != null)
                {
                    PathGroup group = m_file.FindGroupByName(m_pathfile_tree.SelectedNode.Parent.Text);
                    st.SetSelectedPath(group, m_selected_path);
                }

                if (m_field.SelectedWaypoint != null)
                {
                    PathGroup gr;
                    RobotPath path;
                    int index;
                    m_file.FindPathByWaypoint(m_field.SelectedWaypoint, out gr, out path, out index);
                    if (gr != null && path != null)
                        st.SelectedIndex = index;
                }
            }

            m_undo_stack.Add(st);

            //
            // Make sure the undo stack does not grow too large
            //
            while (m_undo_stack.Count > m_undo_length)
                m_undo_stack.RemoveAt(m_undo_stack.Count - 1);

            m_undo_stack_usage.Text = "Undo Stack: " + m_undo_stack.Count.ToString();
        }

        private void PopUndoStack()
        {
            if (m_undo_stack.Count > 0)
            {
                UndoState st = m_undo_stack[m_undo_stack.Count - 1];
                m_file = st.File;
                m_undo_stack.RemoveAt(m_undo_stack.Count - 1);

                m_field.File = m_file;

                GenerateAllSplines();

                SetPath(null);
                SetGroup(null);
                m_field.SelectedWaypoint = null;

                if (st.HasSelectedGroup)
                {
                    PathGroup group = m_file.FindGroupByName(st.SelectedGroup);
                    if (group != null)
                        SetGroup(group);
                }

                if (st.HasSelectedPath)
                {
                    foreach(TreeNode group in m_pathfile_tree.Nodes)
                    {
                        if (group.Text == st.SelectedPath.Item1)
                        {
                            foreach(TreeNode path in group.Nodes)
                            {
                                if (path.Text == st.SelectedPath.Item2)
                                {
                                    m_pathfile_tree.SelectedNode = path;
                                    RobotPath rpath = m_file.FindPathByName(group.Text, path.Text);
                                    SetPath(rpath);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (st.HasSelectedWaypoint)
                {
                    if (m_selected_path != null && st.SelectedIndex < m_selected_path.Points.Length)
                        m_field.SelectedWaypoint = m_selected_path.Points[st.SelectedIndex];
                    else
                        m_field.SelectedWaypoint = null;
                }

                m_detailed.Robot = m_file.Robot;

                m_field.Invalidate();
                m_detailed.Invalidate();
                m_plot.Invalidate();

                UpdateWaypointPropertyWindow();
                UpdateRobotWindow();
                UpdatePathTree();
                UpdatePathWindow();
                m_undo_stack_usage.Text = "Undo Stack: " + m_undo_stack.Count.ToString();
            }
        }
        #endregion

        #region list view editing methods
        private bool FinishedEditingPathView()
        {
            double d;

            if (!double.TryParse(m_text_editor.Text, out d))
                return false;

            if (m_path_view_editing.Text == MaxVelocityLabel)
            {
                PushUndoStack();
                m_selected_path.MaxVelocity = d;
            }
            else if (m_path_view_editing.Text == MaxAccelerationLabel)
            {
                PushUndoStack();
                m_selected_path.MaxAcceleration = d;
            }
            else if (m_path_view_editing.Text == MaxJerkLabel)
            {
                PushUndoStack();
                m_selected_path.MaxJerk = d;
            }
            else if (m_path_view_editing.Text == StartAngleLabel)
            {
                PushUndoStack();
                m_selected_path.StartFacingAngle = XeroUtils.BoundDegrees(d);
            }
            else if (m_path_view_editing.Text == EndAngleLabel)
            {
                PushUndoStack();
                m_selected_path.EndFacingAngle = XeroUtils.BoundDegrees(d);
            }
            else if (m_path_view_editing.Text == RotationStartDelayLabel)
            {
                PushUndoStack();
                m_selected_path.FacingAngleStartDelay = d;
            }
            else if (m_path_view_editing.Text == RotationEndDelayLabel)
            {
                PushUndoStack();
                m_selected_path.FacingAngleEndDelay = d;
            }

            UpdatePathWindow() ;
            m_field.Invalidate();
            m_detailed.Invalidate();
            GenerateSplines(m_selected_path);
            GenerateSegments(m_selected_path);

            return true;
        }

        private bool FinishedEditingRobotParam()
        {
            double d;

            if (m_robot_param_editing.Text == TypeLabel)
            {
                string mytype = (string)m_combo_editor.SelectedItem;
                if (mytype != m_file.Robot.DriveType)
                {
                    PushUndoStack();
                    m_file.Robot.DriveType = mytype;
                }
            }
            else if (m_robot_param_editing.Text == UnitsLabel)
            {
                string myunits = m_combo_editor.SelectedItem as string;

                if (myunits != m_file.Robot.LengthUnits)
                {
                    PushUndoStack();
                    SetUnits();
                    m_file.ConvertUnits(myunits);
                }
            }
            else if (m_robot_param_editing.Text == AngleUnitsLabel)
            {
                string myunits = m_combo_editor.SelectedItem as string;

                if (myunits != m_file.Robot.AngleUnits)
                {
                    PushUndoStack();
                    m_file.Robot.AngleUnits = myunits;
                    m_file.IsDirty = true;
                }
            }
            else
            {
                if (!double.TryParse(m_text_editor.Text, out d))
                    return false;

                PushUndoStack();
                if (m_robot_param_editing.Text == WidthLabel)
                {
                    m_file.Robot.Width = d;
                    m_file.IsDirty = true;
                }
                else if (m_robot_param_editing.Text == LengthLabel)
                {
                    m_file.Robot.Length = d;
                    m_file.IsDirty = true;
                }
                else if (m_robot_param_editing.Text == MaxVelocityLabel)
                {
                    m_file.UpdateMaxVelocity(d);
                    m_file.IsDirty = true;
                }
                else if (m_robot_param_editing.Text == MaxAccelerationLabel)
                {
                    m_file.UpdateMaxAcceleration(d);
                    m_file.IsDirty = true;
                }
                else if (m_robot_param_editing.Text == MaxJerkLabel)
                {
                    m_file.UpdateMaxJerk(d);
                    m_file.IsDirty = true;
                }
                else if (m_robot_param_editing.Text == TimestampLabel)
                {
                    m_file.Robot.TimeStep = d;
                    m_file.IsDirty = true;
                }
            }

            UpdateExistingPaths();
            UpdateRobotWindow();
            UpdateWaypointPropertyWindow();
            GenerateAllSplines();

            m_field.Invalidate();
            m_detailed.Invalidate();
            UpdatePathWindow();
            return true;
        }

        private void UpdateExistingPaths()
        {
            foreach(PathGroup gr in m_file.Groups)
            {
                foreach(RobotPath p in gr.Paths)
                {
                    if (p.MaxVelocity > m_file.Robot.MaxVelocity)
                    {
                        m_file.IsDirty = true;
                        p.MaxVelocity = m_file.Robot.MaxVelocity;
                    }

                    if (p.MaxAcceleration > m_file.Robot.MaxAcceleration)
                    {
                        m_file.IsDirty = true;
                        p.MaxAcceleration = m_file.Robot.MaxAcceleration;
                    }

                    if (p.MaxJerk > m_file.Robot.MaxJerk)
                    {
                        m_file.IsDirty = true;
                        p.MaxJerk = m_file.Robot.MaxJerk;
                    }
                }
            }
        }

        private bool FinishedEditingWaypoint()
        {
            double d;

            if (!double.TryParse(m_text_editor.Text, out d))
                return false;

            PushUndoStack();
            if (m_waypoint_editing.Text == XLabel)
            {
                m_field.SelectedWaypoint.X = d;
                m_field.Invalidate();
            }
            else if (m_waypoint_editing.Text == YLabel)
            {
                m_field.SelectedWaypoint.Y = d;
                m_field.Invalidate();
            }
            else if (m_waypoint_editing.Text == HeadingLabel)
            {
                m_field.SelectedWaypoint.Heading = XeroUtils.BoundDegrees(d);
                m_field.Invalidate();
            }
            else if (m_waypoint_editing.Text == VelocityLabel)
            {
                m_field.SelectedWaypoint.Velocity = d;
                m_field.Invalidate();
            }

            PathGroup group;
            RobotPath path;
            m_file.FindPathByWaypoint(m_field.SelectedWaypoint, out group, out path);
            UpdateWaypointPropertyWindow();

            GenerateSplines(path);
            GenerateSegments(path);

            return true;
        }
        #endregion

        #region menu update methods
        private ToolStripMenuItem FindItem(string title)
        {
            ToolStripMenuItem mitem = null;

            foreach (var item in m_menu.Items)
            {
                ToolStripMenuItem testitem = item as ToolStripMenuItem;
                if (testitem != null && testitem.Text == title)
                {
                    mitem = testitem;
                    break;
                }
            }

            return mitem;
        }

        private void PopulateGameMenu()
        {
            ToolStripMenuItem select = null;
            int year = 0;

            m_games_menu.DropDownItems.Clear();
            foreach (Game g in m_games.Games)
            {
                ToolStripMenuItem mitem = new ToolStripMenuItem(g.Name, null, GameSelected);
                mitem.Tag = g;
                m_games_menu.DropDownItems.Add(mitem);

                if (g.Year > year)
                {
                    select = mitem;
                    year = g.Year;
                }
            }

            GameSelected(select, EventArgs.Empty);
        }

        private void PopulateGeneratorMenu()
        {
            ToolStripMenuItem select = null;

            foreach (PathGenerator g in m_generators.Generators)
            {
                string name = g.Name + " (" + g.Version.ToString() + ")";
                ToolStripMenuItem mitem = new ToolStripMenuItem(name, null, GeneratorSelected);
                mitem.Tag = g;
                m_generators_menu.DropDownItems.Add(mitem);

                if (select == null)
                    select = mitem;
            }

            GeneratorSelected(select, EventArgs.Empty);
        }

        private void ClearChecks(ToolStripMenuItem menu)
        {
            foreach (var item in menu.DropDownItems)
            {
                ToolStripMenuItem mitem = item as ToolStripMenuItem;
                if (mitem != null)
                    mitem.Checked = false;
            }
        }

        private void CheckPlayMenuItem(int offset, bool value)
        {
            ToolStripMenuItem mitem = FindItem("Play");
            foreach (var item in mitem.DropDownItems)
            {
                ToolStripMenuItem subitem = item as ToolStripMenuItem;
                if (subitem != null)
                    subitem.Checked = false;
            }

            if (value)
            {
                ToolStripMenuItem subitem = mitem.DropDownItems[offset] as ToolStripMenuItem;
                if (subitem != null)
                    subitem.Checked = true;
            }
        }
        #endregion

        #region time related methods
        private Nullable<PointF> FindPointAtTime(RobotPath p, double t)
        {
            PathSegment[] segs = p.Segments;
            if (segs == null)
                return null;

            XeroPose pose = p.GetPositionForTime(segs, t);
            return new Nullable<PointF>(new PointF((float)pose.X, (float)pose.Y));
        }

        private double FindTime(RobotPath path, WayPoint pt)
        {
            double lowdist = Double.MaxValue;
            double time = 0.0;

            for(int i = 0; i < path.Segments.Length; i++)
            {
                double x = path.Segments[i].GetValue("x");
                double y = path.Segments[i].GetValue("y");

                double dx = x - pt.X;
                double dy = y - pt.Y;

                double dist = dx * dx + dy * dy;
                if (dist < lowdist)
                {
                    time = path.Segments[i].GetValue("time");
                    lowdist = dist;
                }
            }

            return time;
        }

        private void HighlightTime(RobotPath path, WayPoint pt)
        {
            if (pt == null)
                return;

            if (!path.HasSegments)
            {
                m_plot.Time = 0.0;
                path.SegmentsUpdated += Path_SegmentsUpdated;
                return;
            }

            double time = FindTime(path, pt);
            m_plot.Time = time;
            UpdatePathWindow();
        }
        #endregion

        #region job state methods

        private void UpdateJobsState(PathGenerationStateChangeEvent state)
        {
            if (state.TotalJobs == 0)
                m_running_status.Text = "PathGeneration: Idle";
            else
                m_running_status.Text = "Path Generation: " + state.TotalJobs.ToString() + " jobs";
        }

        private void PathGeneratorJobStateChanged(object sender, PathGenerationStateChangeEvent e)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
                Invoke(new UpdateJobStatusDelegate(UpdateJobsState), new object[] { e });
            else
                UpdateJobsState(e);
        }
        #endregion

        #region windows update methods

        private void UpdateWaypointPropertyWindow()
        {
            ListViewItem item;

            m_waypoint_view.Items.Clear();

            if (m_field.SelectedWaypoint != null)
            {
                item = new ListViewItem(XLabel);
                item.SubItems.Add(m_field.SelectedWaypoint.X.ToString("F1"));
                m_waypoint_view.Items.Add(item);

                item = new ListViewItem(YLabel);
                item.SubItems.Add(m_field.SelectedWaypoint.Y.ToString("F1"));
                m_waypoint_view.Items.Add(item);

                item = new ListViewItem(HeadingLabel);
                item.SubItems.Add(m_field.SelectedWaypoint.Heading.ToString("F1"));
                m_waypoint_view.Items.Add(item);

                item = new ListViewItem(VelocityLabel);
                item.SubItems.Add(m_field.SelectedWaypoint.Velocity.ToString("F1"));
                m_waypoint_view.Items.Add(item);
            }
        }

        private void UpdatePathWindow()
        {
            ListViewItem item;

            m_path_view.Items.Clear();
            if (m_selected_path != null)
            {
                item = new ListViewItem("Total Time");
                string timestr;
                try
                {
                    timestr = m_selected_path.TotalTime.ToString();
                }
                catch
                {
                    timestr = "<Calculating>";
                }
                item.SubItems.Add(timestr);
                m_path_view.Items.Add(item);

                item = new ListViewItem(MaxVelocityLabel);
                item.SubItems.Add(m_selected_path.MaxVelocity.ToString());
                m_path_view.Items.Add(item);

                item = new ListViewItem(MaxAccelerationLabel);
                item.SubItems.Add(m_selected_path.MaxAcceleration.ToString());
                m_path_view.Items.Add(item);

                item = new ListViewItem(MaxJerkLabel);
                item.SubItems.Add(m_selected_path.MaxJerk.ToString());
                m_path_view.Items.Add(item);

                if (m_file.Robot.DriveType == RobotParams.SwerveDriveType)
                {
                    item = new ListViewItem(StartAngleLabel);
                    item.SubItems.Add(m_selected_path.StartFacingAngle.ToString());
                    m_path_view.Items.Add(item);

                    item = new ListViewItem(EndAngleLabel);
                    item.SubItems.Add(m_selected_path.EndFacingAngle.ToString());
                    m_path_view.Items.Add(item);

                    item = new ListViewItem(RotationStartDelayLabel);
                    item.SubItems.Add(m_selected_path.FacingAngleStartDelay.ToString());
                    m_path_view.Items.Add(item);

                    item = new ListViewItem(RotationEndDelayLabel);
                    item.SubItems.Add(m_selected_path.FacingAngleEndDelay.ToString());
                    m_path_view.Items.Add(item);
                }
            }
        }

        private void UpdateRobotWindow()
        {
            if (m_file != null && m_file.Robot != null)
            {
                ListViewItem item;

                m_robot_view.Items.Clear();

                item = new ListViewItem(UnitsLabel);
                item.SubItems.Add(m_file.Robot.LengthUnits);
                m_robot_view.Items.Add(item);

                item = new ListViewItem(AngleUnitsLabel);
                item.SubItems.Add(m_file.Robot.AngleUnits);
                m_robot_view.Items.Add(item);

                item = new ListViewItem(TypeLabel);
                item.SubItems.Add(m_file.Robot.DriveType);
                m_robot_view.Items.Add(item);

                item = new ListViewItem(TimestampLabel);
                item.SubItems.Add(m_file.Robot.TimeStep.ToString());
                m_robot_view.Items.Add(item);

                item = new ListViewItem(WidthLabel);
                item.SubItems.Add(m_file.Robot.Width.ToString());
                m_robot_view.Items.Add(item);

                item = new ListViewItem(LengthLabel);
                item.SubItems.Add(m_file.Robot.Length.ToString());
                m_robot_view.Items.Add(item);

                item = new ListViewItem(MaxVelocityLabel);
                item.SubItems.Add(m_file.Robot.MaxVelocity.ToString());
                m_robot_view.Items.Add(item);

                item = new ListViewItem(MaxAccelerationLabel);
                item.SubItems.Add(m_file.Robot.MaxAcceleration.ToString());
                m_robot_view.Items.Add(item);

                item = new ListViewItem(MaxJerkLabel);
                item.SubItems.Add(m_file.Robot.MaxJerk.ToString());
                m_robot_view.Items.Add(item);
            }
        }

        private void UpdatePathTree()
        {
            m_pathfile_tree.Nodes.Clear();
            foreach (PathGroup gr in m_file.Groups)
            {
                TreeNode group = new TreeNode(gr.Name);
                foreach (RobotPath pa in gr.Paths)
                {
                    TreeNode path = new TreeNode(pa.Name);
                    group.Nodes.Add(path);
                }
                m_pathfile_tree.Nodes.Add(group);
            }
            m_pathfile_tree.ExpandAll();
        }
        #endregion

        #region initialization methods
        private void InitializeGame(Game g)
        {
            m_field.FieldGame = g;
            m_detailed.FieldGame = g;
            m_current_game = g;
            m_current_game.Units = m_file.Robot.LengthUnits;
        }

        private void InitializeGenerator(PathGenerator g)
        {
            if (m_generator != null)
            {
                m_generator.Stop();
                m_generator.StateChanged -= PathGeneratorJobStateChanged;
            }

            m_generator = g;
            m_generator.StateChanged += PathGeneratorJobStateChanged;
            m_generator.Start();
            GenerateAllSplines();
        }
        #endregion

        #region recently opened files

        private RecentFiles ReadRecent()
        {
            RecentFiles files = null;

            string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XeroPath", "recent.json");
            if (File.Exists(appdata))
            {
                string json = File.ReadAllText(appdata);
                try
                {
                    files = JsonConvert.DeserializeObject<RecentFiles>(json);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    files = null;
                }
            }

            if (files == null)
                files = new RecentFiles();

            return files;
        }

        private void UpdateRecentFileList(string add)
        {
            m_recent.RecentFileList.Add(add);
            while (m_recent.RecentFileList.Count > 4)
                m_recent.RecentFileList.RemoveAt(0);

            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(appdata))
            {
                MessageBox.Show("Cannot save preferences, appdata directory '" + appdata + "' does not exist", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            appdata = Path.Combine(appdata, "XeroPath");
            if (!Directory.Exists(appdata))
            {
                DirectoryInfo info;

                try
                {
                    info = Directory.CreateDirectory(appdata);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot save preferences, cannot create directory '" + appdata + "' - " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            appdata = Path.Combine(appdata, "recent.json");
            string json = JsonConvert.SerializeObject(m_recent);
            try
            {
                File.WriteAllText(appdata, json);
            }
            catch (Exception ex)
            {
                m_logger.LogMessage(Logger.MessageType.Warning, "cannot save recently opened files list '" + appdata + "' - " + ex.Message);
            }

            UpdateRecentMenu();
        }

        private void UpdateRecentMenu()
        {
            ToolStripMenuItem recentmenu = m_file_menu.DropDownItems[11] as ToolStripMenuItem;
            if (recentmenu != null)
            {
                recentmenu.DropDownItems.Clear();

                if (m_recent.RecentFileList.Count == 0)
                {
                    recentmenu.Enabled = false;
                }
                else
                {
                    for (int i = m_recent.RecentFileList.Count - 1; i >= 0; i--)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(m_recent.RecentFileList[i]);
                        item.Click += RecentFileSelected;
                        recentmenu.DropDownItems.Add(item);
                    }
                }
            }
        }

        private void RecentFileSelected(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                if (m_file != null && m_file.IsDirty)
                {
                    DialogResult result = MessageBox.Show(
                        "The current path file has unsaved changes.  Do you want to save this file first?",
                        "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (!SaveChanges())
                        {
                            MessageBox.Show("Could not save changes, open operation aborted", "Open Aborted",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                        return;
                }

                string json = File.ReadAllText(item.Text);
                try
                {
                    m_file = JsonConvert.DeserializeObject<PathFile>(json);
                    if (string.IsNullOrEmpty(m_file.Robot.AngleUnits))
                        m_file.Robot.AngleUnits = "Degrees" ;
                    SetUnits();
                }
                catch (Newtonsoft.Json.JsonSerializationException ex)
                {
                    string msg = "Cannot load path file '" + item.Text + "' - invalid file contents - " + ex.Message;
                    MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                m_file.PathName = item.Text;
                m_field.File = m_file;
                m_detailed.Robot = m_file.Robot;
                Text = "Path Editor - " + m_file.PathName;
                m_undo_stack = new List<UndoState>();
                GenerateAllSplines();
                UpdatePathTree();
                UpdateRobotWindow();
            }
        }


        #endregion

        private void UpdateCurrentPath()
        {
            m_field.Invalidate();
            m_detailed.Invalidate();
            GenerateSplines(m_selected_path);
            GenerateSegments(m_selected_path);
            UpdatePathWindow();
            UpdateWaypointPropertyWindow();
        }

        private void Path_SegmentsUpdated(object sender, EventArgs e)
        {
            RobotPath path = sender as RobotPath;
            if (path != null)
            {
                path.SegmentsUpdated -= Path_SegmentsUpdated;
                if (InvokeRequired)
                    Invoke(new HighlightDelegate(HighlightTime), new object[] { path, m_field.SelectedWaypoint });
                else
                    HighlightTime(path, m_field.SelectedWaypoint);
            }
        }

        private bool SaveChanges()
        {
            string json = JsonConvert.SerializeObject(m_file);
            try
            {
                File.WriteAllText(m_file.PathName, json);
                m_file.IsDirty = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not write output file - " + ex.Message, "Error Saving File",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool PathGroupExists(string name)
        {
            foreach(TreeNode node in m_pathfile_tree.Nodes)
            {
                if (node.Text == name)
                    return true;
            }

            return false;
        }

        private string GetNewPathGroupName()
        {
            int index = 0;
            string newname;

            while (true)
            {
                if (index == 0)
                    newname = "NewPathGroup";
                else
                    newname = "NewPathGroup " + index.ToString();

                if (!PathGroupExists(newname))
                    break;

                index++;
            }

            return newname;
        }

        private bool PathExists(TreeNode tn, string name)
        {
            foreach (TreeNode node in tn.Nodes)
            {
                if (node.Text == name)
                    return true;
            }

            return false;
        }

        private string GetNewPathName(TreeNode tn)
        {
            int index = 0;
            string newname;

            while (true)
            {
                if (index == 0)
                    newname = "NewPath";
                else
                    newname = "NewPath " + index.ToString();

                if (!PathExists(tn, newname))
                    break;

                index++;
            }

            return newname;
        }

        private void EditGroupOrPathName(TreeNode tn)
        {
            if (m_text_editor != null)
                return;

            Rectangle b = new Rectangle(tn.Bounds.Left, tn.Bounds.Top, m_pathfile_tree.Width, tn.Bounds.Height);

            m_editing_pathtree = tn;
            m_text_editor = new TextBox();
            m_text_editor.Text = tn.Text;
            m_text_editor.Bounds = b;
            m_text_editor.Parent = m_pathfile_tree;
            m_text_editor.Enabled = true;
            m_text_editor.Visible = true;
            m_text_editor.LostFocus += FinishedEditingNamePathTree;
            m_text_editor.PreviewKeyDown += PreviewEditorKeyPathTree;
            m_text_editor.Focus();
        }

        private void StopEditing()
        {
            if (m_combo_editor != null)
            {
                m_combo_editor.Dispose();
                m_combo_editor = null;
            }
            else
            {
                m_text_editor.Dispose();
                m_text_editor = null;
            }
            m_editing_pathtree = null;
            m_waypoint_editing = null;
            m_robot_param_editing = null;

            if (m_timer != null)
                m_timer.Enabled = true;
        }

        private void GenerateSegments(RobotPath p)
        {
            if (m_generator.TimingConstraintsSupported)
                p.GenerateVelocityConstraints();

            p.SetSegmentsInvalid();
            if (m_generator != null)
            {
                try
                {
                    p.SegmentsUpdated += SegmentUpdateComplete;
                    p.GenerateSegments(m_file.Robot, m_generator);
                }
                catch(Exception ex)
                {
                    string msg = "In path generator '";
                    msg += m_generator.Name;
                    msg += "' detailed path generation failed - " + ex.Message;
                    m_logger.LogMessage(Logger.MessageType.Warning, msg);
                }
            }
        }

        private void SegmentUpdateComplete(object sender, EventArgs e)
        {
            RobotPath p = sender as RobotPath;
            Debug.Assert(p != null);
            p.SegmentsUpdated -= SegmentUpdateComplete;

            if (InvokeRequired)
                Invoke(new UpdatePathWindowDelegate(UpdatePathWindow));
            else
            {
                UpdatePathWindow();
                m_field.Invalidate();
                m_detailed.Invalidate();
            }
        }

        private void GenerateSplines(RobotPath p)
        {
            p.ClearSplines();

            if (m_generator != null)
            {
                try
                {

                    p.GenerateSplines(m_generator);
                }
                catch (Exception ex)
                {
                    string msg = "In path generator '";
                    msg += m_generator.Name;
                    msg += "' spline generation failed - " + ex.Message;
                    m_logger.LogMessage(Logger.MessageType.Warning, msg);
                    p.ClearSplines();
                }
            }
        }

        private void GenerateAllSplines()
        {
            foreach(PathGroup group in m_file.Groups)
            {
                foreach(RobotPath path in group.Paths)
                {
                    GenerateSplines(path);
                    GenerateSegments(path);
                }
            }
        }

        static string[] fields = { "time", "x", "y", "position", "velocity", "acceleration", "jerk", "heading" };
        private void WritePath(string grname, string pathname, string suffix, PathSegment[] segs)
        {
            string filename;

            PathSegment seg = segs[0];
            bool first = true ;

            string basedir = m_file.PathDirectory;
            if (!Path.IsPathRooted(m_file.PathDirectory))
                basedir = Path.Combine(Path.GetDirectoryName(m_file.PathName), m_file.PathDirectory);

            if (string.IsNullOrEmpty(suffix))
                filename = Path.Combine(basedir, grname + "_" + pathname + ".csv");
            else
                filename = Path.Combine(basedir, grname + "_" + pathname + "." + suffix + ".csv");

            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (seg.HasValue(fields[i]))
                        {
                            if (!first)
                                writer.Write(',');

                            writer.Write('"');
                            writer.Write(fields[i]);
                            writer.Write('"');

                            first = false;
                        }
                    }
                    writer.WriteLine();

                    for (int j = 0; j < segs.Length; j++)
                    {
                        first = true;

                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (segs[j].HasValue(fields[i]))
                            {
                                if (!first)
                                    writer.Write(',');

                                if (fields[i] == "heading" && m_file.Robot.AngleUnits == "radians")
                                {
                                    //
                                    // Handle heading special
                                    //
                                    writer.Write(XeroMath.XeroUtils.DegreesToRadians(segs[j].GetValue("heading")));
                                }
                                else
                                {
                                    writer.Write(segs[j].GetValue(fields[i]));
                                }
                                first = false;
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch(IOException ex)
            {
                string msg = "Cannot open output file '" + filename + "' for writing - " + ex.Message;
                MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_logger.LogMessage(Logger.MessageType.Error, "Cannot open output path file '" + filename + "' - " + ex.Message);
            }
        }

        private void GeneratePaths()
        {
            foreach (PathGroup gr in m_file.Groups)
            {
                foreach (RobotPath path in gr.Paths)
                {
                    if (path.HasSegments)
                    {
                        WritePath(gr.Name, path.Name, string.Empty, path.Segments);
                        if (path.HasAdditionalSegments)
                        {
                            foreach (var pair in path.AdditionalSegments)
                                WritePath(gr.Name, path.Name, pair.Key, pair.Value);
                        }
                    }
                }
            }

            MessageBox.Show("Paths written to directory '" + m_file.PathDirectory + "'", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string[] MyCopyright =
        {
            "Copyright 2019 Jack W. (Butch) Griffin - Error Code Xero, FRC 1425",
            "",
            "MIT License",
            "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation",
            "files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use,",
            "copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom",
            "the Software is furnished to do so, subject to the following conditions:",
            "",
            "The above copyright notice and this permission notice shall be included in all copies or substantial portions of the",
            "Software.",
            "",
            "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO",
            "THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS",
            "OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR",
            "OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE."
        };

        private string[] JSonCopyright =
        {
            "",
            "",
            "------------------------------------------------------------------------------------------",
            "This program uses the JSON package from NewtonSoft (https://www.newtonsoft.com/json)",
            "",
            "MIT License",
            "Copyright (c) _____",
            "",
            "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation",
            "files (the \"Software\"), to deal in the Software without restriction, including without limitation the rights to use,",
            "copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom",
            "the Software is furnished to do so, subject to the following conditions:",
            "",
            "The above copyright notice and this permission notice shall be included in all copies or substantial portions of the",
            "Software.",
            "",
            "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO",
            "THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS",
            "OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR",
            "OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE."
        };

        private string[] CSVCopyRight =
        {
            "",
            "",
            "------------------------------------------------------------------------------------------",
            "This program uses the CSV package from Steven Hansen",
            "",
            "The MIT License (MIT)",
            "",
            "Copyright (c) 2015 Steve Hansen",
            "",
            "Permission is hereby granted, free of charge, to any person obtaining a copy",
            "of this software and associated documentation files (the \"Software\"), to deal",
            "in the Software without restriction, including without limitation the rights",
            "to use, copy, modify, merge, publish, distribute, sublicense, and/or sell",
            "copies of the Software, and to permit persons to whom the Software is",
            "furnished to do so, subject to the following conditions:",
            "",
            "The above copyright notice and this permission notice shall be included in all",
            "copies or substantial portions of the Software.",
            "",
            "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR",
            "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,",
            "FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE",
            "AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER",
            "LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,",
            "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE",
            "SOFTWARE."
        };

        private void OutputCopyright()
        {
            m_logger.LogMessage(Logger.MessageType.Info, "PathViewer Version " + Assembly.GetEntryAssembly().GetName().Version.ToString());
            m_logger.LogMessage(Logger.MessageType.Info, "");

            foreach (string str in MyCopyright)
                m_logger.LogMessage(Logger.MessageType.Info, str);

            foreach (string str in JSonCopyright)
                m_logger.LogMessage(Logger.MessageType.Info, str);

            foreach (string str in CSVCopyRight)
                m_logger.LogMessage(Logger.MessageType.Info, str);
        }

        private void UpdatePreferences()
        {
            m_undo_length = m_defaults.UndoStackSize;
            while (m_undo_stack.Count > m_undo_length)
                m_undo_stack.RemoveAt(m_undo_stack.Count - 1);
        }
        #endregion
    }
}

namespace WinDriverStation
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_robot_name = new System.Windows.Forms.TextBox();
            this.m_connect = new System.Windows.Forms.Button();
            this.m_enabled = new System.Windows.Forms.RadioButton();
            this.m_disabled = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_status = new System.Windows.Forms.GroupBox();
            this.m_robot = new WinDriverStation.RedGreenControl();
            this.m_joysticks = new WinDriverStation.RedGreenControl();
            this.m_comms = new WinDriverStation.RedGreenControl();
            this.m_mode = new System.Windows.Forms.GroupBox();
            this.m_test = new System.Windows.Forms.RadioButton();
            this.m_auto = new System.Windows.Forms.RadioButton();
            this.m_teleop = new System.Windows.Forms.RadioButton();
            this.m_control = new System.Windows.Forms.GroupBox();
            this.m_automode = new KnobControl.KnobControl();
            this.label5 = new System.Windows.Forms.Label();
            this.m_servo = new KnobControl.KnobControl();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_status.SuspendLayout();
            this.m_mode.SuspendLayout();
            this.m_control.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Robot";
            // 
            // m_robot_name
            // 
            this.m_robot_name.Location = new System.Drawing.Point(84, 20);
            this.m_robot_name.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.m_robot_name.Name = "m_robot_name";
            this.m_robot_name.Size = new System.Drawing.Size(226, 26);
            this.m_robot_name.TabIndex = 1;
            // 
            // m_connect
            // 
            this.m_connect.Location = new System.Drawing.Point(321, 17);
            this.m_connect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.m_connect.Name = "m_connect";
            this.m_connect.Size = new System.Drawing.Size(112, 35);
            this.m_connect.TabIndex = 2;
            this.m_connect.Text = "Connect";
            this.m_connect.UseVisualStyleBackColor = true;
            this.m_connect.Click += new System.EventHandler(this.ConnectButtonClicked);
            // 
            // m_enabled
            // 
            this.m_enabled.AutoSize = true;
            this.m_enabled.Location = new System.Drawing.Point(33, 31);
            this.m_enabled.Name = "m_enabled";
            this.m_enabled.Size = new System.Drawing.Size(93, 24);
            this.m_enabled.TabIndex = 3;
            this.m_enabled.TabStop = true;
            this.m_enabled.Text = "Enabled";
            this.m_enabled.UseVisualStyleBackColor = true;
            this.m_enabled.CheckedChanged += new System.EventHandler(this.EnableButtonClicked);
            // 
            // m_disabled
            // 
            this.m_disabled.AutoSize = true;
            this.m_disabled.Location = new System.Drawing.Point(33, 76);
            this.m_disabled.Name = "m_disabled";
            this.m_disabled.Size = new System.Drawing.Size(96, 24);
            this.m_disabled.TabIndex = 4;
            this.m_disabled.TabStop = true;
            this.m_disabled.Text = "Disabled";
            this.m_disabled.UseVisualStyleBackColor = true;
            this.m_disabled.CheckedChanged += new System.EventHandler(this.DisableButtonClicked);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 91);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Joysticks";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Robot";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 29);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Communications";
            // 
            // m_status
            // 
            this.m_status.Controls.Add(this.m_robot);
            this.m_status.Controls.Add(this.label4);
            this.m_status.Controls.Add(this.m_joysticks);
            this.m_status.Controls.Add(this.m_comms);
            this.m_status.Controls.Add(this.label2);
            this.m_status.Controls.Add(this.label3);
            this.m_status.Location = new System.Drawing.Point(496, 74);
            this.m_status.Name = "m_status";
            this.m_status.Size = new System.Drawing.Size(294, 128);
            this.m_status.TabIndex = 12;
            this.m_status.TabStop = false;
            this.m_status.Text = "Status";
            // 
            // m_robot
            // 
            this.m_robot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_robot.Location = new System.Drawing.Point(166, 59);
            this.m_robot.Name = "m_robot";
            this.m_robot.Red = true;
            this.m_robot.Size = new System.Drawing.Size(90, 22);
            this.m_robot.TabIndex = 9;
            // 
            // m_joysticks
            // 
            this.m_joysticks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_joysticks.Location = new System.Drawing.Point(166, 91);
            this.m_joysticks.Name = "m_joysticks";
            this.m_joysticks.Red = true;
            this.m_joysticks.Size = new System.Drawing.Size(90, 22);
            this.m_joysticks.TabIndex = 5;
            // 
            // m_comms
            // 
            this.m_comms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_comms.Location = new System.Drawing.Point(166, 27);
            this.m_comms.Name = "m_comms";
            this.m_comms.Red = true;
            this.m_comms.Size = new System.Drawing.Size(90, 22);
            this.m_comms.TabIndex = 10;
            // 
            // m_mode
            // 
            this.m_mode.Controls.Add(this.m_test);
            this.m_mode.Controls.Add(this.m_auto);
            this.m_mode.Controls.Add(this.m_teleop);
            this.m_mode.Location = new System.Drawing.Point(238, 74);
            this.m_mode.Name = "m_mode";
            this.m_mode.Size = new System.Drawing.Size(230, 128);
            this.m_mode.TabIndex = 13;
            this.m_mode.TabStop = false;
            this.m_mode.Text = "Mode";
            // 
            // m_test
            // 
            this.m_test.AutoSize = true;
            this.m_test.Location = new System.Drawing.Point(34, 85);
            this.m_test.Name = "m_test";
            this.m_test.Size = new System.Drawing.Size(65, 24);
            this.m_test.TabIndex = 16;
            this.m_test.TabStop = true;
            this.m_test.Text = "Test";
            this.m_test.UseVisualStyleBackColor = true;
            this.m_test.CheckedChanged += new System.EventHandler(this.TestButtonClicked);
            // 
            // m_auto
            // 
            this.m_auto.AutoSize = true;
            this.m_auto.Location = new System.Drawing.Point(34, 55);
            this.m_auto.Name = "m_auto";
            this.m_auto.Size = new System.Drawing.Size(125, 24);
            this.m_auto.TabIndex = 15;
            this.m_auto.TabStop = true;
            this.m_auto.Text = "Autonomous";
            this.m_auto.UseVisualStyleBackColor = true;
            this.m_auto.CheckedChanged += new System.EventHandler(this.AutoButtonClicked);
            // 
            // m_teleop
            // 
            this.m_teleop.AutoSize = true;
            this.m_teleop.Location = new System.Drawing.Point(34, 25);
            this.m_teleop.Name = "m_teleop";
            this.m_teleop.Size = new System.Drawing.Size(131, 24);
            this.m_teleop.TabIndex = 14;
            this.m_teleop.TabStop = true;
            this.m_teleop.Text = "TeleOperated";
            this.m_teleop.UseVisualStyleBackColor = true;
            this.m_teleop.CheckedChanged += new System.EventHandler(this.TeleopButtonClicked);
            // 
            // m_control
            // 
            this.m_control.Controls.Add(this.m_enabled);
            this.m_control.Controls.Add(this.m_disabled);
            this.m_control.Location = new System.Drawing.Point(22, 74);
            this.m_control.Name = "m_control";
            this.m_control.Size = new System.Drawing.Size(200, 128);
            this.m_control.TabIndex = 14;
            this.m_control.TabStop = false;
            this.m_control.Text = "Control";
            // 
            // m_automode
            // 
            this.m_automode.EndAngle = 405F;
            this.m_automode.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_automode.knobBackColor = System.Drawing.Color.White;
            this.m_automode.KnobPointerStyle = KnobControl.KnobControl.knobPointerStyle.circle;
            this.m_automode.LargeChange = 5;
            this.m_automode.Location = new System.Drawing.Point(20, 66);
            this.m_automode.Maximum = 9;
            this.m_automode.Minimum = 0;
            this.m_automode.Name = "m_automode";
            this.m_automode.PointerColor = System.Drawing.Color.SlateBlue;
            this.m_automode.ScaleColor = System.Drawing.Color.Black;
            this.m_automode.ScaleDivisions = 10;
            this.m_automode.ScaleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.m_automode.ScaleSubDivisions = 4;
            this.m_automode.ShowLargeScale = true;
            this.m_automode.ShowSmallScale = false;
            this.m_automode.Size = new System.Drawing.Size(150, 150);
            this.m_automode.SmallChange = 1;
            this.m_automode.StartAngle = 135F;
            this.m_automode.TabIndex = 15;
            this.m_automode.Value = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "AutoMode";
            // 
            // m_servo
            // 
            this.m_servo.EndAngle = 405F;
            this.m_servo.ImeMode = System.Windows.Forms.ImeMode.On;
            this.m_servo.knobBackColor = System.Drawing.Color.White;
            this.m_servo.KnobPointerStyle = KnobControl.KnobControl.knobPointerStyle.circle;
            this.m_servo.LargeChange = 5;
            this.m_servo.Location = new System.Drawing.Point(191, 66);
            this.m_servo.Maximum = 90;
            this.m_servo.Minimum = -90;
            this.m_servo.Name = "m_servo";
            this.m_servo.PointerColor = System.Drawing.Color.SlateBlue;
            this.m_servo.ScaleColor = System.Drawing.Color.Black;
            this.m_servo.ScaleDivisions = 19;
            this.m_servo.ScaleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.m_servo.ScaleSubDivisions = 4;
            this.m_servo.ShowLargeScale = true;
            this.m_servo.ShowSmallScale = false;
            this.m_servo.Size = new System.Drawing.Size(150, 150);
            this.m_servo.SmallChange = 1;
            this.m_servo.StartAngle = 135F;
            this.m_servo.TabIndex = 17;
            this.m_servo.Value = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(239, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Servo";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_automode);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.m_servo);
            this.groupBox1.Location = new System.Drawing.Point(22, 219);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(768, 260);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GoPiGo3 Operator Interface";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 510);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_control);
            this.Controls.Add(this.m_mode);
            this.Controls.Add(this.m_status);
            this.Controls.Add(this.m_connect);
            this.Controls.Add(this.m_robot_name);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "GoPiGo Driver Station";
            this.m_status.ResumeLayout(false);
            this.m_status.PerformLayout();
            this.m_mode.ResumeLayout(false);
            this.m_mode.PerformLayout();
            this.m_control.ResumeLayout(false);
            this.m_control.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_robot_name;
        private System.Windows.Forms.Button m_connect;
        private System.Windows.Forms.RadioButton m_enabled;
        private System.Windows.Forms.RadioButton m_disabled;
        private RedGreenControl m_joysticks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private RedGreenControl m_robot;
        private RedGreenControl m_comms;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox m_status;
        private System.Windows.Forms.GroupBox m_mode;
        private System.Windows.Forms.RadioButton m_test;
        private System.Windows.Forms.RadioButton m_auto;
        private System.Windows.Forms.RadioButton m_teleop;
        private System.Windows.Forms.GroupBox m_control;
        private KnobControl.KnobControl m_automode;
        private System.Windows.Forms.Label label5;
        private KnobControl.KnobControl m_servo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}


namespace MouseCrank {
    partial class MouseCrank_MainWindow {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MouseCrank_MainWindow));
            SensX = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            SensY = new TrackBar();
            tapsValueX = new Label();
            SensXValue = new Label();
            SensYValue = new Label();
            label4 = new Label();
            toggleKey = new ComboBox();
            Calibration = new GroupBox();
            groupBox2 = new GroupBox();
            btnResetY = new Button();
            label10 = new Label();
            CurveValueY = new Label();
            CurveY = new TrackBar();
            groupBox1 = new GroupBox();
            btnResetX = new Button();
            CurveX = new TrackBar();
            label6 = new Label();
            CurveValueX = new Label();
            menuStrip1 = new MenuStrip();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            MouseCrank_TrayIcon = new NotifyIcon(components);
            checkTrayMinimize = new CheckBox();
            sndVolume = new TrackBar();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)SensX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SensY).BeginInit();
            Calibration.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CurveY).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CurveX).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sndVolume).BeginInit();
            SuspendLayout();
            // 
            // SensX
            // 
            SensX.Location = new Point(123, 22);
            SensX.Maximum = 100;
            SensX.Minimum = 1;
            SensX.Name = "SensX";
            SensX.Size = new Size(308, 45);
            SensX.TabIndex = 0;
            SensX.Value = 50;
            SensX.Scroll += SensX_Scroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(54, 22);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 1;
            label1.Text = "Sensitivity:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(55, 22);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 3;
            label2.Text = "Sensitivity:";
            // 
            // SensY
            // 
            SensY.Location = new Point(123, 22);
            SensY.Maximum = 100;
            SensY.Minimum = 1;
            SensY.Name = "SensY";
            SensY.Size = new Size(308, 45);
            SensY.TabIndex = 2;
            SensY.Value = 50;
            SensY.Scroll += SensY_Scroll;
            // 
            // tapsValueX
            // 
            tapsValueX.AutoSize = true;
            tapsValueX.Location = new Point(438, 19);
            tapsValueX.Name = "tapsValueX";
            tapsValueX.Size = new Size(13, 15);
            tapsValueX.TabIndex = 6;
            tapsValueX.Text = "0";
            // 
            // SensXValue
            // 
            SensXValue.AutoSize = true;
            SensXValue.Location = new Point(436, 22);
            SensXValue.Name = "SensXValue";
            SensXValue.Size = new Size(13, 15);
            SensXValue.TabIndex = 7;
            SensXValue.Text = "0";
            // 
            // SensYValue
            // 
            SensYValue.AutoSize = true;
            SensYValue.Location = new Point(437, 22);
            SensYValue.Name = "SensYValue";
            SensYValue.Size = new Size(13, 15);
            SensYValue.TabIndex = 8;
            SensYValue.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 343);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 9;
            label4.Text = "Toggle key:";
            // 
            // toggleKey
            // 
            toggleKey.DropDownStyle = ComboBoxStyle.DropDownList;
            toggleKey.FormattingEnabled = true;
            toggleKey.ItemHeight = 15;
            toggleKey.Location = new Point(85, 340);
            toggleKey.Name = "toggleKey";
            toggleKey.Size = new Size(79, 23);
            toggleKey.TabIndex = 4;
            toggleKey.SelectedIndexChanged += toggleKey_SelectedIndexChanged;
            // 
            // Calibration
            // 
            Calibration.Controls.Add(groupBox2);
            Calibration.Controls.Add(groupBox1);
            Calibration.Location = new Point(12, 27);
            Calibration.Name = "Calibration";
            Calibration.Size = new Size(484, 298);
            Calibration.TabIndex = 13;
            Calibration.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnResetY);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(CurveValueY);
            groupBox2.Controls.Add(CurveY);
            groupBox2.Controls.Add(SensY);
            groupBox2.Controls.Add(SensYValue);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(6, 155);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(466, 135);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Vertical Calibration";
            // 
            // btnResetY
            // 
            btnResetY.Location = new Point(7, 107);
            btnResetY.Name = "btnResetY";
            btnResetY.Size = new Size(111, 23);
            btnResetY.TabIndex = 10;
            btnResetY.TabStop = false;
            btnResetY.Text = "Reset to Defaults";
            btnResetY.UseVisualStyleBackColor = true;
            btnResetY.Click += btnResetY_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(76, 73);
            label10.Name = "label10";
            label10.Size = new Size(41, 15);
            label10.TabIndex = 12;
            label10.Text = "Curve:";
            // 
            // CurveValueY
            // 
            CurveValueY.AutoSize = true;
            CurveValueY.Location = new Point(437, 73);
            CurveValueY.Name = "CurveValueY";
            CurveValueY.Size = new Size(13, 15);
            CurveValueY.TabIndex = 11;
            CurveValueY.Text = "0";
            // 
            // CurveY
            // 
            CurveY.Location = new Point(124, 73);
            CurveY.Maximum = 100;
            CurveY.Minimum = 1;
            CurveY.Name = "CurveY";
            CurveY.Size = new Size(308, 45);
            CurveY.TabIndex = 3;
            CurveY.Value = 50;
            CurveY.Scroll += CurveY_Scroll;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnResetX);
            groupBox1.Controls.Add(CurveX);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(CurveValueX);
            groupBox1.Controls.Add(SensX);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(SensXValue);
            groupBox1.Controls.Add(tapsValueX);
            groupBox1.Location = new Point(6, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(466, 138);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Horizontal Calibration";
            // 
            // btnResetX
            // 
            btnResetX.Location = new Point(6, 109);
            btnResetX.Name = "btnResetX";
            btnResetX.Size = new Size(111, 23);
            btnResetX.TabIndex = 9;
            btnResetX.TabStop = false;
            btnResetX.Text = "Reset to Defaults";
            btnResetX.UseVisualStyleBackColor = true;
            btnResetX.Click += btnResetX_Click;
            // 
            // CurveX
            // 
            CurveX.Location = new Point(123, 72);
            CurveX.Maximum = 100;
            CurveX.Minimum = 1;
            CurveX.Name = "CurveX";
            CurveX.Size = new Size(308, 45);
            CurveX.TabIndex = 1;
            CurveX.Value = 50;
            CurveX.Scroll += CurveX_Scroll;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(76, 72);
            label6.Name = "label6";
            label6.Size = new Size(41, 15);
            label6.TabIndex = 9;
            label6.Text = "Curve:";
            // 
            // CurveValueX
            // 
            CurveValueX.AutoSize = true;
            CurveValueX.Location = new Point(436, 72);
            CurveValueX.Name = "CurveValueX";
            CurveValueX.Size = new Size(13, 15);
            CurveValueX.TabIndex = 10;
            CurveValueX.Text = "0";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(508, 24);
            menuStrip1.TabIndex = 14;
            menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(61, 20);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // MouseCrank_TrayIcon
            // 
            MouseCrank_TrayIcon.BalloonTipText = "Minimized to tray";
            MouseCrank_TrayIcon.Icon = (Icon)resources.GetObject("MouseCrank_TrayIcon.Icon");
            MouseCrank_TrayIcon.Text = "MouseCrank";
            MouseCrank_TrayIcon.Visible = true;
            MouseCrank_TrayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;
            // 
            // checkTrayMinimize
            // 
            checkTrayMinimize.AutoSize = true;
            checkTrayMinimize.Location = new Point(388, 342);
            checkTrayMinimize.Name = "checkTrayMinimize";
            checkTrayMinimize.RightToLeft = RightToLeft.No;
            checkTrayMinimize.Size = new Size(112, 19);
            checkTrayMinimize.TabIndex = 6;
            checkTrayMinimize.Text = "Minimize to tray";
            checkTrayMinimize.UseVisualStyleBackColor = true;
            checkTrayMinimize.CheckedChanged += checkTrayMinimize_CheckedChanged;
            // 
            // sndVolume
            // 
            sndVolume.Location = new Point(268, 340);
            sndVolume.Maximum = 20;
            sndVolume.Name = "sndVolume";
            sndVolume.Size = new Size(104, 45);
            sndVolume.TabIndex = 5;
            sndVolume.Scroll += sndVolume_Scroll;
            sndVolume.KeyUp += sndVolume_MouseUp;
            sndVolume.MouseUp += sndVolume_MouseUp;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(174, 343);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 15;
            label3.Text = "Toggle volume:";
            // 
            // MouseCrank_MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 379);
            Controls.Add(label3);
            Controls.Add(sndVolume);
            Controls.Add(checkTrayMinimize);
            Controls.Add(Calibration);
            Controls.Add(toggleKey);
            Controls.Add(label4);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MouseCrank_MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MouseCrank for Steel Beasts";
            Resize += MouseCrank_MainWindow_Resize;
            ((System.ComponentModel.ISupportInitialize)SensX).EndInit();
            ((System.ComponentModel.ISupportInitialize)SensY).EndInit();
            Calibration.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CurveY).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)CurveX).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)sndVolume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar SensX;
        private Label label1;
        private Label label2;
        private TrackBar SensY;
        private Label tapsValueX;
        private Label SensXValue;
        private Label SensYValue;
        private Label label4;
        private ComboBox toggleKey;
        private GroupBox Calibration;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private NotifyIcon MouseCrank_TrayIcon;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private TrackBar CurveX;
        private Label label6;
        private Label CurveValueX;
        private Label label10;
        private Label CurveValueY;
        private TrackBar CurveY;
        private Button btnResetY;
        private Button btnResetX;
        private CheckBox checkTrayMinimize;
        private TrackBar sndVolume;
        private Label label3;
    }
}
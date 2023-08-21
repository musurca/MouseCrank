namespace MouseCrank {
    partial class About {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            labelTitle = new Label();
            btnOK = new Button();
            label4 = new Label();
            label6 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(46, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(123, 30);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "MouseCrank v0.1\nfor Steel Beasts Pro PE";
            labelTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(72, 231);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(36, 81);
            label4.Name = "label4";
            label4.Size = new Size(146, 45);
            label4.TabIndex = 4;
            label4.Text = "by Nicholas Musurca\nnick.musurca@gmail.com\nmezentius @ SB Forums";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(9, 142);
            label6.Name = "label6";
            label6.Size = new Size(212, 75);
            label6.TabIndex = 6;
            label6.Text = "DISCLAIMER:\nThis software is an unofficial add-on\nfor Steel Beasts Pro PE -- not a product\nof eSim Games. Please do not ask eSim\nto provide support for MouseCrank.";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 53);
            label2.Name = "label2";
            label2.Size = new Size(206, 15);
            label2.TabIndex = 7;
            label2.Text = "Mouse input for hand-cranked turrets";
            // 
            // About
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(228, 261);
            ControlBox = false;
            Controls.Add(label2);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(btnOK);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "About";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About MouseCrank...";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitle;
        private Button btnOK;
        private Label label4;
        private Label label6;
        private Label label2;
    }
}
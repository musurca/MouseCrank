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
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(59, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(123, 30);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "MouseCrank v0.1\nfor Steel Beasts Pro PE";
            labelTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(87, 256);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(48, 78);
            label4.Name = "label4";
            label4.Size = new Size(146, 45);
            label4.TabIndex = 4;
            label4.Text = "by Nicholas Musurca\nnick.musurca@gmail.com\nmezentius @ SB Forums";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 164);
            label6.Name = "label6";
            label6.Size = new Size(217, 75);
            label6.TabIndex = 6;
            label6.Text = "DISCLAIMER:\r\nThis software is an unofficial add-on for\r\nSteel Beasts Pro PE and is not a product \r\nof eSim Games. Please do not ask eSim\r\nto provide support for MouseCrank.";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 49);
            label2.Name = "label2";
            label2.Size = new Size(206, 15);
            label2.TabIndex = 7;
            label2.Text = "Mouse input for hand-cranked turrets";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(9, 133);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(232, 15);
            linkLabel1.TabIndex = 0;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://github.com/musurca/MouseCrank";
            // 
            // About
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(248, 288);
            ControlBox = false;
            Controls.Add(linkLabel1);
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
        private LinkLabel linkLabel1;
    }
}
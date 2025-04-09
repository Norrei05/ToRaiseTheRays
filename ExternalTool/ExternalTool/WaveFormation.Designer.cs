namespace ExternalTool
{
    partial class WaveFormation
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
            groupBoxGrid = new GroupBox();
            label1 = new Label();
            textBoxType = new TextBox();
            buttonSave = new Button();
            richTextBox2 = new RichTextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            textBoxHealth = new TextBox();
            textBoxSpeed = new TextBox();
            textBoxShot = new TextBox();
            textBoxCollision = new TextBox();
            textBoxWidth = new TextBox();
            label7 = new Label();
            textBoxHeight = new TextBox();
            label8 = new Label();
            textBoxBullet = new TextBox();
            labelBullet = new Label();
            groupBoxGrid.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxGrid
            // 
            groupBoxGrid.Controls.Add(label1);
            groupBoxGrid.Location = new Point(574, 20);
            groupBoxGrid.Margin = new Padding(4, 5, 4, 5);
            groupBoxGrid.Name = "groupBoxGrid";
            groupBoxGrid.Padding = new Padding(4, 5, 4, 5);
            groupBoxGrid.Size = new Size(551, 685);
            groupBoxGrid.TabIndex = 0;
            groupBoxGrid.TabStop = false;
            groupBoxGrid.Text = "Grid";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-526, 17);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 25);
            label1.TabIndex = 3;
            // 
            // textBoxType
            // 
            textBoxType.Location = new Point(173, 165);
            textBoxType.Margin = new Padding(4, 5, 4, 5);
            textBoxType.Name = "textBoxType";
            textBoxType.Size = new Size(391, 31);
            textBoxType.TabIndex = 1;
            textBoxType.TextChanged += textBoxType_TextChanged;
            // 
            // buttonSave
            // 
            buttonSave.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonSave.Location = new Point(17, 574);
            buttonSave.Margin = new Padding(4, 5, 4, 5);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(549, 131);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(17, 20);
            richTextBox2.Margin = new Padding(4, 5, 4, 5);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(547, 119);
            richTextBox2.TabIndex = 4;
            richTextBox2.Text = "The grid represents the map for the game. Click a location on the grid to set the location of an enemy in the wave. \n\nWrite the filename for the type of enemy you want to place below.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 170);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(111, 25);
            label2.TabIndex = 5;
            label2.Text = "Enemy Type:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 266);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(67, 25);
            label3.TabIndex = 6;
            label3.Text = "Health:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 315);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(66, 25);
            label4.TabIndex = 7;
            label4.Text = "Speed:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 511);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(162, 25);
            label5.TabIndex = 8;
            label5.Text = "Damage Multiplier:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(17, 365);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(155, 25);
            label6.TabIndex = 9;
            label6.Text = "Collision Damage:";
            // 
            // textBoxHealth
            // 
            textBoxHealth.Location = new Point(173, 261);
            textBoxHealth.Margin = new Padding(4, 5, 4, 5);
            textBoxHealth.Name = "textBoxHealth";
            textBoxHealth.Size = new Size(391, 31);
            textBoxHealth.TabIndex = 10;
            textBoxHealth.Text = "20";
            textBoxHealth.TextChanged += InputIsInteger;
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Location = new Point(173, 310);
            textBoxSpeed.Margin = new Padding(4, 5, 4, 5);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.Size = new Size(391, 31);
            textBoxSpeed.TabIndex = 11;
            textBoxSpeed.Text = "100";
            textBoxSpeed.TextChanged += InputIsInteger;
            // 
            // textBoxShot
            // 
            textBoxShot.Location = new Point(173, 508);
            textBoxShot.Margin = new Padding(4, 5, 4, 5);
            textBoxShot.Name = "textBoxShot";
            textBoxShot.Size = new Size(391, 31);
            textBoxShot.TabIndex = 12;
            textBoxShot.Text = "10";
            textBoxShot.TextChanged += InputIsInteger;
            // 
            // textBoxCollision
            // 
            textBoxCollision.Location = new Point(173, 360);
            textBoxCollision.Margin = new Padding(4, 5, 4, 5);
            textBoxCollision.Name = "textBoxCollision";
            textBoxCollision.Size = new Size(391, 31);
            textBoxCollision.TabIndex = 13;
            textBoxCollision.Text = "10";
            textBoxCollision.TextChanged += InputIsInteger;
            // 
            // textBoxWidth
            // 
            textBoxWidth.Location = new Point(173, 408);
            textBoxWidth.Margin = new Padding(4, 5, 4, 5);
            textBoxWidth.Name = "textBoxWidth";
            textBoxWidth.Size = new Size(391, 31);
            textBoxWidth.TabIndex = 14;
            textBoxWidth.Text = "50";
            textBoxWidth.TextChanged += InputIsInteger;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 413);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(64, 25);
            label7.TabIndex = 15;
            label7.Text = "Width:";
            // 
            // textBoxHeight
            // 
            textBoxHeight.Location = new Point(173, 457);
            textBoxHeight.Margin = new Padding(4, 5, 4, 5);
            textBoxHeight.Name = "textBoxHeight";
            textBoxHeight.Size = new Size(391, 31);
            textBoxHeight.TabIndex = 16;
            textBoxHeight.Text = "50";
            textBoxHeight.TextChanged += InputIsInteger;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 462);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(69, 25);
            label8.TabIndex = 17;
            label8.Text = "Height:";
            // 
            // textBoxBullet
            // 
            textBoxBullet.Location = new Point(173, 213);
            textBoxBullet.Name = "textBoxBullet";
            textBoxBullet.Size = new Size(391, 31);
            textBoxBullet.TabIndex = 18;
            textBoxBullet.TextChanged += textBox1_TextChanged;
            // 
            // labelBullet
            // 
            labelBullet.AutoSize = true;
            labelBullet.Location = new Point(17, 216);
            labelBullet.Name = "labelBullet";
            labelBullet.Size = new Size(97, 25);
            labelBullet.TabIndex = 19;
            labelBullet.Text = "Bullet Type";
            // 
            // WaveFormation
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 728);
            Controls.Add(labelBullet);
            Controls.Add(textBoxBullet);
            Controls.Add(label8);
            Controls.Add(textBoxHeight);
            Controls.Add(label7);
            Controls.Add(textBoxWidth);
            Controls.Add(textBoxCollision);
            Controls.Add(textBoxShot);
            Controls.Add(textBoxSpeed);
            Controls.Add(textBoxHealth);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(richTextBox2);
            Controls.Add(buttonSave);
            Controls.Add(textBoxType);
            Controls.Add(groupBoxGrid);
            Margin = new Padding(4, 5, 4, 5);
            Name = "WaveFormation";
            Text = "WaveFormation";
            groupBoxGrid.ResumeLayout(false);
            groupBoxGrid.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxGrid;
        private TextBox textBoxType;
        private Button buttonSave;
        private Label label1;
        private RichTextBox richTextBox2;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox textBoxHealth;
        private TextBox textBoxSpeed;
        private TextBox textBoxShot;
        private TextBox textBoxCollision;
        private TextBox textBoxWidth;
        private Label label7;
        private TextBox textBoxHeight;
        private Label label8;
        private TextBox textBoxBullet;
        private Label labelBullet;
    }
}
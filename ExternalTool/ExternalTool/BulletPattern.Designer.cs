namespace ExternalTool
{
    partial class BulletPattern
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
            groupBoxDirection = new GroupBox();
            groupBoxPosition = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxDamage = new TextBox();
            textBoxSpeed = new TextBox();
            textBoxSize = new TextBox();
            buttonAdd = new Button();
            buttonSave = new Button();
            textBoxShots = new TextBox();
            label4 = new Label();
            richTextBox1 = new RichTextBox();
            textBoxDelay = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // groupBoxDirection
            // 
            groupBoxDirection.Location = new Point(17, 293);
            groupBoxDirection.Margin = new Padding(4, 5, 4, 5);
            groupBoxDirection.Name = "groupBoxDirection";
            groupBoxDirection.Padding = new Padding(4, 5, 4, 5);
            groupBoxDirection.Size = new Size(299, 245);
            groupBoxDirection.TabIndex = 0;
            groupBoxDirection.TabStop = false;
            groupBoxDirection.Text = "Direction";
            // 
            // groupBoxPosition
            // 
            groupBoxPosition.Location = new Point(324, 213);
            groupBoxPosition.Margin = new Padding(4, 5, 4, 5);
            groupBoxPosition.Name = "groupBoxPosition";
            groupBoxPosition.Padding = new Padding(4, 5, 4, 5);
            groupBoxPosition.Size = new Size(463, 517);
            groupBoxPosition.TabIndex = 1;
            groupBoxPosition.TabStop = false;
            groupBoxPosition.Text = "Position";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 25);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(83, 25);
            label1.TabIndex = 2;
            label1.Text = "Damage:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 73);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(66, 25);
            label2.TabIndex = 3;
            label2.Text = "Speed:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 122);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(47, 25);
            label3.TabIndex = 4;
            label3.Text = "Size:";
            // 
            // textBoxDamage
            // 
            textBoxDamage.Location = new Point(116, 20);
            textBoxDamage.Margin = new Padding(4, 5, 4, 5);
            textBoxDamage.Name = "textBoxDamage";
            textBoxDamage.Size = new Size(198, 31);
            textBoxDamage.TabIndex = 6;
            textBoxDamage.Text = "10";
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Location = new Point(116, 68);
            textBoxSpeed.Margin = new Padding(4, 5, 4, 5);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.Size = new Size(198, 31);
            textBoxSpeed.TabIndex = 7;
            textBoxSpeed.Text = "10";
            // 
            // textBoxSize
            // 
            textBoxSize.Location = new Point(116, 117);
            textBoxSize.Margin = new Padding(4, 5, 4, 5);
            textBoxSize.Name = "textBoxSize";
            textBoxSize.Size = new Size(198, 31);
            textBoxSize.TabIndex = 8;
            textBoxSize.Text = "20";
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(17, 548);
            buttonAdd.Margin = new Padding(4, 5, 4, 5);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(299, 87);
            buttonAdd.TabIndex = 9;
            buttonAdd.Text = "Add Step";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(17, 643);
            buttonSave.Margin = new Padding(4, 5, 4, 5);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(299, 87);
            buttonSave.TabIndex = 10;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // textBoxShots
            // 
            textBoxShots.Location = new Point(116, 165);
            textBoxShots.Margin = new Padding(4, 5, 4, 5);
            textBoxShots.Name = "textBoxShots";
            textBoxShots.Size = new Size(198, 31);
            textBoxShots.TabIndex = 11;
            textBoxShots.Text = "1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 170);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(99, 25);
            label4.TabIndex = 12;
            label4.Text = "# of Shots:";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(324, 20);
            richTextBox1.Margin = new Padding(4, 5, 4, 5);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(461, 181);
            richTextBox1.TabIndex = 13;
            richTextBox1.Text = "";
            // 
            // textBoxDelay
            // 
            textBoxDelay.Location = new Point(116, 213);
            textBoxDelay.Name = "textBoxDelay";
            textBoxDelay.Size = new Size(198, 31);
            textBoxDelay.TabIndex = 14;
            textBoxDelay.Text = "2.0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 217);
            label5.Name = "label5";
            label5.Size = new Size(60, 25);
            label5.TabIndex = 15;
            label5.Text = "Delay:";
            // 
            // BulletPattern
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(814, 750);
            Controls.Add(label5);
            Controls.Add(textBoxDelay);
            Controls.Add(richTextBox1);
            Controls.Add(label4);
            Controls.Add(textBoxShots);
            Controls.Add(buttonSave);
            Controls.Add(buttonAdd);
            Controls.Add(textBoxSize);
            Controls.Add(textBoxSpeed);
            Controls.Add(textBoxDamage);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(groupBoxPosition);
            Controls.Add(groupBoxDirection);
            Margin = new Padding(4, 5, 4, 5);
            Name = "BulletPattern";
            Text = "BulletPattern";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxDirection;
        private GroupBox groupBoxPosition;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxDamage;
        private TextBox textBoxSpeed;
        private TextBox textBoxSize;
        private Button buttonAdd;
        private Button buttonSave;
        private TextBox textBoxShots;
        private Label label4;
        private RichTextBox richTextBox1;
        private TextBox textBoxDelay;
        private Label label5;
    }
}
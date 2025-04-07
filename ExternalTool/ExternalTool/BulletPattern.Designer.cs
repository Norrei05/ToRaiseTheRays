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
            SuspendLayout();
            // 
            // groupBoxDirection
            // 
            groupBoxDirection.Location = new Point(12, 128);
            groupBoxDirection.Name = "groupBoxDirection";
            groupBoxDirection.Size = new Size(209, 147);
            groupBoxDirection.TabIndex = 0;
            groupBoxDirection.TabStop = false;
            groupBoxDirection.Text = "Direction";
            // 
            // groupBoxPosition
            // 
            groupBoxPosition.Location = new Point(227, 128);
            groupBoxPosition.Name = "groupBoxPosition";
            groupBoxPosition.Size = new Size(324, 310);
            groupBoxPosition.TabIndex = 1;
            groupBoxPosition.TabStop = false;
            groupBoxPosition.Text = "Position";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 2;
            label1.Text = "Damage:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 3;
            label2.Text = "Speed:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 73);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 4;
            label3.Text = "Size:";
            // 
            // textBoxDamage
            // 
            textBoxDamage.Location = new Point(81, 12);
            textBoxDamage.Name = "textBoxDamage";
            textBoxDamage.Size = new Size(140, 23);
            textBoxDamage.TabIndex = 6;
            textBoxDamage.Text = "10";
            // 
            // textBoxSpeed
            // 
            textBoxSpeed.Location = new Point(81, 41);
            textBoxSpeed.Name = "textBoxSpeed";
            textBoxSpeed.Size = new Size(140, 23);
            textBoxSpeed.TabIndex = 7;
            textBoxSpeed.Text = "10";
            // 
            // textBoxSize
            // 
            textBoxSize.Location = new Point(81, 70);
            textBoxSize.Name = "textBoxSize";
            textBoxSize.Size = new Size(140, 23);
            textBoxSize.TabIndex = 8;
            textBoxSize.Text = "20";
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(12, 281);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(209, 78);
            buttonAdd.TabIndex = 9;
            buttonAdd.Text = "Add Step";
            buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(12, 365);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(209, 73);
            buttonSave.TabIndex = 10;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            // 
            // textBoxShots
            // 
            textBoxShots.Location = new Point(81, 99);
            textBoxShots.Name = "textBoxShots";
            textBoxShots.Size = new Size(140, 23);
            textBoxShots.TabIndex = 11;
            textBoxShots.Text = "1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 102);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 12;
            label4.Text = "# of Shots:";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(227, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(324, 110);
            richTextBox1.TabIndex = 13;
            richTextBox1.Text = "";
            // 
            // BulletPattern
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 450);
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
    }
}
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
            groupBoxGrid.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxGrid
            // 
            groupBoxGrid.Controls.Add(label1);
            groupBoxGrid.Location = new Point(402, 12);
            groupBoxGrid.Name = "groupBoxGrid";
            groupBoxGrid.Size = new Size(386, 411);
            groupBoxGrid.TabIndex = 0;
            groupBoxGrid.TabStop = false;
            groupBoxGrid.Text = "Grid";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-368, 10);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 3;
            // 
            // textBoxType
            // 
            textBoxType.Location = new Point(88, 115);
            textBoxType.Name = "textBoxType";
            textBoxType.Size = new Size(299, 23);
            textBoxType.TabIndex = 1;
            textBoxType.TextChanged += textBoxType_TextChanged;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(12, 164);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(375, 259);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(12, 12);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(384, 73);
            richTextBox2.TabIndex = 4;
            richTextBox2.Text = "The grid represents the map for the game. Click a location on the grid to set the location of an enemy in the wave. \n\nWrite the filename for the type of enemy you want to place below.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 118);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 5;
            label2.Text = "Enemy Type:";
            // 
            // WaveFormation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 437);
            Controls.Add(label2);
            Controls.Add(richTextBox2);
            Controls.Add(buttonSave);
            Controls.Add(textBoxType);
            Controls.Add(groupBoxGrid);
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
    }
}
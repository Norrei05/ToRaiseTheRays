namespace ExternalTool
{
    partial class EnemyPattern
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnemyPattern));
            groupBoxGrid = new GroupBox();
            buttonSave = new Button();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // groupBoxGrid
            // 
            groupBoxGrid.Location = new Point(496, 40);
            groupBoxGrid.Margin = new Padding(4, 5, 4, 5);
            groupBoxGrid.Name = "groupBoxGrid";
            groupBoxGrid.Padding = new Padding(4, 5, 4, 5);
            groupBoxGrid.Size = new Size(599, 690);
            groupBoxGrid.TabIndex = 0;
            groupBoxGrid.TabStop = false;
            groupBoxGrid.Text = "Grid";
            // 
            // buttonSave
            // 
            buttonSave.Font = new Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonSave.Location = new Point(17, 390);
            buttonSave.Margin = new Padding(4, 5, 4, 5);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(451, 340);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(17, 40);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(451, 342);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // EnemyPattern
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 750);
            Controls.Add(richTextBox1);
            Controls.Add(buttonSave);
            Controls.Add(groupBoxGrid);
            Margin = new Padding(4, 5, 4, 5);
            Name = "EnemyPattern";
            Text = "EnemyPattern";
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxGrid;
        private Button buttonSave;
        private RichTextBox richTextBox1;
    }
}
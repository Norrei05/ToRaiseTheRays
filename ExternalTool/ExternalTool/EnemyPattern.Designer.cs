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
            groupBoxGrid = new GroupBox();
            buttonSave = new Button();
            SuspendLayout();
            // 
            // groupBoxGrid
            // 
            groupBoxGrid.Location = new Point(347, 24);
            groupBoxGrid.Name = "groupBoxGrid";
            groupBoxGrid.Size = new Size(419, 414);
            groupBoxGrid.TabIndex = 0;
            groupBoxGrid.TabStop = false;
            groupBoxGrid.Text = "Grid";
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(139, 162);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(166, 127);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // EnemyPattern
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonSave);
            Controls.Add(groupBoxGrid);
            Name = "EnemyPattern";
            Text = "EnemyPattern";
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxGrid;
        private Button buttonSave;
    }
}
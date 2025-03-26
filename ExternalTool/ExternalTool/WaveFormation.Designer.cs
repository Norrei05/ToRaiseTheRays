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
            textBoxType = new TextBox();
            buttonSave = new Button();
            SuspendLayout();
            // 
            // groupBoxGrid
            // 
            groupBoxGrid.Location = new Point(380, 27);
            groupBoxGrid.Name = "groupBoxGrid";
            groupBoxGrid.Size = new Size(386, 411);
            groupBoxGrid.TabIndex = 0;
            groupBoxGrid.TabStop = false;
            groupBoxGrid.Text = "Grid";
            // 
            // textBoxType
            // 
            textBoxType.Location = new Point(45, 75);
            textBoxType.Name = "textBoxType";
            textBoxType.Size = new Size(244, 23);
            textBoxType.TabIndex = 1;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(12, 139);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(352, 299);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // WaveFormation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonSave);
            Controls.Add(textBoxType);
            Controls.Add(groupBoxGrid);
            Name = "WaveFormation";
            Text = "WaveFormation";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxGrid;
        private TextBox textBoxType;
        private Button buttonSave;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalTool
{
    public partial class WaveFormation : Form
    {
        private PictureBox[,] grid;
        private Color currentColor;

        private List<string> types;
        private List<Vector2> positions;

        public WaveFormation()
        {
            InitializeComponent();

            types = new List<string>();
            positions = new List<Vector2>();

            int tileWidth = 60;
            int tileHeight = 80;

            grid = new PictureBox[tileWidth, tileHeight];

            // Creating map

            int buffer = 20; // The buffer variable is used to account for the header of the group box
            int borderBuffer = 15; // The borderBuffer variable is used to keep the map from touching the border of the group box

            int tileDimensions = (groupBoxGrid.Height - buffer - borderBuffer * 2) / tileHeight;

            groupBoxGrid.Width = tileDimensions * tileWidth + borderBuffer * 2;

            this.Width = groupBoxGrid.Location.X + groupBoxGrid.Width + 40;

            // The rows are counted in a descending manner so that the boxes stop short of the grou box header
            for (int row = tileHeight - 1; row >= 0; row--)
            {
                for (int col = 0; col < tileWidth; col++)
                {
                    grid[col, row] = new PictureBox();
                    grid[col, row].Location = new Point(borderBuffer + tileDimensions * col, buffer + borderBuffer + tileDimensions * row);

                    grid[col, row].Width = tileDimensions;
                    grid[col, row].Height = tileDimensions;

                    grid[col, row].Name = $"{row},{col}";

                    grid[col, row].BackColor = Color.White;

                    groupBoxGrid.Controls.Add(grid[col, row]);

                    grid[col, row].Click += TileClicked!;
                }
            }
        }

        /// <summary>
        /// Changes the color of the picture box color based on the currently selected color
        /// </summary>
        public void TileClicked(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox tile = (PictureBox)sender;

                tile.BackColor = Color.Blue;

                int row = int.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                int col = int.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));

                positions.Add(new Vector2(col, row));
                types.Add(textBoxType.Text);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSaver = new SaveFileDialog();

            fileSaver.Title = "Save a level file.";
            fileSaver.Filter = "Level File|*.wave";

            DialogResult result = fileSaver.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    StreamWriter output = new StreamWriter(fileSaver.FileName);

                    output.WriteLine($"{positions.Count}");

                    for (int i = 0; i < positions.Count; i++)
                    {
                        output.WriteLine($"{types[i]}");
                        output.WriteLine($"{positions[i].X * 10},{positions[i].Y * 10}");
                    }

                    output.Close();

                    // Alerts the user to successful save
                    MessageBox.Show("File saved successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}

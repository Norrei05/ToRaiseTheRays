using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace ExternalTool
{
    public partial class EnemyPattern : Form
    {
        private PictureBox[,] grid;
        private Color currentColor;

        private List<Point> gridTiles;
        private List<Vector2> movements;

        public EnemyPattern()
        {
            InitializeComponent();

            gridTiles = new List<Point>();
            gridTiles.Add(new Point(20, 20));

            movements = new List<Vector2>();

            int tileWidth = 41;
            int tileHeight = 41;

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

                    if (col == 20 && row == 20)
                    {
                        grid[col, row].BackColor = Color.Blue;
                    }

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

                int row = int.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                int col = int.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));

                for (int i = 0; i < gridTiles.Count; i++)
                {
                    if (gridTiles[i].X < 40 && gridTiles[i].X >= 0 && gridTiles[i].Y < 40 && gridTiles[i].Y >= 0)
                        grid[gridTiles[i].X, gridTiles[i].Y].BackColor = Color.White;

                    gridTiles[i] = new Point(gridTiles[i].X + (20 - col),
                        gridTiles[i].Y + (20 - row));

                    if (gridTiles[i].X < 40 && gridTiles[i].X >= 0 && gridTiles[i].Y < 40 && gridTiles[i].Y >= 0)
                        grid[gridTiles[i].X, gridTiles[i].Y].BackColor = Color.FromArgb(100 + (150 / (i + 1)), 0, 0);
                }

                movements.Add(new Vector2(col - 20, row - 20));

                gridTiles.Add(new Point(20, 20));
                grid[20, 20].BackColor = Color.Blue;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSaver = new SaveFileDialog();

            fileSaver.Title = "Save a level file.";
            fileSaver.Filter = "Level File|*.pattern";

            DialogResult result = fileSaver.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    StreamWriter output = new StreamWriter(fileSaver.FileName);

                    for (int i = 0; i < movements.Count; i++)
                    {
                        output.WriteLine($"{movements[i].X * 5},{movements[i].Y * 5}");
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

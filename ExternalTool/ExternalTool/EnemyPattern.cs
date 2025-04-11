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
    /// <summary>
    /// Form for creating an enemy movement pattern
    /// </summary>
    public partial class EnemyPattern : Form
    {
        // Fields

        private PictureBox[,] grid;

        private List<Point> gridTiles;
        private List<Vector2> movements;

        private List<string> patternNames;

        // Constructor

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

            // Loads Pattern Names

            patternNames = new List<string>();

            StreamReader input = null!;

            try
            {
                input = new StreamReader("..\\..\\..\\PatternNames.txt");

                string line = null!;
                while ((line = input.ReadLine()!) != null)
                {
                    patternNames.Add(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                if (input != null)
                    input.Close();
            }
        }

        // Methods

        /// <summary>
        /// Adds a movement to the pattern with an x and y displacement based on the location clicked
        /// 
        /// The grid updates to show the final location of the movement as the center of the grid
        /// </summary>
        public void TileClicked(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox tile = (PictureBox)sender;

                int row = int.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                int col = int.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));

                foreach (PictureBox t in grid)
                {
                    t.BackColor = Color.White;
                }

                for (int i = 0; i < gridTiles.Count; i++)
                {
                    /*
                    if (gridTiles[i].X < 41 && gridTiles[i].X >= 0 && gridTiles[i].Y < 41 && gridTiles[i].Y >= 0)
                        grid[gridTiles[i].X, gridTiles[i].Y].BackColor = Color.White;
                    */

                    gridTiles[i] = new Point(gridTiles[i].X + (20 - col),
                        gridTiles[i].Y + (20 - row));

                    if (gridTiles[i].X < 41 && gridTiles[i].X >= 0 && gridTiles[i].Y < 41 && gridTiles[i].Y >= 0)
                        grid[gridTiles[i].X, gridTiles[i].Y].BackColor = Color.FromArgb(100 + (150 / (i + 1)), 0, 0);
                }

                movements.Add(new Vector2(col - 20, row - 20));

                gridTiles.Add(new Point(20, 20));
                grid[20, 20].BackColor = Color.Blue;
            }
        }

        /// <summary>
        /// Saves information on an enemy movement pattern
        /// 
        /// File Format:
        /// 
        /// First Line: number of movements
        /// Repeatable Lines: {x displacement},{y displacement}
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileSaver = new SaveFileDialog();

            fileSaver.Title = "Save a level file.";
            fileSaver.Filter = "Movement File|*.pattern";

            DialogResult result = fileSaver.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    // Saves the actual file

                    StreamWriter output = new StreamWriter(fileSaver.FileName);

                    output.WriteLine($"{movements.Count}");

                    for (int i = 0; i < movements.Count; i++)
                    {
                        output.WriteLine($"{movements[i].X * 10},{movements[i].Y * 10}");
                    }

                    output.Close();

                    // Adds filename to file, so that its existence is recorded

                    StreamWriter nameSaver = new StreamWriter("..\\..\\..\\PatternNames.txt");
                    patternNames.Add(fileSaver.FileName.Substring(fileSaver.FileName.LastIndexOf("\\") + 1, fileSaver.FileName.LastIndexOf(".") - fileSaver.FileName.LastIndexOf("\\") - 1));

                    for (int i = 0; i < patternNames.Count; i++)
                    {
                        nameSaver.WriteLine(patternNames[i]);
                    }

                    nameSaver.Close();

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

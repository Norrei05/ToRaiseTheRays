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
    public partial class BulletPattern : Form
    {
        private PictureBox[,] positionGrid;
        private PictureBox[,] directionGrid;

        private Vector2 currentDirection;

        private List<Vector2> directions;
        private List<Vector2> positions;
        private List<int[]> bulletInfo;
        private List<float> delays;

        private List<Bullet> bullets;

        private List<string> bulletNames;

        public BulletPattern()
        {
            InitializeComponent();

            CreateMaps();

            currentDirection = new Vector2(0, 0);

            directions = new List<Vector2>();
            positions = new List<Vector2>();
            bulletInfo = new List<int[]>();
            delays = new List<float>();

            bullets = new List<Bullet>();

            // Loads Pattern Names

            bulletNames = new List<string>();

            StreamReader input = null!;

            try
            {
                input = new StreamReader("..\\..\\..\\BulletNames.txt");

                string line = null!;
                while ((line = input.ReadLine()!) != null)
                {
                    bulletNames.Add(line);
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


        private void CreateMaps()
        {
            int tileWidth = 5;
            int tileHeight = 5;

            positionGrid = new PictureBox[tileWidth, tileHeight];
            directionGrid = new PictureBox[tileWidth, tileHeight];

            // Creating map

            int buffer = 20; // The buffer variable is used to account for the header of the group box
            int borderBuffer = 15; // The borderBuffer variable is used to keep the map from touching the border of the group box

            int tileDimensionsPosition = (groupBoxPosition.Height - buffer - borderBuffer * 2) / tileHeight;

            groupBoxPosition.Width = tileDimensionsPosition * tileWidth + borderBuffer * 2;

            this.Width = groupBoxPosition.Location.X + groupBoxPosition.Width + 40;

            // The rows are counted in a descending manner so that the boxes stop short of the grou box header
            for (int row = tileHeight - 1; row >= 0; row--)
            {
                for (int col = 0; col < tileWidth; col++)
                {
                    positionGrid[col, row] = new PictureBox();
                    positionGrid[col, row].Location = new Point(borderBuffer + tileDimensionsPosition * col, buffer + borderBuffer + tileDimensionsPosition * row);

                    positionGrid[col, row].Width = tileDimensionsPosition;
                    positionGrid[col, row].Height = tileDimensionsPosition;

                    positionGrid[col, row].Name = $"{row},{col}";

                    positionGrid[col, row].BackColor = Color.White;

                    if (col == 2 && row == 2)
                    {
                        positionGrid[col, row].BackColor = Color.Red;
                    }

                    groupBoxPosition.Controls.Add(positionGrid[col, row]);

                    positionGrid[col, row].Click += PositionTileClicked!;
                }
            }

            int tileDimensionsDirection = (groupBoxDirection.Height - buffer - borderBuffer * 2) / tileHeight;

            groupBoxDirection.Width = tileDimensionsDirection * tileWidth + borderBuffer * 2;

            for (int row = tileHeight - 1; row >= 0; row--)
            {
                for (int col = 0; col < tileWidth; col++)
                {
                    directionGrid[col, row] = new PictureBox();
                    directionGrid[col, row].Location = new Point(borderBuffer + tileDimensionsDirection * col, buffer + borderBuffer + tileDimensionsDirection * row);

                    directionGrid[col, row].Width = tileDimensionsDirection;
                    directionGrid[col, row].Height = tileDimensionsDirection;

                    directionGrid[col, row].Name = $"{row},{col}";

                    directionGrid[col, row].BackColor = Color.White;

                    if (col == 2 && row == 2)
                    {
                        directionGrid[col, row].BackColor = Color.Red;
                    }

                    groupBoxDirection.Controls.Add(directionGrid[col, row]);

                    directionGrid[col, row].Click += DirectionTileClicked!;
                }
            }
        }

        public void PositionTileClicked(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox tile = (PictureBox)sender;

                float row = float.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                row = row - 2;

                float col = float.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));
                col = col - 2;

                tile.BackColor = Color.Blue;

                directions.Add(currentDirection);
                positions.Add(new Vector2(col * 5, row * 5));

                int[] info = { int.Parse(textBoxDamage.Text),
                    int.Parse(textBoxSpeed.Text),
                    int.Parse(textBoxSize.Text),
                    int.Parse(textBoxShots.Text)};

                bulletInfo.Add(info);

                delays.Add(float.Parse(textBoxDelay.Text));
            }
        }

        public void DirectionTileClicked(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox tile = (PictureBox)sender;

                for (int i = 0; i < directionGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < directionGrid.GetLength(1); j++)
                    {
                        directionGrid[i, j].BackColor = Color.White;
                    }
                }

                directionGrid[2, 2].BackColor = Color.Red;

                tile.BackColor = Color.Blue;

                float row = float.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                row = row - 2;

                float col = float.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));
                col = col - 2;

                float magnitude = (float)Math.Sqrt(Math.Pow(col, 2) + Math.Pow(row, 2));

                row = row / magnitude;
                col = col / magnitude;

                currentDirection = new Vector2(col, row);
            }
        }

        private void buttonAdd_Click()
        {
            bullets.Add(new Bullet(directions, positions, bulletInfo, delays));

            directions.Clear();
            positions.Clear();
            bulletInfo.Clear();
            delays.Clear();

            for (int i = 0; i < directionGrid.GetLength(0); i++)
            {
                for (int j = 0; j < directionGrid.GetLength(1); j++)
                {
                    directionGrid[i, j].BackColor = Color.White;
                }
            }

            directionGrid[2, 2].BackColor = Color.Red;

            for (int i = 0; i < positionGrid.GetLength(0); i++)
            {
                for (int j = 0; j < positionGrid.GetLength(1); j++)
                {
                    positionGrid[i, j].BackColor = Color.White;
                }
            }

            positionGrid[2, 2].BackColor = Color.Red;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            buttonAdd_Click();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            buttonAdd_Click();

            SaveFileDialog fileSaver = new SaveFileDialog();

            fileSaver.Title = "Save a level file.";
            fileSaver.Filter = "Level File|*.bullet";

            DialogResult result = fileSaver.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    StreamWriter output = new StreamWriter(fileSaver.FileName);

                    output.WriteLine($"{bullets.Count}");

                    for (int i = 0; i < bullets.Count; i++)
                    {
                        output.WriteLine($"{bullets[i].Count}");

                        for (int j = 0; j < bullets[i].Count; j++)
                        {
                            output.Write(bullets[i].ToString());
                        }
                    }

                    output.Close();

                    // Alerts the user to successful save
                    MessageBox.Show("File saved successfully.", "File saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Saves the existance of the new bullet pattern

                    StreamWriter nameSaver = new StreamWriter("..\\..\\..\\BulletNames.txt");
                    bulletNames.Add(fileSaver.FileName.Substring(fileSaver.FileName.LastIndexOf("\\") + 1, fileSaver.FileName.LastIndexOf(".") - fileSaver.FileName.LastIndexOf("\\") - 1));

                    for (int i = 0; i < bulletNames.Count; i++)
                    {
                        nameSaver.WriteLine(bulletNames[i]);
                    }

                    nameSaver.Close();

                    this.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}

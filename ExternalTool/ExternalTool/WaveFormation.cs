using Microsoft.VisualBasic.Devices;
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
    /// <summary>
    /// Form for creating an enemy wave formation
    /// </summary>
    public partial class WaveFormation : Form
    {
        // Fields

        private PictureBox[,] grid;
        private Color currentColor;

        private List<string> types;
        private List<Vector2> positions;

        private List<int[]> enemyInfo;

        private List<string> patternNames;
        private bool patternChosen;

        // Constructors

        public WaveFormation()
        {
            InitializeComponent();

            types = new List<string>();
            positions = new List<Vector2>();

            enemyInfo = new List<int[]>();

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

            patternChosen = false;
        }

        // Methods

        /// <summary>
        /// Adds an enemy to the wave with a movement pattern determined by the filename in
        /// the text box and a location determined by the point clicked on the grid
        /// </summary> 
        public void TileClicked(object sender, EventArgs e)
        {
            if (patternChosen)
            {
                if (sender is PictureBox)
                {
                    PictureBox tile = (PictureBox)sender;

                    int row = int.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                    int col = int.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));


                    if (tile.BackColor == Color.White)
                    {
                        tile.BackColor = Color.Blue;

                        positions.Add(new Vector2(col, row));
                        types.Add(textBoxType.Text);

                        int[] info = { int.Parse(textBoxHealth.Text), 
                            int.Parse(textBoxSpeed.Text), 
                            int.Parse(textBoxShot.Text),
                            int.Parse(textBoxCollision.Text), 
                            int.Parse(textBoxWidth.Text), 
                            int.Parse(textBoxHeight.Text)};

                        enemyInfo.Add(info);
                    }
                    else
                    {
                        tile.BackColor = Color.White;

                        types.RemoveAt(positions.IndexOf(new Vector2(col, row)));
                        positions.Remove(new Vector2(col, row));
                    }

                }
            }
            else
            {
                MessageBox.Show("Must choose proper enemy pattern", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        /// <summary>
        /// Saves the information on the enemy types and starting location in a wave
        /// 
        /// File Format:
        /// 
        /// First Line: Number of Enemies 
        /// (helps file reading to ensure the required number of lines are read)
        /// 
        /// Repeatable Lines:
        /// 
        /// Line 1: string for the filename of enemy type (movement patterns)
        /// Line 2: {x location}, {y location} - enemy goes to this starting point on spawn
        /// </summary>
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
                        output.WriteLine($"{enemyInfo[i][0]},{enemyInfo[i][1]},{enemyInfo[i][2]},{enemyInfo[i][3]},{enemyInfo[i][4]},{enemyInfo[i][5]}");
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

        private void textBoxType_TextChanged(object sender, EventArgs e)
        {
            bool patternFound = false;

            for (int i = 0; i < patternNames.Count && patternFound == false; i++)
            {
                if (textBoxType.Text == patternNames[i])
                    patternFound = true;
            }

            patternChosen = patternFound;
        }

        private void InputIsInteger(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox myInput = (TextBox) sender;

                int result = 0;

                if (!int.TryParse(myInput.Text, out result))
                {
                    MessageBox.Show("The input in this text box must be an integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    myInput.Text = "10";
                }
            }
        }
    }
}

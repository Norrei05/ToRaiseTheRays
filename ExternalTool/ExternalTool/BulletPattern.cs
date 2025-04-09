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

        public BulletPattern()
        {
            InitializeComponent();

            CreateMaps();

            currentDirection = new Vector2(0, 0);

            directions = new List<Vector2>();
            positions = new List<Vector2>();
            bulletInfo = new List<int[]>();
            delays = new List<float>();
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
                        positionGrid[col, row].BackColor = Color.Blue;
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
                        directionGrid[col, row].BackColor = Color.Blue;
                    }

                    groupBoxDirection.Controls.Add(directionGrid[col, row]);

                    directionGrid[col, row].Click += DirectionTileClicked!;
                }
            }
        }

        public void PositionTileClicked(object sender, EventArgs e)
        {

        }
        public void DirectionTileClicked(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox tile = (PictureBox)sender;

                int row = int.Parse(tile.Name.Substring(0, tile.Name.IndexOf(",")));
                int col = int.Parse(tile.Name.Substring(tile.Name.IndexOf(",") + 1));

                tile.BackColor = Color.Blue;

                
            }
        }
    }
}

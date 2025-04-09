using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExternalTool
{
    internal class Bullet
    {
        // Fields

        private List<Vector2> directions;
        private List<Vector2> positions;
        private List<int[]> bulletInfo;
        private List<float> delays;

        // Properties

        public int Count
        {
            get
            {
                return directions.Count;
            }
        }

        // Constructor

        public Bullet(List<Vector2> directions, List<Vector2> positions, List<int[]> bulletInfo, List<float> delays)
        {
            this.directions = new List<Vector2>();
            foreach (Vector2 v in directions)
            {
                this.directions.Add(new Vector2(v.X, v.Y));
            }

            this.positions = new List<Vector2>();
            foreach (Vector2 v in positions)
            {
                this.positions.Add(new Vector2(v.X, v.Y));
            }

            this.bulletInfo = new List<int[]>();
            foreach (int[] i in bulletInfo)
            {
                this.bulletInfo.Add(i);
            }

            this.delays = new List<float>();
            foreach (float f in delays)
            {
                this.delays.Add(f);
            }
        }

        // Method

        public override string ToString()
        {
            string output = "";
            
            for (int i = 0; i < directions.Count; i++)
            {
                output += $"{directions[i].X},{directions[i].Y}\n";
                output += $"{positions[i].X},{positions[i].Y}\n";

                output += $"{bulletInfo[i][0]}";

                for (int j = 1; j < bulletInfo[i].Length; j++)
                {
                    output += $",{bulletInfo[i][j]}";
                }

                output += "\n";

                output += $"{delays[i]}\n";
            }

            return output;
        }
    }
}

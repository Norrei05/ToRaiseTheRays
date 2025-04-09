using System;
using System.Collections.Generic;
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
            this.directions = directions;
            this.positions = positions;
            this.bulletInfo = bulletInfo;
            this.delays = delays;
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToRaiseTheRays
{
    internal class EnemyGenerator
    {
        private List<string> waves;
        private int spawnTime;
        private double timer;

        private int curWave;

        private Texture2D enemyTexture;
        private int dimensions; 

        public EnemyGenerator(List<string> waves, int spawnTime, Texture2D enemyTexture, int dimensions)
        {
            this.waves = waves;
            this.spawnTime = spawnTime;
            timer = spawnTime / 2;
            curWave = 0;

            this.enemyTexture = enemyTexture;
            this.dimensions = dimensions;
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= spawnTime)
            {
                StreamReader input = null!;

                try
                {
                    input = new StreamReader("..\\..\\..\\"  + waves[curWave]);

                    int numEnemies = int.Parse(input.ReadLine());

                    for (int i = 0; i < numEnemies; i++)
                    {
                        string filename = input.ReadLine();

                        string pos = input.ReadLine();

                        int posX = int.Parse(pos.Substring(0, pos.IndexOf(",")));
                        int posY = int.Parse(pos.Substring(pos.IndexOf(",") + 1));

                        string[] enemyStatsText = input.ReadLine().Split(",");
                        int[] enemyStats = new int[enemyStatsText.Length];

                        // Index 0 is health, 1 is speed, 2 is shot damage, 3 is collision damage, 4 is width, 5 is height
                        for (int j = 0; j < enemyStats.Length; j++)
                        {
                            enemyStats[j] = int.Parse(enemyStatsText[j]);
                        }

                        Game1.ActiveEntities.Add(new Enemy(enemyTexture, new Rectangle(0, 0, enemyStats[4], enemyStats[5]), new Vector2(enemyStats[1], 0), Alignment.ENEMY, new Vector2(posX, posY), 1, filename, enemyStats[0], enemyStats[2], enemyStats[3]));
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

                timer = 0;
                curWave++;

                if (curWave >= waves.Count)
                {
                    curWave = 0;
                }
            }
        }
    }
}

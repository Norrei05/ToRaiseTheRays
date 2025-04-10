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
        // Fields

        private List<string> normalEnemies;
        private List<string> fastEnemies;
        private List<string> bosses;

        private int normalWait;
        private double timer;

        private int fastWait;
        private double fastTimer;

        private int normalWavesCount;
        private int currentWave;

        private int fastTypesCount;

        private Texture2D enemyTexture;
        private Texture2D bulletTexture;

        private bool inPlay;

        Random rng;

        // Properties

        public bool InPlay => inPlay;

        // Constructor

        public EnemyGenerator(int normalWait, int fastWait, Texture2D enemyTexture, Texture2D bulletTexture)
        {
            normalEnemies = new List<string>();
            fastEnemies = new List<string>();
            bosses = new List<string>();
            
            this.normalWait = normalWait;
            this.fastWait = fastWait;

            timer = 0;
            fastTimer = 0;

            normalWavesCount = 3;
            currentWave = 0;

            fastTypesCount = 0;

            this.enemyTexture = enemyTexture;
            this.bulletTexture = bulletTexture;

            inPlay = false;

            rng = new Random();
        }

        public void Update(GameTime gameTime)
        {
            if (inPlay)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                fastTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= normalWait)
                {

                    if (currentWave < normalWavesCount)
                    {
                        currentWave++;
                        GenerateWave(normalEnemies[rng.Next(normalEnemies.Count)]);
                    }
                    else if (currentWave == normalWavesCount)
                    {
                        GenerateWave(bosses[rng.Next(bosses.Count)]);
                    }

                    timer = 0;
                }

                if (fastTimer >= fastWait)
                {
                    GenerateWave(fastEnemies[rng.Next(fastTypesCount)]);

                    fastTimer = 0;
                }

                if (currentWave == normalWavesCount)
                {
                    bool defeatedBoss = true;

                    for (int i = 0; i < Game1.ActiveEntities.Count; i++)
                    {
                        if (Game1.ActiveEntities[i] is Enemy)
                        {
                            defeatedBoss = false;
                        }
                    }

                    if (defeatedBoss)
                    {
                        inPlay = false;
                    }
                }
            }
        }

        public void Start()
        {
            inPlay = true;
        }

        public void NextLevel()
        {
            inPlay = true;

            normalWavesCount += 2;
            currentWave = 0;
            
            if (fastTypesCount < fastEnemies.Count)
            {
                fastTypesCount++;
            }
        }

        public void Reset()
        {
            normalWavesCount = 3;
            fastTypesCount = 1;
        }

        public void AddNormal(string filename)
        {
            normalEnemies.Add(filename + ".wave");
        }

        public void AddFast(string filename)
        {
            fastEnemies.Add(filename + ".wave");
        }

        public void AddBoss(string filename)
        {
            bosses.Add(filename + ".wave");
        }

        private void GenerateWave(string filename)
        {
            StreamReader input = null!;

            try
            {
                input = new StreamReader("..\\..\\..\\" + filename);

                int numEnemies = int.Parse(input.ReadLine());

                for (int i = 0; i < numEnemies; i++)
                {
                    string patternName = input.ReadLine();
                    string bulletName = input.ReadLine();

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

                    Game1.ActiveEntities.Add(new Enemy(enemyTexture, new Rectangle(0, 0, enemyStats[4], enemyStats[5]), new Vector2(enemyStats[1], 0), Alignment.ENEMY, new Vector2(posX, posY), 1, patternName, bulletName, enemyStats[0], enemyStats[2], enemyStats[3], bulletTexture));
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
        
    }
}

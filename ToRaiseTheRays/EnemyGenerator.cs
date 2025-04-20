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
    /// <summary>
    /// Handles wave progression
    /// </summary>
    internal class EnemyGenerator
    {
        // Fields

        private List<string> normalEnemies;
        private List<string> fastEnemies;
        private List<string> bosses;

        private List<Texture2D> bossTextures;

        private int normalWait;
        private double timer;

        private int fastWait;
        private double fastTimer;

        private int normalWavesCount;
        private int currentWave;

        private int typesCount;

        private Texture2D enemyTexture;
        private Texture2D fastTexture;

        private Texture2D bulletTexture;

        private bool inPlay;

        Random rng;

        // Properties

        public bool InPlay => inPlay;

        // Constructor

        /// <summary>
        /// Custom Constructor
        /// </summary>
        /// <param name="normalWait">time for each wave to generate after the last is defeated</param>
        /// <param name="fastWait">time between each minor wave of enemies</param>
        public EnemyGenerator(int normalWait, int fastWait, Texture2D enemyTexture, Texture2D fastTexture, Texture2D bulletTexture)
        {
            normalEnemies = new List<string>();
            fastEnemies = new List<string>();
            bosses = new List<string>();

            bossTextures = new List<Texture2D>();
            
            this.normalWait = normalWait;
            this.fastWait = fastWait;

            timer = 0;
            fastTimer = 0;

            normalWavesCount = 0;
            currentWave = 0;

            typesCount = 0;

            this.enemyTexture = enemyTexture;
            this.fastTexture = fastTexture;
            this.bulletTexture = bulletTexture;

            inPlay = false;

            rng = new Random();
        }

        /// <summary>
        /// Handles wave generation
        /// 
        /// If the generator is "inPlay":
        /// 
        /// Each normal wave (chosen randomly) generates after the last is defeated
        /// 
        /// Fast waves continously spawn during each wave
        /// 
        /// The boss fight loads after the last normal wave is completed
        ///     If the boss is defeated "inPlay" becomes false
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (inPlay)
            {
                fastTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= normalWait)
                {
                    if (!EnemiesRemain())
                    {
                        if (currentWave < normalWavesCount)
                        {
                            GenerateWave(normalEnemies[rng.Next(Math.Clamp(typesCount, 1, normalEnemies.Count))], enemyTexture, 50);
                            currentWave++;
                        }
                        else if (currentWave == normalWavesCount)
                        {
                            int thisBoss = rng.Next(bosses.Count);

                            GenerateWave(bosses[thisBoss], bossTextures[thisBoss], 500);
                            currentWave++;
                        }
                        else
                        {
                            inPlay = false;
                        }

                        timer = 0;
                    }
                }
                else
                {
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (fastTimer >= fastWait)
                {
                    GenerateWave(fastEnemies[rng.Next(Math.Clamp(typesCount, 1, fastEnemies.Count))], fastTexture, 5);

                    fastTimer = 0;
                }
            }
        }

        /// <summary>
        /// Starts the enemy generator
        /// </summary>
        public void Start()
        {
            inPlay = true;
        }

        /// <summary>
        /// Increments the number of waves that will be generated
        /// and the number of types of fast enemies that can appear
        /// </summary>
        public void NextLevel()
        {
            inPlay = true;

            timer = 0;
            fastTimer = 0;

            normalWavesCount += 1;
            currentWave = 0;

            typesCount++;
        }

        /// <summary>
        /// Resets fields to initial
        /// </summary>
        public void Reset()
        {
            normalWavesCount = 0;
            typesCount = 0;

            timer = 0;
            fastTimer = 0;
        }

        /// <summary>
        /// Adds a normal wave 
        /// </summary>
        public void AddNormal(string filename)
        {
            normalEnemies.Add(filename + ".wave");
        }

        /// <summary>
        /// Adds a fast wave
        /// </summary>
        public void AddFast(string filename)
        {
            fastEnemies.Add(filename + ".wave");
        }

        /// <summary>
        /// Adds a boss and its texture
        /// </summary>
        /// <param name="filename"></param>
        public void AddBoss(string filename, Texture2D texture)
        {
            bosses.Add(filename + ".wave");
            bossTextures.Add(texture);
        }

        /// <summary>
        /// Checks if enemies still remain alive
        /// </summary>
        public bool EnemiesRemain()
        {
            bool remain = false;

            for (int i = 0; i < Game1.ActiveEntities.Count; i++)
            {
                if (Game1.ActiveEntities[i] is Enemy)
                {
                    remain = true;
                }
            }

            return remain;
        }

        /// <summary>
        /// Generates enemies by reading an external file for the wave
        /// </summary>
        private void GenerateWave(string filename, Texture2D texture, int points)
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

                    Game1.ActiveEntities.Add(new Enemy(texture, new Rectangle(0, 0, enemyStats[4], enemyStats[5]), new Vector2(enemyStats[1], 0), Alignment.ENEMY, new Vector2(posX, posY), 1, patternName, bulletName, enemyStats[0], enemyStats[2], enemyStats[3], points, bulletTexture));
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
        

        public void Draw(Texture2D texture, Rectangle screenBounds, SpriteFont font)
        {
            if (currentWave > normalWavesCount)
            {
                for (int i = 0; i < Game1.ActiveEntities.Count; i++)
                {
                    if (Game1.ActiveEntities[i] is Enemy e && bossTextures.Contains(e.Texture))
                    {
                        Game1.SpriteBatch.Draw(texture, new Rectangle(530, screenBounds.Height - 15 - (e.Health * 2), 50, e.Health * 2), Color.DarkRed);

                        if (e.Health > 0)
                            Game1.SpriteBatch.DrawString(font, "+", new Vector2(screenBounds.Width - 54, screenBounds.Height - 55), Color.Yellow);
                    }
                }
            }
        }
    }
}

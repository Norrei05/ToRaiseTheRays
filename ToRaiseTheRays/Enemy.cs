using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace ToRaiseTheRays;

/// <summary>
/// Game objects that can damage enemies
/// </summary>
public class Enemy : GameObject 
{
    
    // Fields/Properties
     
    public bool Spawning { get; protected set; }
    public int Health { get; protected set; }
    public Vector2 Direction { get; protected set; }
    public int ShotDamage { get; protected set; }
    public int CollisionDamage { get; protected set; }

    private double timer;

    private double invulTime;

    // Movement

    private Vector2 spawnTarget;
    private double spawnDuration;

    private List<Vector2> movements;
    private int patternIndex;

    private Vector2 lastPosition;
    private Vector2 target;

    // Bullets

    private List<List<Bullet>> bulletPattern;
    private Texture2D bulletTexture;

    private List<int> shots;
    private List<float> delays;

    private int patternCount;
    private int shotCount;
    private double shotTimer;

    // Properties

    public Texture2D Texture => texture;

    // Constructors

    /// <summary>
    /// Custom Constructor
    /// </summary>
    public Enemy(Texture2D texture, Rectangle position, Vector2 velocity, Alignment alignment, Vector2 spawnPosition, double spawnTime, string patternName, string bulletName,
        int health, int shotDamage, int collisionDamage, Texture2D bulletTexture) 
        : base(texture, position, velocity, alignment)
    {
        Health = health;
        ShotDamage = shotDamage;
        CollisionDamage = collisionDamage;

        invulTime = 0;

        Spawning = true;
        
        spawnTarget = new Vector2(spawnPosition.X - position.Width / 2, spawnPosition.Y - position.Height / 2);
        spawnDuration = spawnTime;
        timer = 0;
        
        // Start at a random edge of the top third of the screen
        Random random = new Random();
        int edge = random.Next(3); // 0 = top, 1 = left, 2 = right
        
        this.position.X = edge switch {
            0 => random.Next(Game1.ScreenBounds.Width - this.position.Width),
            1 => -this.position.Width,
            _ => Game1.ScreenBounds.Width
        };
        
        // Only randomize Y if not coming from the top
        this.position.Y = edge == 0 ? -this.position.Height : random.Next(Game1.ScreenBounds.Height / 3) - this.position.Height;

        // Loads enemy movement pattern from file

        movements = new List<Vector2>();

        ReadMovements(patternName);
        patternIndex = 0;

        // Loads enemy bullet pattern from file

        bulletPattern = new List<List<Bullet>>();
        this.bulletTexture = bulletTexture;

        shots = new List<int>();
        delays = new List<float>();

        ReadBullets(bulletName);

        patternCount = 0;
        shotCount = 0;
        shotTimer = 0;

        // Sets information for enemy movement

        lastPosition = new Vector2(this.position.X, this.position.Y);
        target = spawnTarget + movements[patternIndex];
    }

    // Methods

    /// <summary>
    /// Handles enemy movement
    /// 
    /// Enemies start by coming from a random point off the screen to their spawn location
    /// Then they repeat their movement pattern
    /// </summary>
    public void Move(GameTime gameTime) {
        if (Spawning) 
        {
            // Location is updated to be a point along the line from their last position 
            // to the spawn location
            // The exact point is determined by ratio between timer and the spawntime
            // This allows the enemy to move to a point at a constant speed

            timer += gameTime.ElapsedGameTime.TotalSeconds;
            float t = (float)(timer / spawnDuration);
            
            position.X = (int)MathHelper.Lerp(lastPosition.X, (int)spawnTarget.X, t);
            position.Y = (int)MathHelper.Lerp(lastPosition.Y, (int)spawnTarget.Y, t);

            if (timer >= spawnDuration) 
            {
                Spawning = false;
                timer = 0;

                lastPosition = new Vector2(position.X, position.Y);
            }
        }
        else
        {
            // Movement works the same as when spawning, but the time is determined by the distance needed to travel
            // divided by magnitude of velocity (d = vt) 

            float speed = (float) Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
            float distance = (float)Math.Sqrt(Math.Pow(movements[patternIndex].X, 2) + Math.Pow(movements[patternIndex].Y, 2));

            float timeReq = distance / speed;

            timer += gameTime.ElapsedGameTime.TotalSeconds;

            float t = (float)(timer / timeReq);

            position.X = (int)MathHelper.Lerp(lastPosition.X, (int)target.X, t);
            position.Y = (int)MathHelper.Lerp(lastPosition.Y, (int)target.Y, t);

            // When movement to one location concluded, the index of movements updates
            // in order for the enemy to move to its next position
            if (timer >= timeReq)
            {
                position.X = (int) target.X;
                position.Y = (int) target.Y;

                patternIndex++;
                
                if (patternIndex >= movements.Count)
                {
                    patternIndex = 0;
                }

                target = new Vector2(position.X + movements[patternIndex].X, position.Y + movements[patternIndex].Y);
                lastPosition = new Vector2(position.X, position.Y);

                timer = 0;
            }

            // Shooting 

            if (bulletPattern.Count > 0)
            {
                shotTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (shotTimer > delays[patternCount])
                {
                    Shoot(bulletPattern[patternCount]);

                    shotTimer = 0;
                    shotCount++;

                    if (shotCount >= shots[patternCount])
                    {
                        shotCount = 0;
                        patternCount++;

                        if (patternCount >= bulletPattern.Count)
                        {
                            patternCount = 0;
                        }
                    }
                }
            }
        }


        if (invulTime > 0)
        {
            invulTime -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    /// <summary>
    /// Checks collisions for the enemy
    /// 
    /// If it hits a player bullet it takes damage and 
    /// if it hits the player it dies
    /// </summary>
    /// <param name="other">GameObject being checked for collisions</param>
    public override void CheckCollision(GameObject other) {
        if (other.Alignment == Alignment) return;
        if (position.Intersects(other.position) && !Spawning) {
            if (other is Bullet bullet) TakeDamage(bullet.Damage);
            else if (other is Player && invulTime <= 0)
            {
                TakeDamage(20);
                invulTime = 1.2;
            }
        }

        if (!Game1.ScreenBounds.Intersects(position) && !Spawning) Health = 0;
    }

    /// <summary>
    /// Substract damage from health
    /// </summary>
    /// <param name="damage">decrease in health</param>
    public virtual void TakeDamage(int damage) => Health -= damage;

    /// <summary>
    /// Shoots list of bullets
    /// </summary>
    /// <param name="ammo"></param>
    public void Shoot(List<Bullet> ammo)
    {
        for (int i = 0; i < ammo.Count; i++)
        {
            Game1.ActiveEntities.Add(new Bullet(bulletTexture, 
                                    new Rectangle(this.position.X + this.position.Width / 2 + ammo[i].position.X, 
                                    this.position.Y + this.position.Height + ammo[i].position.Y, 
                                    ammo[i].position.Width, ammo[i].position.Height),
                                    new Vector2(ammo[i].Velocity.X, ammo[i].Velocity.Y),
                                    ammo[i].Damage, this, this.Alignment));
        }
    }


    // File Inputs

    /// <summary>
    /// Loads the enemies movement pattern from an external file
    /// </summary>
    /// <param name="filename">indicator for the external file being read</param>
    public void ReadMovements(string filename)
    {
        StreamReader input = null!;

        try
        {
            input = new StreamReader("..\\..\\..\\" + filename + ".pattern");

            int numMoves = int.Parse(input.ReadLine());

            for (int i = 0; i < numMoves; i++)
            {
                string move = input.ReadLine();

                int moveX = int.Parse(move.Substring(0, move.IndexOf(",")));
                int moveY = int.Parse(move.Substring(move.IndexOf(",") + 1));

                movements.Add(new Vector2(moveX, moveY));
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

    /// <summary>
    /// Loads bullet pattern by reading external files and 
    /// saving the bullets to a 2 dimensional list
    /// </summary>
    public void ReadBullets(string filename)
    {
        StreamReader input = null!;

        try
        {
            input = new StreamReader("..\\..\\..\\" + filename + ".bullet");

            int numSteps = int.Parse(input.ReadLine());

            for (int i = 0; i < numSteps; i++)
            {
                bulletPattern.Add(new List<Bullet>());

                int numBullets = int.Parse(input.ReadLine());

                for (int j = 0; j < numBullets; j++)
                {
                    // Direction

                    string direction = input.ReadLine();

                    float directionX = float.Parse(direction.Substring(0, direction.IndexOf(",")));
                    float directionY = float.Parse(direction.Substring(direction.IndexOf(",") + 1));

                    // Relative Position

                    string position = input.ReadLine();

                    int positionX = int.Parse(position.Substring(0, position.IndexOf(",")));
                    int positionY = int.Parse(position.Substring(position.IndexOf(",") + 1));

                    // Other Info

                    string[] bulletText = input.ReadLine().Split(",");
                    int[] bulletStats = new int[bulletText.Length];

                    for (int k = 0; k < bulletStats.Length; k++)
                    {
                        bulletStats[k] = int.Parse(bulletText[k]);
                    }

                    // Adding Bullet

                    bulletPattern[i].Add(new Bullet(bulletTexture, new Rectangle(positionX, positionY, bulletStats[2], bulletStats[2]),
                                                    new Vector2(directionX * bulletStats[1], directionY * bulletStats[1]), bulletStats[0], this, this.Alignment));
                }

                // Pattern

                int shot = int.Parse(input.ReadLine());
                float delay = float.Parse(input.ReadLine());

                shots.Add(shot);
                delays.Add(delay);
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

    public override void Draw(Color overlay)
    {
        if (Spawning)
        {
            Game1.SpriteBatch.Draw(texture, position, Color.Gray);
        }
        else
        {
            base.Draw(overlay);
        }
    }
}
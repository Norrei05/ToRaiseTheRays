using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace ToRaiseTheRays;

public class Enemy : GameObject {
    public bool Spawning { get; protected set; }
    public int Health { get; protected set; }
    public Vector2 Direction { get; protected set; }
    public int ShotDamage { get; protected set; }
    public int CollisionDamage { get; protected set; }

    private double timer;

    private Vector2 spawnTarget;
    private double spawnDuration;

    private List<Vector2> movements;
    private int patternIndex;
    private Vector2 target;

    private string filename;

    public Enemy(Texture2D texture, Rectangle position, Vector2 velocity, Alignment alignment, Vector2 spawnPosition, double spawnTime, string filename) 
        : base(texture, position, velocity, alignment)
    {
        Health = 30;
        ShotDamage = 10;
        CollisionDamage = 10;

        Spawning = true;
        
        spawnTarget = spawnPosition;
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

        movements = new List<Vector2>();

        ReadMovements(filename);

        patternIndex = 0;

        target = spawnPosition + movements[patternIndex];

        this.filename = filename;
    }

    public void Move(GameTime gameTime) {
        if (Spawning) 
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            float t = (float)(timer / spawnDuration);
            
            position.X = (int)MathHelper.Lerp(position.X, (int)spawnTarget.X, t);
            position.Y = (int)MathHelper.Lerp(position.Y, (int)spawnTarget.Y, t);

            if (timer >= spawnDuration) 
            {
                Spawning = false;
                timer = 0;
            }
        }
        else
        {
            float speed = (float) Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
            float distance = (float)Math.Sqrt(Math.Pow(movements[patternIndex].X, 2) + Math.Pow(movements[patternIndex].Y, 2));

            float timeReq = distance / speed;

            timer += gameTime.ElapsedGameTime.TotalSeconds;

            float t = (float)(timer / timeReq);

            position.X = (int)MathHelper.Lerp(position.X, (int)target.X, t);
            position.Y = (int)MathHelper.Lerp(position.Y, (int)target.Y, t);

            if (timer >= spawnDuration)
            {
                patternIndex++;
                
                if (patternIndex >= movements.Count)
                {
                    patternIndex = 0;
                }

                target = new Vector2(position.X + movements[patternIndex].X, position.Y + movements[patternIndex].Y);

                timer = 0;
            }
        }

    }

    public override void CheckCollision(GameObject other) {
        if (other.Alignment == Alignment) return;
        if (position.Intersects(other.position)) {
            if (other is Bullet bullet) TakeDamage(bullet.Damage);
            else if (other is Player)
            {
                Health = 0; // Dies instantly on crash
            }
        }
    }

    public virtual void TakeDamage(int damage) => Health -= damage;

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
}
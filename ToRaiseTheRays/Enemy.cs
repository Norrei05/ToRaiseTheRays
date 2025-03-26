using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ToRaiseTheRays;

public abstract class Enemy : GameObject {
    public bool Spawning { get; protected set; }
    public int Health { get; protected set; }
    public Vector2 Direction { get; protected set; }
    public int ShotDamage { get; protected set; }
    public int CollisionDamage { get; protected set; }

    private Vector2 spawnTarget;
    private double spawnTimer;
    private double spawnDuration;

    protected Enemy(Texture2D texture, Rectangle position, Vector2 velocity, Alignment alignment, Vector2 spawnPosition, double spawnTime) 
        : base(texture, position, velocity, alignment) {
        Spawning = true;
        
        spawnTarget = spawnPosition;
        spawnDuration = spawnTime;
        spawnTimer = 0;
        
        // Start at a random edge of the top third of the screen
        Random random = new Random();
        int edge = random.Next(3); // 0 = top, 1 = left, 2 = right
        
        position.X = edge switch {
            0 => random.Next(Game1.ScreenBounds.Width - position.Width),
            1 => -position.Width,
            _ => Game1.ScreenBounds.Width
        };
        
        // Only randomize Y if not coming from the top
        position.Y = edge == 0 ? -position.Height : random.Next(Game1.ScreenBounds.Height / 3) - position.Height;
    }

    public override void Move() {
        if (Spawning) {
            spawnTimer += 1.0 / 60.0; // Assuming 60 FPS
            float t = (float)(spawnTimer / spawnDuration);
            
            position.X = (int)MathHelper.Lerp(position.X, (int)spawnTarget.X, t);
            position.Y = (int)MathHelper.Lerp(position.Y, (int)spawnTarget.Y, t);
            
            if (spawnTimer >= spawnDuration) Spawning = false;
        }
        else base.Move();
    }

    public override void CheckCollision(GameObject other) {
        if (other.Alignment == Alignment) return;
        if (position.Intersects(other.position)) {
            if (other is Bullet bullet) TakeDamage(bullet.Damage);
            else if (other is Player) Health = 0; // Dies instantly on crash
        }
    }

    public virtual void TakeDamage(int damage) => Health -= damage;
}
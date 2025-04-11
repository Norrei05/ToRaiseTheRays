using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ToRaiseTheRays;

/// <summary>
/// Represents the gameobject the player controls
/// </summary>
public class Player : GameObject {
    public int Health { get => health; }
    private int health;
    private int invulLength;
    
    private Vector2 acceleration;
    private float accelModifier;
    
    // Velocity is in GameObject
    private float velocityCap;
    private float friction;
    
    private float reloadTime;
    private float currentReloadTime;
    
    // private Texture2D fogTexture;
    // private Rectangle fogPosition;
    
    private Dictionary<string, Texture2D> directionSprites;

    /// <summary>
    /// Custom Constructor
    /// </summary>
    public Player(Dictionary<string, Texture2D> sprites, Rectangle position, Texture2D fogTexture)
        : base(sprites["Barque_N"], position, Vector2.Zero, Alignment.FRIENDLY) {
        health = 100;
        invulLength = 60; // 1 second at 60fps
        
        acceleration = Vector2.Zero;
        accelModifier = 0.4f;
        
        velocityCap = 8.0f;
        friction = 0.1f;
        
        reloadTime = 0.2f;
        currentReloadTime = 0;
        
        // this.fogTexture = fogTexture;
        // this.fogPosition = new Rectangle(0, 0, Game1.ScreenBounds.Width, Game1.ScreenBounds.Height);
        
        directionSprites = sprites;
    }

    /// <summary>
    /// Generates bullet
    /// </summary>
    public void Shoot() {
        if (currentReloadTime <= 0 && Keyboard.GetState().IsKeyDown(Keys.Space)) {
            base.Shoot(new Vector2(0, -1), 10, 10, 20);
            currentReloadTime = reloadTime;
        }
    }

    /// <summary>
    /// Decreases player health
    /// </summary>
    public void TakeDamage(int damage) {
        health -= damage;
        invulLength = 60;
    }

    /// <summary>
    /// Updates player position
    /// </summary>
    public override void Move() {
        KeyboardState state = Keyboard.GetState();

        // Update acceleration
        acceleration = Vector2.Zero;
        if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) acceleration.X -= accelModifier;
        if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) acceleration.X += accelModifier;
        if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) acceleration.Y -= accelModifier;
        if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) acceleration.Y += accelModifier;

        // Update velocity
        velocity += acceleration;
        if (velocity != Vector2.Zero) velocity -= Vector2.Normalize(velocity) * friction;
        if (velocity.Length() > velocityCap) velocity = Vector2.Normalize(velocity) * velocityCap;

        // Update position
        position.X += (int)velocity.X;
        position.Y += (int)velocity.Y;
        
        // Screen bounds position checking
        position.X = MathHelper.Clamp(position.X, 0, Game1.ScreenBounds.Width - position.Width);
        position.Y = MathHelper.Clamp(position.Y, 0, Game1.ScreenBounds.Height - position.Height);
        
        // Update velocity and acceleration when hitting bounds
        if (position.X <= 0 || position.X >= Game1.ScreenBounds.Width - position.Width) {
            velocity.X = position.X <= 0 ? Math.Max(velocity.X, 0) : Math.Min(velocity.X, 0);
            acceleration.X = position.X <= 0 ? Math.Max(acceleration.X, 0) : Math.Min(acceleration.X, 0);
        }
        if (position.Y <= 0 || position.Y >= Game1.ScreenBounds.Height - position.Height) {
            velocity.Y = position.Y <= 0 ? Math.Max(velocity.Y, 0) : Math.Min(velocity.Y, 0);
            acceleration.Y = position.Y <= 0 ? Math.Max(acceleration.Y, 0) : Math.Min(acceleration.Y, 0);
        }

        UpdateSprite(state);
        
        if (currentReloadTime > 0) currentReloadTime -= 1.0f/60.0f;
        if (invulLength > 0) invulLength--;
    }

    /// <summary>
    /// Changes the players appearance depending on what direction it moves in
    /// </summary>
    private void UpdateSprite(KeyboardState state) {
        string direction = "";
        
        int vertical = (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up) ? 1 : 0) - (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down) ? 1 : 0);
        int horizontal = (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right) ? 1 : 0) - (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left) ? 1 : 0);
        
        if (vertical > 0) direction += "U";
        else if (vertical < 0) direction += "D";

        if (horizontal > 0) direction += "R";
        else if (horizontal < 0) direction += "L";
        
        if (direction == "") direction = "N";
        
        texture = directionSprites["Barque_" + direction];
    }

    /// <summary>
    /// Checks if the player has collided with an enemy or bullet
    /// and makes proper changes
    /// </summary>
    public override void CheckCollision(GameObject other) {
        if (other.Alignment == Alignment || invulLength > 0) return;
        if (position.Intersects(other.position)) {
            if (other is Bullet bullet) TakeDamage(bullet.Damage);
            else if (other is Enemy enemy) TakeDamage(enemy.CollisionDamage);
        }
    }

    /// <summary>
    /// Resets health
    /// </summary>
    public void Reset()
    {
        health = 100;
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ToRaiseTheRays;

public class Player : GameObject {
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

    public Player(Dictionary<string, Texture2D> sprites, Rectangle position, Texture2D fogTexture)
        : base(sprites["Barque_N"], position, Vector2.Zero, Alignment.FRIENDLY) {
        this.health = 100;
        this.invulLength = 60; // 1 second at 60fps
        
        this.acceleration = Vector2.Zero;
        this.accelModifier = 0.4f;
        
        this.velocityCap = 8.0f;
        this.friction = 0.1f;
        
        this.reloadTime = 0.2f;
        this.currentReloadTime = 0;
        
        // this.fogTexture = fogTexture;
        // this.fogPosition = new Rectangle(0, 0, Game1.ScreenBounds.Width, Game1.ScreenBounds.Height);
        
        this.directionSprites = sprites;
    }

    public void Shoot(List<Bullet> bullets, Texture2D bulletTexture) {
        if (currentReloadTime <= 0 && Keyboard.GetState().IsKeyDown(Keys.Space)) {
            Rectangle bulletPos = new Rectangle(position.X + position.Width/2 - 10, position.Y, 20, 20);
            bullets.Add(new Bullet(bulletTexture, bulletPos, new Vector2(0, -10), 10, Alignment.FRIENDLY));
            currentReloadTime = reloadTime;
        }
    }

    public void TakeDamage(int damage) {
        if (invulLength <= 0) {
            health -= damage;
            invulLength = 60;
        }
    }

    public override void Move() {
        KeyboardState state = Keyboard.GetState();

        acceleration = Vector2.Zero;
        if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) acceleration.X -= accelModifier;
        if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) acceleration.X += accelModifier;
        if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up)) acceleration.Y -= accelModifier;
        if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down)) acceleration.Y += accelModifier;

        velocity += acceleration;
        if (velocity != Vector2.Zero) velocity -= Vector2.Normalize(velocity) * friction;
        if (velocity.Length() > velocityCap) velocity = Vector2.Normalize(velocity) * velocityCap;

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

    public override void CheckCollision(GameObject other) {
        if (position.Intersects(other.position) && other.alignment != Alignment.FRIENDLY) {
            if (other is Bullet bullet) TakeDamage(bullet.Damage);
            else TakeDamage(10); // Default damage for non-bullet collisions
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        
        // Debug text position
        Vector2 debugTextPosition = new Vector2(position.X, position.Y - 40);
        
        // Create debug strings
        string velocityText = $"Velocity: {velocity.X:F2}, {velocity.Y:F2}";
        string accelerationText = $"Acceleration: {acceleration.X:F2}, {acceleration.Y:F2}";
        
        SpriteFont font = Game1.PapyrusFont;
        
        // Draw debug text
        spriteBatch.DrawString(font, velocityText, debugTextPosition, Color.White);
        spriteBatch.DrawString(font, accelerationText, debugTextPosition + new Vector2(0, 20), Color.White);
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToRaiseTheRays;

public enum Alignment {FRIENDLY, NEUTRAL, ENEMY}

/// <summary>
/// Representing an interactable object in the game
/// </summary>
public abstract class GameObject {
    protected Texture2D texture;
    public Rectangle position;
    protected Vector2 velocity;
    public Alignment Alignment { get; protected set; }

    public Vector2 Velocity => velocity;

    /// <summary>
    /// Custom constructor
    /// </summary>
    public GameObject(Texture2D texture, Rectangle position, Vector2 velocity, Alignment alignment) {
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
        Alignment = alignment;
    }

    /// <summary>
    /// Updates position
    /// </summary>
    public virtual void Move() {
        position.X += (int)velocity.X;
        position.Y += (int)velocity.Y;
    }

    /// <summary>
    /// Checks for collsiion with other game objects
    /// </summary>
    public abstract void CheckCollision(GameObject other);

    /// <summary>
    /// Generates bullet
    /// </summary>
    public virtual void Shoot(Vector2 direction, float speed, int damage, int size) {
        Rectangle bulletPos = new Rectangle(
            position.X + position.Width/2 - size/2,
            position.Y + position.Height/2 - size/2,
            size, size
        );
        Game1.ActiveEntities.Add(new Bullet(Game1.BulletTexture, bulletPos, direction * speed, damage, this, Alignment));
    }

    /// <summary>
    /// Draws object
    /// </summary>
    public virtual void Draw(Color overlay) => Game1.SpriteBatch.Draw(texture, position, overlay);
}
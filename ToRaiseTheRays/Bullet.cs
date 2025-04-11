using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToRaiseTheRays;

/// <summary>
/// Represents bullet that are shot by the enemies and player
/// </summary>
public class Bullet : GameObject {

    // Properties

    public bool Alive { get; private set; }
    public int Damage { get; set; }
    public GameObject Parent { get; private set; }
    
    // Constructor

    /// <summary>
    /// Custom Constructor
    /// </summary>
    public Bullet(Texture2D texture, Rectangle position, Vector2 velocity, int damage, GameObject parent, Alignment alignment) 
        : base(texture, position, velocity, alignment) {
        Alive = true;
        Damage = damage;
        Parent = parent;
    }

    /// <summary>
    /// Updates bullet position
    /// </summary>
    public override void Move() {
        base.Move();
        if (!Game1.ScreenBounds.Intersects(position)) Alive = false;
    }

    /// <summary>
    /// Checks if the bullet has hit another game object or gone off the screen
    /// </summary>
    public override void CheckCollision(GameObject other) {
        if (other.Alignment == Alignment || other is Bullet) return;
        if (position.Intersects(other.position)) Alive = false;
    }
}
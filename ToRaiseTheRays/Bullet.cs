using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToRaiseTheRays;

public class Bullet : GameObject {
    public bool IsActive { get; private set; }
    public int Damage { get; set; }

    public Bullet(Texture2D texture, Rectangle position, Vector2 velocity, int damage, Alignment alignment) 
        : base(texture, position, velocity, alignment) {
        IsActive = true;
        Damage = damage;
    }

    public override void Move() {
        base.Move();
        if (!Game1.ScreenBounds.Intersects(position)) IsActive = false;
    }

    public override void CheckCollision(GameObject other) {
        if (position.Intersects(other.position) && other.alignment != this.alignment) IsActive = false;
    }
}
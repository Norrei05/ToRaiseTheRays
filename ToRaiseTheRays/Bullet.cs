using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToRaiseTheRays;

public class Bullet : GameObject {
    public bool Alive { get; private set; }
    public int Damage { get; set; }
    public GameObject Parent { get; private set; }

    public Bullet(Texture2D texture, Rectangle position, Vector2 velocity, int damage, GameObject parent, Alignment alignment) 
        : base(texture, position, velocity, alignment) {
        Alive = true;
        Damage = damage;
        Parent = parent;
    }

    public override void Move() {
        base.Move();
        if (!Game1.ScreenBounds.Intersects(position)) Alive = false;
    }

    public override void CheckCollision(GameObject other) {
        if (position.Intersects(other.position) && other.Alignment != this.Alignment) Alive = false;
    }
}
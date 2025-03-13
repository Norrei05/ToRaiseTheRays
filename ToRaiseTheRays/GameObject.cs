using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToRaiseTheRays {
    public abstract class GameObject {
        protected Texture2D texture;
        public Rectangle position;
        protected Vector2 velocity;
        public Alignment alignment { get; protected set; }

        public GameObject(Texture2D texture, Rectangle position, Vector2 velocity, Alignment alignment) {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.alignment = alignment;
        }

        public virtual void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, Color.White);

        public virtual void Move() {
            position.X += (int)velocity.X;
            position.Y += (int)velocity.Y;
        }

        public abstract void CheckCollision(GameObject other);
    }
}
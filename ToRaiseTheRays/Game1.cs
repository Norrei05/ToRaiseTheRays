using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ToRaiseTheRays
{
    public enum Alignment {FRIENDLY, NEUTRAL, ENEMY}

    public class Game1 : Game
    {
        public static Rectangle ScreenBounds { get; private set; }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private Dictionary<string, Texture2D> playerSprites;
        private List<Bullet> bullets;
        private Texture2D bulletTexture;
        // private Texture2D fogTexture;  // Commented out for now
        public static SpriteFont PapyrusFont { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 600,   // Window width
                PreferredBackBufferHeight = 800  // Window height
            };
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            bullets = new List<Bullet>();
            playerSprites = new Dictionary<string, Texture2D>();
        }

        protected override void Initialize()
        {
            ScreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            PapyrusFont = Content.Load<SpriteFont>("Papyrus");
            bulletTexture = Content.Load<Texture2D>("Bullet");

            // Load all player sprites
            foreach (string direction in new string[]{ "N", "U", "D", "L", "R", "UL", "UR", "DL", "DR" })
            {
                playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"PlayerSprites/Barque_{direction}");
            }

            // Create player of size 44 x 44 px
            Rectangle playerPos = new(ScreenBounds.Width / 2 - 22, ScreenBounds.Height - 100, 44, 44);
            player = new Player(playerSprites, playerPos, null);  // Passing null instead of fogTexture
        }

        protected override void Update(GameTime gameTime)
        {
            // Don't ask me why this line is so long. It's the default exit line.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            player.Move();
            player.Shoot(bullets, bulletTexture);

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Move();
                if (!bullets[i].IsActive) bullets.RemoveAt(i);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            player.Draw(_spriteBatch);
            foreach (var bullet in bullets) bullet.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

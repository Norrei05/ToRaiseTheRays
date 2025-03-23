using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ToRaiseTheRays
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private string gameState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font=Content.Load<SpriteFont>("Papyrus");
            // Starting gamestate is the title screen
            gameState = "title";
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            setState(gameState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (gameState == "title")
            {
                _spriteBatch.DrawString(font, "TO RAISE THE RAYS", new Vector2(180, 190), Color.OrangeRed);
                _spriteBatch.DrawString(font, "PRESS ENTER TO BEGIN", new Vector2(160, 230), Color.OrangeRed);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void setState(string state)
        {
            switch (state)
            {
                case "title":
                    //displays title, start message(PRESS START TO BEGIN)
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = "day";
                    }
                    break;
                case "score":
                    //displays scores
                    break;
                case "day":
                    //Remove enemies and fog, begin restoration
                    break;
                case "night":
                    //begin spawning enemies, create fog
                    break;
                case "game over":
                    //halts movement, display game over message
                    break;
                default:
                    //Should not be triggered, put error code here
                    break;
            }
        }
    }
}

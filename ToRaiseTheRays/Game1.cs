using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ToRaiseTheRays;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private string gameState;
    private SpriteFont papyrus;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Default is title as the starting state
        gameState = "title";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        papyrus = Content.Load<SpriteFont>("Papyrus");
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        setState(gameState);

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin();
        _spriteBatch.DrawString(papyrus, "TO RAISE THE RAYS", new Vector2(250, 200),  Color.OrangeRed, 0, new Vector2(0, 0), 2, SpriteEffects.None, 1);
        _spriteBatch.DrawString(papyrus, "PRESS START TO BEGIN", new Vector2(265, 230), Color.OrangeRed, 0, new Vector2(0, 0), (float)1.5, SpriteEffects.None, 1);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    private void setState(string state)
    {
        switch (state)
        {
            case "title":
                //displays title, start message(PRESS START TO BEGIN)

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

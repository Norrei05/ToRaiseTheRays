using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ToRaiseTheRays;

public class Game1 : Game
{
    public static Rectangle ScreenBounds { get; private set; }

    private GraphicsDeviceManager _graphics;
    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont PapyrusFont { get; private set; }

    private Dictionary<string, Texture2D> playerSprites;
    public static Texture2D BulletTexture { get; private set; }
    // private Texture2D fogTexture;

    private Player player;
    public static List<GameObject> ActiveEntities { get; private set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 600,   // Window width
            PreferredBackBufferHeight = 800  // Window height
        };
        _graphics.ApplyChanges();
        
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        playerSprites = new Dictionary<string, Texture2D>();
        ActiveEntities = new List<GameObject>();
    }

    protected override void Initialize()
    {
        ScreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        PapyrusFont = Content.Load<SpriteFont>("Papyrus");
        BulletTexture = Content.Load<Texture2D>("Bullet");

        // Load all player sprites
        foreach (string direction in new string[]{ "N", "U", "D", "L", "R", "UL", "UR", "DL", "DR" })
        {
            playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"PlayerSprites/Barque_{direction}");
        }

        // Create player of size 44x44 at the bottom of the screen
        Rectangle playerPos = new(ScreenBounds.Width / 2 - 22, ScreenBounds.Height - 100, 44, 44);
        player = new Player(playerSprites, playerPos, null);  // Passing null instead of fogTexture
        ActiveEntities.Add(player);
    }

    protected override void Update(GameTime gameTime) {
        // Don't ask me why this line is so long. It's the default exit line.
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        player.Shoot();

        foreach (GameObject entity in ActiveEntities) entity.Move();
        foreach (GameObject entity in ActiveEntities)
        {
            foreach (GameObject other in ActiveEntities) entity.CheckCollision(other);
        }

        // Remove dead entities
        for (int i = ActiveEntities.Count - 1; i >= 0; i--)
        {
            if ((ActiveEntities[i] is Bullet bullet && !bullet.Alive) ||
                (ActiveEntities[i] is Enemy enemy && enemy.Health <= 0) ||
                (ActiveEntities[i] is Player player && player.Health <= 0))
            {
                ActiveEntities.RemoveAt(i);
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Sienna);

        SpriteBatch.Begin();
        foreach (GameObject entity in ActiveEntities) entity.Draw();
        SpriteBatch.End();
    
        base.Draw(gameTime);
    }
}
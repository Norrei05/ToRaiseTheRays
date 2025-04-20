using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ToRaiseTheRays;

public enum GameState
{
    Start,
    Day,
    Night,
    Pause,
    End
}

public class Game1 : Game
{
    // Fields/Properties

    public static Rectangle ScreenBounds { get; private set; }
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont PapyrusFont { get; private set; }
   
    private Dictionary<string, Texture2D> playerSprites;
    public static Texture2D BulletTexture { get; private set; }
    private Texture2D enemyTexture;

    private Texture2D wingedBoss;
    private Texture2D twinHead;
    private Texture2D hornBoss;

    // private Texture2D fogTexture;

    // Static to allow access in other classes (Player)
    public static GameState gameState;
    private GameState prevState;
    private KeyboardState prevKeyboard;

    private Player player;
    private EnemyGenerator generator;

    private Texture2D dayTileset;
    private Texture2D nightTileset;
    private Rectangle[,] map;

    private Texture2D startScreen;
    private Texture2D endScreen;

    private float animationSpeedFPS;
    private float secondsPerFrame;
    private float timeCounter;
    private float secondTimeCounter;

    private Vector2 position;
    private Vector2 newPosition;

    private Texture2D healthBar;

    private int dayTime;
    private double dayTimer;

    private float opacity;

    public static List<GameObject> ActiveEntities { get; private set; }

    // Constructor

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

    // Methods

    /// <summary>
    /// Initializes field
    /// </summary>
    protected override void Initialize()
    {
        ScreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        // Initialize empty 2D Rectangle array for the future map
        map = new Rectangle[ScreenBounds.Width / 16 + 1, (ScreenBounds.Height / 16) * 2];

        animationSpeedFPS = 10;
        secondsPerFrame = 1.0f / animationSpeedFPS;
        timeCounter = 0;
        secondTimeCounter = 0;

        dayTime = 5;
        dayTimer = 0;

        position = new Vector2(0, map[0, 0].Y);
        newPosition = new Vector2(1, 1);

        gameState = GameState.Start;

        opacity = 0;

        base.Initialize();
    }

    /// <summary>
    /// Loads assets and images in gameloop and initializes fields requiring such assets
    /// </summary>
    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        PapyrusFont = Content.Load<SpriteFont>("Papyrus");
        BulletTexture = Content.Load<Texture2D>("Bullet2");

        enemyTexture = Content.Load<Texture2D>("Basic_Enemy");
        wingedBoss = Content.Load<Texture2D>("Brighter_Winged_Snake");
        twinHead = Content.Load<Texture2D>("Brighter_TwoHeaded_Snake");
        hornBoss = Content.Load<Texture2D>("Brighter_Horned_Snake");

        // Load all player sprites
        foreach (string direction in new string[]{ "N", "U", "D", "L", "R", "UL", "UR", "DL", "DR" })
        {
            playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"PlayerSpritesNight/Barque_{direction}");
            playerSprites["Barque_" + direction + "_Day"] = Content.Load<Texture2D>($"PlayerSpritesDay/Barque_{direction}_Day");
        }

        // Load in fog of war asset
        //fogTexture = Content.Load<Texture2D>("Fog_Of_War");

        // Load in tilesets and generate a map of source rectangles based on placement of tiles in tileset images
        dayTileset = Content.Load<Texture2D>("Tiles_Day_1");
        nightTileset = Content.Load<Texture2D>("Tiles_Night_1");
        CreateMap();

        // Load in both start and end screen textures
        startScreen = Content.Load<Texture2D>("Title_Day");
        endScreen = Content.Load<Texture2D>("Title_Night");

        // Create player of size 44x44 at the bottom of the screen
        Rectangle playerPos = new(ScreenBounds.Width / 2 - 50, ScreenBounds.Height - 100, 100, 100);
        player = new Player(playerSprites, playerPos, null); // Passing null instead of fogTexture
        ActiveEntities.Add(player);

        healthBar = Content.Load<Texture2D>("Health_Bar");

        generator = new EnemyGenerator(5, 10, enemyTexture, BulletTexture);

        generator.AddFast("Advance");
        generator.AddFast("Pincer");

        generator.AddNormal("BasicWave");
        generator.AddNormal("VShooters");
        generator.AddNormal("Circling");

        generator.AddBoss("Winged", wingedBoss);
        //generator.AddBoss("Twister", wingedBoss);
        generator.AddBoss("TwinHead", twinHead);
        //generator.AddBoss("SlowEnd", twinHead);
        generator.AddBoss("Horned", hornBoss);
        //generator.AddBoss("KillerSnake", hornBoss);
    }

    /// <summary>
    /// Update fields in gameloop
    /// </summary>
    protected override void Update(GameTime gameTime) {
        // Don't ask me why this line is so long. It's the default exit line.
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        KeyboardState kb = Keyboard.GetState();

        switch (gameState)
        {
            case GameState.Start:
                if (kb.IsKeyDown(Keys.Space))
                {
                    gameState = GameState.Day;
                    generator.Start();

                    player.position.X = ScreenBounds.Width / 2 - player.position.Width / 2;
                    player.position.Y = ScreenBounds.Height - 100;
                }

                break;
            case GameState.Day:

                player.Reset();

                player.Shoot();

                // Do all movement before checking collision.
                for (int i = 0; i < ActiveEntities.Count; i++)
                {
                    if (ActiveEntities[i] is Enemy)
                    {
                        Enemy enemy = (Enemy)ActiveEntities[i];
                        enemy.Move(gameTime);
                    }
                    else
                    {
                        ActiveEntities[i].Move();
                    }
                }

                foreach (GameObject entity in ActiveEntities)
                {
                    // Collision checking is one-sided per GameObject, so this doesn't double-check.
                    // Self-checking is fine, since the alignment of an object will always match itself.
                    foreach (GameObject other in ActiveEntities) entity.CheckCollision(other);
                }

                // Remove dead entities
                for (int i = ActiveEntities.Count - 1; i >= 0; i--)
                {
                    /*
                    SpriteBatch = new SpriteBatch(GraphicsDevice);
                    PapyrusFont = Content.Load<SpriteFont>("Papyrus");
                    */
                    // Starting gamestate is the title screen
                    //gameState = "title";
                    if ((ActiveEntities[i] is Bullet bullet && !bullet.Alive) ||
                        (ActiveEntities[i] is Enemy enemy && enemy.Health <= 0))
                    {
                        ActiveEntities.RemoveAt(i);
                    }
                }

                dayTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (dayTimer >= dayTime)
                {
                    generator.NextLevel();
                    dayTimer = 0;

                    gameState = GameState.Night;
                    opacity = 1;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                {
                    prevState = gameState;
                    gameState = GameState.Pause;
                }

                break;
            case GameState.Night:

                player.Shoot();

                generator.Update(gameTime);

                // Do all movement before checking collision.
                for (int i = 0; i < ActiveEntities.Count; i++)
                {
                    if (ActiveEntities[i] is Enemy)
                    {
                        Enemy enemy = (Enemy)ActiveEntities[i];
                        enemy.Move(gameTime);
                    }
                    else
                    {
                        ActiveEntities[i].Move();
                    }
                }

                foreach (GameObject entity in ActiveEntities)
                {
                    // Collision checking is one-sided per GameObject, so this doesn't double-check.
                    // Self-checking is fine, since the alignment of an object will always match itself.
                    foreach (GameObject other in ActiveEntities) entity.CheckCollision(other);
                }

                // Remove dead entities
                for (int i = ActiveEntities.Count - 1; i >= 0; i--)
                {
                    if ((ActiveEntities[i] is Bullet bullet && !bullet.Alive) ||
                        (ActiveEntities[i] is Enemy enemy && enemy.Health <= 0))
                    {
                        ActiveEntities.RemoveAt(i);
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                {
                    prevState = gameState;
                    gameState = GameState.Pause;
                }
                else if (player.Health <= 0)
                {
                    gameState = GameState.End;

                    for (int i = ActiveEntities.Count - 1; i >= 0; i--)
                    {
                        if ((ActiveEntities[i] is Bullet bullet) ||
                            (ActiveEntities[i] is Enemy enemy))
                        {
                            ActiveEntities.RemoveAt(i);
                        }
                    }
                }
                else if (!generator.InPlay)
                {
                    gameState = GameState.Day;
                    opacity = 1;
                }

                break;
            case GameState.Pause:
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                {
                    gameState = prevState;
                }
                break;
            case GameState.End:
                if (kb.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.Start;
                    player.Reset();
                    generator.Reset();
                }

                break;
        }


        prevKeyboard = Keyboard.GetState();

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws images and assets in gameloop
    /// </summary>
    protected override void Draw(GameTime gameTime) {        
        GraphicsDevice.Clear(Color.Sienna);
        
        //setState(gameState);

        SpriteBatch.Begin();

        switch (gameState)
        {
            case GameState.Start:

                SpriteBatch.Draw(startScreen, new Rectangle(0, 0, ScreenBounds.Width, ScreenBounds.Height), Color.AntiqueWhite);

                // Display information about controls to player
                SpriteBatch.DrawString(PapyrusFont, " TO RAISE\nTHE RAYS", new Vector2(ScreenBounds.Width / 2 - 98, 127), Color.Black);
                SpriteBatch.DrawString(PapyrusFont, " TO RAISE\nTHE RAYS", new Vector2(ScreenBounds.Width / 2 - 100, 125), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, "PRESS SPACE TO\n     BEGIN GAME", new Vector2(ScreenBounds.Width / 2 - 178, ScreenBounds.Height - 273), Color.Black);
                SpriteBatch.DrawString(PapyrusFont, "PRESS SPACE TO\n     BEGIN GAME", new Vector2(ScreenBounds.Width / 2 - 180, ScreenBounds.Height - 275), Color.OrangeRed);

                break;
            case GameState.Night:

                DrawMap(SpriteBatch, gameTime, true, opacity);

                if (opacity > 0)
                {
                    opacity -= 0.01f;
                }
                else
                {
                    opacity = 0f;
                }


                // Draws health bar to screen, updating based on the current player health
                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.White);

                generator.Draw(healthBar, ScreenBounds, PapyrusFont);

                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.OrangeRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw();

                break;
            case GameState.Day:

                DrawMap(SpriteBatch, gameTime, false, opacity);

                if (opacity > 0)
                {
                    opacity -= 0.01f;
                }
                else
                {
                    opacity = 0f;
                }
                

                // Draws health bar to screen, updating based on the current player health
                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.White);

                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.OrangeRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw();

                break;
            case GameState.Pause:
                if (prevState == GameState.Day)
                {
                    DrawMap(SpriteBatch, gameTime, false, 1);
                }
                else
                {
                    DrawMap(SpriteBatch, gameTime, true, 1);
                }
                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.White);
                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.OrangeRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw();
                break;
            case GameState.End:

                SpriteBatch.Draw(endScreen, new Rectangle(0, 0, ScreenBounds.Width, ScreenBounds.Height), Color.White);

                // Display information about controls to player
                SpriteBatch.DrawString(PapyrusFont, "GAME OVER", new Vector2(ScreenBounds.Width / 2 - 128, 127), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "GAME OVER", new Vector2(ScreenBounds.Width / 2 - 130, 125), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, "PRESS ENTER TO\n     START OVER", new Vector2(ScreenBounds.Width / 2 - 178, ScreenBounds.Height - 273), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "PRESS ENTER TO\n     START OVER", new Vector2(ScreenBounds.Width / 2 - 180, ScreenBounds.Height - 275), Color.OrangeRed);

                break;
        }


        SpriteBatch.End();
    
        base.Draw(gameTime);
    }

    /// <summary>
    /// Helper method to create map out of tilesets based on the pre-determined dimensions of the tilesets.
    /// </summary>
    private void CreateMap()
    {
        if (map == null)
            return;

        Rectangle[,] tileDivides = new Rectangle[7, 8];

        // Loop through empty 2D array and fill it with the dimensions of each tile
        for (int col = 0; col < tileDivides.GetLength(0); col++)
        {
            for (int row = 0; row < tileDivides.GetLength(1); row++)
                tileDivides[col, row] = new Rectangle(col * 16, row * 16, 16, 16);
        }

        // Grab the tiles needed from 2D array and then loop through each row, adding the same river pattern to the right side
        Rectangle sandToGrass = tileDivides[2, 2];
        Rectangle grass = tileDivides[2, 0];
        Rectangle grassToSand = tileDivides[0, 2];
        Rectangle sandToWater = tileDivides[5, 2];
        Rectangle water = tileDivides[6, 2];

        for (int row = 0; row < map.GetLength(1); row++)
        {
            map[map.GetLength(0) - 1, row] = water;
            map[map.GetLength(0) - 2, row] = water;
            map[map.GetLength(0) - 3, row] = water;
            map[map.GetLength(0) - 4, row] = water;
            map[map.GetLength(0) - 5, row] = sandToWater;
            map[map.GetLength(0) - 6, row] = grassToSand;
            map[map.GetLength(0) - 7, row] = grass;
            map[map.GetLength(0) - 8, row] = sandToGrass;
        }

        Random randomGen = new Random();

        int tile;

        // Populate the rest of the map with randomized sand tiles, favoring empty sand (cases 0-7) over sand with rocks (cases 8-9)
        for (int col = 0; col < map.GetLength(0) - 8; col++)
        {
            for (int row = 0; row < map.GetLength(1); row++)
            {
                tile = randomGen.Next(0, 10);

                switch (tile)
                {
                    case 0: case 1: case 2: case 3: case 4: case 5: case 6:
                    case 7:
                        map[col, row] = tileDivides[0, 0];
                        break;
                    case 8:
                        map[col, row] = tileDivides[5, 0];
                        break;
                    case 9:
                        map[col, row] = tileDivides[6, 0];
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Helper method to draw the map based on if it is night or not using the provided SpriteBatch.
    /// Notice: This method assumes that Begin() and End() are already taken care of.
    /// </summary>
    /// <param name="sb">Sprite batch being used to draw</param>
    /// <param name="gameTime">Current game time and scalar for auto scrolling map</param>
    /// <param name="isNight">Whether or not the game state is night</param>
    private void DrawMap(SpriteBatch sb, GameTime gameTime, bool isNight, float opacity)
    {
        timeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

        Texture2D tileset;
        Texture2D oppositeTiles;
        Color color;
        Color oppositeColor;

        // Adjusts the tileset and color overlay based on whether or not it is night
        if (isNight)
        {
            tileset = nightTileset;
            oppositeTiles = dayTileset;

            color = Color.LightSlateGray;
            oppositeColor = Color.WhiteSmoke;
        }
        else
        {
            tileset = dayTileset;
            color = Color.WhiteSmoke;

            oppositeTiles = nightTileset;
            oppositeColor = Color.LightSlateGray;
        }

        // Loops through every tile in map, displaying to screen and moving based on how long the game's been running
        for (int col = map.GetLength(0) - 1; col >= 0; col--)
        {
            for (int row = map.GetLength(1) - 1; row >= 0; row--)
            {
                // Normal map that scrolls based on time
                position = new Vector2((col * 16), 0 - row * 16 + (timeCounter * 400));
                sb.Draw(tileset, position, map[col, row], color);

                if (opacity > 0)
                    sb.Draw(oppositeTiles, position, map[col, row], oppositeColor*opacity);

                // Other tiles that fill in wherever the normal map does not cover
                if (position.Y >= 0)
                    newPosition = new Vector2(position.X, position.Y - ScreenBounds.Height);
                else
                    newPosition = new Vector2(position.X, position.Y + ScreenBounds.Height);

                sb.Draw(tileset, newPosition, map[col, row], color);

                if (opacity > 0)
                    sb.Draw(oppositeTiles, newPosition, map[col, row], oppositeColor*opacity);
            }
        }

        // Resets the timer whenever the map gets too low to prevent too large calculations
        if (position.Y >= ScreenBounds.Height)
            timeCounter = 0;
    }

}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; // My enemy is here at last. Justified by File I/O.

namespace ToRaiseTheRays;

public enum GameState
{
    Start,
    Day,
    Night,
    Pause,
    End,
    HighScoreSave,
    HighScoreView
}

public class Game1 : Game
{
    // Fields/Properties

    public static Rectangle ScreenBounds { get; private set; }
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont PapyrusFont { get; private set; }

    // Textures

    private Dictionary<string, Texture2D> playerSprites;

    public static Texture2D BulletTexture { get; private set; }
    private Texture2D enemyTexture;
    private Texture2D fastEnemy;

    private Texture2D wingedBoss;
    private Texture2D twinHead;
    private Texture2D hornBoss;

    private Texture2D healthBar;

    private Texture2D dayTileset;
    private Texture2D nightTileset;
    private Rectangle[,] map;

    private Texture2D startScreen;
    private Texture2D endScreen;

    // private Texture2D fogTexture;

    // Static to allow access in other classes (Player)
    public static GameState gameState;
    private GameState prevState;
    private KeyboardState prevKeyboard;
    private List<Keys> validAlphabet;

    private Player player;
    private EnemyGenerator generator;

    private float animationSpeedFPS;
    private float secondsPerFrame;
    private float timeCounter;
    private float secondTimeCounter;

    private Vector2 position;
    private Vector2 newPosition;

    private int dayTime;
    private double dayTimer;
    private int dayCounter;

    private float opacity;

    public static int score;
    private string name;
    private List<string> highScores;

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

        validAlphabet = new List<Keys> { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L,
            Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z };

        animationSpeedFPS = 10;
        secondsPerFrame = 1.0f / animationSpeedFPS;
        timeCounter = 0;
        secondTimeCounter = 0;

        dayTime = 5;
        dayTimer = 0;
        dayCounter = 1;

        position = new Vector2(0, map[0, 0].Y);
        newPosition = new Vector2(1, 1);

        gameState = GameState.Start;

        opacity = 0;

        score = 0;
        name = "";
        highScores = new List<string>();

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
        fastEnemy = Content.Load<Texture2D>("Small_Basic_Enemy");
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

        generator = new EnemyGenerator(5, 10, enemyTexture, fastEnemy, BulletTexture);

        generator.AddFast("Advance");
        generator.AddFast("Cross");
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

                if (kb.IsKeyDown(Keys.Space) && prevKeyboard.IsKeyUp(Keys.Space))
                {
                    gameState = GameState.Day;
                    prevState = GameState.Start;
                    generator.Start();

                    player.position.X = ScreenBounds.Width / 2 - player.position.Width / 2;
                    player.position.Y = ScreenBounds.Height - 100;

                    score = 0;
                }
                else if ((kb.IsKeyDown(Keys.LeftShift) && prevKeyboard.IsKeyUp(Keys.LeftShift)) || (kb.IsKeyDown(Keys.RightShift) && prevKeyboard.IsKeyUp(Keys.RightShift)))
                    gameState = GameState.HighScoreView;

                    break;
            case GameState.HighScoreView:
                highScores = GetHighScoresFromFile();

                if ((kb.IsKeyDown(Keys.LeftShift) && prevKeyboard.IsKeyUp(Keys.LeftShift)) || (kb.IsKeyDown(Keys.RightShift) && prevKeyboard.IsKeyUp(Keys.RightShift)))
                    gameState = GameState.Start;

                break;
            case GameState.Day:
                player.Heal();

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
                    // Starting gamestate is the title screen
                    //gameState = "title";
                    if ((ActiveEntities[i] is Bullet bullet && !bullet.Alive) ||
                        (ActiveEntities[i] is Enemy enemy && enemy.Health <= 0))
                    {
                        ActiveEntities.RemoveAt(i);
                    }
                }

                dayTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Switch to Night state when timer runs out, and Pause state when the Enter key is pressed once
                if (dayTimer >= dayTime)
                {
                    generator.NextLevel();
                    dayTimer = 0;

                    gameState = GameState.Night;
                    opacity = 1;

                    dayCounter++;
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

                // Switch to Pause state when the Enter key is pressed once and Day state when the player health reaches 0 or the wave ends
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                {
                    prevState = gameState;
                    gameState = GameState.Pause;
                }
                else if (player.Health <= 0)
                {
                    prevState = gameState;
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
                    prevState = gameState;
                    gameState = GameState.Day;
                    opacity = 1;
                }

                break;
            case GameState.Pause:
                // Returns to original state it was paused in when the Enter key is pressed again
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                    gameState = prevState;

                break;
            case GameState.End:
                if (kb.IsKeyDown(Keys.Space) && prevKeyboard.IsKeyUp(Keys.Space))
                    gameState = GameState.Start;
                else if ((kb.IsKeyDown(Keys.LeftShift) && prevKeyboard.IsKeyUp(Keys.LeftShift)) || (kb.IsKeyDown(Keys.RightShift) && prevKeyboard.IsKeyUp(Keys.RightShift)))
                    gameState = GameState.HighScoreSave;

                break;
            case GameState.HighScoreSave:
                // Constructs a string version for their name based on alphabetical text input from user (not exceeding 5 characters long)
                if (GetAlphabeticalTextInput() != null && name.Length < 5)
                    name += GetAlphabeticalTextInput();
                else if (kb.IsKeyDown(Keys.Back) && prevKeyboard.IsKeyUp(Keys.Back) && name.Length > 0)
                    name = name.Substring(0, name.Length - 1);

                // Returns to Game Over screen if either Shift is pressed again
                if (kb.IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter))
                {
                    player.Reset();
                    generator.Reset();
                    dayCounter = 1;

                    gameState = GameState.Start;

                    if (name.Length > 0)
                        highScores = UpdateHighScores(score, name);
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
                SpriteBatch.DrawString(PapyrusFont, "PRESS SPACE TO\n      BEGIN GAME", new Vector2(ScreenBounds.Width / 2 - 178, ScreenBounds.Height - 373), Color.Black);
                SpriteBatch.DrawString(PapyrusFont, "PRESS SPACE TO\n      BEGIN GAME", new Vector2(ScreenBounds.Width / 2 - 180, ScreenBounds.Height - 375), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, "    PRESS SHIFT TO\nVIEW  HIGH SCORES", new Vector2(ScreenBounds.Width / 2 - 203, ScreenBounds.Height - 233), Color.Black);
                SpriteBatch.DrawString(PapyrusFont, "    PRESS SHIFT TO\nVIEW  HIGH SCORES", new Vector2(ScreenBounds.Width / 2 - 205, ScreenBounds.Height - 235), Color.OrangeRed);

                break;
            case GameState.HighScoreView:
                SpriteBatch.Draw(startScreen, new Rectangle(0, 0, ScreenBounds.Width, ScreenBounds.Height), Color.LightGray);

                SpriteBatch.DrawString(PapyrusFont, "HIGH SCORES", new Vector2(ScreenBounds.Width / 2 - 158, 127), Color.Black);
                SpriteBatch.DrawString(PapyrusFont, "HIGH SCORES", new Vector2(ScreenBounds.Width / 2 - 160, 125), Color.OrangeRed);

                // Always prints out 10 slots, with either file-generated information or blank slots to be taken up later
                for (int i = 0; i < 10; i++)
                {
                    if (i < highScores.Count)
                    {
                        string[] split = highScores[i].Split("|");
                        string formattedName = split[0];

                        if (split[0].Length < 5)
                            for (int space = 0; space < (5 - split[0].Length); space++)
                                formattedName = formattedName + "    ";
                                
                        String formattedScore = String.Format("{0:0000000}", Convert.ToInt16(split[1]));

                        SpriteBatch.DrawString(PapyrusFont, (formattedName + "     " + formattedScore), new Vector2(ScreenBounds.Width / 2 - 163, 202 + (50 * i)), Color.Black);
                        SpriteBatch.DrawString(PapyrusFont, (formattedName + "     " + formattedScore), new Vector2(ScreenBounds.Width / 2 - 165, 200 + (50 * i)), Color.OrangeRed);
                    } 
                    else
                    {
                        SpriteBatch.DrawString(PapyrusFont, ("________     0000000"), new Vector2(ScreenBounds.Width / 2 - 163, 202 + (50 * i)), Color.Black);
                        SpriteBatch.DrawString(PapyrusFont, ("________     0000000"), new Vector2(ScreenBounds.Width / 2 - 165, 200 + (50 * i)), Color.OrangeRed);
                    }
                }

                break;
            case GameState.Day:

                DrawMap(SpriteBatch, gameTime, false, false, opacity);

                // Change opacity for transition
                if (opacity > 0)
                    opacity -= 0.01f;
                else
                    opacity = 0f;
                
                // Draws health bar to screen, updating based on the current player health
                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.White);

                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.OrangeRed);

                // Display current score to player
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(17, 17), Color.LightYellow);
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(15, 15), Color.OrangeRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw(Color.White);

                // Stops displaying Day # and possible instructions around 3/4 of the way through the Day state
                if (dayTimer <= dayTime * 0.75)
                {
                    SpriteBatch.DrawString(PapyrusFont, $"DAY {dayCounter}", new Vector2(ScreenBounds.Width / 2 - 53, 127), Color.Black);
                    SpriteBatch.DrawString(PapyrusFont, $"DAY {dayCounter}", new Vector2(ScreenBounds.Width / 2 - 55, 125), Color.OrangeRed);

                    if (prevState == GameState.Start)
                    {
                        SpriteBatch.DrawString(PapyrusFont, "PRESS ENTER\n    TO PAUSE", new Vector2(ScreenBounds.Width / 2 - 153, ScreenBounds.Height - 273), Color.Black);
                        SpriteBatch.DrawString(PapyrusFont, "PRESS ENTER\n    TO PAUSE", new Vector2(ScreenBounds.Width / 2 - 155, ScreenBounds.Height - 275), Color.OrangeRed);
                    }
                }

                break;
            case GameState.Night:

                DrawMap(SpriteBatch, gameTime, true, false, opacity);

                // Change opacity for transition
                if (opacity > 0)
                    opacity -= 0.01f;
                else
                    opacity = 0f;

                // Draws health bar to screen, updating based on the current player health
                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.White);

                generator.Draw(healthBar, ScreenBounds, PapyrusFont);

                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.OrangeRed);

                // Display current score to player
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(17, 17), Color.Yellow);
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(15, 15), Color.OrangeRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw(Color.White);

                break;
            case GameState.Pause:

                // Use different maps based on the previous state
                if (prevState == GameState.Day)
                    DrawMap(SpriteBatch, gameTime, false, true, 0);
                else
                    DrawMap(SpriteBatch, gameTime, true, true, 0);

                SpriteBatch.Draw(healthBar, new Rectangle(15, ScreenBounds.Height - 15 - (player.Health * 3), 50, player.Health * 3), Color.LightSlateGray);

                if (player.Health > 0)
                    SpriteBatch.DrawString(PapyrusFont, "+", new Vector2(32, ScreenBounds.Height - 55), Color.DarkRed);

                foreach (GameObject entity in ActiveEntities) entity.Draw(Color.LightSlateGray);

                // Display pause screen text to players
                SpriteBatch.DrawString(PapyrusFont, "GAME PAUSED", new Vector2(ScreenBounds.Width / 2 - 163, 127), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "GAME PAUSED", new Vector2(ScreenBounds.Width / 2 - 165, 125), Color.OrangeRed);

                SpriteBatch.DrawString(PapyrusFont, "  PRESS ENTER TO\n RETURN TO GAME", new Vector2(ScreenBounds.Width / 2 - 203, ScreenBounds.Height - 273), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "  PRESS ENTER TO\n RETURN TO GAME", new Vector2(ScreenBounds.Width / 2 - 205, ScreenBounds.Height - 275), Color.OrangeRed);

                // Display current score to player
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(17 ,17), Color.White);
                SpriteBatch.DrawString(PapyrusFont, $"SCORE: {score}", new Vector2(15, 15), Color.OrangeRed);

                break;
            case GameState.End:
                SpriteBatch.Draw(endScreen, new Rectangle(0, 0, ScreenBounds.Width, ScreenBounds.Height), Color.White);

                // Display game over information to player, including their final score (formatted a certain way) and how to progress to different states
                String finalScore = String.Format("{0:0000000}", score);

                SpriteBatch.DrawString(PapyrusFont, "GAME OVER", new Vector2(ScreenBounds.Width / 2 - 128, 127), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "GAME OVER", new Vector2(ScreenBounds.Width / 2 - 130, 125), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, $"FINAL SCORE:\n           {finalScore}", new Vector2(ScreenBounds.Width / 2 - 145, 217), Color.White);
                SpriteBatch.DrawString(PapyrusFont, $"FINAL SCORE:\n           {finalScore}", new Vector2(ScreenBounds.Width / 2 - 147, 215), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, " PRESS SPACE TO\nRETURN TO TITLE", new Vector2(ScreenBounds.Width / 2 - 193, ScreenBounds.Height - 373), Color.White);
                SpriteBatch.DrawString(PapyrusFont, " PRESS SPACE TO\nRETURN TO TITLE", new Vector2(ScreenBounds.Width / 2 - 195, ScreenBounds.Height - 375), Color.OrangeRed);
                SpriteBatch.DrawString(PapyrusFont, "   PRESS SHIFT TO\nSAVE HIGH SCORE", new Vector2(ScreenBounds.Width / 2 - 198, ScreenBounds.Height - 233), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "   PRESS SHIFT TO\nSAVE HIGH SCORE", new Vector2(ScreenBounds.Width / 2 - 200, ScreenBounds.Height - 235), Color.OrangeRed);

                break;
            case GameState.HighScoreSave:
                SpriteBatch.Draw(endScreen, new Rectangle(0, 0, ScreenBounds.Width, ScreenBounds.Height), Color.LightGray);

                // Displays a prompt for user alphabetical text input, and displays the input to the screen
                SpriteBatch.DrawString(PapyrusFont, "PLEASE ENTER\n     YOUR NAME\n   (MAX 5 CHARS)", new Vector2(ScreenBounds.Width / 2 - 168, 127), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "PLEASE ENTER\n     YOUR NAME\n   (MAX 5 CHARS)", new Vector2(ScreenBounds.Width / 2 - 170, 125), Color.OrangeRed);

                SpriteBatch.DrawString(PapyrusFont, name, new Vector2(ScreenBounds.Width / 2 - 98, ScreenBounds.Height - 373), Color.White);
                SpriteBatch.DrawString(PapyrusFont, name, new Vector2(ScreenBounds.Width / 2 - 100, ScreenBounds.Height - 375), Color.OrangeRed);

                SpriteBatch.DrawString(PapyrusFont, "________", new Vector2(ScreenBounds.Width / 2 - 98, ScreenBounds.Height - 348), Color.White);
                SpriteBatch.DrawString(PapyrusFont, "________", new Vector2(ScreenBounds.Width / 2 - 100, ScreenBounds.Height - 350), Color.OrangeRed);

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
    private void DrawMap(SpriteBatch sb, GameTime gameTime, bool isNight, bool isPaused, float opacity)
    {
        if (!isPaused)
            timeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

        Texture2D tileset;
        Texture2D oppositeTiles;
        Color color;
        Color oppositeColor;

        // Adjusts the tileset and color overlay based on whether or not it is night and/or paused
        if (isNight)
        {
            tileset = nightTileset;
            oppositeTiles = dayTileset;

            // Puts a slightly darker overlay over the screen when paused
            if (isPaused)
                color = Color.DarkSlateGray;
            else
                color = Color.LightSlateGray;

            oppositeColor = Color.WhiteSmoke;
        }
        else
        {
            tileset = dayTileset;
            oppositeTiles = nightTileset;

            // Puts a slightly darker overlay over the screen when paused
            if (isPaused)
                color = Color.LightSlateGray;
            else
                color = Color.WhiteSmoke;

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
                    sb.Draw(oppositeTiles, position, map[col, row], oppositeColor * opacity);

                // Other tiles that fill in wherever the normal map does not cover
                if (position.Y >= 0)
                    newPosition = new Vector2(position.X, position.Y - ScreenBounds.Height);
                else
                    newPosition = new Vector2(position.X, position.Y + ScreenBounds.Height);

                sb.Draw(tileset, newPosition, map[col, row], color);

                if (opacity > 0)
                    sb.Draw(oppositeTiles, newPosition, map[col, row], oppositeColor * opacity);
            }
        }

        // Resets the timer whenever the map gets too low to prevent too large calculations
        if (position.Y >= ScreenBounds.Height)
            timeCounter = 0;
    }

    /// <summary>
    /// Method to update the external high scores file with the new player name and high score.
    /// </summary>
    /// <param name="newScore">New high score to be added to the file</param>
    /// <param name="playerName">Player name attached to new high score</param>
    /// <returns>List of high scores as strings from file, or just the current high score from this game if none are in the file</returns>
    private List<string> UpdateHighScores(int newScore, string playerName)
    {
        List<string> scores = new List<string>();

        try
        {
            // Read existing scores from the file
            if (File.Exists("highScores.txt")) scores = File.ReadAllLines("highScores.txt").ToList();

            // Add the new score
            scores.Add($"{playerName}|{newScore}");

            // Sort scores in descending order
            scores = scores.OrderByDescending(s => int.Parse(s.Split('|')[1])).ToList();

            // Keep only the top 10 scores
            if (scores.Count > 10) scores = scores.Take(10).ToList();

            // Write updated scores back to the file
            File.WriteAllLines("highScores.txt", scores);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Something went wrong when trying to read and write to the file 'highScores.txt'. Please double check the file and try again." + "\n" + e.Message);
        }

        return scores;
    }

    /// <summary>
    /// Method to load in but not update the high scores from its designated file 'highScores.txt'.
    /// </summary>
    /// <returns>List of high scores from file, if any</returns>
    private List<string> GetHighScoresFromFile()
    {
        List<string> scores = new List<string>();

        try
        {
            // Read existing scores from the file
            if (File.Exists("highScores.txt")) scores = File.ReadAllLines("highScores.txt").ToList();

            // Sort scores in descending order
            scores = scores.OrderByDescending(s => int.Parse(s.Split('|')[1])).ToList();

            // Keep only the top 10 scores
        }
        catch (Exception e)
        {
            Debug.WriteLine("Something went wrong when trying to read the file 'highScores.txt'. Please double check the file and try again." + "\n" + e.Message);
        }

        return scores;
    }

    /// <summary>
    /// Method to get alphabetical text input (and nothing outside of those keys).
    /// </summary>
    /// <returns>String version of the key being pressed, or null if nothing/an invalid key is pressed</returns>
    private String GetAlphabeticalTextInput()
    {
        KeyboardState kb = Keyboard.GetState();

        // Checks each key to see if it is one of the valid alphabet keys
        foreach (Keys key in validAlphabet)
        {
            if (kb.IsKeyDown(key) && prevKeyboard.IsKeyUp(key))
                return key.ToString();
        }

        return null!;
    }

}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

    private Texture2D dayTileset;

    private Texture2D nightTileset;

    private Rectangle[,] map;

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

        // Initialize empty 2D Rectangle array for the future map
        map = new Rectangle[ScreenBounds.Width / 16 + 1, (ScreenBounds.Height / 16) * 2];

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        PapyrusFont = Content.Load<SpriteFont>("Papyrus");
        BulletTexture = Content.Load<Texture2D>("Bullet2");

        // Load all player sprites
        foreach (string direction in new string[]{ "N", "U", "D", "L", "R", "UL", "UR", "DL", "DR" })
        {
            if (direction == "U")
                playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"Barque_{direction}_Night"); 
            else if (direction == "N")
                playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"Barque_U_Night");
            else
                playerSprites["Barque_" + direction] = Content.Load<Texture2D>($"PlayerSprites/Barque_{direction}");
        }

        // Load in fog of war asset
        // fogTexture = Content.Load<Texture2D>("Fog_Of_War");

        // Load in tilesets and generate a map of source rectangles based on placement of tiles in tileset images
        dayTileset = Content.Load<Texture2D>("Tiles_Day_1");
        nightTileset = Content.Load<Texture2D>("Tiles_Night_1");
        CreateMap();

        // Create player of size 44x44 at the bottom of the screen
        Rectangle playerPos = new(ScreenBounds.Width / 2 - 22, ScreenBounds.Height - 100, 100, 100);
        player = new Player(playerSprites, playerPos, null); // Passing null instead of fogTexture
        ActiveEntities.Add(player);
    }

    protected override void Update(GameTime gameTime) {
        // Don't ask me why this line is so long. It's the default exit line.
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        player.Shoot();

        // Do all movement before checking collision.
        foreach (GameObject entity in ActiveEntities) entity.Move();

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

        // Loop through map, displaying it to screen
        for (int col = 0; col < map.GetLength(0); col++)
        {
            for (int row = 0; row < map.GetLength(1); row++)
                SpriteBatch.Draw(nightTileset, new Vector2(col * 16, row * 16), map[col, row], Color.LightSlateGray);
        }

        foreach (GameObject entity in ActiveEntities) entity.Draw();

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

        // Populate the rest of the map with randomized sand tiles, favoring empty sand over sand with rocks
        Random randomGen = new Random();

        int tile;

        for (int col = 0; col < map.GetLength(0) - 8; col++)
        {
            for (int row = 0; row < map.GetLength(1); row++)
            {
                tile = randomGen.Next(0, 10);

                switch (tile)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
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
}
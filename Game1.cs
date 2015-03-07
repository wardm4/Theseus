#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
#endregion

namespace Theseus
{
    /// <summary>
    /// This is the main type.
    /// </summary>

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D _floor;
        private Texture2D _wall;
        private Texture2D wall1;
        private Texture2D wall2;
        private Texture2D wall3;
        private Texture2D _life;
        private Texture2D _background;
        private IMap _map;
        private Player _player;
        private InputState _inputState;
        private List<AggressiveEnemy> _aggressiveEnemies = new List<AggressiveEnemy>();
        public int elapsedTime;
        private SoundEffect swordswipe;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            Content.RootDirectory = "Content";
            _inputState = new InputState();
            elapsedTime = 0;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        protected override void Initialize()
        {
            /*
             * Initializes Map and Camera
             */

            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 50, 7, 3);
            _map = Map.Create(mapCreationStrategy);
            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 545;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent gets called once per game. 
        /// This loads all of the sprites, art, and enemies.
        /// </summary>

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _floor = this.Content.Load<Texture2D>("floor4");
            wall1 = this.Content.Load<Texture2D>("rock");
            wall2 = this.Content.Load<Texture2D>("rock2");
            wall3 = this.Content.Load<Texture2D>("rock3");
            _life = this.Content.Load<Texture2D>("life");
            _background = this.Content.Load<Texture2D>("background3");
            Cell startingCell = GetRandomEmptyCell();
            Global.Camera.CenterOn(startingCell);
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = this.Content.Load<Texture2D>("theseus4"),
                Health = 1,
                Damage = 1,
                Name = "Hilby"
            };
            UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
            AddAggressiveEnemies(3);
            Global.CombatManager = new CombatManager(_player, _aggressiveEnemies);
            swordswipe = this.Content.Load<SoundEffect>("sword1");
            Global.CombatManager.effect = swordswipe;
        }

        /// <summary>
        /// UnloadContent is called once per game and unloads all content.
        /// </summary>

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            if (_inputState.IsExitGame(PlayerIndex.One))
            {
                Exit();
            }
            else
            {
                if (Global.GameState == GameStates.PlayerTurn && _player.HandleInput(_inputState, _map))
                {
                    UpdatePlayerFieldOfView();
                    if (elapsedTime % 2 == 1)
                    {
                        Global.GameState = GameStates.EnemyTurn;
                    }
                    elapsedTime++;
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    foreach (var enemy in _aggressiveEnemies)
                    {
                        enemy.Update();
                    }
                    Global.GameState = GameStates.PlayerTurn;
                }
            }

            Global.Camera.HandleInput(_inputState, PlayerIndex.One);
            Global.Camera.CenterOn(_map.GetCell(_player.X, _player.Y));
            _inputState.Update();
            foreach (var enemy in _aggressiveEnemies)
            {
                enemy.Animate();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// Note: Draw logic separate from Update to animate.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null, Global.Camera.TranslationMatrix);
            spriteBatch.Draw(_background, new Vector2(0, 0), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 1);

            foreach (Cell cell in _map.GetAllCells())
            {
                foreach (var enemy in _aggressiveEnemies)
                {
                    if (Global.GameState == GameStates.Debugging
                        || _map.IsInFov(enemy.X, enemy.Y))
                    {
                        enemy.Draw(spriteBatch);
                    }
                }
                if (!cell.IsExplored && Global.GameState != GameStates.Debugging)
                {
                    continue;
                }
                Color tint = Color.White;
                if (!cell.IsInFov && Global.GameState != GameStates.Debugging)
                {
                    tint = Color.Gray;
                }
                var position = new Vector2(cell.X * Global.SpriteWidth, cell.Y * Global.SpriteHeight);
                if (cell.IsWalkable)
                {
                    spriteBatch.Draw(_floor, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, LayerDepth.Cells);
                }
                else
                {
                    if (cell.X % 3 == 0)
                        _wall = wall1;
                    else if (cell.X % 3 == 1)
                        _wall = wall2;
                    else
                        _wall = wall3;
                    spriteBatch.Draw(_wall, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, LayerDepth.Cells);
                }
            }

            _player.Draw(spriteBatch);
            spriteBatch.Draw(_life, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Cell GetRandomEmptyCell()
        {
            while (true)
            {
                int x = Global.Random.Next(24);
                int y = Global.Random.Next(14);
                if (_map.IsWalkable(x, y))
                {
                    return _map.GetCell(x, y);
                }
            }
        }

        private void UpdatePlayerFieldOfView()
        {
            _map.ComputeFov(_player.X, _player.Y, 15, true);
            foreach (Cell cell in _map.GetAllCells())
            {
                if (_map.IsInFov(cell.X, cell.Y))
                {
                    _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        private void AddAggressiveEnemies(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Cell enemyCell = GetRandomEmptyCell();
                var pathFromAggressiveEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("white"));
                pathFromAggressiveEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
                Texture2D texture = Content.Load<Texture2D>("ravenanimate");
                var enemy = new AggressiveEnemy(texture, 3, 3, pathFromAggressiveEnemy, _map)
                {
                    X = enemyCell.X,
                    Y = enemyCell.Y,
                    Health = 1,
                    Damage = 1,
                    Name = "Raven"
                };
                _aggressiveEnemies.Add(enemy);
            }
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            graphics.ApplyChanges();
        }
    }
}

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
        private Texture2D floor1;
        private Texture2D floor2;
        private Texture2D floor3;
        private Texture2D _wall;
        private Texture2D wall1;
        private Texture2D wall2;
        private Texture2D wall3;
        private Texture2D _life;
        private Texture2D life0;
        private Texture2D life1;
        private Texture2D life2;
        private Texture2D theseusLeft;
        private Texture2D theseusRight;
        private Texture2D theseusDead;
        private Texture2D _background;
        private Texture2D _weapon;
        private Texture2D spaceitem;
        private Texture2D titlescreen;
        private Texture2D losescreen;
        private Texture2D winscreen;
        private IMap _map;
        private Player _player;
        private InputState _inputState;
        private List<AggressiveEnemy> _aggressiveEnemies = new List<AggressiveEnemy>();
        public int elapsedTime;
        private SoundEffect swordswipe;
        public int deadTime;
        public int winTime;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = false;
            Content.RootDirectory = "Content";
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

            _inputState = new InputState();
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
            elapsedTime = 0;
            deadTime = 0;
            winTime = 0;
            _aggressiveEnemies.Clear();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            floor1 = this.Content.Load<Texture2D>("floor1");
            floor2 = this.Content.Load<Texture2D>("floor2");
            floor3 = this.Content.Load<Texture2D>("floor3");
            wall1 = this.Content.Load<Texture2D>("rock");
            wall2 = this.Content.Load<Texture2D>("rock2");
            wall3 = this.Content.Load<Texture2D>("rock3");
            life0 = this.Content.Load<Texture2D>("life0");
            life1 = this.Content.Load<Texture2D>("life");
            life2 = this.Content.Load<Texture2D>("life2");
            _life = life1;
            theseusLeft = this.Content.Load<Texture2D>("theseusLeft");
            theseusRight = this.Content.Load<Texture2D>("theseusRight");
            theseusDead = this.Content.Load<Texture2D>("theseusDead");
            _background = this.Content.Load<Texture2D>("background");
            _weapon = this.Content.Load<Texture2D>("weapon1");
            spaceitem = this.Content.Load<Texture2D>("spaceitem");
            titlescreen = this.Content.Load<Texture2D>("titlescreen");
            losescreen = this.Content.Load<Texture2D>("losescreen");
            winscreen = this.Content.Load<Texture2D>("winscreen");
            Cell startingCell = GetRandomEmptyCell();
            Global.Camera.CenterOn(startingCell);
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = theseusLeft,
                Health = 1,
                Damage = 1,
                Name = "Hilby"
            };
            UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
            AddAggressiveEnemies();
            Global.CombatManager = new CombatManager(_player, _aggressiveEnemies);
            swordswipe = this.Content.Load<SoundEffect>("sword1");
            Global.CombatManager.effect = swordswipe;
        }

        /// <summary>
        /// UnloadContent is called once per game and unloads all content.
        /// </summary>

        protected override void UnloadContent()
        {
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
                if ((Global.GameState == GameStates.PlayerTurn && _player.HandleInput(_inputState, _map)) || _inputState.IsAction(PlayerIndex.One))
                {
                    Console.WriteLine(elapsedTime);
                    UpdatePlayerFieldOfView();
                    Global.GameState = GameStates.EnemyTurn;
                    elapsedTime++;
                    if (_player.Health <= 0)
                    {
                        deadTime++;
                    }
                    if (_aggressiveEnemies.ToArray().GetLength(0) == 0)
                    {
                        winTime++;
                    }
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    foreach (var enemy in _aggressiveEnemies)
                    {
                        enemy.Update();
                    }
                    Global.GameState = GameStates.PlayerTurn;
                }

                if (_inputState.IsLeft(PlayerIndex.One)) 
                {
                    _player.Sprite = theseusLeft;
                }
                if (_inputState.IsRight(PlayerIndex.One))
                {
                    _player.Sprite = theseusRight;
                }
                if (_inputState.IsEnter(PlayerIndex.One))
                {
                    Global.GameState = GameStates.Debugging;
                }
            }

            Global.Camera.HandleInput(_inputState, PlayerIndex.One);
            Global.Camera.CenterOn(_map.GetCell(_player.X, _player.Y));
            _inputState.Update();

            foreach (var enemy in _aggressiveEnemies)
            {
                enemy.Animate();
            }

            if (_player.Health <= 0)
            {
                _player.Sprite = theseusDead;
            }

            if (winTime > 1 || deadTime > 1)
            {
                UnloadContent();
                Initialize();
                LoadContent();
            }

            switch (_player.Health)
            {
                case 1: _life = life1;
                    break;
                case 0: _life = life0;
                    break;
                case 2: _life = life2;
                    break;
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

            if (elapsedTime == 0)
            {
                spriteBatch.Draw(titlescreen, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
            }

            else
            {
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
                        if (cell.X % 3 == 0)
                            _floor = floor1;
                        else if (cell.X % 3 == 1)
                            _floor = floor2;
                        else
                            _floor = floor3;
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

                if (deadTime >= 1)
                {
                    spriteBatch.Draw(losescreen, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0.55f);
                }

                if (winTime > 0)
                {
                    spriteBatch.Draw(winscreen, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0.3f);
                }


                _player.Draw(spriteBatch);
                spriteBatch.Draw(_life, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
                spriteBatch.Draw(_weapon, Global.Camera.ScreenToWorld(new Vector2(96, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
                spriteBatch.Draw(spaceitem, Global.Camera.ScreenToWorld(new Vector2(160, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
            }
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

        private void AddAggressiveEnemies()
        {
            int numberOfRavens = Global.Random.Next(1,5);
            for (int i = 0; i < numberOfRavens; i++)
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
                    Name = "Raven",
                    isStunned = false
                };
                _aggressiveEnemies.Add(enemy);
            }
            int numberOfFires = Global.Random.Next(1,5);
            for (int i = 0; i < numberOfFires; i++)
            {
                Cell enemyCell = GetRandomEmptyCell();
                var pathFromAggressiveEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("white"));
                pathFromAggressiveEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
                Texture2D texture = Content.Load<Texture2D>("fireanimated");
                var enemy = new AggressiveEnemy(texture, 3, 3, pathFromAggressiveEnemy, _map)
                {
                    X = enemyCell.X,
                    Y = enemyCell.Y,
                    Health = 2,
                    Damage = 1,
                    Name = "Fire",
                    isStunned = false
                };
                _aggressiveEnemies.Add(enemy);
            }
        }
    }
}

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private Texture2D _level;
        private Texture2D level1;
        private Texture2D level2;
        private Texture2D level3;
        private Texture2D level4;
        private Texture2D level5;
        private Texture2D level6;
        private Texture2D level7;

        private Texture2D _itemImage;
        private Texture2D elixir;
        private Texture2D _spaceItem;
        private Texture2D spaceEmpty;
        private Texture2D spaceElixir;
        private Texture2D pandora;
        private Texture2D spacePandora;
        private Texture2D _weapon;
        private Texture2D sword;
        private Texture2D mjolnir;
        private Texture2D equippedMjolnir;
        private Texture2D equippedSword;

        private Texture2D theseusLeft;
        private Texture2D theseusRight;
        private Texture2D theseusDead;
        private Texture2D mjolnirLeft;
        private Texture2D mjolnirRight;

        private Texture2D _background;
        private Texture2D titlescreen;
        private Texture2D losescreen;
        private Texture2D winscreen;
        private Texture2D portal;
        private Player _player;
        private InputState _inputState;
        public int elapsedTime;
        //private SoundEffect swordswipe;
        public int deadTime;
        public int winTime;
        private Zone currZone;
        private bool isMinotaurAlive;
        //private Song song;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = false;
            Content.RootDirectory = "Content";
            Global.EnemyList = new List<AggressiveEnemy>();
            Global.ItemList = new List<string>();
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
            Global.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 545;
            currZone = new Zone(0);
            isMinotaurAlive = true;

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
            Global.EnemyList.Clear();
            Global.ItemList.Clear();
            Global.ItemList.Add("Elixir");
            Global.ItemList.Add("Pandora");

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

            level1 = this.Content.Load<Texture2D>("level1");
            level2 = this.Content.Load<Texture2D>("level2");
            level3 = this.Content.Load<Texture2D>("level3");
            level4 = this.Content.Load<Texture2D>("level4");
            level5 = this.Content.Load<Texture2D>("level5");
            level6 = this.Content.Load<Texture2D>("level6");
            level7 = this.Content.Load<Texture2D>("level7");
            _level = level1;

            spaceEmpty = this.Content.Load<Texture2D>("spaceitem");
            elixir = this.Content.Load<Texture2D>("elixir");
            spaceElixir = this.Content.Load<Texture2D>("spaceElixir");
            pandora = this.Content.Load<Texture2D>("pandora");
            spacePandora = this.Content.Load<Texture2D>("spacePandora");

            theseusLeft = this.Content.Load<Texture2D>("theseusLeft");
            theseusRight = this.Content.Load<Texture2D>("theseusRight");
            theseusDead = this.Content.Load<Texture2D>("theseusDead");
            mjolnirLeft = this.Content.Load<Texture2D>("mjolnirLeft");
            mjolnirRight = this.Content.Load<Texture2D>("mjolnirRight");

            _background = this.Content.Load<Texture2D>("background2");
            sword = this.Content.Load<Texture2D>("sword");
            equippedSword = this.Content.Load<Texture2D>("weapon1");
            mjolnir = this.Content.Load<Texture2D>("mjolnir");
            equippedMjolnir = this.Content.Load<Texture2D>("weapon2");
            _weapon = sword;

            titlescreen = this.Content.Load<Texture2D>("titlescreen");
            losescreen = this.Content.Load<Texture2D>("losescreen");
            winscreen = this.Content.Load<Texture2D>("winscreen");
            portal = this.Content.Load<Texture2D>("portal");
            //song = this.Content.Load<Song>("gamesoundtrack");
            Cell startingCell = currZone.GetRandomEmptyCell();
            Global.Camera.CenterOn(startingCell);
            _player = new Player
            {
                X = startingCell.X,
                Y = startingCell.Y,
                Sprite = theseusLeft,
                Health = 1,
                Damage = 1,
                Level = 1,
                XP = 0,
                Item = "None",
                Weapon = "Sword",
                Name = "Hilby"
            };
            AddAggressiveEnemies(currZone);
            //AddItem(currZone);
            UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
            Global.CombatManager = new CombatManager(_player);
            //swordswipe = this.Content.Load<SoundEffect>("sword1");
            //Global.CombatManager.effect = swordswipe;
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
            Global.XPTally = 0;
            if (_inputState.IsExitGame(PlayerIndex.One))
            {
                Exit();
            }
            else
            {
                //MediaPlayer.Play(song);
                if ((Global.GameState == GameStates.PlayerTurn && _player.HandleInput(_inputState, currZone.Layout)) || _inputState.IsAction(PlayerIndex.One))
                {
                    UpdatePlayerFieldOfView();
                    Global.GameState = GameStates.EnemyTurn;
                    elapsedTime++;
                    if (_player.Health <= 0)
                    {
                        deadTime++;
                    }
                    if (!isMinotaurAlive)
                    {
                        winTime++;
                    }
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    foreach (var enemy in Global.EnemyList)
                    {
                        enemy.Update();
                    }
                    Global.GameState = GameStates.PlayerTurn;
                }

                if (Global.GameState == GameStates.PlayerTurn && _inputState.IsAction(PlayerIndex.One) && _player.X == currZone.Exit.X && _player.Y == currZone.Exit.Y)
                {
                    Global.EnemyList.Clear();
                    currZone = new Zone(currZone.ID);
                    Cell newStart = currZone.GetRandomEmptyCell();
                    _player.X = newStart.X;
                    _player.Y = newStart.Y;
                    Global.GameState = GameStates.EnemyTurn;
                    AddAggressiveEnemies(currZone);
                    AddItem(currZone);
                    elapsedTime++;
                    UpdatePlayerFieldOfView();
                }

                if (Global.GameState == GameStates.PlayerTurn && _inputState.IsItemUse(PlayerIndex.One))
                {
                    switch (_player.Item)
                    {
                        case "Elixir":
                            _player.Health++;
                            _player.Item = "None";
                            break;
                        case "Pandora":
                            AOEDamage(5);
                            _player.Item = "None";
                            break;
                    }
                    elapsedTime++;
                    Global.GameState = GameStates.EnemyTurn;
                }
            }

            Global.Camera.CenterOn(currZone.Layout.GetCell(_player.X, _player.Y));
            _inputState.Update();

            foreach (var enemy in Global.EnemyList)
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

            if (_player.X == currZone.ItemLocation.X && _player.Y == currZone.ItemLocation.Y && currZone.Item != "None")
            {

                ItemSwap();
            }

            UpdateGUI();

            if (_inputState.IsLeft(PlayerIndex.One))
            {
                switch (_player.Weapon)
                {
                    case "Sword":
                        _player.Sprite = theseusLeft;
                        break;
                    case "Mjolnir":
                        _player.Sprite = mjolnirLeft;
                        break;
                }
            }
            if (_inputState.IsRight(PlayerIndex.One))
            {
                switch (_player.Weapon)
                {
                    case "Sword":
                        _player.Sprite = theseusRight;
                        break;
                    case "Mjolnir":
                        _player.Sprite = mjolnirRight;
                        break;
                }
            }

            IterateXP(Global.XPTally);
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
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);

            if (elapsedTime == 0)
            {
                spriteBatch.Draw(titlescreen, Global.Camera.ScreenToWorld(new Vector2(0, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
            }

            else
            {
                spriteBatch.Draw(_background, new Vector2(0, 0), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 1);

                foreach (Cell cell in currZone.Layout.GetAllCells())
                {
                    foreach (var enemy in Global.EnemyList)
                    {
                        if (currZone.Layout.IsInFov(enemy.X, enemy.Y))
                        {
                            enemy.Draw(spriteBatch);
                        }
                    }
                    if (!cell.IsExplored)
                    {
                        continue;
                    }
                    Color tint = Color.White;
                    if (!cell.IsInFov)
                    {
                        tint = Color.Gray;
                    }
                    var position = new Vector2(cell.X * Global.SpriteWidth, cell.Y * Global.SpriteHeight);

                    if (cell.X == currZone.Exit.X && cell.Y == currZone.Exit.Y)
                    {
                        spriteBatch.Draw(portal, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, 0.7f);
                    }
                    if (cell.X == currZone.ItemLocation.X && cell.Y == currZone.ItemLocation.Y)
                    {
                        spriteBatch.Draw(_itemImage, position, null, null, null, 0.0f, Vector2.One, tint, SpriteEffects.None, 0.7f);
                    }

                    if (cell.IsWalkable)
                    {
                        switch (cell.X % 3)
                        {
                            case 0: _floor = floor1;
                                break;
                            case 1: _floor = floor2;
                                break;
                            case 2: _floor = floor3;
                                break;
                        }
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
                spriteBatch.Draw(_spaceItem, Global.Camera.ScreenToWorld(new Vector2(160, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
                spriteBatch.Draw(_level, Global.Camera.ScreenToWorld(new Vector2(832, 0)), null, null, null, 0.0f, Vector2.One, Color.White, SpriteEffects.None, 0);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdatePlayerFieldOfView()
        {
            currZone.Layout.ComputeFov(_player.X, _player.Y, 15, true);
            foreach (Cell cell in currZone.Layout.GetAllCells())
            {
                if (currZone.Layout.IsInFov(cell.X, cell.Y))
                {
                    currZone.Layout.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        private void AddAggressiveEnemies(Zone zone)
        {
            Global.EnemyList.TrimExcess();
            int numberOfRavens = Global.Random.Next(1,5);
            for (int i = 0; i < numberOfRavens; i++)
            {
                Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                var pathFromAggressiveEnemy = new PathToPlayer(_player, currZone.Layout, Content.Load<Texture2D>("white"));
                pathFromAggressiveEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
                Texture2D texture = Content.Load<Texture2D>("ravenanimate");
                var enemy = new AggressiveEnemy(texture, 3, 3, pathFromAggressiveEnemy, currZone.Layout)
                {
                    X = enemyCell.X,
                    Y = enemyCell.Y,
                    Health = 1,
                    Damage = 1,
                    Name = "Raven",
                    isStunned = false
                };
                Global.EnemyList.Add(enemy);
            }

            int numberOfFires = Global.Random.Next(1,5);
            for (int i = 0; i < numberOfFires; i++)
            {
                Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                var pathFromAggressiveEnemy = new PathToPlayer(_player, currZone.Layout, Content.Load<Texture2D>("white"));
                pathFromAggressiveEnemy.CreateFrom(enemyCell.X, enemyCell.Y);
                Texture2D texture = Content.Load<Texture2D>("fireanimated");
                var enemy = new AggressiveEnemy(texture, 3, 3, pathFromAggressiveEnemy, currZone.Layout)
                {
                    X = enemyCell.X,
                    Y = enemyCell.Y,
                    Health = 2,
                    Damage = 1,
                    Name = "Fire",
                    isStunned = false
                };
                Global.EnemyList.Add(enemy);
            }
        }

        private void AddItem(Zone zone)
        {
            if (Global.Random.Next(1, 3) == 1)
            {
                int n = Global.Random.Next(Global.ItemList.Count - 1);
                zone.Item = Global.ItemList[n];
                Global.ItemList.Remove(zone.Item);
            }
        }

        public void AOEDamage(int strength)
        {
            Global.EnemyList.TrimExcess();
            int n = Global.EnemyList.Count;
            List<AggressiveEnemy> tmp = new List<AggressiveEnemy>();
            for (int i = 0; i < n; i++)
            {
                AggressiveEnemy enemy = Global.EnemyList[i];
                enemy.Health -= strength;
                if (enemy.Health <= 0)
                {
                    switch (enemy.Name)
                    {
                        case "Raven":
                            Global.XPTally += 1;
                            break;
                        case "Fire":
                            Global.XPTally += 2;
                            break;
                        case "Dragon":
                            Global.XPTally += 3;
                            break;
                    }
                }
                else
                {
                    tmp.Add(enemy);
                }
            }
            tmp.TrimExcess();
            Global.EnemyList = tmp;
        }

        private void IterateXP(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _player.XP++;
                switch (_player.XP)
                {
                    case 8:
                        _player.Level = 2;
                        _player.Damage = 2;
                        break;
                    case 13:
                        _player.Level = 3;
                        _player.Damage = 3;
                        break;
                    case 22:
                        _player.Level = 4;
                        _player.Damage = 4;
                        break;
                    case 35:
                        _player.Level = 5;
                        _player.Damage = 5;
                        break;
                }
            }
        }

        private void UpdateGUI()
        {
            switch (_player.Health)
            {
                case 1: _life = life1;
                    break;
                case 0: _life = life0;
                    break;
                case 2: _life = life2;
                    break;
            }

            switch (_player.Level)
            {
                case 1: _level = level1;
                    break;
                case 2: _level = level2;
                    break;
                case 3: _level = level3;
                    break;
                case 4: _level = level4;
                    break;
                case 5: _level = level5;
                    break;
                case 6: _level = level6;
                    break;
                case 7: _level = level7;
                    break;
            }

            switch (currZone.Item)
            {
                case "None": _itemImage = floor1;
                    break;
                case "Elixir": _itemImage = elixir;
                    break;
                case "Pandora": _itemImage = pandora;
                    break;
                case "Mjolnir": _itemImage = mjolnir;
                    break;
                case "Sword": _itemImage = sword;
                    break;
            }

            switch (_player.Item)
            {
                case "None": _spaceItem = spaceEmpty;
                    break;
                case "Elixir": _spaceItem = spaceElixir;
                    break;
                case "Pandora": _spaceItem = spacePandora;
                    break;
            }

            switch (_player.Weapon)
            {
                case "Sword": _weapon = equippedSword;
                    break;
                case "Mjolnir": _weapon = equippedMjolnir;
                    break;
            }
        }

        private void ItemSwap()
        {
            switch (currZone.Item)
            {
                case "Elixir":
                case "Pandora":
                    string swap1 = _player.Item;
                    _player.Item = currZone.Item;
                    currZone.Item = swap1;
                    currZone.ItemLocation = currZone.Layout.GetCell(_player.X + 1, _player.Y);
                    break;
                case "Mjolnir":
                case "Sword":
                    string swap3 = _player.Weapon;
                    _player.Weapon = currZone.Item;
                    currZone.Item = swap3;
                    currZone.ItemLocation = currZone.Layout.GetCell(_player.X + 1, _player.Y);
                    break;
            }
        }
    }
}

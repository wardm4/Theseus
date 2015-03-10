﻿#region Using Statements
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
using System.Diagnostics;
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

        //GUI and Level Display
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

        //Items
        private Texture2D _itemImage;
        private Texture2D elixir;
        private Texture2D _spaceItem;
        private Texture2D spaceEmpty;
        private Texture2D spaceElixir;
        private Texture2D pandora;
        private Texture2D spacePandora;
        private Texture2D boots;
        private Texture2D spaceBoots;
        private Texture2D _weapon;
        private Texture2D sword;
        private Texture2D mjolnir;
        private Texture2D bident;
        private Texture2D equippedMjolnir;
        private Texture2D equippedSword;
        private Texture2D equippedBident;

        //Player Sprite
        private Texture2D theseusLeft;
        private Texture2D theseusRight;
        private Texture2D theseusDead;
        private Texture2D mjolnirLeft;
        private Texture2D mjolnirRight;
        private Texture2D bidentLeft;
        private Texture2D bidentRight;

        //Enemy Sprites
        private Texture2D ravenanimate;
        private Texture2D fireanimate;
        private Texture2D dragonanimate;

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
        public bool speed;
        int speedCounter;

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
            //Initializes Some Values
            elapsedTime = 0;
            deadTime = 0;
            winTime = 0;
            speedCounter = 0;
            Global.EnemyList.Clear();
            Global.ItemList.Clear();
            Global.ItemList.Add("Elixir");
            Global.ItemList.Add("Pandora");
            Global.ItemList.Add("Boots");
            Global.ItemList.Add("Mjolnir");
            Global.ItemList.Add("Bident");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads all Content Up Front
            floor1 = Content.Load<Texture2D>("floor1");
            floor2 = Content.Load<Texture2D>("floor2");
            floor3 = Content.Load<Texture2D>("floor3");
            wall1 = Content.Load<Texture2D>("rock");
            wall2 = Content.Load<Texture2D>("rock2");
            wall3 = Content.Load<Texture2D>("rock3");

            life0 = Content.Load<Texture2D>("life0");
            life1 = Content.Load<Texture2D>("life");
            life2 = Content.Load<Texture2D>("life2");
            _life = life1;

            level1 = Content.Load<Texture2D>("level1");
            level2 = Content.Load<Texture2D>("level2");
            level3 = Content.Load<Texture2D>("level3");
            level4 = Content.Load<Texture2D>("level4");
            level5 = Content.Load<Texture2D>("level5");
            level6 = Content.Load<Texture2D>("level6");
            level7 = Content.Load<Texture2D>("level7");
            _level = level1;

            spaceEmpty = Content.Load<Texture2D>("spaceitem");
            elixir = Content.Load<Texture2D>("elixir");
            spaceElixir = Content.Load<Texture2D>("spaceElixir");
            pandora = Content.Load<Texture2D>("pandora");
            spacePandora = Content.Load<Texture2D>("spacePandora");
            boots = Content.Load<Texture2D>("boots");
            spaceBoots = Content.Load<Texture2D>("spaceBoots");

            theseusLeft = Content.Load<Texture2D>("theseusLeft");
            theseusRight = Content.Load<Texture2D>("theseusRight");
            theseusDead = Content.Load<Texture2D>("theseusDead");
            mjolnirLeft = Content.Load<Texture2D>("mjolnirLeft");
            mjolnirRight = Content.Load<Texture2D>("mjolnirRight");
            bidentLeft = Content.Load<Texture2D>("bidentLeft");
            bidentRight = Content.Load<Texture2D>("bidentRight");

            ravenanimate = Content.Load<Texture2D>("ravenanimate");
            fireanimate = Content.Load<Texture2D>("fireanimated");
            dragonanimate = Content.Load<Texture2D>("dragonanimated");

            _background = Content.Load<Texture2D>("background6");
            sword = Content.Load<Texture2D>("sword");
            equippedSword = Content.Load<Texture2D>("weapon1");
            mjolnir = Content.Load<Texture2D>("mjolnir");
            equippedMjolnir = Content.Load<Texture2D>("weapon2");
            bident = Content.Load<Texture2D>("bident");
            equippedBident = Content.Load<Texture2D>("weapon3");
            _weapon = sword;

            titlescreen = Content.Load<Texture2D>("titlescreen");
            losescreen = Content.Load<Texture2D>("losescreen");
            winscreen = Content.Load<Texture2D>("winscreen");
            portal = Content.Load<Texture2D>("portal");
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
                Multiplier = 1,
                isLeft = true,
                Name = "Hilby"
            };
            AddZoneEnemies(currZone);
            AddItem(currZone);
            UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
            Global.CombatManager = new CombatManager(_player);
            //swordswipe = this.Content.Load<SoundEffect>("sword1");
            //Global.CombatManager.effect = swordswipe;
            speed = false;
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
                    
                    if (elapsedTime % 20 == 0)
                    {
                        List<Cell> newFireLocations = new List<Cell>();
                        foreach (var enemy in Global.EnemyList)
                        {
                            if (enemy.Name == "Dragon" && currZone.Layout.IsInFov(enemy.X, enemy.Y))
                            {
                                if (currZone.Layout.GetCell(enemy.X - 1, enemy.Y).IsWalkable)
                                    newFireLocations.Add(currZone.Layout.GetCell(enemy.X - 1, enemy.Y));
                                else if (currZone.Layout.GetCell(enemy.X + 1, enemy.Y).IsWalkable)
                                    newFireLocations.Add(currZone.Layout.GetCell(enemy.X + 1, enemy.Y));
                                else if (currZone.Layout.GetCell(enemy.X, enemy.Y - 1).IsWalkable)
                                    newFireLocations.Add(currZone.Layout.GetCell(enemy.X, enemy.Y - 1));
                                else
                                    newFireLocations.Add(currZone.Layout.GetCell(enemy.X, enemy.Y + 1));
                            }
                        }
                        foreach (var cell in newFireLocations)
                        {
                            AddEnemyAt("Fire", cell);
                        }
                    }
                    UpdatePlayerFieldOfView();
                }

                if (Global.GameState == GameStates.EnemyTurn)
                {
                    if (speed == true)
                    {
                        speedCounter = 20;
                        speed = false;
                    }
                    if (speedCounter > 0)
                    {
                        if (speedCounter % 2 == 1)
                        {
                            foreach (var enemy in Global.EnemyList)
                            {
                                enemy.Update();
                                if (elapsedTime % 2 == 0)
                                    enemy.isStunned = false;
                            }
                        }
                        Global.GameState = GameStates.PlayerTurn;
                        speedCounter--;
                    }
                    else
                    {
                        foreach (var enemy in Global.EnemyList)
                        {
                            enemy.Update();
                            if (elapsedTime % 2 == 0)
                                enemy.isStunned = false;
                        }
                        Global.GameState = GameStates.PlayerTurn;
                    }
                }

                if (Global.GameState == GameStates.PlayerTurn && _inputState.IsAction(PlayerIndex.One) && _player.X == currZone.Exit.X && _player.Y == currZone.Exit.Y)
                {
                    Global.EnemyList.Clear();
                    currZone = new Zone(currZone.ID);
                    Cell newStart = currZone.GetRandomEmptyCell();
                    _player.X = newStart.X;
                    _player.Y = newStart.Y;
                    Global.GameState = GameStates.EnemyTurn;
                    AddZoneEnemies(currZone);
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
                        case "Boots":
                            speed = true;
                            _player.Item = "None";
                            break;
                    }
                    elapsedTime++;
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
                Stopwatch timer = Stopwatch.StartNew();
                while (timer.ElapsedMilliseconds < 500)
                {
                }
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
                    case "Bident":
                        _player.Sprite = bidentLeft;
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
                    case "Bident":
                        _player.Sprite = bidentRight;
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

        private void AddZoneEnemies(Zone zone)
        {
            switch (zone.ID)
            {
                case 1:
                case 2:
                    Global.EnemyList.TrimExcess();
                    int numberOfRavens = Global.Random.Next(1,5);
                    for (int i = 0; i < numberOfRavens; i++)
                    {
                        Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                        AddEnemyAt("Raven", enemyCell);
                    }

                    int numberOfFires = Global.Random.Next(1,5);
                    for (int i = 0; i < numberOfFires; i++)
                    {
                        Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                        AddEnemyAt("Fire", enemyCell);
                    }
                    break;

                case 3:
                case 4:
                    Global.EnemyList.TrimExcess();
                    int numberOfRavens2 = Global.Random.Next(1,6);
                    for (int i = 0; i < numberOfRavens2; i++)
                    {
                        Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                        AddEnemyAt("Raven", enemyCell);
                    }
                    int numberOfDragons = Global.Random.Next(1, 4);
                    for (int i = 0; i < numberOfDragons; i++)
                    {
                        Cell enemyCell = zone.GetEnemyCell(_player.X, _player.Y);
                        AddEnemyAt("Dragon", enemyCell);
                    }
                    break;
            }
        }

        private void AddEnemyAt(string Name, Cell Place)
        {
            var pathFromAggressiveEnemy = new PathToPlayer(_player, currZone.Layout, Content.Load<Texture2D>("white"));
            pathFromAggressiveEnemy.CreateFrom(Place.X, Place.Y);
            switch (Name)
            {
                case "Raven":
                    var enemy = new AggressiveEnemy(ravenanimate, 3, 3, pathFromAggressiveEnemy, currZone.Layout)
                    {
                        X = Place.X,
                        Y = Place.Y,
                        Health = 1,
                        Damage = 1,
                        Name = "Raven",
                        isStunned = false
                    };
                    Global.EnemyList.Add(enemy);
                    break;

                case "Fire":
                    var enemy2 = new AggressiveEnemy(fireanimate, 3, 3, pathFromAggressiveEnemy, currZone.Layout)
                    {
                        X = Place.X,
                        Y = Place.Y,
                        Health = 2,
                        Damage = 1,
                        Name = "Fire",
                        isStunned = false
                    };
                    Global.EnemyList.Add(enemy2);
                    break;

                case "Dragon":
                    var enemy3 = new AggressiveEnemy(dragonanimate, 3, 3, pathFromAggressiveEnemy, currZone.Layout)
                    {
                        X = Place.X,
                        Y = Place.Y,
                        Health = 4,
                        Damage = 1,
                        Name = "Dragon",
                        isStunned = false
                    };
                    Global.EnemyList.Add(enemy3);
                    break;
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
                case "Boots": _itemImage = boots;
                    break;
                case "Mjolnir": _itemImage = mjolnir;
                    break;
                case "Sword": _itemImage = sword;
                    break;
                case "Bident": _itemImage = bident;
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
                case "Boots": _spaceItem = spaceBoots;
                    break;
            }

            switch (_player.Weapon)
            {
                case "Sword": 
                    _weapon = equippedSword;
                    _player.Multiplier = 1;
                    break;

                case "Mjolnir": 
                    _weapon = equippedMjolnir;
                    _player.Multiplier = 2;
                    break;

                case "Bident":
                    _weapon = equippedBident;
                    _player.Multiplier = 1;
                    break;
            }
        }

        private void ItemSwap()
        {
            switch (currZone.Item)
            {
                case "Elixir":
                case "Pandora":
                case "Boots":
                    string swap1 = _player.Item;
                    _player.Item = currZone.Item;
                    currZone.Item = swap1;
                    break;

                case "Mjolnir":
                    string swap2 = _player.Weapon;
                    _player.Weapon = currZone.Item;
                    currZone.Item = swap2;
                    if (_player.isLeft)
                        _player.Sprite = mjolnirLeft;
                    else
                        _player.Sprite = mjolnirRight;
                    break;

                case "Sword":
                    string swap3 = _player.Weapon;
                    _player.Weapon = currZone.Item;
                    currZone.Item = swap3;
                    if (_player.isLeft)
                        _player.Sprite = theseusLeft;
                    else
                        _player.Sprite = theseusRight;
                    break;

                case "Bident":
                    string swap4 = _player.Weapon;
                    _player.Weapon = currZone.Item;
                    currZone.Item = swap4;
                    if (_player.isLeft)
                        _player.Sprite = bidentLeft;
                    else
                        _player.Sprite = bidentRight;
                    break;
            }
            if (currZone.Layout.GetCell(_player.X + 1, _player.Y).IsWalkable)
                currZone.ItemLocation = currZone.Layout.GetCell(_player.X + 1, _player.Y);
            else if (currZone.Layout.GetCell(_player.X - 1, _player.Y).IsWalkable)
                currZone.ItemLocation = currZone.Layout.GetCell(_player.X - 1, _player.Y);
            else if (currZone.Layout.GetCell(_player.X, _player.Y + 1).IsWalkable)
                currZone.ItemLocation = currZone.Layout.GetCell(_player.X, _player.Y + 1);
            else
                currZone.ItemLocation = currZone.Layout.GetCell(_player.X, _player.Y - 1);
        }
    }
}

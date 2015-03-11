using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Theseus
{
    public class AggressiveEnemy : Figure
    {
        private readonly PathToPlayer _path;
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int count;
        private readonly Zone _zone;
        private readonly IMap _map;
        private bool _isAwareOfPlayer;
        private int k;

        public AggressiveEnemy(Texture2D texture, int rows, int columns, PathToPlayer path, Zone zone)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            count = 0;
            _path = path;
            _zone = zone;
            _map = zone.Layout;
            k = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)X * width, (int)Y * height, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.One, SpriteEffects.None, LayerDepth.Enemies);
        }

        public void Animate()
        {
            switch (Name)
            {
                case "Raven":
                case "Minotaur":
                    if (count % 3 == 2)
                        currentFrame++;
                    count++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                    break;

                case "Fire":
                    if (count % 10 == 9)
                        currentFrame++;
                    count++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                    break;

                case "Dragon":
                    if (count % 8 == 7)
                        currentFrame++;
                    count++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                    break;
                
                case "Snake":
                    if (count % 5 == 4)
                        currentFrame++;
                    count++;
                    if (currentFrame == totalFrames)
                        currentFrame = 0;
                    break;
            }
        }

        public int Update()
        {
            if (!_isAwareOfPlayer)
            {
                if (_map.IsInFov(X, Y))
                {
                    _isAwareOfPlayer = true;
                }
            }

            if (_isAwareOfPlayer)
            {
                switch (Name)
                {
                    case "Raven":
                        _path.CreateFrom(X, Y);
                        if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y) && !isStunned)
                        {
                            Global.CombatManager.Attack(this, Global.CombatManager.FigureAt(_path.FirstCell.X, _path.FirstCell.Y));
                        }
                        else if (_path.cellList() != null && !isStunned && !Global.CombatManager.IsEnemyAt(_path.FirstCell.X, _path.FirstCell.Y))
                        {
                            X = _path.FirstCell.X;
                            Y = _path.FirstCell.Y;
                        }
                        break;

                    case "Fire":
                        if (k % 2 == 1)
                        {
                            int i = 1;
                            while (i >= 0)
                            {
                                _path.CreateFrom(X, Y);
                                if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y) && !isStunned)
                                {
                                    Global.CombatManager.Attack(this, Global.CombatManager.FigureAt(_path.FirstCell.X, _path.FirstCell.Y));
                                }
                                else if (_path.cellList() != null && !isStunned && !Global.CombatManager.IsEnemyAt(_path.FirstCell.X, _path.FirstCell.Y))
                                {
                                    X = _path.FirstCell.X;
                                    Y = _path.FirstCell.Y;
                                }
                                i--;
                            }
                        }
                        k++;
                        break;

                    case "Dragon":
                        break;

                    case "Snake":
                        Figure theseus = Global.CombatManager.IsPlayerDiagonal(X,Y);
                        if (theseus != null && !isStunned)
                        {
                            Global.CombatManager.Attack(this, Global.CombatManager.FigureAt(theseus.X, theseus.Y));
                        }
                        else
                        {
                            Cell tmpPosition = _zone.GetRandomEmptyCell();
                            int j = Global.Random.Next(1,5);
                            switch (j)
                            {
                                case 1: tmpPosition = _zone.Layout.GetCell(X + 1, Y + 1);
                                    break;
                                case 2: tmpPosition = _zone.Layout.GetCell(X - 1, Y - 1);
                                    break;
                                case 3: tmpPosition = _zone.Layout.GetCell(X + 1, Y - 1);
                                    break;
                                case 4: tmpPosition = _zone.Layout.GetCell(X - 1, Y + 1);
                                    break;
                                default: tmpPosition = _zone.Layout.GetCell(X, Y);
                                    break;
                            }
                            if (!Global.CombatManager.IsEnemyAt(tmpPosition.X, tmpPosition.Y) && !Global.CombatManager.IsPlayerAt(tmpPosition.X, tmpPosition.Y) && tmpPosition.IsWalkable)
                            {
                                X = tmpPosition.X;
                                Y = tmpPosition.Y;
                            }
                        }
                        break;

                    case "Minotaur":
                        _path.CreateFrom(X, Y);
                        if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y) && !isStunned)
                        {
                            Global.CombatManager.Attack(this, Global.CombatManager.FigureAt(_path.FirstCell.X, _path.FirstCell.Y));
                        }
                        else 
                        {
                            Cell testPosition = _zone.GetRandomEmptyCell();
                            while ( _zone.Distance(testPosition.X, testPosition.Y, X, Y) > 4) 
                            {
                                testPosition = _zone.GetRandomEmptyCell();
                            }
                            if (!Global.CombatManager.IsEnemyAt(testPosition.X, testPosition.Y) && !Global.CombatManager.IsPlayerAt(testPosition.X, testPosition.Y))
                            {
                                X = testPosition.X;
                                Y = testPosition.Y;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        break;
                }
            }
            return 0;
        }
    }
}

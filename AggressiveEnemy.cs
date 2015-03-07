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
        private readonly IMap _map;
        private bool _isAwareOfPlayer;

        public AggressiveEnemy(Texture2D texture, int rows, int columns, PathToPlayer path, IMap map)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            count = 0;
            _path = path;
            _map = map;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)X * width, (int)Y * height, width, height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.One, SpriteEffects.None, LayerDepth.Figures);
        }

        public void Animate()
        {
            if (count % 3 == 2)
                currentFrame++;
            count++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Update()
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
                _path.CreateFrom(X, Y);
                if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y))
                {
                    Global.CombatManager.Attack(this,
                        Global.CombatManager.FigureAt(_path.FirstCell.X, _path.FirstCell.Y));
                }
                else if (_path.cellList() != null)
                {
                    X = _path.FirstCell.X;
                    Y = _path.FirstCell.Y;
                }
            }
        }
    }
}

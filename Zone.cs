using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RogueSharp;
using RogueSharp.DiceNotation;

namespace Theseus
{
    public class Zone
    {
        public Map Layout {get; set;}
        public string Item { get; set; }
        public int ID { get; set; }
        public Cell Exit { get; set; }
        public Cell ItemLocation { get; set; }
        private RandomRoomsMapCreationStrategy<Map> mapCreationStrategy;

        public Zone(int currID)
        {
            currID++;
            ID = currID;
            if (currID < 6)
            {
                mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 50, 7, 3);
            }
            else
            {
                mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Global.MapWidth, Global.MapHeight, 3, 15, 15);
            }
            Layout = Map.Create(mapCreationStrategy);
            Exit = GetRandomEmptyCell();
            Item = "None";
            ItemLocation = GetRandomEmptyCell();
        }

        public Cell GetRandomEmptyCell()
        {
            while (true)
            {
                int x = Global.Random.Next(Global.MapWidth - 1);
                int y = Global.Random.Next(Global.MapHeight - 1);
                if (Layout.IsWalkable(x, y))
                {
                    return Layout.GetCell(x, y);
                }
            }
        }

        public Cell GetEnemyCell(int playerX, int playerY)
        {
            while (true)
            {
                int x = Global.Random.Next(Global.MapWidth - 1);
                int y = Global.Random.Next(Global.MapHeight - 1);
                if (Layout.IsWalkable(x, y) && IsLegitDistance(x, y, playerX, playerY))
                {
                    return Layout.GetCell(x, y);
                }
            }
        }

        public int Distance(int firstx, int firsty, int secondx, int secondy)
        {
            return ((firstx - secondx) * (firstx - secondx) + (firsty - secondy) * (firsty - secondy));
        }

        private bool IsLegitDistance(int x, int y, int playerX, int playerY)
        {
            bool legit = true;
            foreach (var enemy in Global.EnemyList)
            {
                if (Distance(x, y, enemy.X, enemy.Y) <= 4)
                {
                    legit = false;
                }
            }
            if (Distance(x, y, playerX, playerY) <= 4)
            {
                legit = false;
            }
            return legit;
        }
    }
}

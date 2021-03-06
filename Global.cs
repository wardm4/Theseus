﻿using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theseus
{
    public enum GameStates
    {
        None = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Debugging = 3
    }

    public class Global
    {
        public static readonly IRandom Random = new DotNetRandom();
        public static GameStates GameState { get; set; }
        public static readonly int MapWidth = 40;
        public static readonly int MapHeight = 20;
        public static readonly int SpriteWidth = 64;
        public static readonly int SpriteHeight = 64;
        public static readonly Camera Camera = new Camera();
        public static CombatManager CombatManager;
        public static List<AggressiveEnemy> EnemyList { get; set; }
        public static int XPTally { get; set; }
        public static List<string> ItemList { get; set; }
        public static bool isMinotaurAlive;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace Theseus
{
    public class Figure
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public bool isStunned { get; set; }
    }
}
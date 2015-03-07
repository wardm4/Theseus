using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theseus
{
    public class CombatManager
    {
        private readonly Player _player;
        private readonly List<AggressiveEnemy> _aggressiveEnemies;
        public SoundEffect effect { get; set; }

        public CombatManager( Player player, List<AggressiveEnemy> aggressiveEnemies) 
        {
            _player = player;
            _aggressiveEnemies = aggressiveEnemies;
        }

        public void Attack(Figure attacker, Figure defender)
        {
            defender.Health -= attacker.Damage;
            defender.isStunned = true;
            if (defender.Health <= 0)
            {
                if (defender is AggressiveEnemy)
                {
                    var enemy = defender as AggressiveEnemy;
                    effect.Play(0.1f, 0.0f, 0.0f);
                    _aggressiveEnemies.Remove(enemy);
                }
            }
        }

        public Figure FigureAt(int x, int y)
        {
            if (IsPlayerAt(x, y))
            {
                return _player;
            }
            return EnemyAt(x, y);
        }

        public bool IsPlayerAt(int x, int y)
        {
            return (_player.X == x && _player.Y == y);
        }

        public AggressiveEnemy EnemyAt(int x, int y)
        {
            foreach (var enemy in _aggressiveEnemies)
            {
                if (enemy.X == x && enemy.Y == y)
                {
                    return enemy;
                }
            }
            return null;
        }

        public bool IsEnemyAt(int x, int y)
        {
            return EnemyAt(x, y) != null;
        }
    }
}

using System;
using RollOfTheDice.Models;
using UnityEngine;

namespace RollOfTheDice.Controllers
{
    public class GameController
    {
        public Action OnPlayerTurnComplete;
        public Action OnEnemyTurnComplete;
        public Action OnRoundComplete;

        private Player _player;
        private Enemy _enemy;

        public void SetUpRound(Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;
        }

        public void SubmitPlayerTurn(int attackDamage)
        {
            _enemy.TakeDamage(attackDamage);
            
            Debug.Log($"Damage this turn: {attackDamage}\r\nEnemy health: {_enemy.Health}");

            if (_enemy.Dead)
            {
                OnRoundComplete?.Invoke();
                return;
            }
            
            OnPlayerTurnComplete?.Invoke();
        }

        public void SubmitEnemyTurn(int attackDamage)
        {
            _player.TakeDamage(attackDamage);
            
            Debug.Log($"Damage this turn: {attackDamage}\r\nTotal enemy damage: {_player.Health}");
            
            if (_player.Dead)
            {
                OnRoundComplete?.Invoke();
                return;
            }
            
            OnEnemyTurnComplete?.Invoke();
        }
    }
}
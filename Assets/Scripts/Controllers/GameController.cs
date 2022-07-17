using System;
using RollOfTheDice.Models;
using RollOfTheDice.Services;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Controllers
{
    public class GameController
    {
        public Action OnPlayerTurnComplete;
        public Action OnEnemyTurnComplete;
        public Action<int[]> OnDiceRolled;
        public Action OnRoundComplete;

        private DiceService _diceService;

        private Player _player;
        private Enemy _enemy;

        [Inject]
        public void Constructor(DiceService diceService)
        {
            _diceService = diceService;
        }

        public void SetUpRound(Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;
        }

        public void SubmitPlayerTurn(PlayerTurnData turnData)
        {
            _player.AddShield(turnData.Defend);
            _enemy.TakeDamage(turnData.Attack);

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

        public void RollDice()
        {
            var diceRolls = _diceService.RollDice(_player.DiceCount);
            OnDiceRolled?.Invoke(diceRolls);
            
            Debug.Log("Dice rolled");
        }
    }
}
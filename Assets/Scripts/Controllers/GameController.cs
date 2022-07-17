using System;
using RollOfTheDice.Models;
using RollOfTheDice.Services;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Controllers
{
    public class GameController
    {
        public Action OnRoundStart;
        public Action OnPlayerTurnComplete;
        public Action OnEnemyTurnComplete;
        public Action<int[]> OnDiceRolled;
        public Action OnRoundComplete;

        public Player Player;

        private DiceService _diceService;
        
        private Enemy _enemy;

        [Inject]
        public void Constructor(DiceService diceService)
        {
            _diceService = diceService;
        }

        public void SetUpRound(Player player, Enemy enemy)
        {
            Player = player;
            _enemy = enemy;
            
            OnRoundStart?.Invoke();
        }

        public void SubmitPlayerTurn(PlayerTurnData turnData)
        {
            Player.AddShield(turnData.Defend);
            _enemy.TakeDamage(turnData.Attack);

            if (_enemy.Dead)
            {
                OnRoundComplete?.Invoke();
                return;
            }
            
            OnPlayerTurnComplete?.Invoke();
        }

        public void SubmitEnemyTurn(EnemyIntent intent)
        {
            switch (intent.MoveType)
            {
                case MoveType.Attack:
                    Player.TakeDamage(intent.MovePower);
                    break;
                case MoveType.Defend:
                    _enemy.AddShield(intent.MovePower);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            if (Player.Dead)
            {
                OnRoundComplete?.Invoke();
                return;
            }
            
            OnEnemyTurnComplete?.Invoke();
        }

        public void RollDice()
        {
            var diceRolls = _diceService.RollDice(Player.DiceCount);
            OnDiceRolled?.Invoke(diceRolls);
            
            Debug.Log("Dice rolled");
        }
    }
}
using System;
using RollOfTheDice.Models;
using RollOfTheDice.Services;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Controllers
{
    public class GameController
    {
        public Action OnGameReset;
        public Action OnRoundStart;
        public Action OnPlayerTurnComplete;
        public Action OnEnemyTurnComplete;
        public Action<int[]> OnDiceRolled;
        public Action OnRoundComplete;

        public Player Player;
        public Enemy Enemy;

        private DiceService _diceService;

        [Inject]
        public void Constructor(DiceService diceService)
        {
            _diceService = diceService;
        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void SetEnemy(Enemy enemy)
        {
            Enemy = enemy;
        }

        public void Reset()
        {
            OnGameReset?.Invoke();
        }

        public void StartRound()
        {
            Player.ResetShield();
            OnRoundStart?.Invoke();
        }

        public void SubmitPlayerTurn(PlayerTurnData turnData)
        {
            Player.AddShield(turnData.Defend);
            Enemy.TakeDamage(turnData.Attack);

            if (Enemy.Dead)
            {
                OnRoundComplete?.Invoke();
                return;
            }
            
            OnPlayerTurnComplete?.Invoke();
        }

        public void SubmitEnemyTurn()
        {
            var intent = Enemy.GetNextIntent();
            
            switch (intent.MoveType)
            {
                case MoveType.Attack:
                    Player.TakeDamage(intent.MovePower);
                    break;
                case MoveType.Defend:
                    Enemy.AddShield(intent.MovePower);
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
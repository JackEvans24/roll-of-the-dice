using System;
using System.Collections;
using System.Collections.Generic;
using RollOfTheDice.Models;
using RollOfTheDice.Services;
using RollOfTheDice.UIComponents;
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

        public Action<PlayerUpdateData> OnPlayerUpdate;
        public Action<EnemyUpdateData> OnEnemyUpdate;

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

        public IEnumerator SubmitPlayerTurn(IEnumerable<DropZone> dropZones)
        {
            foreach (var dropZone in dropZones)
            {
                if (dropZone.CurrentDie == null)
                    continue;

                switch (dropZone.DropZoneType)
                {
                    case DropZoneType.Attack:
                        Enemy.TakeDamage(dropZone.CurrentDie.Value);
                        OnPlayerUpdate(new PlayerUpdateData(Player, "Attack"));
                        OnEnemyUpdate(new EnemyUpdateData(Enemy, "Hurt"));
                        yield return new WaitForSeconds(Player.AttackTime);
                        break;
                    case DropZoneType.Defence:
                        Player.AddShield(dropZone.CurrentDie.Value);
                        OnPlayerUpdate(new PlayerUpdateData(Player, "Shield"));
                        yield return new WaitForSeconds(Player.ShieldTime);
                        break;
                }

                if (!Enemy.Dead)
                    continue;
                
                OnRoundComplete?.Invoke();
                yield break;
            }
            
            OnPlayerTurnComplete?.Invoke();
        }

        public IEnumerator SubmitEnemyTurn()
        {
            var intent = Enemy.GetNextIntent();
            
            switch (intent.MoveType)
            {
                case MoveType.Attack:
                    Player.TakeDamage(intent.MovePower);
                    OnEnemyUpdate(new EnemyUpdateData(Enemy, "Attack"));
                    OnPlayerUpdate(new PlayerUpdateData(Player, "Hurt"));
                    yield return new WaitForSeconds(Player.AttackTime);
                    break;
                case MoveType.Defend:
                    Enemy.AddShield(intent.MovePower);
                    OnEnemyUpdate(new EnemyUpdateData(Enemy, "Shield"));
                    yield return new WaitForSeconds(Player.AttackTime);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            if (Player.Dead)
            {
                OnRoundComplete?.Invoke();
                yield break;
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
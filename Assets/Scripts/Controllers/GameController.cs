using System;
using UnityEngine;

namespace RollOfTheDice.Controllers
{
    public class GameController
    {
        public Action OnPlayerTurnComplete;
        public Action OnEnemyTurnComplete;
        
        private int _totalEnemyDamage;
        private int _totalPlayerDamage;
        
        public void SubmitPlayerTurn(int attackDamage)
        {
            _totalEnemyDamage += attackDamage;
            
            Debug.Log($"Damage this turn: {attackDamage}\r\nTotal enemy damage: {_totalEnemyDamage}");
            
            OnPlayerTurnComplete?.Invoke();
        }

        public void SubmitEnemyTurn(int attackDamage)
        {
            _totalPlayerDamage += attackDamage;
            
            Debug.Log($"Damage this turn: {attackDamage}\r\nTotal enemy damage: {_totalPlayerDamage}");
            
            OnEnemyTurnComplete?.Invoke();
        }
    }
}
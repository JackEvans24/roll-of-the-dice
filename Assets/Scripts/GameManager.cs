using System.Collections;
using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
using UnityEngine;
using Zenject;

namespace RollOfTheDice
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Players")]
        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;
        
        [Header("Variables")]
        [SerializeField] private float _turnWait = 0.6f;

        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _player.Initialise();
            _enemy.Initialise();
            _gameController.SetUpRound(_player, _enemy);

            _gameController.OnPlayerTurnComplete += EnemyDealDamage;
            _gameController.OnRoundComplete += RoundComplete;
            
            _gameController.RollDice();
        }
        
        private void EnemyDealDamage() => StartCoroutine(WaitAndDealDamage());

        private IEnumerator WaitAndDealDamage()
        {
            yield return new WaitForSeconds(_turnWait);
            _gameController.SubmitEnemyTurn(_enemy.AttackDamage);
            _gameController.RollDice();
        }

        private void RoundComplete() => Debug.Log("Round complete");

        private void OnDestroy()
        {
            _gameController.OnPlayerTurnComplete -= EnemyDealDamage;
            _gameController.OnRoundComplete -= RoundComplete;
        }
    }
}
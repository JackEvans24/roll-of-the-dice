using System.Collections;
using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
using RollOfTheDice.Views;
using UnityEngine;
using Zenject;

namespace RollOfTheDice
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Players")]
        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;

        [Header("Views")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private EnemyView _enemyView;
        
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

            _playerView.UpdateDetails(_player);
            _enemyView.UpdateDetails(_enemy);

            _gameController.OnPlayerTurnComplete += PlayerTurnComplete;
            _gameController.OnRoundComplete += RoundComplete;
            
            _gameController.RollDice();
        }

        private void PlayerTurnComplete()
        {
            UpdateStatuses();
            StartCoroutine(WaitAndDealDamage());
        }

        private IEnumerator WaitAndDealDamage()
        {
            yield return new WaitForSeconds(_turnWait);
            _gameController.SubmitEnemyTurn(_enemy.GetNextIntent());
            
            UpdateStatuses();
            
            _gameController.RollDice();
        }

        private void RoundComplete()
        {
            UpdateStatuses();
            Debug.Log("Round complete");
        }

        private void UpdateStatuses()
        {
            _playerView.UpdateDetails(_player);
            _enemyView.UpdateDetails(_enemy);
        }

        private void OnDestroy()
        {
            _gameController.OnPlayerTurnComplete -= PlayerTurnComplete;
            _gameController.OnRoundComplete -= RoundComplete;
        }
    }
}
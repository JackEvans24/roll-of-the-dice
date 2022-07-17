using System.Collections;
using System.Collections.Generic;
using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
using RollOfTheDice.UIComponents;
using RollOfTheDice.Views;
using UnityEngine;
using Zenject;

namespace RollOfTheDice
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Players")]
        [SerializeField] private Player _player;
        [SerializeField] private Enemy[] _enemies;

        [Header("Views")]
        [SerializeField] private GameView _gameView;
        [SerializeField] private RoundEndView _roundEndView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private EnemyView _enemyView;

        [Header("Variables")]
        [SerializeField] private float _turnWait = 0.6f;

        private GameController _gameController;
        private Queue<Enemy> _enemyQueue = new Queue<Enemy>();
        private float _multiplier = 1f;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _gameController.OnGameReset += StartGame;
            _gameController.OnRoundStart += NewRound;
            _gameController.OnPlayerTurnComplete += PlayerTurnComplete;
            _gameController.OnRoundComplete += RoundComplete;

            _gameView.OnTurnConfirmed += CompleteTurn;
        }

        private void StartGame()
        {
            _multiplier = 1f;
            
            _player.Initialise();
            _gameController.SetPlayer(_player);
            
            SetEnemyQueue();

            _gameController.StartRound();
        }

        private void NewRound()
        {
            var enemy = GetEnemy();
            _gameController.SetEnemy(enemy);
            
            _enemyView.Initialise(_gameController.Enemy);
            
            UpdateStatuses();
            
            _gameController.RollDice();
        }
        
        public void CompleteTurn()
        {
            var dropZones = new List<DropZone>(_playerView.DropZones);
            dropZones.AddRange(_enemyView.DropZones);
            StartCoroutine(_gameController.SubmitPlayerTurn(dropZones));
        }

        private void PlayerTurnComplete()
        {
            StartCoroutine(WaitAndDealDamage());
        }

        private IEnumerator WaitAndDealDamage()
        {
            yield return new WaitForSeconds(_turnWait);
            yield return _gameController.SubmitEnemyTurn();
            
            UpdateStatuses();
            
            _gameController.RollDice();
        }

        private void RoundComplete()
        {
            UpdateStatuses();
            _roundEndView.enabled = true;
        }

        private void SetEnemyQueue()
        {
            _enemyQueue = new Queue<Enemy>(_enemies);
        }

        private Enemy GetEnemy()
        {
            var enemy = _enemyQueue.Dequeue();
            enemy.Initialise(_multiplier);
            if (_enemyQueue.Count <= 0)
            {
                SetEnemyQueue();
                _multiplier += 0.5f;
            }
            return enemy;
        }

        private void UpdateStatuses()
        {
            _playerView.UpdateDetails(_gameController.Player);
            _enemyView.UpdateDetails(_gameController.Enemy);
        }

        private void OnDestroy()
        {
            _gameController.OnGameReset -= StartGame;
            _gameController.OnRoundStart -= NewRound;
            _gameController.OnPlayerTurnComplete -= PlayerTurnComplete;
            _gameController.OnRoundComplete -= RoundComplete;
            
            _gameView.OnTurnConfirmed -= CompleteTurn;
        }
    }
}
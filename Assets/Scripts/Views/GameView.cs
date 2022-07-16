using RollOfTheDice.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace RollOfTheDice.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        
        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;

            _gameController.OnPlayerTurnComplete += DisableInput;
            _gameController.OnEnemyTurnComplete += EnableInput;
            _gameController.OnRoundComplete += DisableInput;
        }

        private void DisableInput() => _eventSystem.enabled = false;
        private void EnableInput() => _eventSystem.enabled = true;

        private void OnDestroy()
        {
            _gameController.OnPlayerTurnComplete -= DisableInput;
            _gameController.OnEnemyTurnComplete -= EnableInput;
            _gameController.OnRoundComplete -= DisableInput;
        }
    }
}
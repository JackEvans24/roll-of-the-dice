using RollOfTheDice.Controllers;
using RollOfTheDice.UIComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace RollOfTheDice.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Die[] _diceComponents;
        
        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _gameController.OnDiceRolled += DiceRolled;
            _gameController.OnPlayerTurnComplete += DisableInput;
            _gameController.OnRoundComplete += DisableInput;
        }

        private void DiceRolled(int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                if (i >= _diceComponents.Length)
                    break;
                _diceComponents[i].SetValue(values[i]);
            }
            
            EnableInput();
        }
        
        private void DisableInput()
        {
            foreach (var die in _diceComponents)
                die.gameObject.SetActive(false);
            _eventSystem.enabled = false;
        }
        
        private void EnableInput()
        {
            foreach (var die in _diceComponents)
                die.gameObject.SetActive(true);
            _eventSystem.enabled = true;
        }

        private void OnDestroy()
        {
            _gameController.OnDiceRolled -= DiceRolled;
            _gameController.OnPlayerTurnComplete -= DisableInput;
            _gameController.OnRoundComplete -= DisableInput;
        }
    }
}
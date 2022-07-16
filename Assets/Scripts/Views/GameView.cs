using RollOfTheDice.Controllers;
using RollOfTheDice.UIComponents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RollOfTheDice.Views
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _completeButton;
        [SerializeField] private Die[] _dice;
        
        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _completeButton.onClick.AddListener(CompleteTurn);
            _gameController.OnDiceRolled += DiceRolled;
            foreach (var die in _dice)
                die.OnDiePlaced += CheckDice;
        }

        public void CompleteTurn()
        {
            var totalDamage = 0;
            foreach (var die in _dice)
            {
                die.Enable(false);
                totalDamage += die.Value;
            }
            
            _gameController.SubmitPlayerTurn(totalDamage);
            _completeButton.interactable = false;
        }

        private void CheckDice()
        {
            var allDicePlaced = true;
            foreach (var die in _dice)
            {
                if (die._dropZone != null)
                    continue;

                allDicePlaced = false;
                break;
            }

            _completeButton.interactable = allDicePlaced;
        }

        private void DiceRolled(int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                if (i >= _dice.Length)
                    break;
                _dice[i].SetValue(values[i]);
                _dice[i].Reset();
            }

            _completeButton.interactable = false;
        }

        private void OnDestroy()
        {
            _completeButton.onClick.RemoveAllListeners();
            _gameController.OnDiceRolled -= DiceRolled;
            foreach (var die in _dice)
                die.OnDiePlaced -= CheckDice;
        }
    }
}
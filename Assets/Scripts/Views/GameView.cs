using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
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
            var totalAttack = 0;
            var totalDefend = 0;
            
            foreach (var die in _dice)
            {
                die.Enable(false);

                switch (die.DropZone.DropZoneType)
                {
                    case DropZoneType.Attack:
                        totalAttack += die.Value;
                        break;
                    case DropZoneType.Defence:
                        totalDefend += die.Value;
                        break;
                }
            }

            var turnData = new PlayerTurnData(totalAttack, totalDefend);
            _gameController.SubmitPlayerTurn(turnData);
            _completeButton.interactable = false;
        }

        private void CheckDice()
        {
            var allDicePlaced = true;
            foreach (var die in _dice)
            {
                if (die.DropZone != null)
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
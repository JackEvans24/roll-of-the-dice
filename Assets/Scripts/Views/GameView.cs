using System;
using System.Collections.Generic;
using RollOfTheDice.Controllers;
using RollOfTheDice.UIComponents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RollOfTheDice.Views
{
    public class GameView : MonoBehaviour
    {
        public Action OnTurnConfirmed;
        
        [SerializeField] private Button _completeButton;
        [SerializeField] private Die _diePrefab;
        [SerializeField] private Transform _diePlacementPosition;
        [SerializeField] private float _dieSpacing;

        private GameController _gameController;
        private List<Die> _dice = new List<Die>();
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _completeButton.onClick.AddListener(CompleteTurn);
            _gameController.OnDiceRolled += DiceRolled;
            _gameController.OnRoundStart += GenerateDice;
            _gameController.OnRoundComplete += RemoveDice;
        }

        public void CompleteTurn()
        {
            _completeButton.interactable = false;
            OnTurnConfirmed?.Invoke();
        }

        private void GenerateDice()
        {
            var diceCount = _gameController.Player.DiceCount;
            for (var i = 0; i < diceCount; i++)
            {
                var newDie = Instantiate(_diePrefab, _diePlacementPosition);

                var position = GetDiePosition(i, diceCount);
                newDie.transform.position = position;
                newDie.SetPosition(position);
                newDie.OnDiePlaced += CheckDice;
                _dice.Add(newDie);
            }
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
                if (i >= _dice.Count)
                    break;
                _dice[i].SetValue(values[i]);
                _dice[i].Reset();
            }

            _completeButton.interactable = false;
        }

        private void RemoveDice()
        {
            foreach (var die in _dice)
            {
                die.OnDiePlaced -= CheckDice;
                Destroy(die.gameObject);
            }
            
            _dice.Clear();
        }
        
        private Vector2 GetDiePosition(int i, int totalDice)
        {
            var centerPosition = _diePlacementPosition.position;
            var centerOffset = i - (totalDice - 1f) / 2;
            return new Vector2(centerPosition.x + centerOffset * _dieSpacing, centerPosition.y);
        }

        private void OnDestroy()
        {
            _completeButton.onClick.RemoveAllListeners();
            _gameController.OnDiceRolled -= DiceRolled;
            _gameController.OnRoundStart -= GenerateDice;
            _gameController.OnRoundComplete += RemoveDice;
            foreach (var die in _dice)
                die.OnDiePlaced -= CheckDice;
        }
    }
}
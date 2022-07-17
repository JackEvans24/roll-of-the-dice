using RollOfTheDice.Controllers;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Views
{
    public class RoundEndView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _roundEndLabel;
        [SerializeField] private TMP_Text _roundEndButtonText;
        
        [Header("Text content")]
        [SerializeField] private string _winRoundTitle = "Round Won";
        [SerializeField] private string _loseRoundTitle = "You Died";
        [SerializeField] private string _winRoundButtonText = "Next Round";
        [SerializeField] private string _loseRoundButtonText = "Try Again";

        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void OnEnable()
        {
            _canvas.enabled = true;

            var roundWon = _gameController.Player.Health > 0;
            _roundEndLabel.text = roundWon ? _winRoundTitle : _loseRoundTitle;
            _roundEndButtonText.text = roundWon ? _winRoundButtonText : _loseRoundButtonText;
        }

        public void Continue()
        {
            if (_gameController.Player.Health > 0)
                _gameController.StartRound();
            else
                _gameController.Reset();

            enabled = false;
        }

        private void OnDisable()
        {
            _canvas.enabled = false;
        }
    }
}
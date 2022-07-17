using RollOfTheDice.Controllers;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Views
{
    public class StartGameView : MonoBehaviour
    {
        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        public void StartGame()
        {
            _gameController.Reset();
            gameObject.SetActive(false);
        }

        public void OpenTwitter()
        {
            Application.OpenURL("https://twitter.com/Jevans_Games");
        }
    }
}
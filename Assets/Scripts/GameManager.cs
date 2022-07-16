using System.Collections;
using RollOfTheDice.Controllers;
using UnityEngine;
using Zenject;

namespace RollOfTheDice
{
    public class GameManager : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private float _turnWait = 0.6f;

        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;

            _gameController.OnPlayerTurnComplete += EnemyDealDamage;
        }
        
        private void EnemyDealDamage() => StartCoroutine(WaitAndDealDamage());

        private IEnumerator WaitAndDealDamage()
        {
            yield return new WaitForSeconds(_turnWait);
            _gameController.SubmitEnemyTurn(3);
        }

        private void OnDestroy()
        {
            _gameController.OnPlayerTurnComplete -= EnemyDealDamage;
        }
    }
}
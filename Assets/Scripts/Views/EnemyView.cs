using System;
using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
using RollOfTheDice.UIComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RollOfTheDice.Views
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Drop Zones")]
        public DropZone[] DropZones;

        [Header("Sprite")]
        [SerializeField] private GameObject _spriteContainer;
        [SerializeField] private Animator _animator;

        [Header("Value References")]
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private GameObject _shieldIndicator;
        [SerializeField] private TMP_Text _shieldLabel;

        [Header("Intents")]
        [SerializeField] private Image _intentImageWithText;
        [SerializeField] private Image _intentImageOnly;
        [SerializeField] private TMP_Text _intentLabel;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _defendSprite;

        private GameObject _sprite;

        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _gameController.OnEnemyUpdate += EnemyUpdate;
        }

        public void Initialise(Enemy enemy)
        {
            if (_sprite != null)
                Destroy(_sprite);

            _sprite = Instantiate(enemy.Sprite, _spriteContainer.transform);
        }

        public void UpdateDetails(Enemy enemy)
        {
            _healthLabel.text = enemy.Health.ToString();
            
            _shieldIndicator.SetActive(enemy.Shield > 0);
            _shieldLabel.text = enemy.Shield.ToString();

            var intent = enemy.PeekNextIntent();
            var image = _intentImageOnly;
            switch (intent.MoveType)
            {
                case MoveType.Attack:
                {
                    _intentLabel.text = intent.MovePower.ToString();
                    _intentImageOnly.enabled = false;
                    _intentLabel.gameObject.SetActive(true);
                    _intentImageWithText.enabled = true;
                    image = _intentImageWithText;
                    break;
                }
                case MoveType.Defend:
                {
                    _intentLabel.gameObject.SetActive(false);
                    _intentImageWithText.enabled = false;
                    _intentImageOnly.enabled = true;
                    break;
                }
            }

            image.sprite = intent.MoveType switch
            {
                MoveType.Attack => _attackSprite,
                MoveType.Defend => _defendSprite,
                _ => throw new NotImplementedException()
            };
        }

        private void EnemyUpdate(EnemyUpdateData updateData)
        {
            _animator.SetTrigger(updateData.AnimationName);
            UpdateDetails(updateData.Enemy);
        }

        private void OnDestroy()
        {
            _gameController.OnEnemyUpdate -= EnemyUpdate;
        }
    }
}
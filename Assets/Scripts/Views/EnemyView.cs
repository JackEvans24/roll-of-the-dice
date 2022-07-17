using System;
using RollOfTheDice.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RollOfTheDice.Views
{
    public class EnemyView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private GameObject _shieldIndicator;
        [SerializeField] private TMP_Text _shieldLabel;

        [Header("Intents")]
        [SerializeField] private Image _intentImageWithText;
        [SerializeField] private Image _intentImageOnly;
        [SerializeField] private TMP_Text _intentLabel;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _defendSprite;

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
    }
}
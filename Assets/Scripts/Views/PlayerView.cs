using System;
using RollOfTheDice.Controllers;
using RollOfTheDice.Models;
using RollOfTheDice.UIComponents;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollOfTheDice.Views
{
    public class PlayerView : MonoBehaviour
    {
        [Header("UI References")]
        public DropZone[] DropZones;
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private GameObject _shieldIndicator;
        [SerializeField] private TMP_Text _shieldLabel;
        
        [Header("Animator")]
        [SerializeField] private Animator _animator;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _shieldClip;
        [SerializeField] private AudioClip _hurtClip;

        private GameController _gameController;
        
        [Inject]
        public void Constructor(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Start()
        {
            _gameController.OnPlayerUpdate += PlayerUpdate;
        }

        public void UpdateDetails(Player player)
        {
            _healthLabel.text = player.Health.ToString();
            
            _shieldIndicator.SetActive(player.Shield > 0);
            _shieldLabel.text = player.Shield.ToString();
        }

        private void PlayerUpdate(PlayerUpdate updateData)
        {
            _animator.SetTrigger(updateData.AnimationName);
            UpdateDetails(updateData.Player);

            var clip = updateData.UpdateType switch
            {
                UpdateType.Attack => _attackClip,
                UpdateType.Hurt => _hurtClip,
                UpdateType.Shield => _shieldClip,
                _ => throw new NotImplementedException()
            };
            _audioSource.PlayOneShot(clip);
        }

        private void OnDestroy()
        {
            _gameController.OnPlayerUpdate -= PlayerUpdate;
        }
    }
}
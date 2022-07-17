using RollOfTheDice.Models;
using TMPro;
using UnityEngine;

namespace RollOfTheDice.Views
{
    public class PlayerView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private GameObject _shieldIndicator;
        [SerializeField] private TMP_Text _shieldLabel;
        
        public void UpdateDetails(Player player)
        {
            _healthLabel.text = player.Health.ToString();
            
            _shieldIndicator.SetActive(player.Shield > 0);
            _shieldLabel.text = player.Shield.ToString();
        }
    }
}
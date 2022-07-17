using RollOfTheDice.Models;
using TMPro;
using UnityEngine;

namespace RollOfTheDice.Views
{
    public class PlayerView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _healthLabel;
        
        public void UpdateDetails(Player player)
        {
            _healthLabel.text = player.Health.ToString();
        }
    }
}
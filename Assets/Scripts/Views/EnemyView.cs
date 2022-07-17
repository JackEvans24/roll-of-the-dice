using RollOfTheDice.Models;
using TMPro;
using UnityEngine;

namespace RollOfTheDice.Views
{
    public class EnemyView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private TMP_Text _intentLabel;
        
        public void UpdateDetails(Enemy enemy)
        {
            _healthLabel.text = enemy.Health.ToString();
            _intentLabel.text = enemy.AttackDamage.ToString();
        }
    }
}
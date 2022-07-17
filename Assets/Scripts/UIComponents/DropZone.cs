using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RollOfTheDice.UIComponents
{
    public class DropZone : MonoBehaviour
    {
        public DropZoneType DropZoneType;

        [Header("Drop zone icon")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _defendSprite;
        
        [HideInInspector] public Die CurrentDie;

        private void Start()
        {
            _iconImage.sprite = DropZoneType switch
            {
                DropZoneType.Attack => _attackSprite,
                DropZoneType.Defence => _defendSprite,
                _ => _iconImage.sprite
            };
        }

        public bool TryDropDie(Die die)
        {
            if (CurrentDie != null)
                return false;

            CurrentDie = die;
            return true;
        }

        public void RemoveDie()
        {
            CurrentDie = null;
        }
    }

    public enum DropZoneType
    {
        Attack,
        Defence
    }
}
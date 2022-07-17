using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    public class DropZone : MonoBehaviour
    {
        public DropZoneType DropZoneType;

        [Header("Drop zone icon")]
        [SerializeField] private SpriteRenderer _iconSpriteRenderer;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _defendSprite;
        
        [HideInInspector] public Die CurrentDie;

        private void Start()
        {
            _iconSpriteRenderer.sprite = DropZoneType switch
            {
                DropZoneType.Attack => _attackSprite,
                DropZoneType.Defence => _defendSprite,
                _ => _iconSpriteRenderer.sprite
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
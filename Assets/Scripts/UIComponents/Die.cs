using System;
using UnityEngine;
using UnityEngine.UI;

namespace RollOfTheDice.UIComponents
{
    public class Die : Draggable
    {
        [Header("References")]
        [SerializeField] private Image _valueImage;

        [Header("Dice sprites")]
        [SerializeField] private Sprite[] _valueSprites;

        [Header("Variables")]
        [SerializeField] private float _dropZoneCheckRadius = 1f;
        [SerializeField] private LayerMask _dropZoneLayer;

        public Action OnDiePlaced;
        public int Value { get; private set; }
        [HideInInspector] public DropZone DropZone;

        private Vector3 _startPosition;

        private void Start()
        {
            if (_valueSprites.Length != 6)
                throw new InvalidProgramException("Not enough sprites");
            
            _startPosition = transform.position;
            _targetPosition = _startPosition;
            
            OnLift += Lift;
            OnDrop += FindDropZone;
        }

        public void SetValue(int value)
        {
            Value = value;
            _valueImage.sprite = _valueSprites[Value - 1];
        }

        public void SetPosition(Vector3 position)
        {
            _targetPosition = position;
        }

        public void Enable(bool enable)
        {
            _dragEnabled = enable;
        }

        public void Reset()
        {
            RemoveDropZone();
            _targetPosition = _startPosition;
            _dragEnabled = true;
        }

        private void FindDropZone()
        {
            if (!_dragEnabled)
                return;
            
            var foundCollider = Physics2D.OverlapCircle(transform.position, _dropZoneCheckRadius, _dropZoneLayer);
            if (foundCollider == null)
            {
                _targetPosition = _startPosition;
                return;
            }
            
            var foundDropZone = foundCollider.GetComponent<DropZone>();
            if (foundDropZone == null || !foundDropZone.TryDropDie(this))
            {
                _targetPosition = _startPosition;
                return;
            }

            DropZone = foundDropZone;
            _targetPosition = DropZone.transform.position;
            
            OnDiePlaced?.Invoke();
        }

        private void Lift()
        {
            if (!_dragEnabled)
                return;
            RemoveDropZone();
        }

        private void RemoveDropZone()
        {
            if (DropZone == null)
                return;
            DropZone.RemoveDie();
            DropZone = null;
            
            OnDiePlaced?.Invoke();
        }

        private void OnDestroy()
        {
            OnLift -= Lift;
            OnDrop -= FindDropZone;
        }
    }
}
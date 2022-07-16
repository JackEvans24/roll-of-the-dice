using System;
using TMPro;
using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    public class Die : Draggable
    {
        [Header("References")]
        [SerializeField] private TMP_Text _valueLabel;

        [Header("Variables")]
        [SerializeField] private float _dropZoneCheckRadius = 1f;
        [SerializeField] private LayerMask _dropZoneLayer;

        public Action OnDiePlaced;
        public int Value { get; private set; }
        [HideInInspector] public DropZone _dropZone;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
            _targetPosition = _startPosition;
            
            OnLift += RemoveDropZone;
            OnDrop += FindDropZone;
        }

        public void SetValue(int value)
        {
            Value = value;
            _valueLabel.text = Value.ToString();
            _dragEnabled = true;
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

            _dropZone = foundDropZone;
            _targetPosition = _dropZone.transform.position;
            
            OnDiePlaced?.Invoke();
        }

        private void RemoveDropZone()
        {
            if (!_dragEnabled)
                return;
            if (_dropZone == null)
                return;
            _dropZone.RemoveDie();
            _dropZone = null;
            
            OnDiePlaced?.Invoke();
        }

        private void OnDestroy()
        {
            OnLift -= RemoveDropZone;
            OnDrop -= FindDropZone;
        }
    }
}
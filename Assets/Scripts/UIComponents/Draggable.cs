using System;
using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Draggable : MonoBehaviour
    {
        [Header("Drag Variables")]
        [SerializeField] private float _dragSmoothing = 0.05f;
        
        protected bool _dragEnabled;

        protected Action OnLift;
        protected Action OnDrop;
        
        protected Vector3 _targetPosition;
        private Vector3 _currentVelocity;
        private bool _dragging;

        private void Update()
        {
            transform.position =
                Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _dragSmoothing);
        }

        private void OnMouseDown()
        {
            if (!_dragEnabled)
                return;
            
            _dragging = true;
            OnLift?.Invoke();
        }

        private void OnMouseDrag()
        {
            if (!_dragging)
                return;

            _targetPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            _dragging = false;
            OnDrop?.Invoke();
        }
    }
}
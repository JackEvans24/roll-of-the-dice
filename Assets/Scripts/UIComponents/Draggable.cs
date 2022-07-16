using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Draggable : MonoBehaviour
    {
        [Header("Drag Variables")]
        [SerializeField] private float _dragSmoothing = 0.05f;
        
        protected bool _dragEnabled;
        
        private Vector3 _targetPosition;
        private Vector3 _currentVelocity;
        private bool _dragging;

        private void Update()
        {
            if (!_dragging)
                return;

            transform.position =
                Vector3.SmoothDamp(transform.position, _targetPosition, ref _currentVelocity, _dragSmoothing);
        }

        private void OnMouseDown()
        {
            if (!_dragEnabled)
                return;
            
            _dragging = true;
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
        }
    }
}
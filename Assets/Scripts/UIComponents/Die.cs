using TMPro;
using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    public class Die : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueLabel;
        
        public int Value { get; private set; }

        public void SetValue(int value)
        {
            Value = value;
            _valueLabel.text = Value.ToString();
        }
    }
}
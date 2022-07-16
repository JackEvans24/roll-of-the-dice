using UnityEngine;

namespace RollOfTheDice.UIComponents
{
    public class DropZone : MonoBehaviour
    {
        [HideInInspector] public Die CurrentDie;

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
}
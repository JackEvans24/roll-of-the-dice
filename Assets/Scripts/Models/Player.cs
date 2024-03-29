using UnityEngine;

namespace RollOfTheDice.Models
{ 
    [CreateAssetMenu(menuName = "Game/Player")]
    public class Player : UnitWithHealth
    {
        public int DiceCount;
        public float ShieldTime;
        public float AttackTime;
    }
}
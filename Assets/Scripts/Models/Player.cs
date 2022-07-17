using System;

namespace RollOfTheDice.Models
{ 
    [Serializable]
    public class Player : UnitWithHealth
    {
        public int DiceCount;
    }
}
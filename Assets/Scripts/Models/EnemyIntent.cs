using System;

namespace RollOfTheDice.Models
{
    [Serializable]
    public class EnemyIntent
    {
        public MoveType MoveType;
        public int MovePower;
        public int QueueInstances = 1;
    }

    public enum MoveType
    {
        Attack,
        Defend
    }
}
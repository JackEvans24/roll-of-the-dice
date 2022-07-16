using System.Collections.Generic;
using UnityEngine;

namespace RollOfTheDice.Services
{
    public class DiceService
    {
        public int RollDie() => Random.Range(1, 7);
        
        public int[] RollDice(int diceCount)
        {
            var results = new int[diceCount];
            for (var i = 0; i < diceCount; i++)
                results[i] = RollDie();
            return results;
        }
    }
}
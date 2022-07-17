using System;
using UnityEngine;

namespace RollOfTheDice.Models
{
    [Serializable]
    public class Player : IPerishable
    {
        public bool Dead => Health <= 0;
        public int Health { get; private set; }
        public Action OnDeath { get; }
        
        public int MaxHealth;
        public int DiceCount;

        public void Initialise()
        {
            Health = MaxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            if (Dead)
                return;
            
            Health = Mathf.Max(Health - damage, 0);
            if (Dead)
                OnDeath?.Invoke();
        }
    }
}
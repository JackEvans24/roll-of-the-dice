using System;
using UnityEngine;

namespace RollOfTheDice.Models
{
    public class UnitWithHealth : IPerishable
    {
        public bool Dead => Health <= 0;
        public int Health { get; private set; }
        public int Shield { get; private set; }
        public Action OnDeath { get; }
        
        public int MaxHealth;

        public virtual void Initialise()
        {
            Health = MaxHealth;
        }

        public void AddShield(int shield)
        {
            Shield += shield;
        }
        
        public void TakeDamage(int damage)
        {
            if (Dead)
                return;

            var remainingDamage = damage - Shield;
            if (remainingDamage < 0)
            {
                Shield -= damage;
                return;
            }

            Shield = 0;
            Health = Mathf.Max(Health - remainingDamage, 0);
            if (Dead)
                OnDeath?.Invoke();
        }
    }
}
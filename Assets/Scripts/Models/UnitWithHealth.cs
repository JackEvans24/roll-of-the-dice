using System;
using UnityEngine;

namespace RollOfTheDice.Models
{
    public class UnitWithHealth : ScriptableObject, IPerishable
    {
        public bool Dead => Health <= 0;
        public int Health { get; private set; }
        public int Shield { get; private set; }
        
        public int MaxHealth;

        public virtual void Initialise(float multiplier = 1f)
        {
            Health = MaxHealth;
            Shield = 0;
        }

        public void AddShield(int shield)
        {
            Shield += shield;
        }

        public void ResetShield() => Shield = 0;
        
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
        }
    }
}
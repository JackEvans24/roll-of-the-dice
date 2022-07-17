using System;

namespace RollOfTheDice.Models
{
    public interface IPerishable
    {
        int Health { get; }
        int Shield { get; }
        bool Dead { get; }

        void Initialise();
        void TakeDamage(int damage);
    }
}
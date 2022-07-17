using System.Collections.Generic;
using RollOfTheDice.Extensions;
using UnityEngine;

namespace RollOfTheDice.Models
{
    [CreateAssetMenu(menuName = "Game/Enemy")]
    public class Enemy : UnitWithHealth
    {
        public EnemyIntent[] Intents;

        private Queue<EnemyIntent> _intentQueue = new Queue<EnemyIntent>();

        public override void Initialise()
        {
            base.Initialise();
            PopulateIntentQueue();
        }

        public EnemyIntent PeekNextIntent() => _intentQueue.Peek();

        public EnemyIntent GetNextIntent()
        {
            var intent = _intentQueue.Dequeue();
            if (_intentQueue.Count <= 0)
                PopulateIntentQueue();
            return intent;
        }

        private void PopulateIntentQueue()
        {
            _intentQueue = Intents.GetRandomisedQueue(intent => intent.QueueInstances);
        }
    }
}
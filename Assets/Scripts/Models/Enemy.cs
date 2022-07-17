using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace RollOfTheDice.Models
{
    [Serializable]
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
            var randomisedList = new List<EnemyIntent>();
            foreach (var intent in Intents)
            {
                for (var i = 0; i < intent.QueueInstances; i++)
                    randomisedList.Add(intent);
            }
            
            for (var i = 0; i < randomisedList.Count; i++)
            {
                var randomIndex = Random.Range(0, randomisedList.Count);
                (randomisedList[i], randomisedList[randomIndex]) = (randomisedList[randomIndex], randomisedList[i]);
            }

            foreach (var intent in randomisedList)
                _intentQueue.Enqueue(intent);
        }
    }
}
using System.Collections.Generic;
using RollOfTheDice.Extensions;
using UnityEngine;

namespace RollOfTheDice.Models
{
    [CreateAssetMenu(menuName = "Game/Enemy")]
    public class Enemy : UnitWithHealth
    {
        public GameObject Sprite;
        public EnemyIntent[] Intents;

        private float _multiplier;

        private Queue<EnemyIntent> _intentQueue = new Queue<EnemyIntent>();

        public override void Initialise(float multiplier)
        {
            base.Initialise(multiplier);
            _multiplier = multiplier;
            
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
            var tempIntentList = new List<EnemyIntent>();
            foreach (var intent in Intents)
            {
                var movePower = intent.MoveType == MoveType.Attack ? Mathf.RoundToInt(intent.MovePower * _multiplier) : intent.MovePower;
                tempIntentList.Add(new EnemyIntent()
                {
                    MovePower = movePower,
                    MoveType = intent.MoveType,
                    QueueInstances = intent.QueueInstances
                });
            }
            _intentQueue = tempIntentList.GetRandomisedQueue(intent => intent.QueueInstances);
        }
    }
}
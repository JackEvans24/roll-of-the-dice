using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace RollOfTheDice.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Queue<T> GetRandomisedQueue<T>(this IEnumerable<T> list)
            => GetRandomisedQueue(list, (item) => 1);

        public static Queue<T> GetRandomisedQueue<T>(this IEnumerable<T> list, Func<T, int> GetInstanceCount)
        {
            var randomisedList = new List<T>();
            foreach (var item in list)
            {
                for (var i = 0; i < GetInstanceCount(item); i++)
                    randomisedList.Add(item);
            }
            
            for (var i = 0; i < randomisedList.Count; i++)
            {
                var randomIndex = Random.Range(0, randomisedList.Count);
                (randomisedList[i], randomisedList[randomIndex]) = (randomisedList[randomIndex], randomisedList[i]);
            }

            var queue = new Queue<T>();
            foreach (var item in randomisedList)
                queue.Enqueue(item);

            return queue;
        }
    }
}
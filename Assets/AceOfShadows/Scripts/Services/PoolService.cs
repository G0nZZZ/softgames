using System.Collections.Generic;
using UnityEngine;

namespace AceOfShadows
{
    public class PoolService : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int initialSize = 145;

        private Queue<GameObject> pool = new Queue<GameObject>();

        void Awake()
        {
            ServiceLocator.Register(this);

            for (int i = 0; i < initialSize; i++)
            {
                var go = Instantiate(cardPrefab, transform);
                go.SetActive(false);
                pool.Enqueue(go);
            }
        }

        public GameObject Get()
        {
            if (pool.Count == 0)
            {
                var extra = Instantiate(cardPrefab, transform);
                extra.SetActive(false);
                pool.Enqueue(extra);
            }
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}

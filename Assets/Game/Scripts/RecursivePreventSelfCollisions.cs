using UnityEngine;

namespace Game
{
    public class RecursivePreventSelfCollisions : MonoBehaviour
    {
        [SerializeField] private bool _applyToBaseObject;
        
        private void Awake()
        {
            if (_applyToBaseObject)
            {
                var collider1 = GetComponent<Collider>();
                if (collider1 != null)
                {
                    foreach (Transform child2 in transform)
                    {
                        var collider2 = child2.GetComponent<Collider>();
                        if (collider2 == null)
                            continue;

                        Physics.IgnoreCollision(collider1, collider2);
                    }
                }
            }

            foreach (Transform child1 in transform)
            {
                var collider1 = child1.GetComponent<Collider>();
                if (collider1 == null)
                    continue;
                foreach (Transform child2 in transform)
                {
                    var collider2 = child2.GetComponent<Collider>();
                    if (collider2 == null)
                        continue;
                    Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }
    }
}
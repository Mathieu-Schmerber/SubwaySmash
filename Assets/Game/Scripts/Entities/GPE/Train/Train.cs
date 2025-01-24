using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class Train : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable == null) return;
            var dir = (transform.forward + Vector3.up * 0.5f).normalized;
            killable.Kill(dir,50);
        }
    }
}
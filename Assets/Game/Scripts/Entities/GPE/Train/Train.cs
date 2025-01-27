using Game.Systems.Score;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class Train : MonoBehaviour
    {
        private ScoreEntryIdentifier _score;

        private void Awake()
        {
            _score = GetComponent<ScoreEntryIdentifier>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable == null) return;
            var dir = (transform.forward + Vector3.up * 0.5f).normalized;
            killable.Kill(dir,50);
            if (!other.CompareTag("Player"))
                _score.OnTrigger();
        }
    }
}
using FMODUnity;
using Game.Systems.Audio;
using Game.Systems.Score;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class Train : MonoBehaviour
    {
        [SerializeField] private EventReference _killAudio;
        private MMF_Player _feedback;
        private ScoreEntryIdentifier _score;

        private void Awake()
        {
            _score = GetComponent<ScoreEntryIdentifier>();
            _feedback = GetComponent<MMF_Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var killable = other.GetComponent<IKillable>();
            if (killable == null) 
                return;
            
            var dir = (transform.forward + Vector3.up * 0.5f).normalized;
            killable.Kill(dir,50);
            _feedback.PlayFeedbacks();
            AudioManager.PlayOneShot(_killAudio);
            if (!other.CompareTag("Player"))
                _score.OnTrigger();
        }
    }
}
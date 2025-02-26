using FMODUnity;
using Game.Systems.Audio;
using Game.Systems.Kill;
using LemonInc.Core.Utilities.Extensions;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.GPE.Train
{
    public class Train : MonoBehaviour
    {
        [SerializeField] private EventReference _killAudio;
        private MMF_Player _feedback;

        private void Awake()
        {
            _feedback = GetComponent<MMF_Player>();
        }

        private void Update()
        {
            GetComponent<Rigidbody>().linearVelocity = GetComponent<Rigidbody>().linearVelocity.WithY(0);
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
        }
    }
}
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class StunEffect : MonoBehaviour
    {
        private ParticleSystem _ps;

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            _ps.Play();
        }
    }
}
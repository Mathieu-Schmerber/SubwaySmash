using Game.Inputs;
using UnityEngine;

namespace Game.Entities.Player
{
    public class AimIndicator : MonoBehaviour
    {
        private IInputProvider _input;

        private void Awake()
        {
            _input = GetComponentInParent<IInputProvider>();
        }

        private void Update()
        {
            var rot = Quaternion.LookRotation(_input.AimDirection).eulerAngles;
            transform.rotation = Quaternion.Euler(0, rot.y, 0);
        }
    }
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Utilities
{
    public class LockToTransform : MonoBehaviour
    {
        [Serializable, InlineProperty]
        public struct Locked
        {
            [HorizontalGroup, LabelWidth(15)] public bool X;
            [HorizontalGroup, LabelWidth(15)] public bool Y;
            [HorizontalGroup, LabelWidth(15)] public bool Z;
        }

        [SerializeField] private Transform _follow;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Locked _position;
        [SerializeField] private Locked _rotation;

        private void LateUpdate()
        {
            var newPosition = transform.position;
            if (!_position.X) newPosition.x = _follow.position.x;
            if (!_position.Y) newPosition.y = _follow.position.y;
            if (!_position.Z) newPosition.z = _follow.position.z;
            transform.position = newPosition + _offset;

            var newRotation = transform.eulerAngles;
            if (!_rotation.X) newRotation.x = _follow.eulerAngles.x;
            if (!_rotation.Y) newRotation.y = _follow.eulerAngles.y;
            if (!_rotation.Z) newRotation.z = _follow.eulerAngles.z;
            transform.eulerAngles = newRotation;
        }
    }
}
using System;
using System.Collections;
using Databases;
using Game.Systems.Push;
using UnityEngine;

namespace Game.Entities.Player.Abilities
{
    public class PushAbility : AbilityBase
    {
        [SerializeField] private float _sphereCheckRadius;
        [SerializeField] private Vector3 _sphereCheckOffset;
        
        private IInputProvider _input;

        private void Awake()
        {
            _input = GetComponent<IInputProvider>();
        }

        private void Start()
        {
            SetCooldown(RuntimeDatabase.Data.PlayerData.PushCooldown);
        }

        private void OnEnable()
        {
            _input.Push.OnPressed += OnPushPressed;
        }
        
        private void OnDisable()
        {
            _input.Push.OnPressed -= OnPushPressed;
        }

        private void OnPushPressed()
        {
            if (IsReady())
                Perform(() => {});
        }

        protected override IEnumerator OnPerform(Action performed)
        {
            Push();
            performed?.Invoke();
            yield return null;
        }

        private void Push()
        {
            var aim = _input.AimDirection;
            var front = transform.position + aim * _sphereCheckRadius + _sphereCheckOffset;
            var force = RuntimeDatabase.Data.PlayerData.PushForce;
            
            // Find all colliders within the sphere
            var hitColliders = Physics.OverlapSphere(front, _sphereCheckRadius);

            foreach (var coll in hitColliders)
            {
                var pushable = coll.GetComponent<Pushable>();
                if (pushable == null) continue;

                pushable.ApplyPush(aim, force);
            }
        }

        private void OnDrawGizmos()
        {
            var aim = Application.isPlaying ? _input.AimDirection : transform.forward;
            var front = transform.position + aim * _sphereCheckRadius + _sphereCheckOffset;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(front, _sphereCheckRadius);
        }
    }
}
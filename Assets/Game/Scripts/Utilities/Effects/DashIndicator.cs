using Databases;
using Game.Systems.Inputs;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game.Utilities.Effects
{
    public class DashIndicator : MonoBehaviour
    {
        private IInputProvider _input;
        private Transform _player;

        private float _dashDistance;
        
        private void Awake()
        {
            _input = GetComponentInParent<IInputProvider>();
            _player = transform.parent;
        }

        private void Start()
        {
            _dashDistance = RuntimeDatabase.Data.PlayerData.DashDistance;
        }

        private void Update()
        {
            var dir = _input.AimDirection;
            var target = _player.position + dir * _dashDistance;
            transform.position = target.WithY(0.5f);
        }
    }
}

using Databases;
using Game.Entities.Player;
using Game.Inputs;
using LemonInc.Core.Utilities.Extensions;
using UnityEngine;

namespace Game
{
    public class DashIndicatorFx : MonoBehaviour
    {
        private IInputProvider _input;
        private Transform _player;

        private void Awake()
        {
            _input = GetComponentInParent<IInputProvider>();
            _player = transform.parent;
        }

        private void Update()
        {
            var dir = _input.AimDirection;
            var target = _player.position + dir * RuntimeDatabase.Data.PlayerData.DashDistance;
            transform.position = target.WithY(0.5f);
        }
    }
}

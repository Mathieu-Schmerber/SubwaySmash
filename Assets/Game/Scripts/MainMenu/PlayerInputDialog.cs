using Game.Entities.Player;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.MainMenu
{
    public class PlayerInputDialog : MonoBehaviour
    {
        [SerializeField] private float _idleTime = 2;
        private float _idleTimer;

        [SerializeField] private MMF_Player _open;
        [SerializeField] private MMF_Player _close;
        
        private PlayerInputProvider _input;
        private bool _isDialogVisible = true;

        private void Awake()
        {
            _input = GetComponentInParent<PlayerInputProvider>();
        }

        private void ToggleDialog(bool show)
        {
            _isDialogVisible = show;
            if (show)
                _open.PlayFeedbacks();
            else
                _close.PlayFeedbacks();
        }
        
        private void Update()
        {
            var isMoving = _input.MovementDirection.magnitude > 0;
            if (isMoving)
            {
                _idleTimer = 0f;
                if (_isDialogVisible)
                    ToggleDialog(false);
            }
            else
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer >= _idleTime && !_isDialogVisible)
                    ToggleDialog(true);
            }
        }
    }
}
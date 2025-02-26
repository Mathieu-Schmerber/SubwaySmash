using Game.Entities.Player;
using LemonInc.Core.Utilities;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Ui
{
    public class PlayerInputDialog : MonoBehaviour
    {
        [SerializeField] private float _idleTime = 2;
        private float _idleTimer;

        [SerializeField] private Transform _moveInstruction;
        [SerializeField] private Transform _menuInstruction;
        
        [SerializeField] private MMF_Player _open;
        [SerializeField] private MMF_Player _close;
        
        private PlayerInputProvider _input;
        private bool _isDialogVisible = true;
        private bool _menuNavOpen = false;

        private void Awake()
        {
            _input = GetComponentInParent<PlayerInputProvider>();
        }

        private void Start()
        {
            _menuInstruction.gameObject.SetActive(false);
            _moveInstruction.gameObject.SetActive(true);
        }

        public void ShowMenuNav(bool show)
        {
            _menuNavOpen = show;
            if (show)
            {
                _menuInstruction.gameObject.SetActive(true);
                _open.PlayFeedbacks();
            }
            else
            {
                _close.PlayFeedbacks();
                Awaiter.WaitAndExecute(0.2f, () => _menuInstruction.gameObject.SetActive(false));
            }
        }

        private void ToggleDialog(bool show)
        {
            _moveInstruction.gameObject.SetActive(show);
            _isDialogVisible = show;
            if (show)
            {
                _open.PlayFeedbacks();
            }
            else
                _close.PlayFeedbacks();
        }
        
        private void Update()
        {
            if (_menuNavOpen)
                return;
            
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
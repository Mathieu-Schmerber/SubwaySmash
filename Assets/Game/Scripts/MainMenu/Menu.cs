using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.MainMenu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private bool _startOpen;
        [SerializeField] private MMF_Player _open;
        [SerializeField] private MMF_Player _close;

        [ShowInInspector, ReadOnly] public bool IsOpen { get; private set; }

        private PhysicalButton[] _buttons;

        private void Awake()
        {
            _buttons = GetComponentsInChildren<PhysicalButton>();
        }

        private void Start()
        {
            if (_startOpen)
            {
                GetComponent<CanvasGroup>().alpha = 1;
                IsOpen = true;
                SetInteractable();
            }
        }

        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            _open.PlayFeedbacks();
            Invoke(nameof(SetInteractable), _open.TotalDuration);
        }

        private void SetInteractable()
        {
            foreach (var button in _buttons)
            {
                button.Interactable = true;
                button.IsSelected = false;
            }
        }

        public void Close()
        {
            if (!IsOpen) return;
            
            foreach (var button in _buttons)
            {
                button.Interactable = false;
                button.IsSelected = false;
            }
            
            IsOpen = false;
            _close.PlayFeedbacks();
        }

        [Button("Open")]
        public void EditorOpen()
        {
            GetComponent<CanvasGroup>().alpha = 1;
        }
        
        [Button("Close")]
        public void EditorClose()
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
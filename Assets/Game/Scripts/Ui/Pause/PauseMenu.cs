using Game.Systems;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Ui.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private MMF_Player _openFeedback;
        [SerializeField] private MMF_Player _closeFeedback;

        private bool _paused = false;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            Core.MenuInput.PauseGame.OnPressed += PauseGame;
        }

        private void OnDisable()
        {
            Core.MenuInput.PauseGame.OnPressed -= PauseGame;
        }
        
        private void PauseGame()
        {
            if (_paused)
            {
                ResumeGame();
            }
            else
            {
                _paused = true;
                _canvasGroup.interactable = true;
                _openFeedback.PlayFeedbacks();
            }
        }

        public void ResumeGame()
        {
            _paused = false;
            _canvasGroup.interactable = false;
            _closeFeedback.PlayFeedbacks();
        }

        public void ExitGame()
        {
            Core.Instance.LoadStageByName(Core.Stages.MainMenu);
            _canvasGroup.interactable = false;
            _closeFeedback.PlayFeedbacks();
        }
        
        #if UNITY_EDITOR

        [Button]
        private void Open()
        {
            var group = GetComponent<CanvasGroup>();
            group.alpha = 1;
            group.interactable = true;
        }

        [Button]
        private void Close()
        {
            var group = GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable = false;
        }
        
        #endif
    }
}
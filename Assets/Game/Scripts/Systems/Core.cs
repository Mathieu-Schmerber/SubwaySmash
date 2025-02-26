using System.Collections;
using Game.Entities.Player;
using Game.Systems.Alert;
using Game.Systems.Audio;
using Game.Systems.Stage;
using Game.Systems.Waypoint;
using Game.Ui;
using Game.Ui.Notifications;
using Game.Ui.Shared;
using LemonInc.Core.Pooling;
using LemonInc.Core.Pooling.Contracts;
using LemonInc.Core.Pooling.Providers;
using LemonInc.Core.Utilities;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems
{
    /// <summary>
    /// References the game systems.
    /// </summary>
    /// <seealso cref="Core" />
    [DefaultExecutionOrder(1)]
    public class Core : ManagerSingleton<Core>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _player;
        [SerializeField] private StageData _stages;
        [SerializeField] private bool _assertSystems = true;
        [SerializeField] private MMF_Player _closeSceneFeedback;
        [SerializeField] private MMF_Player _openSceneFeedback;
        [SerializeField] private bool _lockAlert;
        
        private Exit[] _levelExists;
        private AsyncOperation _sceneLoadOperation;
        
        private IPoolProvider<string> _poolProvider;
        private AlertSystem _alertSystem;
        private MenuInputProvider _menuInput;
        private NotificationManagerUi _notificationManager;
        private LevelClearCondition _levelClearCondition;

        /// <summary>
        /// Pooling access.
        /// </summary>
        public static class Pooling
        {
            /// <summary>
            /// Returns a pool.
            /// </summary>
            /// <param name="pool">The pool.</param>
            public static IPool From(Pool pool) => From(pool.ToString());

            /// <summary>
            /// Returns a pool.
            /// </summary>
            /// <param name="pool">The pool.</param>
            public static IPool From(string pool) => Instance._poolProvider.Get(pool);
        }

        // Public singleton access
        public static AlertSystem AlertSystem => Instance._alertSystem ??= Instance.Fetch<AlertSystem>();
        public static Camera Camera => Instance._camera ??= FindFirstObjectByType<Camera>();
        public static Transform CameraRig => Camera.transform.parent.parent;
        public static MenuInputProvider MenuInput => Instance._menuInput ??= Instance.Fetch<MenuInputProvider>();
        public static NotificationManagerUi NotificationManager => Instance._notificationManager ??= Instance.Fetch<NotificationManagerUi>();
        public static Exit[] LevelExists => Instance._levelExists;
        public static StageData Stages => Instance._stages;
        public static Transform Player => Instance._player;
        public static LevelClearCondition LevelClearCondition => Instance._levelClearCondition;
        
        #if UNITY_EDITOR
        
        /// <summary>
        /// Editor quality of life, binds default fields.
        /// </summary>
        [Button]
        private void BindPlayerAndCamera()
        {
            _camera = FindFirstObjectByType<Camera>();
            _player = FindFirstObjectByType<PlayerStateMachine>().transform;
        }
        #endif
        
        private void Awake()
        {
            _camera = FindFirstObjectByType<Camera>();
            _poolProvider = Fetch<NamedObjectPoolProvider>();
            _levelExists = FindObjectsByType<Exit>(FindObjectsSortMode.None);
            _menuInput = Fetch<MenuInputProvider>();
            _notificationManager = Fetch<NotificationManagerUi>();
            _levelClearCondition = Fetch<LevelClearCondition>();
        }

        /// <summary>
        /// Fetches the provided <see cref="Component"/> from core's children.
        /// </summary>
        /// <typeparam name="T">The component to fetch.</typeparam>
        /// <returns>The fetched <see cref="Component"/>.</returns>
        /// <exception cref="MissingComponentException">If the component does not exist.</exception>
        private T Fetch<T>() where T : Component
        {
            var result = GetComponentInChildren<T>();
            if (result == null && _assertSystems)
                throw new MissingComponentException(typeof(T).Name);
            return result;
        }

        private void OnEnable()
        {
            _menuInput.RestartStage.OnPressed += RestartLevel;
        }

        private void OnDisable()
        {
            _menuInput.RestartStage.OnPressed -= RestartLevel;
        }

        private void Start()
        {
            _openSceneFeedback.PlayFeedbacks();
            Core.AlertSystem?.LockAlert(_lockAlert);
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void RestartLevel() => ResetLevel();
        
        private void ResetLevel()
        {
            if (_openSceneFeedback.IsPlaying || _closeSceneFeedback.IsPlaying)
                return;
            
            AudioManager.Instance.StopAllSFX();
            var targetScene = SceneManager.GetActiveScene().name;
            _sceneLoadOperation = SceneManager.LoadSceneAsync(targetScene);
    
            // Don't allow the scene to activate immediately
            _sceneLoadOperation.allowSceneActivation = false;

            StartCoroutine(WaitForFeedbackThenSwitchScene());
        }

        /// <summary>
        /// Loads a stage by its name.
        /// </summary>
        /// <param name="stageName">The stage name.</param>
        public void LoadStageByName(string stageName)
        {
            if (_openSceneFeedback.IsPlaying || _closeSceneFeedback.IsPlaying)
                return;
            
            AudioManager.Instance.StopAllSFX();
            _sceneLoadOperation = _stages.GetStage(stageName);
    
            // Don't allow the scene to activate immediately
            _sceneLoadOperation.allowSceneActivation = false;

            StartCoroutine(WaitForFeedbackThenSwitchScene());
        }
        
        /// <summary>
        /// Loads the next stage.
        /// </summary>
        public void LoadNextStage()
        {
            AudioManager.Instance.StopAllSFX();
            _sceneLoadOperation = _stages.GetNextStage();
    
            // Don't allow the scene to activate immediately
            _sceneLoadOperation.allowSceneActivation = false;

            StartCoroutine(WaitForFeedbackThenSwitchScene());
        }
        
        /// <summary>
        /// Waits for the end of the scene transition before switching scene.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/>.</returns>
        private IEnumerator WaitForFeedbackThenSwitchScene()
        {
            yield return _closeSceneFeedback.PlayFeedbacksCoroutine(Vector3.zero);

            while (!_sceneLoadOperation.isDone)
            {
                if (_sceneLoadOperation.progress >= 0.9f)
                    _sceneLoadOperation.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}
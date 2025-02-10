using System.Collections;
using Game.Entities.Player;
using Game.MainMenu;
using Game.Systems.Alert;
using Game.Systems.Audio;
using Game.Systems.Stage;
using Game.Systems.Waypoint;
using LemonInc.Core.Pooling;
using LemonInc.Core.Pooling.Contracts;
using LemonInc.Core.Pooling.Providers;
using LemonInc.Core.Utilities;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// References the game systems.
    /// </summary>
    /// <seealso cref="Core" />
    [DefaultExecutionOrder(1)]
    public class Core : ManagerSingleton<Core>
    {
        [SerializeField]private Camera _camera;
        
        private IPoolProvider<string> _poolProvider;
        private AlertSystem _alertSystem;
        private MenuInputProvider _menuInput;
        [HideInInspector] public Exit[] _levelExists;

        [SerializeField] private Transform _player;
        [SerializeField] private StageData _stages;
        [SerializeField] private bool _assertSystems = true;
        [SerializeField] private MMF_Player _closeSceneFeedback;
        [SerializeField] private MMF_Player _openSceneFeedback;
        [SerializeField] private bool _lockAlert;
        private AsyncOperation _sceneLoadOperation;
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

        private T Fetch<T>()
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

            StartCoroutine(WaitForFeedbackAndSwitchScene());
        }

        public void LoadStageByName(string stageName)
        {
            if (_openSceneFeedback.IsPlaying || _closeSceneFeedback.IsPlaying)
                return;
            
            AudioManager.Instance.StopAllSFX();
            _sceneLoadOperation = _stages.GetStage(stageName);
    
            // Don't allow the scene to activate immediately
            _sceneLoadOperation.allowSceneActivation = false;

            StartCoroutine(WaitForFeedbackAndSwitchScene());
        }
        
        public void LoadNextStage()
        {
            AudioManager.Instance.StopAllSFX();
            _sceneLoadOperation = _stages.GetNextStage();
    
            // Don't allow the scene to activate immediately
            _sceneLoadOperation.allowSceneActivation = false;

            StartCoroutine(WaitForFeedbackAndSwitchScene());
        }
        
        private IEnumerator WaitForFeedbackAndSwitchScene()
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
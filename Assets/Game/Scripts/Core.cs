using Game.Entities.Player;
using Game.Systems.Alert;
using Game.Systems.Score;
using Game.Systems.Waypoint;
using LemonInc.Core.Pooling;
using LemonInc.Core.Pooling.Contracts;
using LemonInc.Core.Pooling.Providers;
using LemonInc.Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    /// <summary>
    /// References the game systems.
    /// </summary>
    /// <seealso cref="LemonInc.Core.Utilities.ManagerSingleton&lt;Game.Core&gt;" />
    [DefaultExecutionOrder(1)]
    public class Core : ManagerSingleton<Core>
    {
        private Camera _camera;
        
        private IPoolProvider<string> _poolProvider;
        private ScoreSystem _scoreSystem;
        private AlertSystem _alertSystem;
        private Exit[] _levelExists;

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

        public static ScoreSystem ScoreSystem => Instance._scoreSystem ??= Instance.Fetch<ScoreSystem>();
        public static AlertSystem AlertSystem => Instance._alertSystem ??= Instance.Fetch<AlertSystem>();
        public static Camera Camera => Instance._camera;
        public static Exit[] LevelExists => Instance._levelExists;
        
        private void Awake()
        {
            _camera = FindFirstObjectByType<Camera>();
            _scoreSystem = Fetch<ScoreSystem>();
            _poolProvider = Fetch<NamedObjectPoolProvider>();
            _levelExists = FindObjectsByType<Exit>(FindObjectsSortMode.None);
        }

        private T Fetch<T>() => GetComponentInChildren<T>() ?? throw new MissingComponentException(typeof(T).Name);

        private void OnEnable()
        {
            PlayerStateMachine.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            PlayerStateMachine.OnPlayerDeath -= OnPlayerDeath;
        }
        
        private void OnPlayerDeath(Transform player)
        {
            Invoke(nameof(ResetLevel), 3f);
        }

        private void ResetLevel()
        {
            var targetScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(targetScene);
        }
    }
}
using Game.Entities.Player;
using Game.Systems.Score;
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
    public class Core : ManagerSingleton<Core>
    {
        private IPoolProvider<string> _poolProvider;
        private ScoreSystem _scoreSystem;

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
            public static IPool From(string pool) => Core.Instance._poolProvider.Get(pool);
        }

        public static ScoreSystem ScoreSystem => Instance._scoreSystem ??= Instance.GetComponentInChildren<ScoreSystem>() ?? throw new MissingComponentException("ScoreSystem");

        private void Awake()
        {
            _scoreSystem = GetComponentInChildren<ScoreSystem>() ?? throw new MissingComponentException("ScoreSystem");
            _poolProvider = GetComponentInChildren<NamedObjectPoolProvider>();
        }

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
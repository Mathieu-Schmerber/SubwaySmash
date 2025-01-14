using LemonInc.Core.Pooling;
using LemonInc.Core.Pooling.Contracts;
using LemonInc.Core.Pooling.Providers;
using LemonInc.Core.Utilities;

namespace Game
{
    /// <summary>
    /// References the game systems.
    /// </summary>
    /// <seealso cref="LemonInc.Core.Utilities.ManagerSingleton&lt;Game.Core&gt;" />
    public class Core : ManagerSingleton<Core>
    {
        private IPoolProvider<string> _poolProvider;

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

        private void Awake()
        {
            _poolProvider = GetComponentInChildren<NamedObjectPoolProvider>();
        }
    }
}
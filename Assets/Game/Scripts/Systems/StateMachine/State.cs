namespace Game.Systems.StateMachine
{
    /// <summary>
    /// Describes a state.
    /// </summary>
    /// <typeparam name="TPayload">The shared data between states.</typeparam>
    public abstract class State<TPayload>
        where TPayload : struct
    {
        /// <summary>
        /// The state machine which this state belongs to.
        /// </summary>
        public StateMachine<TPayload> StateMachine { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>
        /// The payload.
        /// </value>
        public TPayload Payload => StateMachine.Payload;

        /// <summary>
        /// Initializes the specified state machine.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stateMachine">The state machine.</param>
        /// <returns></returns>
        public static T Init<T>(StateMachine<TPayload> stateMachine)
            where T : State<TPayload>, new()
        {
            var state = new T();
            state.Init(stateMachine);
            state.Awake();
            return state;
        }

        /// <summary>
        /// Initializes the specified state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine.</param>
        private void Init(StateMachine<TPayload> stateMachine)
        {
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Awakes the state.
        /// </summary>
        public virtual void Awake() { }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public virtual void Enter() {}

        /// <summary>
        /// Called every frame if the state is active.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public virtual void Exit() { }
    }
}
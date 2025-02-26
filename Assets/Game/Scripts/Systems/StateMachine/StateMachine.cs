using UnityEngine;

namespace Game.Systems.StateMachine
{
    /// <summary>
    /// Defines a state machine.
    /// </summary>
    /// <typeparam name="TPayload">The shared data between states.</typeparam>
    public class StateMachine<TPayload>
        where TPayload : struct
    {
        /// <summary>
        /// Locked state.
        /// </summary>
        public State<TPayload> LockedState { get; private set; }
        
        /// <summary>
        /// The current state.
        /// </summary>
        public State<TPayload> CurrentState { get; private set; }

        /// <summary>
        /// The state machine owner.
        /// </summary>
        public Transform Owner { get; private set; }

        /// <summary>
        /// The shared data payload.
        /// </summary>
        public TPayload Payload { get; private set; }

        /// <summary>
        /// Switches state.
        /// </summary>
        /// <param name="state">state.</param>
        public void SwitchState(State<TPayload> state)
        {
            if (LockedState != null)
                return;
            
            CurrentState?.Exit();
            state.Enter();
            CurrentState = state;
        }

        public void LockState(State<TPayload> state)
        {
            if (state == null) return;
            
            SwitchState(state);
            LockedState = state;
        }

        public void UnlockState()
        {
            LockedState = null;
        }

        /// <summary>
        /// Updates the state machine.
        /// </summary>
        public void Update() => CurrentState?.Update();

        /// <summary>
        /// Sets the ownership.
        /// </summary>
        /// <param name="transform">The transform.</param>
        public void SetOwnership(Transform transform)
        {
            Owner = transform;
        }

        /// <summary>
        /// Sets the payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public void SetPayload(TPayload payload)
        {
            Payload = payload;
        }
    }
}
using UnityEngine;

namespace Game.MainMenu
{
    public abstract class AudioIncrementUi : MonoBehaviour
    {
        public bool Active { get; private set; }

        public void Increment()
        {
            if (!Active)
                IncrementAnimation();
            Active = true;
        } 
        
        public void Decrement()
        {
            if (Active)
                DecrementAnimation();
            Active = false;
        }

        protected abstract void IncrementAnimation();
        protected abstract void DecrementAnimation();
    }
}
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainMenu
{
    public class AudioIncrementUi : MonoBehaviour
    {
        private Image _image;

        public bool Active { get; private set; }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Increment()
        {
            if (!Active)
                Tween.Value(0.15f, 0, value => _image.pixelsPerUnitMultiplier = value, .1f, 0, Tween.EaseOut);
            Active = true;
        } 
        
        public void Decrement()
        {
            if (Active)
                Tween.Value(0, 0.15f, value => _image.pixelsPerUnitMultiplier = value, .1f, 0, Tween.EaseOut);
            Active = false;
        } 
    }
}
using Game.Ui.Shared;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.Pause
{
    public class PauseMenuAudioIncrementUi : AudioIncrementUi
    {
        [SerializeField] private Color _decrementColor;
        [SerializeField] private Color _incrementColor;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        
        protected override void IncrementAnimation()
        {
            Tween.Color(_image, _incrementColor, .15f, 0f, Tween.EaseOut, obeyTimescale: false);
        }

        protected override void DecrementAnimation()
        {
            Tween.Color(_image, _decrementColor, .15f, 0f, Tween.EaseOut, obeyTimescale: false);
        }
    }
}
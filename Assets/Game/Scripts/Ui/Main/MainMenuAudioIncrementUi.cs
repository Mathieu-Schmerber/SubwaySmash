using Game.Ui.Shared;
using Pixelplacement;
using UnityEngine.UI;

namespace Game.Ui.Main
{
    public class MainMenuAudioIncrementUi : AudioIncrementUi
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        
        protected override void IncrementAnimation()
        {
            Tween.Value(0.15f, 0, value => _image.pixelsPerUnitMultiplier = value, .1f, 0, Tween.EaseOut);
        }

        protected override void DecrementAnimation()
        {
            Tween.Value(0, 0.15f, value => _image.pixelsPerUnitMultiplier = value, .1f, 0, Tween.EaseOut);
        }
    }
}
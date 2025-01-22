using UnityEngine;

namespace Game.Systems.Ui
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField, Range(0, 1)] private float _progress;

        public float Progress => _progress;
        
        public void SetProgress(float progress)
        {
            _progress = progress;
            _fill.localScale = new Vector3(_progress, 1, 1);
        }

        private void OnValidate()
        {
            SetProgress(_progress);
        }
    }
}
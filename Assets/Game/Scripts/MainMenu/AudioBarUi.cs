using System;
using System.Linq;
using Game.Systems.Audio;
using UnityEngine;

namespace Game.MainMenu
{
    public class AudioBarUi : MonoBehaviour
    {
        public enum AudioType
        {
            MASTER,
            MUSIC,
            SFX
        }
        
        [SerializeField] private AudioType _audioType;
        private AudioIncrementUi[] _audioIncrements;

        private void Awake()
        {
            _audioIncrements = GetComponentsInChildren<AudioIncrementUi>();
        }

        private void Start()
        {
            SetVolumeDisplay(GetVolume());
        }

        private float GetVolume()
        {
            switch (_audioType)
            {
                case AudioType.MASTER:
                    return AudioManager.GetMasterVolume();
                case AudioType.MUSIC:
                    return AudioManager.GetMusicVolume();
                case AudioType.SFX:
                    return AudioManager.GetSfxVolume();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetVolume(float volume)
        {
            switch (_audioType)
            {
                case AudioType.MASTER:
                    AudioManager.SetMasterVolume(volume);
                    break;
                case AudioType.MUSIC:
                    AudioManager.SetMusicVolume(volume);
                    break;
                case AudioType.SFX:
                    AudioManager.SetSfxVolume(volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void SetVolumeDisplay(float volume)
        {
            SetVolume(volume);
            var step = volume * _audioIncrements.Length; 
            for (var i = 0; i < _audioIncrements.Length; i++)
            {
                if (i < step)
                    _audioIncrements[i].Increment();
                else
                    _audioIncrements[i].Decrement();
            }
        }

        public void IncrementVolume()
        {
            var step = 1f / _audioIncrements.Length; 
            var increment = _audioIncrements.FirstOrDefault(x => !x.Active);
            
            var volume = GetVolume();
            SetVolume(volume + step);
            
            increment?.Increment();
        }

        public void DecrementVolume()
        {
            var step = 1f / _audioIncrements.Length; 
            var increment = _audioIncrements.LastOrDefault(x => x.Active);

            var volume = GetVolume();
            SetVolume(volume - step);
            increment?.Decrement();
        }
    }
}
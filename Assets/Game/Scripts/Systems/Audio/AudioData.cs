using System;
using System.Linq;
using FMODUnity;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Systems.Audio
{
    [Serializable]
    public class MaterialAudio
    {
        public EventReference FmodEvent;
        public float MinVolume = 0.1f;
        public float MaxVolume = .3f;
        public float MinForce = 1.0f;
        public float MaxForce = 10.0f;
    }
    
    [CreateAssetMenu(fileName = "New Audio Data", menuName = "Data/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [Serializable]
        public class MaterialAudioDictionary : SerializedDictionary<string, MaterialAudio> { }
        
        public MaterialAudioDictionary MaterialAudios = new();
        
#if UNITY_EDITOR
        public string[] GetKeys() => MaterialAudios.Keys.ToArray();
#endif
    }
}
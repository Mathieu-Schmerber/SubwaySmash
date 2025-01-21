using System;
using System.Collections.Generic;
using System.Linq;
using LemonInc.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Systems.Score
{
    [Serializable]
    public struct ScoreEntry
    {
        [HorizontalGroup("Push"), LabelWidth(100)] public bool OnPush;
        [HorizontalGroup("Push")] [ShowIf(nameof(OnPush))] public ScoreModifier PushScore;
        [HorizontalGroup("Trigger"), LabelWidth(100)] public bool OnTrigger;
        [HorizontalGroup("Trigger")] [ShowIf(nameof(OnTrigger))] public ScoreModifier TriggerScore;
        [HorizontalGroup("Kill"), LabelWidth(100)] public bool OnKill;
        [HorizontalGroup("Kill")] [ShowIf(nameof(OnKill))] public ScoreModifier KillScore;
    }
    
    [CreateAssetMenu(menuName = "Data/Score")]
    public class ScoreData : ScriptableObject
    {
        [Serializable]
        public class ScoreEntryDictionary : SerializedDictionary<string, ScoreEntry> { }

        public float ComboCooldown;
        public float BaseComboProgress;
        public float ComboProgressModifier;
        public ScoreEntryDictionary Entries = new();
        
#if UNITY_EDITOR
        public string[] GetKeys() => Entries.Keys.ToArray();
#endif
        
        public bool TryGet(string identifier, out ScoreEntry scoreEntry) => Entries.TryGetValue(identifier, out scoreEntry);
    }
}
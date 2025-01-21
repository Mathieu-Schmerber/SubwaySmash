using System;
using Sirenix.OdinInspector;

namespace Game.Systems.Score
{
    [Serializable, HideLabel]
    public struct ScoreModifier
    {
        [HorizontalGroup, LabelWidth(80)] public float Points;
    }
}
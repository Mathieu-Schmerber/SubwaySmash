using System;

namespace Game
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class SceneAttribute : Attribute { }
}
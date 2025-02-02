using System;
using System.Collections;
using System.IO;
using System.Linq;
using LemonInc.Core.Utilities.Editor.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems.Stage
{
    [Serializable, InlineProperty]
    public struct SerializedScene : IEquatable<SerializedScene>
    {
        [ValueDropdown(nameof(SelectScene), DropdownTitle = "Scene Selection"),HideLabel]
        public string Name;
        
        public static implicit operator string(SerializedScene serializedScene)
        {
            return serializedScene.Name;
        }
        
        private static IEnumerable SelectScene()
        {
            var filesPath = SceneHelper.GetAllBuiltScene();
            var fileNameList = filesPath
                .Select(x => x.Name)
                .Distinct()
                .ToList();

            return fileNameList;
        }

        public bool Equals(SerializedScene other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is SerializedScene other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
    
    [CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
    public class StageData : ScriptableObject
    {
        public SerializedScene MainMenu;
        public SerializedScene[] Stages;

        public AsyncOperation GetNextStage()
        {
            var currentScene = SceneManager.GetActiveScene();
            
            for (var i = 0; i < Stages.Length; i++)
            {
                var check = Stages[i];
                
                if (check != currentScene.name)
                    continue;
                var load = i + 1 < Stages.Length ? Stages[i + 1].Name : MainMenu.Name;
                return SceneManager.LoadSceneAsync(load);
            }
            
            return SceneManager.LoadSceneAsync(MainMenu);
        }

        public AsyncOperation GetStage(string stageName)
        {
            foreach (var check in Stages)
            {
                if (check != stageName)
                    continue;
                return SceneManager.LoadSceneAsync(check);
            }

            Debug.LogError($"Stage '{stageName}' not found");
            return SceneManager.LoadSceneAsync(MainMenu);
        }
    }
}
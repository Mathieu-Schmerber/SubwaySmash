using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems.Stage
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
    public class StageData : ScriptableObject
    {
        [Serializable, InlineProperty]
        public struct Stage
        {
            [Scene, HideLabel] public string Name;
            
            public static implicit operator string(Stage stage)
            {
                return stage.Name;
            }
        }
        
        [Scene] public string MainMenu;
        [Scene] public Stage[] Stages;

        public AsyncOperation GetNextStage()
        {
            var currentScene = SceneManager.GetActiveScene();
            
            for (var i = 0; i < Stages.Length; i++)
            {
                var check = Stages[i];
                
                if (check != currentScene.name)
                    continue;
                var load = i + 1 < Stages.Length ? Stages[i + 1] : MainMenu;
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

        public string[] GetAllStages() => Stages.Select(x => x.Name).ToArray();
    }
}
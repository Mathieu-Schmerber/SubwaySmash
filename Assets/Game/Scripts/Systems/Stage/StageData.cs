using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
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
            [ValueDropdown("GetAvailableScenes")]
            public string[] Levels;

            private static ValueDropdownList<string> GetAvailableScenes()
            {
                var sceneList = new ValueDropdownList<string>();
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    var scenePath = scene.path;
                    var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                    sceneList.Add(sceneName);
                }
                return sceneList;
            }
        }

        [Scene] public string MainMenu;
        [Scene] public Stage[] Stages;

        public AsyncOperation GetNextStage()
        {
            var currentScene = SceneManager.GetActiveScene().name;

            for (var stageIndex = 0; stageIndex < Stages.Length; stageIndex++)
            {
                var stage = Stages[stageIndex];
                for (var levelIndex = 0; levelIndex < stage.Levels.Length; levelIndex++)
                {
                    if (stage.Levels[levelIndex] != currentScene)
                        continue;

                    // Load the next level within the stage
                    if (levelIndex + 1 < stage.Levels.Length)
                        return SceneManager.LoadSceneAsync(stage.Levels[levelIndex + 1]);

                    // Load the first level of the next stage
                    if (stageIndex + 1 < Stages.Length)
                        return SceneManager.LoadSceneAsync(Stages[stageIndex + 1].Levels.First());

                    // No more levels, return to MainMenu
                    return SceneManager.LoadSceneAsync(MainMenu);
                }
            }

            return SceneManager.LoadSceneAsync(MainMenu);
        }

        public AsyncOperation GetStage(string stageName)
        {
            foreach (var stage in Stages)
            {
                var level = stage.Levels.FirstOrDefault(x => x == stageName);
                if (level == null)
                    continue; 
                return SceneManager.LoadSceneAsync(level);
            }

            Debug.LogError($"Stage '{stageName}' not found");
            return SceneManager.LoadSceneAsync(MainMenu);
        }

        public Stage[] GetAllStageEntries() => Stages;
    }
}

using UnityEditor;
using UnityEngine;

namespace Game.Ui.Main
{
    public class MainMenu : MonoBehaviour
    {
        public void Quit()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
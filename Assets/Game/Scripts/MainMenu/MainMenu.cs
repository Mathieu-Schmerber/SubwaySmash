#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game.MainMenu
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
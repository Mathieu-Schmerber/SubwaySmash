
#if UNITY_EDITOR
				using UnityEditor;
				using UnityEngine;
				namespace LemonInc.Generated {
			        internal static class PanelMenuItems
			        {
			            
		        [MenuItem("Tools/LemonInc/Panels/Game Design")]
		        public static void OpenGameDesign()
		        {
		            var window = UnityEditor.EditorWindow.CreateWindow<LemonInc.Tools.Panels.PanelEditorWindow>();
		            window.titleContent = new GUIContent("Game Design");
					window.Init("Game Design");
		            window.Show();
		            window.Focus();
		        }
		        
			        }
			    }
#endif
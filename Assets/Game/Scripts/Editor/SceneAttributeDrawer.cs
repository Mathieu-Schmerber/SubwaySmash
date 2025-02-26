using System.Collections.Generic;
using System.Linq;
using Game.Utilities.Attributes;
using LemonInc.Core.Utilities.Editor.Helpers;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;  // Import ReorderableList namespace

namespace Game.Editor
{
    public class SceneAttributeDrawer : OdinAttributeDrawer<SceneAttribute>
    {
        private List<string> sceneNames;
        private ReorderableList reorderableList;

        protected override void Initialize()
        {
            base.Initialize();
            sceneNames = SelectScene();
            InitializeReorderableList();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (sceneNames == null || sceneNames.Count == 0)
            {
                sceneNames = SelectScene();
                InitializeReorderableList(); // Reinitialize when scenes are empty
            }

            if (Property.ValueEntry.TypeOfValue == typeof(string))
            {
                DrawStringDropdown(label);
            }
            else if (Property.ValueEntry.TypeOfValue == typeof(string[]))
            {
                DrawReorderableList(label);
            }
            else
            {
                CallNextDrawer(label);
            }
        }

        private void DrawStringDropdown(GUIContent label)
        {
            var selectedScene = Property.ValueEntry.WeakSmartValue as string;
            Property.ValueEntry.WeakSmartValue = SirenixEditorFields.Dropdown(label, selectedScene, sceneNames);
        }

        private void DrawReorderableList(GUIContent label)
        {
            var selectedScenes = Property.ValueEntry.WeakSmartValue as string[] ?? new string[0];
            reorderableList.list = selectedScenes.ToList();

            reorderableList.DoList(GUILayoutUtility.GetRect(200, 100));  // Define size

            // Convert the list back to an array and assign it to the property value
            Property.ValueEntry.WeakSmartValue = ((List<string>)reorderableList.list).ToArray();
        }

        private void InitializeReorderableList()
        {
            reorderableList = new ReorderableList(sceneNames, typeof(string), true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Select Scenes");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    if (index >= sceneNames.Count) return;

                    // Draw the Popup for the scene name selection
                    var prev = index;
                    index = EditorGUI.Popup(rect, index, sceneNames.ToArray());

                    // Handle the key event for deletion (SUPPR key)
                    if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
                    {
                        if (index >= 0 && index < sceneNames.Count)
                        {
                            sceneNames.RemoveAt(index); // Remove item from sceneNames
                            reorderableList.list.RemoveAt(index); // Remove item from reorderableList
                            Property.ValueEntry.WeakSmartValue = sceneNames.ToArray(); // Update the WeakSmartValue
                            Event.current.Use(); // Consume the event so it doesn't propagate
                        }
                    }

                    // Ensure that the scene name is updated
                    sceneNames[prev] = sceneNames[index];
                },
                onAddCallback = (ReorderableList list) =>
                {
                    list.list.Add(sceneNames.FirstOrDefault() ?? ""); // Add a new empty scene string
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    // Remove the scene at the selected index from the list
                    int selectedIndex = list.index;
                    if (selectedIndex >= 0 && selectedIndex < sceneNames.Count)
                    {
                        list.list.RemoveAt(selectedIndex);  // Remove from reorderable list

                        /*// Update the main sceneNames list
                        var tmp = list.list.Cast<string>().ToList();

                        // Update the WeakSmartValue to reflect the change in the property
                        Property.ValueEntry.WeakSmartValue = tmp.ToArray();*/
                    }
                }
            };
        }

        private static List<string> SelectScene()
        {
            var filesPath = SceneHelper.GetAllBuiltScene();
            return filesPath.Select(x => x.Name).Distinct().ToList();
        }
    }
}

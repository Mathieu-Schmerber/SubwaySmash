using System;
using LemonInc.Core.Utilities.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Entities
{
    public class NpcSkinGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private bool _disabledNpc;
        [SerializeField, ShowIf(nameof(_disabledNpc))] private Transform _chair;

        [Button]
        public void GenerateNpcSkin()
        {
            var skin = Resources.LoadAll<Transform>("Skins/Skins").Random();
            TransferMeshProperties(skin, _root);

            if (_disabledNpc && _chair)
            {
                var chair = Resources.LoadAll<Transform>("Skins/Wheelchairs").Random();
                var sourceRndr = chair.GetComponent<MeshRenderer>();
                var sourceFilter = chair.GetComponent<MeshFilter>();
                var targetRndr = _chair.GetComponent<MeshRenderer>();
                var targetFilter = _chair.GetComponent<MeshFilter>();
                
                targetRndr.sharedMaterial = sourceRndr.sharedMaterial;
                targetFilter.sharedMesh = sourceFilter.sharedMesh;
            }
        }
        
        private void TransferMeshProperties(Transform original, Transform ragdoll)
        {
            // Iterate through all children of the original object recursively
            foreach (var originalRenderer in original.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                // Find a SkinnedMeshRenderer with a matching name in the ragdoll recursively
                Debug.Log($"Looking for {originalRenderer.name}");
                var ragdollRenderer = FindChildByName(ragdoll, originalRenderer.name)?.GetComponent<SkinnedMeshRenderer>();

                if (ragdollRenderer != null)
                {
                    ragdollRenderer.sharedMesh = originalRenderer.sharedMesh;
                    ragdollRenderer.sharedMaterials = originalRenderer.sharedMaterials;
                }
            }

            foreach (var originalFilter in original.GetComponentsInChildren<MeshFilter>(true))
            {
                // Find a MeshFilter with a matching name in the ragdoll recursively
                var ragdollFilter = FindChildByName(ragdoll, originalFilter.name)?.GetComponent<MeshFilter>();

                if (ragdollFilter != null)
                {
                    ragdollFilter.sharedMesh = originalFilter.sharedMesh;
                }

                // Copy corresponding MeshRenderer materials if found
                var originalRenderer = originalFilter.GetComponent<MeshRenderer>();
                var ragdollRenderer = ragdollFilter?.GetComponent<MeshRenderer>();
                if (originalRenderer != null && ragdollRenderer != null)
                {
                    ragdollRenderer.sharedMaterials = originalRenderer.sharedMaterials;
                }
            }
        }
        
        public static Transform FindDeepChild(Transform parent, string name)
        {
            // Check if the current parent is the one we're looking for
            if (parent.name == name)
                return parent;

            // Search through all children
            foreach (Transform child in parent)
            {
                Transform result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }

            // Return null if no match found
            return null;
        }
        
        private Transform FindChildByName(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (string.Equals(child.name, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return child;
                }

                var result = FindChildByName(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
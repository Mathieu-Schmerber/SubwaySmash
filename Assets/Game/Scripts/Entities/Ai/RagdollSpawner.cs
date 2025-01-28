using System;
using Game.Entities.GPE.BBQ;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class RagdollSpawner : MonoBehaviour
    {
        [SerializeField, AssetsOnly, AssetSelector] private RagdollSettings _ragdoll;

        public Rigidbody SpawnRagdoll()
        {
            // Instantiate the ragdoll
            var ragdoll = Instantiate(_ragdoll, transform.position, transform.rotation);

            // Transfer SkinnedMeshRenderer and MeshFilter values
            TransferMeshProperties(transform, ragdoll.transform);
            
            TransferTransforms(transform, ragdoll.transform);

            var rb = GetComponent<Rigidbody>();
            ragdoll.MainRigidbody.linearVelocity = rb.linearVelocity;
            
            var myIgnite = GetComponent<Ignitable>();
            if (myIgnite)
            {
                var ignite = ragdoll.GetComponents<Ignitable>();
                var onfire = GetComponent<OnFire>();
                if (onfire)
                {
                    foreach (var i in ignite)
                    {
                        i.StartIgnite(myIgnite.BurnTime, myIgnite.BurnProgress);
                    }
                    
                    // Move OnFire component
                    var newOnFire = ragdoll.AddComponent<OnFire>();
                    newOnFire.Copy(onfire);

                    // Optionally remove the original OnFire component
                    Destroy(onfire);
                }
                
            }

            return ragdoll.MainRigidbody;
        }

        private void TransferTransforms(Transform source, Transform target)
        {
            if (source == null || target == null) return;

            target.localPosition = source.localPosition;
            target.localRotation = source.localRotation;

            for (int i = 0; i < source.childCount; i++)
            {
                var sourceChild = source.GetChild(i);
                var targetChild = FindDeepChild(target, sourceChild.name);
                if (targetChild != null)
                    TransferTransforms(sourceChild, targetChild);
            }
        }

        private void TransferMeshProperties(Transform original, Transform ragdoll)
        {
            // Iterate through all children of the original object recursively
            foreach (var originalRenderer in original.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                // Find a SkinnedMeshRenderer with a matching name in the ragdoll recursively
                var ragdollRenderer = FindChildByName(ragdoll, originalRenderer.name)?.GetComponent<SkinnedMeshRenderer>();

                if (ragdollRenderer != null)
                {
                    ragdollRenderer.sharedMesh = originalRenderer.sharedMesh;
                    ragdollRenderer.materials = originalRenderer.materials;
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
                    ragdollRenderer.materials = originalRenderer.materials;
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

using System;
using Game.Entities.GPE.BBQ;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Entities.Ai
{
    public class RagdollSpawner : MonoBehaviour
    {
        [SerializeField] private RagdollSettings _ragdoll;

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

            // Copy the local transform values from source to target
            target.localPosition = source.localPosition;
            target.localRotation = source.localRotation;
            target.localScale = source.localScale;

            // Iterate through all children of the source
            foreach (Transform sourceChild in source)
            {
                // Find a child in the target with the same name as the source child
                var targetChild = FindChildByName(target, sourceChild.name);
                if (targetChild != null)
                {
                    // Recursively copy transform values
                    TransferTransforms(sourceChild, targetChild);
                }
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

using System;
using Game.Entities.GPE.BBQ;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class RagdollSpawner : MonoBehaviour
    {
        [SerializeField, AssetsOnly, AssetSelector] private RagdollSettings _ragdoll;

        public Rigidbody SpawnRagdoll()
        {
            var ragdoll = Instantiate(_ragdoll, transform.position, transform.rotation);

            var sourceColliders = GetComponentsInChildren<Collider>();
            var ragdollColliders = ragdoll.GetComponentsInChildren<Collider>();

            foreach (var sourceCollider in sourceColliders)
            {
                foreach (var ragdollCollider in ragdollColliders)
                {
                    Physics.IgnoreCollision(sourceCollider, ragdollCollider);
                }
            }
            
            TransferMeshProperties(transform, ragdoll.transform);

            var gfx = FindDeepChild(ragdoll.transform, "GFX");
            if (gfx)
            {
                gfx.rotation = FindDeepChild(transform, "GFX").rotation;
            }
            
            var chair = FindDeepChild(ragdoll.transform, "Chair");
            if (chair)
            {
                chair.rotation = FindDeepChild(transform, "Chair").rotation;
            }
            
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
                    
                    var newOnFire = ragdoll.AddComponent<OnFire>();
                    newOnFire.Copy(onfire);

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
            
            for (var i = 0; i < source.childCount; i++)
            {
                var sourceChild = source.GetChild(i);
                var targetChild = FindDeepChild(target, sourceChild.name);
                if (targetChild != null)
                    TransferTransforms(sourceChild, targetChild);
            }
        }

        private void TransferMeshProperties(Transform original, Transform ragdoll)
        {
            foreach (var originalRenderer in original.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                var ragdollRenderer = FindChildByName(ragdoll, originalRenderer.name)?.GetComponent<SkinnedMeshRenderer>();

                if (ragdollRenderer != null)
                {
                    ragdollRenderer.sharedMesh = originalRenderer.sharedMesh;
                    ragdollRenderer.materials = originalRenderer.materials;
                }
            }

            foreach (var originalFilter in original.GetComponentsInChildren<MeshFilter>(true))
            {
                var ragdollFilter = FindChildByName(ragdoll, originalFilter.name)?.GetComponent<MeshFilter>();

                if (ragdollFilter != null)
                {
                    ragdollFilter.sharedMesh = originalFilter.sharedMesh;
                }

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
            if (parent.name == name)
                return parent;

            foreach (Transform child in parent)
            {
                var result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }

            return null;
        }
        
        private Transform FindChildByName(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (string.Equals(child.name, name, StringComparison.CurrentCultureIgnoreCase))
                    return child;

                var result = FindChildByName(child, name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}

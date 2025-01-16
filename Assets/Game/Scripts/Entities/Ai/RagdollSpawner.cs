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

            return ragdoll.MainRigidbody;
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
                if (child.name == name)
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

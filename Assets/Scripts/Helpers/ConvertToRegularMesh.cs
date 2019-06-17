using UnityEngine;

namespace Helpers
{
    public class ConvertToRegularMesh : MonoBehaviour
    {
        [ContextMenu("Convert to regular Mesh")]
        void Convert()
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

            meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
            meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;
            
            DestroyImmediate(skinnedMeshRenderer);
            DestroyImmediate(this);
        }
    }
}

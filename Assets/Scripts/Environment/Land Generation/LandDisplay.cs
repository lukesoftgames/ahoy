using UnityEngine;

public class LandDisplay : MonoBehaviour {
    [SerializeField] private MeshFilter meshFilter;

    public void drawMesh(MeshData meshData) {
        meshFilter.sharedMesh = meshData.createMesh();
    }
}
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour {
    private Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    private const float MOVE_THRESHOLD = 25.0f;
    private const float SQR_MOVE_THRESHOLD = MOVE_THRESHOLD * MOVE_THRESHOLD;
    private static float maxViewDist;
    private int chunkSize;
    private int chunksVisibleInView;
    private static LandGenerator landGenerator;
    private List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();
    [SerializeField] private LODInfo[] detailLevels;
    [SerializeField] private Material material;
    [SerializeField] private Transform viewer;
    public static Vector2 viewerPosition;
    private Vector2 oldViewerPosition;

    private void Awake() {
        maxViewDist = detailLevels[detailLevels.Length - 1].visibleDistThreshold;
        chunkSize = LandGenerator.CHUNK_SIZE - 1;
        chunksVisibleInView = Mathf.RoundToInt(maxViewDist / chunkSize);
        landGenerator = FindObjectOfType<LandGenerator>();

        updateVisibleChunks();
    }

    private void Update() {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if((oldViewerPosition - viewerPosition).sqrMagnitude > SQR_MOVE_THRESHOLD) {
            oldViewerPosition = viewerPosition;
            updateVisibleChunks();
        }
    }

    private void updateVisibleChunks() {
        for(int i = 0; i < visibleTerrainChunks.Count; i++) {
            visibleTerrainChunks[i].setVisible(false);
        }

        visibleTerrainChunks.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunksVisibleInView; yOffset <= chunksVisibleInView; yOffset++) {
            for(int xOffset = -chunksVisibleInView; xOffset <= chunksVisibleInView; xOffset++) {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChunks.ContainsKey(viewedChunkCoord)) {
                    terrainChunks[viewedChunkCoord].updateChunk();

                    if(terrainChunks[viewedChunkCoord].getVisible()) {
                        visibleTerrainChunks.Add(terrainChunks[viewedChunkCoord]);
                    }
                } else {
                    terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, detailLevels, chunkSize, transform, material));
                }
            }
        }
    }

    public class TerrainChunk {
        private bool landDataReceived;
        private Bounds bounds;
        private float[,] landData;
        private GameObject meshObj;
        private int prevLOD = -1;
        private LODInfo[] detailLevels;
        private LODMesh[] lodMeshes;
        private LODMesh colLODMesh;
        private MeshCollider col;
        private MeshFilter filter;
        private MeshRenderer rend;
        private Vector2 position;

        public TerrainChunk(Vector2 coord, LODInfo[] detailLevels, int size, Transform parent, Material material) {
            this.detailLevels = detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 posV3 = new Vector3(position.x, 0.0f, position.y);

            meshObj = new GameObject("Terrain Chunk");
            rend = meshObj.AddComponent<MeshRenderer>();
            filter = meshObj.AddComponent<MeshFilter>();
            col = meshObj.AddComponent<MeshCollider>();
            meshObj.transform.position = posV3;
            meshObj.transform.parent = parent;
            rend.material = material;
            setVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++) {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, updateChunk);

                if(detailLevels[i].useForCollider) {
                    colLODMesh = lodMeshes[i];
                }
            }

            landGenerator.requestLandData(position, onLandDataReceived);
        }

        public bool getVisible() {
            return meshObj.activeSelf;
        }

        private void onLandDataReceived(float[,] landData) {
            this.landData = landData;
            landDataReceived = true;

            updateChunk();
        }

        public void setVisible(bool visible) {
            meshObj.SetActive(visible);
        }

        public void updateChunk() {
            if(landDataReceived) {
                float viewerDist = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDist <= maxViewDist;

                if(visible) {
                    int lodIndex = 0;

                    for(int i = 0; i < detailLevels.Length - 1; i++) {
                        if(viewerDist > detailLevels[i].visibleDistThreshold) {
                            lodIndex = i + 1;
                        } else {
                            break;
                        }
                    }

                    if(lodIndex != prevLOD) {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if(lodMesh.hasMesh) {
                            prevLOD = lodIndex;
                            filter.mesh = lodMesh.mesh;

                            if(lodIndex == 0) {
                                col.sharedMesh = colLODMesh.mesh;
                            }
                        } else if(!lodMesh.hasRequestedMesh) {
                            lodMesh.requestMesh(landData);

                            if(lodIndex == 0) {
                                colLODMesh.requestMesh(landData);
                            }
                        }
                    }

                    /*Actual video
                     * 
                     if(lodIndex == 0) {
                        if(colLODMesh.hasMesh) {
                            col.sharedMesh = colLODMesh.mesh;
                        } else if(!colLODMesh.hasRequestedMesh) {
                            colLODMesh.requestMesh(landData);
                        }
                     }
                     *
                     */
                }

                setVisible(visible);
            }
        }
    }

    class LODMesh {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        private int lod;
        private System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback) {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        private void onMeshDataReceived(MeshData meshData) {
            mesh = meshData.createMesh();
            hasMesh = true;

            updateCallback();
        }

        public void requestMesh(float[,] landData) {
            hasRequestedMesh = true;
            landGenerator.requestMeshData(landData, lod, onMeshDataReceived);
        }
    }

    [System.Serializable]
    public struct LODInfo {
        public int lod;
        public float visibleDistThreshold;
        public bool useForCollider;
    }
}

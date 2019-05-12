using UnityEngine;

public class LandGenerator : MonoBehaviour {
    [SerializeField] private float flattening;
    [SerializeField] private float lacunarity;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private float noiseScale;
    [SerializeField] [Range(0, 1)] private float persistence;
    [SerializeField] private int mapHeight;
    [SerializeField] private int mapWidth;
    [SerializeField] private int octaves;
    [SerializeField] private int seed;

    private void OnValidate() {
        if(lacunarity < 1) {
            lacunarity = 1;
        }

        if(mapHeight < 1) {
            mapHeight = 1;
        }

        if(mapWidth < 1) {
            mapWidth = 1;
        }

        if(octaves < 0) {
            lacunarity = 0;
        }
    }

    private void Awake() {
        generateLand();
    }

    public void generateLand() {
        float[,] noiseMap = Noise.generateNoiseMap(seed, mapHeight, mapWidth, noiseScale, octaves, persistence, lacunarity);

        FindObjectOfType<LandDisplay>().drawMesh(MeshGenerator.generateTerrainMesh(noiseMap, meshHeightMultiplier, flattening));
    }
}
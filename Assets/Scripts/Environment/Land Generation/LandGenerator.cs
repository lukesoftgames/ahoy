using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LandGenerator : MonoBehaviour {
    [Header("Island Size and Frequency")]
    [SerializeField] private float curveFactor;
    [SerializeField] private float shiftFactor;
    private float[,] islandFalloff;

    [Header("Noise Generation Settings")]
    [SerializeField] private float flattening;
    [SerializeField] private float lacunarity;
    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private float noiseScale;
    [SerializeField] [Range(0, 1)] private float persistence;
    public const int CHUNK_SIZE = 241;
    [SerializeField] private int octaves;
    [SerializeField] private int seed;
    private Queue<LandThreadInfo<float[,]>> landDataThreadQueue = new Queue<LandThreadInfo<float[,]>>();
    private Queue<LandThreadInfo<MeshData>> meshDataThreadQueue = new Queue<LandThreadInfo<MeshData>>();

    private void Awake() {
        islandFalloff = FalloffGenerator.generateFalloff(CHUNK_SIZE, curveFactor, shiftFactor);

        if(seed == 0) {
            seed = (int)UnityEngine.Random.Range(-100000000.0f, 100000000.0f);
        }
    }

    private void OnValidate() {
        if(lacunarity < 1) {
            lacunarity = 1;
        }

        if(octaves < 0) {
            octaves = 0;
        }
    }

    private void Update() {
        if(landDataThreadQueue.Count > 0) {
            for(int i =0; i < landDataThreadQueue.Count; i++) {
                LandThreadInfo<float[,]> threadInfo = landDataThreadQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if(meshDataThreadQueue.Count > 0) {
            for(int i = 0; i < meshDataThreadQueue.Count; i++) {
                LandThreadInfo<MeshData> threadInfo = meshDataThreadQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private float[,] generateData(bool generateIsland, Vector2 centre) {
        if(generateIsland) {
            float[,] mapData = Noise.generateNoiseMap(seed, CHUNK_SIZE, CHUNK_SIZE, noiseScale, octaves, persistence, lacunarity, centre);
            
            for(int i = 0; i < CHUNK_SIZE; i++) {
                for(int j = 0; j < CHUNK_SIZE; j++) {
                    mapData[i, j] -= islandFalloff[i, j];
                }
            }

            return mapData;
        } else {
            float[,] mapData = new float[CHUNK_SIZE, CHUNK_SIZE];

            for(int i = 0; i < CHUNK_SIZE; i++) {
                for(int j = 0; j < CHUNK_SIZE; j++) {
                    mapData[i, j] = 0;
                }
            }

            return mapData;
        }
    }

    private void landDataThread(bool generateIsland, Vector2 centre, Action<float[,]> callback) {
        float[,] heightMap = generateData(generateIsland, centre);

        lock(landDataThreadQueue) {
            landDataThreadQueue.Enqueue(new LandThreadInfo<float[,]>(callback, heightMap));
        }
    }

    public void meshDataThread(float[,] mapData, int lod, Action<MeshData> callback) {
        MeshData meshData = MeshGenerator.generateTerrainMesh(mapData, meshHeightMultiplier, flattening, lod);

        lock(meshDataThreadQueue) {
            meshDataThreadQueue.Enqueue(new LandThreadInfo<MeshData>(callback, meshData));
        }
    }

    public void requestLandData(bool generateIsland, Vector2 centre, Action<float[,]> callback) {
        ThreadStart threadStart = delegate {
            landDataThread(generateIsland, centre, callback);
        };

        new Thread(threadStart).Start();
    }

    public void requestMeshData(float[,] mapData, int lod, Action<MeshData> callback) {
        ThreadStart threadStart = delegate {
            meshDataThread(mapData, lod, callback);
        };
        
        new Thread(threadStart).Start();
    }

    private struct LandThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public LandThreadInfo(Action<T> callback, T parameter) {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}
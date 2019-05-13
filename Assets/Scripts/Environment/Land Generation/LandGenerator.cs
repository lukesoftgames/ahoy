using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LandGenerator : MonoBehaviour {
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

    private float[,] generateData(Vector2 centre) {
        return Noise.generateNoiseMap(seed, CHUNK_SIZE, CHUNK_SIZE, noiseScale, octaves, persistence, lacunarity, centre);
    }

    private void landDataThread(Vector2 centre, Action<float[,]> callback) {
        float[,] heightMap = generateData(centre);

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

    public void requestLandData(Vector2 centre, Action<float[,]> callback) {
        ThreadStart threadStart = delegate {
            landDataThread(centre, callback);
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
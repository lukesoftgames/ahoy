using UnityEngine;

public static class MeshGenerator {
    public static MeshData generateTerrainMesh(float[,] heightMap, float heightMultiplier, int levelOfDetail) {
        int height= heightMap.GetLength(1);
        int width = heightMap.GetLength(0);
        int simplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / simplificationIncrement + 1;
        float topLeftX = (width - 1) / -2.0f;
        float topLeftZ = (height - 1) / 2.0f;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for(int y = 0; y < height; y += simplificationIncrement) {
            for(int x  = 0; x < width; x += simplificationIncrement) {
                float curHeight = Mathf.Clamp(heightMap[x, y] * heightMultiplier, 0, float.MaxValue);
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, curHeight, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1) {
                    meshData.addTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.addTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshHeight, int meshWidth) {
        vertices = new Vector3[meshHeight * meshWidth];
        uvs = new Vector2[meshHeight * meshWidth];
        triangles = new int[(meshHeight - 1) * (meshWidth - 1) * 6];
    }

    public void addTriangle(int a, int b, int c) {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh createMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
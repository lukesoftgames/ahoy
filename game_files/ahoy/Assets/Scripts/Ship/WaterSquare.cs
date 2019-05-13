using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generates a plane with a specific resolution and transforms the plane to make waves
public class WaterSquare
{
    public Transform squareTransform;
    
    //Add the wave mesh to the MeshFilter
	public MeshFilter terrainMeshFilter;

	//The total size in m
	private float size;
	//Resolution = Width of one square
	public float spacing;
	//The total number of vertices we need to generate based on size and spacing
	private int width;

    //For the thread to update the water
    //The local center position of this square to fake transformpoint in a thread
    public Vector3 centerPos;
    //The latest vertices that belong to this square
    public Vector3[] vertices;

    public WaterSquare(GameObject waterSquareObj, float size, float spacing)
    {
        this.squareTransform = waterSquareObj.transform;

        this.size = size;
        this.spacing = spacing;

        this.terrainMeshFilter = squareTransform.GetComponent<MeshFilter>();


        //Calculate the data we need to generate the water mesh   
        width = (int)(size / spacing);
        //Because each square is 2 vertices, so we need one more
        width += 1;

        //Center the sea
        float offset = -((width - 1) * spacing) / 2;

        Vector3 newPos = new Vector3(offset, squareTransform.position.y, offset);

        squareTransform.position += newPos;

        //Save the center position of the square
        this.centerPos = waterSquareObj.transform.localPosition;


        //Generate the sea
        //To calculate the time it took to generate the terrain
        float startTime = System.Environment.TickCount;

        GenerateMesh();
        
        //Calculate the time it took to generate the terrain in seconds
        float timeToGenerateSea = (System.Environment.TickCount - startTime) / 1000f;

        Debug.Log("Sea was generated in " + timeToGenerateSea.ToString() + " seconds");


        //Save the vertices so we can update them in a thread
        this.vertices = terrainMeshFilter.mesh.vertices;
    }

    //If we are updating the square from outside of a thread 
	public void MoveSea(Vector3 oceanPos, float timeSinceStart)
    {
        Vector3[] vertices = terrainMeshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
			Vector3 vertex = vertices[i];

            //From local to global
            //Vector3 vertexGlobal = squareTransform.TransformPoint(vertex);

            Vector3 vertexGlobal = vertex + centerPos + oceanPos;

            //Unnecessary because no rotation nor scale
            //Vector3 vertexGlobalTest2 = squareTransform.rotation * Vector3.Scale(vertex, squareTransform.localScale) + squareTransform.position;

            //Debug 
            if (i == 0)
            {
                //Debug.Log(vertexGlobal + " " + vertexGlobalTest);
            }

            //Get the water height at this coordinate
            vertex.y = WaterController.current.GetWaveYPos(vertexGlobal, timeSinceStart);

            //From global to local - not needed if we use the saved local x,z position
            //vertices[i] = transform.InverseTransformPoint(vertex);

            //Don't need to go from global to local because the y pos is always at 0
            vertices[i] = vertex;
        }

		terrainMeshFilter.mesh.vertices = vertices;

        terrainMeshFilter.mesh.RecalculateNormals();
	}

    //Generate the water mesh
    public void GenerateMesh()
    {
        //Vertices
        List<Vector3[]> verts = new List<Vector3[]>();
		//Triangles
		List<int> tris = new List<int>();
		//Texturing
		//List<Vector2> uvs = new List<Vector2>();
		
		for (int z = 0; z < width; z++)
        {
			
			verts.Add(new Vector3[width]);
			
			for (int x = 0; x < width; x++)
            {
				Vector3 current_point = new Vector3();
				
				//Get the corrdinates of the vertice
				current_point.x = x * spacing;
				current_point.z = z * spacing;
				current_point.y = squareTransform.position.y;
				
				verts[z][x] = current_point;
				
				//uvs.Add(new Vector2(x,z));
				
				//Don't generate a triangle the first coordinate on each row
				//Because that's just one point
				if (x <= 0 || z <= 0)
                {
					continue;
				}

				//Each square consists of 2 triangles

				//The triangle south-west of the vertice
				tris.Add(x 		+ z * width);
				tris.Add(x 		+ (z-1) * width);
				tris.Add((x-1) 	+ (z-1) * width);
				
				//The triangle west-south of the vertice
				tris.Add(x 		+ z * width);
				tris.Add((x-1) 	+ (z-1) * width);
				tris.Add((x-1)	+ z * width);
			}
		}
		
		//Unfold the 2d array of verticies into a 1d array.
		Vector3[] unfolded_verts = new Vector3[width * width];

        int i = 0;
		foreach (Vector3[] v in verts)
        {
			//Copies all the elements of the current 1D-array to the specified 1D-array
			v.CopyTo(unfolded_verts, i * width);

            i++;
		}
		
		//Generate the mesh object
		Mesh newMesh = new Mesh();
        newMesh.vertices = unfolded_verts;
        //newMesh.uv = uvs.ToArray();
        newMesh.triangles = tris.ToArray();

        //Ensure the bounding volume is correct
        newMesh.RecalculateBounds();
        //Update the normals to reflect the change
        newMesh.RecalculateNormals();


		//Add the generated mesh to this GameObject
		terrainMeshFilter.mesh.Clear();
		terrainMeshFilter.mesh = newMesh;
		terrainMeshFilter.mesh.name = "Water Mesh";

        Debug.Log(terrainMeshFilter.mesh.vertices.Length);
	}
}
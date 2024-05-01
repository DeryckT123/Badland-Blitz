using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class meshGenerator : MonoBehaviour
{
    public NavMeshSurface surface;

    Mesh mesh;
    private int MESH_SCALE = 100;
    public GameObject[] objects;
    [SerializeField] private AnimationCurve heightCurve;
    private Vector3[] vertices;
    private int[] triangles;

    Vector2[] uvs;
    public GameObject Player;
    LayerMask groundMask;



    private Color[] colors;
    [SerializeField] private Gradient gradient;

    private float minTerrainheight;
    private float maxTerrainheight;

    public int xSize;
    public int zSize;

    public float scale;
    public int octaves;
    public float lacunarity;

    public int seed;

    private float lastNoiseHeight;

    public Transform target;

    bool playerSpawned = false;

    void Start()
    {
        // Use this method if you havn't filled out the properties in the inspector
        // SetNullProperties(); 

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateNewMap();
        surface.BuildNavMesh();
    }

    private void SetNullProperties()
    {
        if (xSize <= 0) xSize = 50;
        if (zSize <= 0) zSize = 50;
        if (octaves <= 0) octaves = 5;
        if (lacunarity <= 0) lacunarity = 2;
        if (scale <= 0) scale = 50;
    }

    public void CreateNewMap()
    {
        CreateMeshShape();
        CreateTriangles();
        ColorMap();
        UpdateMesh();
    }

    private void CreateMeshShape()
    {
        // Creates seed
        Vector2[] octaveOffsets = GetOffsetSeed();

        if (scale <= 0) scale = 0.0001f;

        // Create vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Set height of vertices
                float noiseHeight = GenerateNoiseHeight(z, x, octaveOffsets);
                SetMinMaxHeights(noiseHeight);
                vertices[i] = new Vector3(x, noiseHeight, z);
                i++;
            }
        }
    }

    private Vector2[] GetOffsetSeed()
    {
        seed = Random.Range(0, 1000);

        // changes area of map
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int o = 0; o < octaves; o++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[o] = new Vector2(offsetX, offsetY);
        }
        return octaveOffsets;
    }

    private float GenerateNoiseHeight(int z, int x, Vector2[] octaveOffsets)
    {
        float amplitude = 20;
        float frequency = 1;
        float persistence = 0.5f;
        float noiseHeight = 0;

        // loop over octaves
        for (int y = 0; y < octaves; y++)
        {
            float mapZ = z / scale * frequency + octaveOffsets[y].y;
            float mapX = x / scale * frequency + octaveOffsets[y].x;

            //The *2-1 is to create a flat floor level
            float perlinValue = (Mathf.PerlinNoise(mapZ, mapX)) * 2 - 1;
            noiseHeight += heightCurve.Evaluate(perlinValue) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistence;
        }
        return noiseHeight;
    }

    private void SetMinMaxHeights(float noiseHeight)
    {
        // Set min and max height of map for color gradient
        if (noiseHeight > maxTerrainheight)
            maxTerrainheight = noiseHeight;
        if (noiseHeight < minTerrainheight)
            minTerrainheight = noiseHeight;
    }


    private void CreateTriangles()
    {
        // Need 6 vertices to create a square (2 triangles)
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        // Go to next row
        for (int z = 0; z < xSize; z++)
        {
            // fill row
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x /xSize, (float) z/zSize);
                i++;
            }
        }
    }

    private void ColorMap()
    {
        colors = new Color[vertices.Length];

        // Loop over vertices and apply a color from the depending on height (y axis value)
        for (int i = 0, z = 0; z < vertices.Length; z++)
        {
            float height = Mathf.InverseLerp(minTerrainheight, maxTerrainheight, vertices[i].y);
            colors[i] = gradient.Evaluate(height);
            i++;
        }
    }

    private void MapEmbellishments()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // find actual position of vertices in the game
            Vector3 worldPt = transform.TransformPoint(mesh.vertices[i]);
            var noiseHeight = worldPt.y;

            // Stop generation if height difference between 2 vertices is too steep
            if (System.Math.Abs(worldPt.y) > 5)
            {
                //x and z coordinates
                float x = mesh.vertices[i].x * MESH_SCALE;
                float z = mesh.vertices[i].z * MESH_SCALE;

                if (x > 400 && z > 400 && x < 800 && z < 800 && !playerSpawned) //Random player spawn
                {
                    playerSpawned = true;
                    GameObject playerSpawnPoint = objects[0];
                    var plane = Instantiate(playerSpawnPoint, new Vector3(x, worldPt.y, z), Quaternion.identity); //Keeps player and objects on land

                    if (plane.name == "Crashed_Boeing(Clone)")
                    {
                        Player.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y + 1.5f, plane.transform.position.z);
                        Player.transform.Rotate(0, 0, 0);
                        Player.GetComponent<CharacterController>().enabled = true; //Allows player to move
                        playerSpawned = true;
                        continue; //Skips iteration so player does not spawn with additional items
                    }
                }

                // min height for object generation
                if (noiseHeight > 5 && noiseHeight < 35)
                {
                    // Chance to generate
                    if (Random.Range(1, 4) == 1) 
                    {
                        GameObject objectToSpawn = objects[Random.Range(1, objects.Length - 1)]; //building, well, and skull spawn chances
                        var spawnAboveTerrainBy = noiseHeight * 1.0000001f; //Keeps objects attached to land
                        Instantiate(objectToSpawn, new Vector3(x, spawnAboveTerrainBy, z), Quaternion.identity);
                    }
                    else if (Random.Range(1, 5) == 2 || Random.Range(1, 5) == 3 || Random.Range(1, 5) == 1) //turret spawn chances
                    {
                        GameObject objectToSpawn = objects[objects.Length - 1];
                        var obj = Instantiate(objectToSpawn, new Vector3(x, worldPt.y, z), Quaternion.identity);
                    }
                }
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        mesh.uv = uvs;

        GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.transform.localScale = new Vector3(MESH_SCALE, MESH_SCALE, MESH_SCALE);

        MapEmbellishments();
    }

}
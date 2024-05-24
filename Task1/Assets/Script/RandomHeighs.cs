using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]

public class TerrainTextureData
{ 
    public Texture2D terrainTexture;
    public Vector2 tileSize;

    public float minHeight;
    public float maxHeight;

}

[System.Serializable]
public class TreeData
{
    public GameObject treeMesh;
    public float minheight;
    public float maxHeight;

}

public class RandomHeighs : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float minRandomHeightRange = 0.0f;

    [SerializeField]
    [Range(0f, 1f)]
    private float maxRandomHeightRange = 0.1f;

    [SerializeField]
    private bool flattenTerrain = true;


    [Header("Perlin Noise")]
    [SerializeField]
      private bool perlinNoise = false;

    [SerializeField]
    private float PerlinNoiseWidthScale = 0.01f;

    [SerializeField]
    private float perliNoiseHeightScale = 0.01f;

    [Header("Texture Data")]
    [SerializeField]
    private List<TerrainTextureData> terrainTextureData;

    [SerializeField]
    private bool addTerrainTexure = false;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [Header("Tree Data")]
    [SerializeField]
    private List<TreeData> treeData;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treespacing = 10;

    [SerializeField]
    private bool addTrees = false;

    [SerializeField]
    private int terrainLayerIndex;

    [Header("Path Texture")]
    [SerializeField]
    private Texture2D pathTexture;

    [Header("Water")]
    [SerializeField]
    private GameObject water;

    [SerializeField]
    private float waterHeight = 0.3f;

    public Material Skymateial;

    [Header("Path")]
    [SerializeField]
    private float pathWidth = 5.0f; // Width of the path

    [SerializeField]
    private float pathDepth = 0.2f; // Depth of the path

    [Header("Player")]
    [SerializeField]
    private GameObject playerPrefab; // Player prefab

    [SerializeField]
    private Vector3 playerStartPosition = new Vector3(0, 50, 0); // Player start position above the terrain

    // Start is called before the first frame update
    void Start()
    {
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

  
        AddTerrainTexure();
        AddTree();
        OnDestory();
        AddWater();
        GeneratePath();
        AddPlayer();

        RenderSettings.skybox = Skymateial;
    }

    void Update()
    {

              GenerateRandomHeights();
    }
        private void GenerateRandomHeights()
        {
            float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

            for(int width = 0; width < terrainData.heightmapResolution; width++)
            {
                for(int height = 0; height < terrainData.heightmapResolution; height++)
                {

                if (perlinNoise)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * PerlinNoiseWidthScale, height * perliNoiseHeightScale);
                }
                else
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }
            }
            }

            terrainData.SetHeights(0, 0, heightMap);

        }

        private void FlattenTerrain()
        {
            float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

            for (int width = 0; width < terrainData.heightmapResolution; width++)
            {
                for (int height = 0; height < terrainData.heightmapResolution; height++)
                {
                    heightMap[width, height] = 0;
                }
            }

            terrainData.SetHeights(0, 0, heightMap);

        }
    private void AddTerrainTexure()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];

        for(int i = 0; i< terrainTextureData.Count; i++)
        {
            if(addTerrainTexure)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;

            }
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] hegihtMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphamapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers ];

        for(int height =0; height<terrainData.alphamapHeight; height++)
        {
            for(int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] alphamap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureData.Count; i++)
                {
                    float heightBegain = terrainTextureData[i].minHeight - terrainTextureBlendOffset;
                    float heightEnd = terrainTextureData[i].maxHeight + terrainTextureBlendOffset;

                    if (hegihtMap[width, height] >= heightBegain && hegihtMap[width, height] <= heightEnd)
                    {
                        alphamap[i] = 1;
                    }
                }
                    Blend(alphamap);

                    for(int j = 0; j< terrainTextureData.Count; j++)
                    {
                        alphamapList[width, height, j] = alphamap[j];
                    }

                
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamapList);
    }

    private void Blend(float[] alphamap)
    {
        float total = 0;

        for(int i = 0; i< alphamap.Length; i++)
        {
            total += alphamap[i];
        }

        for(int i = 0; i< alphamap.Length; i++)
        {
            alphamap[i] /= total;
        }
    }

    private void AddTree()
    {
        TreePrototype[] trees = new TreePrototype[treeData.Count];

        for(int i = 0; i< treeData.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeData[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstancesList = new List<TreeInstance>();

        if (addTrees)
        {
            for(int z = 0; z< terrainData.size.z; z+= treespacing)
            {
                for(int x = 0; x< terrainData.size.x; x+= treespacing)
                {
                    for(int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {
                       
                        if(treeInstancesList.Count < maxTrees)
                        {
                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if(currentHeight >= treeData[treeIndex].minheight && currentHeight <= treeData[treeIndex].maxHeight)
                            {
                                float randomX = (x + Random.Range(-5.0f, 5.0f)) / terrainData.size.x;

                                float randomZ = (z + Random.Range(-5.0f, 5.0f)) / terrainData.size.z;

                                Vector3 treePosition = new Vector3(randomX * terrainData.size.x, 
                                                                currentHeight * terrainData.size.y, 
                                                                randomZ * terrainData.size.z) + this.transform.position;

                                RaycastHit raycastHit;

                                int layerMask = 1 << terrainLayerIndex;

                                if(Physics.Raycast(treePosition, -Vector3.up, out raycastHit, 100, layerMask) || 
                                    Physics.Raycast(treePosition,Vector3.up, out raycastHit, 100, layerMask))
                                {

                                    float treeDistance = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    TreeInstance treeInstance = new TreeInstance();

                                    treeInstance.position = new Vector3(randomX, treeDistance, randomZ);
                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstancesList.Add(treeInstance);
                                }
                                
                               
                                
                                
                               
                               



                            }
                        }

                    }
                }
            }
        }

        terrainData.treeInstances = treeInstancesList.ToArray();

    }

    private void AddWater()
    {
        GameObject waterGameObject = Instantiate(water, this.transform.position ,this.transform.rotation);
        waterGameObject.name = "Water";
        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y,
        terrainData.size.z / 2);
        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    private void GeneratePath()
    {
        if (pathTexture == null)
        {
            Debug.LogError("Path texture is not assigned.");
            return;
        }

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int pathStartX = Random.Range(0, terrainData.heightmapResolution);
        int pathStartZ = 0; // Start at the bottom of the map
        int pathEndX = Random.Range(0, terrainData.heightmapResolution);
        int pathEndZ = terrainData.heightmapResolution - 1; // End at the top of the map

        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPos = new Vector2Int(pathStartX, pathStartZ);
        path.Add(currentPos);

        Debug.Log("Starting path generation at: " + currentPos);

        while (currentPos.y < pathEndZ)
        {
            int direction = Random.Range(0, 3);
            if (direction == 0 && currentPos.x > 0)
            {
                currentPos += Vector2Int.left;
            }
            else if (direction == 1 && currentPos.x < terrainData.heightmapResolution - 1)
            {
                currentPos += Vector2Int.right;
            }
            else
            {
                currentPos += Vector2Int.up;
            }
            path.Add(currentPos);
            Debug.Log("Path position added: " + currentPos);
        }

        foreach (Vector2Int pos in path)
        {
            for (int x = -Mathf.FloorToInt(pathWidth / 2); x <= Mathf.FloorToInt(pathWidth / 2); x++)
            {
                for (int z = -Mathf.FloorToInt(pathWidth / 2); z <= Mathf.FloorToInt(pathWidth / 2); z++)
                {
                    int adjustedX = Mathf.Clamp(pos.x + x, 0, terrainData.heightmapResolution - 1);
                    int adjustedZ = Mathf.Clamp(pos.y + z, 0, terrainData.heightmapResolution - 1);

                    heightMap[adjustedX, adjustedZ] = Mathf.Max(0, heightMap[adjustedX, adjustedZ] - pathDepth);
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);

        // Apply path texture
        float[,,] alphamap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);

        // Ensure the path texture is within the range of layers
        int pathTextureLayer = terrainTextureData.Count < terrainData.alphamapLayers ? terrainTextureData.Count : terrainData.alphamapLayers - 1;

        foreach (Vector2Int pos in path)
        {
            for (int x = -Mathf.FloorToInt(pathWidth / 2); x <= Mathf.FloorToInt(pathWidth / 2); x++)
            {
                for (int z = -Mathf.FloorToInt(pathWidth / 2); z <= Mathf.FloorToInt(pathWidth / 2); z++)
                {
                    int adjustedX = Mathf.Clamp(pos.x + x, 0, terrainData.alphamapWidth - 1);
                    int adjustedZ = Mathf.Clamp(pos.y + z, 0, terrainData.alphamapHeight - 1);
                    alphamap[adjustedZ, adjustedX, pathTextureLayer] = 1;
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamap);

        Debug.Log("Path generation complete");
    }


    private void OnDestory()
    {
        if (flattenTerrain)
        { 
            FlattenTerrain();
        }
    }

    private void AddPlayer()
    {
        if (playerPrefab != null)
        {
            // Set player to random position
            int terrainWidth = terrainData.heightmapResolution;
            int terrainHeight = terrainData.heightmapResolution;

            int x = Random.Range(0, terrainWidth);
            int z = Random.Range(0, terrainHeight);

            float normalizedX = (float)x / terrainWidth;
            float normalizedZ = (float)z / terrainHeight;

            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;
            Vector3 playerPosition = new Vector3(normalizedX * terrainData.size.x, currentHeight * terrainData.size.y + 1, normalizedZ * terrainData.size.z);

            GameObject playerObject = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            playerObject.name = "Player";
            Debug.Log("Player added at position: " + playerPosition);
        }
        else
        {
            Debug.LogError("Player prefab is not assigned.");
        }
    }



}

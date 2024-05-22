using System.Collections.Generic;
using UnityEngine;

public class Flat : MonoBehaviour 
{

    [Header("House Configuration")]
    [SerializeField] private Vector3 baseFloorSize = new Vector3(10f, 0.4f, 10f); 
    [SerializeField] private Vector3 baseRoofSize = new Vector3(11f, 0.4f, 11f);
    [SerializeField] private float wallHeight = 3f;
    [SerializeField] private float doorHeight = 2f;
    [SerializeField] private float windowHeight = 1.5f;
    [SerializeField] private float windowWidth = 2f;

    [SerializeField] private int numberOfHouses = 5;
    [SerializeField] private Vector2 areaSize = new Vector2(500f, 500f);




    void Start()
    {

        GenerateHouses();
    }

    void GenerateHouses()
    {
        for (int i = 0; i < numberOfHouses; i++)
        {
            // Random Position and Size
            Vector3 housePosition = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0f, 
                Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );
            Vector3 floorSize = baseFloorSize * Random.Range(0.8f, 1.2f);
            Vector3 roofSize = baseRoofSize * floorSize.x / baseFloorSize.x; 

            GameObject house = new GameObject($"House {i + 1}");
            house.transform.position = housePosition;

            CreateHouseComponents(house, floorSize, roofSize);
        }
    }

    void CreateHouseComponents(GameObject house, Vector3 floorSize, Vector3 roofSize)
    {

        // Structure
        CreatePrimitive(house, "Floor", floorSize, new Vector3(0, floorSize.y / 2, 0), GroundMaterialList()[0]); // Access GrassMaterial (index 0)
        CreatePrimitive(house, "Roof", roofSize, new Vector3(0, wallHeight + floorSize.y + roofSize.y / 2, 0), RoofMaterialList()[0]); // Access MagentaMaterial (index 0) (Assuming you are using the roof material list)

        Vector3 wallSize = new Vector3(floorSize.x, wallHeight, 0.1f);
        CreatePrimitive(house, "Front Wall", wallSize, new Vector3(0, wallHeight / 2, floorSize.z / 2), FrontBackWallMaterialList()[0]); // Access BricksMaterial (index 0)
        CreatePrimitive(house, "Back Wall", wallSize, new Vector3(0, wallHeight / 2, -floorSize.z / 2), FrontBackWallMaterialList()[0]); // Access BricksMaterial (index 0)

        wallSize = new Vector3(0.1f, wallHeight, floorSize.z);
        CreatePrimitive(house, "Left Wall", wallSize, new Vector3(-floorSize.x / 2, wallHeight / 2, 0), SideWallMaterialList()[0]); // Access BricksMaterial (index 0)
        CreatePrimitive(house, "Right Wall", wallSize, new Vector3(floorSize.x / 2, wallHeight / 2, 0), SideWallMaterialList()[0]); // Access BricksMaterial (index 0)

        // Door and Windows (randomized positions and sizes)
        float doorWidth = Random.Range(0.15f, 0.3f) * floorSize.x;
        CreatePrimitive(house, "Door", new Vector3(doorWidth, doorHeight, 0.1f),
            new Vector3(Random.Range(-floorSize.x / 2 + doorWidth / 2, floorSize.x / 2 - doorWidth / 2), doorHeight / 2, floorSize.z / 2 + 0.05f),
            MainDoorMaterialList()[0]); // Access Door material (index 0)


        for (int i = 0; i < 2; i++)
        {
            float windowX = Random.Range(-floorSize.x / 2 + windowWidth / 2, floorSize.x / 2 - windowWidth / 2);
            float windowY = Random.Range(1f, wallHeight - windowHeight / 2);

            List<Material> groundMaterials = GroundMaterialList(); // Call the method to get the list
            CreatePrimitive(house, $"Window {i + 1}", new Vector3(windowWidth, windowHeight, 0.1f),
                new Vector3(windowX, windowY, floorSize.z / 2 + 0.05f),
                groundMaterials[6]); // Access the 6th material from the list

        }


    }

    void CreatePrimitive(GameObject parent, string name, Vector3 size, Vector3 localPosition, Material material)
    {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        primitive.name = name;
        primitive.transform.parent = parent.transform;
        primitive.transform.localScale = size;
        primitive.transform.localPosition = localPosition;
        primitive.GetComponent<Renderer>().material = material;
    }

    private List<Material> GroundMaterialList()
    {
        List<Material> groundMaterialList = new List<Material>();

        Material GrassMaterial = Resources.Load<Material>("Materials/Grass");

        groundMaterialList.Add(GrassMaterial);

        return groundMaterialList;
    }

    private List<Material> FrontBackWallMaterialList()
    {
        List<Material> frontBackWallMaterialList = new List<Material>();

        Material bricksMaterial = Resources.Load<Material>("Materials/bricks");

        frontBackWallMaterialList.Add(bricksMaterial);

        return frontBackWallMaterialList;
    }

    private List<Material> SideWallMaterialList()
    {
        List<Material> sideWallMaterialList = new List<Material>();

        Material bricksMaterial = Resources.Load<Material>("Materials/bricks");

        sideWallMaterialList.Add(bricksMaterial);

        return sideWallMaterialList;
    }

    private List<Material> FloorMaterialList()
    {
        List<Material> floorMaterialList = new List<Material>();

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black;

        floorMaterialList.Add(blackMaterial);

        return floorMaterialList;
    }

    private List<Material> RoofMaterialList()
    {
        List<Material> roofMaterialList = new List<Material>();

        Material magentaMaterial = new Material(Shader.Find("Specular"));
        magentaMaterial.color = Color.magenta;

        roofMaterialList.Add(magentaMaterial);

        return roofMaterialList;
    }

    private List<Material> MainDoorMaterialList()
    {
        List<Material> mainDoorMaterialList = new List<Material>();

        //Material redMaterial = new Material(Shader.Find("Specular"));
        //redMaterial.color = Color.red;

        GameObject doorPrefab = Resources.Load<GameObject>("Prefab/Door");
        Renderer doorRenderer = doorPrefab.GetComponent<Renderer>();


        mainDoorMaterialList.Add(doorRenderer.sharedMaterial);

        return mainDoorMaterialList;
    }

    private List<Material> WindowMaterialList()
    {
        List<Material> windowMaterialList = new List<Material>();

        Material grayMaterial = new Material(Shader.Find("Specular"));
        grayMaterial.color = Color.gray;

        windowMaterialList.Add(grayMaterial);

        return windowMaterialList;
    }


}
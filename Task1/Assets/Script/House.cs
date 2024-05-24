using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class House : MonoBehaviour
{
    [SerializeField]
    private Vector3 floorSize = new Vector3(100f, 0.4f, 100f);
    [SerializeField]
    private Vector3 roofSize = new Vector3(100f,0.400000006f,105f);
    [SerializeField]
    private Vector3 sideWallSize = new Vector3(100f, 150f, 5f);
    [SerializeField]
    private Vector3 frontBackWallSize = new Vector3(5f, 150f, 100f);
    [SerializeField]
    private Vector3 mainDoorSize = new Vector3(5f,40f,40f);
    [SerializeField]
    private Vector3 frontWindowSize = new Vector3(5f,25f,25f);
    [SerializeField]
    private Vector3 sideWindowSize = new Vector3(35f,35f,5f);
    [SerializeField]
    private Vector3 groundSize = new Vector3(300f, 0.2f, 300f);
    [SerializeField]
    private Vector3 floorPosition = new Vector3(0f,0.8f,0f);
    [SerializeField]
    private Vector3 roofPosition = new Vector3(0f,252f,0.300003052f);
    [SerializeField]
    private Vector3 leftWallPosition = new Vector3(0f,101.199997f,100.300003f);
    [SerializeField]
    private Vector3 rightWallPosition = new Vector3(0f,101.199997f,-99.6999969f);
    [SerializeField]
    private Vector3 frontWallPosition = new Vector3(-95f,101.199997f,5.30000305f);
    [SerializeField]
    private Vector3 backWallPosition = new Vector3(95f,101.199997f,0f);
    [SerializeField]
    private Vector3 mainDoorPosition = new Vector3(-96f,40f,1f);    
    [SerializeField]
    private Vector3 leftFrontWindowPosition = new Vector3(-96f,145f,50f); 
    [SerializeField]
    private Vector3 rightFrontWindowPosition = new Vector3(-96f,145f,-49f);

    [SerializeField]
    private Vector3 leftWindowPosition = new Vector3(7f,139f,101f);
    [SerializeField]
    private Vector3 rightWindowPosition = new Vector3(0f,139f,-100f);


    GameObject houseGameObject;


    // Start is called before the first frame update
    void Start()
    {
        //CreateGround();
        SpawnHouses(5);
    }

    private void SpawnHouses(int numberOfHouses)
    {
        // List to store spawn points
        List<Vector3> spawnPoints = new List<Vector3>()
      {
        new Vector3(-1600f, 0f,350f),
         new Vector3(-1350f, 0f,350f),
        new Vector3(-1150f, 0f,350f),
        new Vector3(-850f, 0f,350f),
        new Vector3(0f, 0f,0f),
        new Vector3(0f, 0f, 500f),
        new Vector3(-700f, 0f, -200f),
        new Vector3(-700f, 0f, -400f),
        new Vector3(-700f, 0f, -600f),
        new Vector3(-1200f, 0f, -150f),
        new Vector3(-1200f, 0f, -350f),
        new Vector3(-1200f, 0f, -550f),
        new Vector3(-1200f, 0f, -300f),
        new Vector3(1200f, 0f, -300f),
        new Vector3(1200f, 0f, -500f),
        new Vector3(1200f, 0f, -350f),

      };

        for (int i = 0; i < numberOfHouses; i++)
        {
            if (spawnPoints.Count == 0) // Handle case where all spawn points are used
            {
                Debug.LogWarning("No more spawn points available!");
                break;
            }

            // Get a random spawn point from the list
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Vector3 randomSpawnPoint = spawnPoints[randomIndex];

            // Create a house at the random spawn point
            CreateHouse(randomSpawnPoint);

            // Remove the used spawn point to avoid duplicates
            spawnPoints.RemoveAt(randomIndex);
        }
    }

    private void InitializeHouse(){
        houseGameObject = new GameObject();
        houseGameObject.name = "House";
        houseGameObject.transform.position = this.transform.position;
        houseGameObject.transform.rotation = this.transform.rotation;
        houseGameObject.transform.localScale = this.transform.localScale;
        houseGameObject.transform.parent = this.transform;
    }

    private void CreateHouse(Vector3 spawnPoint)
    {
        InitializeHouse();
        CreateFloor();
        CreateRoof();
        //Left Wall
        CreateWall(sideWallSize, "Left Wall", leftWallPosition, SideWallMaterialList());
        //Right Wall
        CreateWall(sideWallSize, "Right Wall", rightWallPosition, SideWallMaterialList());
        //Front Wall
        CreateWall(frontBackWallSize, "Front Wall", frontWallPosition, FrontBackWallMaterialList());
        //Back Wall
        CreateWall(frontBackWallSize, "Back Wall", backWallPosition, FrontBackWallMaterialList());
        //Door
        CreateAperture(mainDoorSize, "Main Door", mainDoorPosition, MainDoorMaterialList());
        //Left front Window
        CreateAperture(frontWindowSize, "Left Front Window", leftFrontWindowPosition, WindowMaterialList());
        //Right front Window
        CreateAperture(frontWindowSize, "Right Front Window", rightFrontWindowPosition, WindowMaterialList());
        //Left Side Window
        CreateAperture(sideWindowSize, "Left Side Window", leftWindowPosition, WindowMaterialList());
        //Right Side Window
        CreateAperture(sideWindowSize, "Right Side Window", rightWindowPosition, WindowMaterialList());

        houseGameObject.transform.position = spawnPoint;



    }


    private void CreateFloor(){
        GameObject cube = new GameObject();
        cube.name = "Floor";
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0,0,0,0,0,0);
        cubeScript.UpdateCubeMaterialsList(FloorMaterialList());        
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = floorSize;
        cube.transform.position = floorPosition;
        cube.transform.parent = houseGameObject.transform;
    }

    private void CreateRoof(){
        GameObject cube = new GameObject();
        cube.name = "Roof";
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0,0,0,0,0,0);
        cubeScript.UpdateCubeMaterialsList(RoofMaterialList());        
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = roofSize;
        cube.transform.position = roofPosition;
        cube.transform.parent = houseGameObject.transform;
    }


    private void CreateWall(Vector3 wallSize, string name, Vector3 position, 
                            List<Material> wallMaterialList){
        GameObject cube = new GameObject();
        cube.name = name;
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0,0,0,0,0,0);
        cubeScript.UpdateCubeMaterialsList(wallMaterialList);
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = wallSize;
        cube.transform.position = position;
        cube.transform.parent = houseGameObject.transform;
    }

    private void CreateAperture(Vector3 apertureSize, string name, Vector3 position, 
                            List<Material> apertureMaterialList){
        GameObject cube = new GameObject();
        cube.name = name;
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0,0,0,0,0,0);
        cubeScript.UpdateCubeMaterialsList(apertureMaterialList);
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = apertureSize;
        cube.transform.position = position;
        cube.transform.parent = houseGameObject.transform;
    }

    /*private void CreateGround(){
        GameObject cube = new GameObject();
        cube.name = "Ground";
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0,0,0,0,0,0);
        cubeScript.UpdateCubeMaterialsList(GroundMaterialList());
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = groundSize;
        cube.transform.position = this.transform.position;
        cube.transform.parent = this.transform;
    }*/

    //--------------  Material List ----------------    


    private List<Material> GroundMaterialList(){
        List<Material> groundMaterialList = new List<Material>();

        Material GrassMaterial = Resources.Load<Material>("Materials/Grass");

        groundMaterialList.Add(GrassMaterial);

        return groundMaterialList;
    }

    private List<Material> FrontBackWallMaterialList(){
        List<Material> frontBackWallMaterialList = new List<Material>();

        Material bricksMaterial = Resources.Load<Material>("Materials/bricks");

        frontBackWallMaterialList.Add(bricksMaterial);

        return frontBackWallMaterialList;
    }

    private List<Material> SideWallMaterialList(){
        List<Material> sideWallMaterialList = new List<Material>();

        Material bricksMaterial = Resources.Load<Material>("Materials/bricks");

        sideWallMaterialList.Add(bricksMaterial);

        return sideWallMaterialList;
    }

    private List<Material> FloorMaterialList(){
        List<Material> floorMaterialList = new List<Material>();

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black;

        floorMaterialList.Add(blackMaterial);

        return floorMaterialList;
    }

    private List<Material> RoofMaterialList(){
        List<Material> roofMaterialList = new List<Material>();

        Material RoofMaterial = Resources.Load<Material>("Materials/M_Wood1");

        roofMaterialList.Add(RoofMaterial);

        return roofMaterialList;
    }    

    private List<Material> MainDoorMaterialList(){
        List<Material> mainDoorMaterialList = new List<Material>();

        Material DoorMaterial = Resources.Load<Material>("Materials/M_Wood2");

        mainDoorMaterialList.Add(DoorMaterial);

        return mainDoorMaterialList;
    }

    private List<Material> WindowMaterialList(){
        List<Material> windowMaterialList = new List<Material>();

        Material grayMaterial = new Material(Shader.Find("Specular"));
        grayMaterial.color = Color.gray;

        windowMaterialList.Add(grayMaterial);

        return windowMaterialList;
    }    


}

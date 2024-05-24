using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField]
    float pavementWidth = 4f;
    [SerializeField]
    float laneWidth = 30f;
    [SerializeField]
    float roadMarkerLineWidth = 0.4f;
    [SerializeField]
    float roadHeight = 0.2f;
    [SerializeField]
    float roadLength = 600f;

    [SerializeField]
    Vector3 verticalRoadInitialPosition = new Vector3(-425f, 0f, 294f);
    [SerializeField]
    Vector3 horizontalRoadInitialPosition = new Vector3(170f, 0f, -435f);
    [SerializeField]
    Vector3 verticalRoad2InitialPosition = new Vector3(-1029f, 0f, -5f); // New road 1 position
    [SerializeField]
    Vector3 horizontalRoad2InitialPosition = new Vector3(-1040f, 0f, -602); // New road 2 position

    GameObject verticalRoad;
    GameObject horizontalRoad;
    GameObject additionalRoad1; // New road 1 GameObject
    GameObject additionalRoad2; // New road 2 GameObject

    [SerializeField]
    GameObject Plain;

    [SerializeField]
    GameObject ball;

    [SerializeField]
    GameObject carPrefab;

    GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        InitializeVerticalRoad();
        InitializeHorizontalRoad();
        InitializeAdditionalRoad1();
        InitializeAdditionalRoad2();
        CreateVerticalRoad();
        CreateHorizontalRoad();
        CreateAdditionalRoad1();
        CreateAdditionalRoad2();

        InitializeCar();
        Initializeplane();
        Initializeball();
    }

    private void Update()
    {


    }
    private void InitializeCar()
    {
        float carZOffset = -60f;
        float carXOffset = 8f;

        Vector3 carSpawnPos = new Vector3(verticalRoadInitialPosition.x + carXOffset,
                                          verticalRoadInitialPosition.y,
                                          verticalRoadInitialPosition.z + carZOffset);

        car = Instantiate(carPrefab, carSpawnPos, Quaternion.identity);
        car.name = "Car";
        car.transform.parent = this.transform;
    }

    private void Initializeplane()
    {
        Vector3  plainspawn = new Vector3(0,0,0);
        Plain = Instantiate(Plain, plainspawn, Quaternion.identity);
        Plain.name = "Plain";
    }

    private void Initializeball()
    {
        Vector3 ballspawn = new Vector3(0, 0, 0);
        ball = Instantiate(ball, ballspawn, Quaternion.identity);
        ball.name = "ball";
    }
    private void InitializeVerticalRoad()
    {
        verticalRoad = new GameObject();
        verticalRoad.name = "Vertical Road";
        verticalRoad.transform.position = this.transform.position;
        verticalRoad.transform.rotation = this.transform.rotation;
        verticalRoad.transform.localScale = this.transform.localScale;
        verticalRoad.transform.parent = this.transform;
    }

    private void InitializeHorizontalRoad()
    {
        horizontalRoad = new GameObject();
        horizontalRoad.name = "Horizontal Road";
        horizontalRoad.transform.position = this.transform.position;
        horizontalRoad.transform.rotation = this.transform.rotation;
        horizontalRoad.transform.localScale = this.transform.localScale;
        horizontalRoad.transform.parent = this.transform;
    }

    private void InitializeAdditionalRoad1()
    {
        additionalRoad1 = new GameObject();
        additionalRoad1.name = "Additional Road 1";
        additionalRoad1.transform.position = this.transform.position;
        additionalRoad1.transform.rotation = this.transform.rotation;
        additionalRoad1.transform.localScale = this.transform.localScale;
        additionalRoad1.transform.parent = this.transform;
    }

    private void InitializeAdditionalRoad2()
    {
        additionalRoad2 = new GameObject();
        additionalRoad2.name = "Additional Road 2";
        additionalRoad2.transform.position = this.transform.position;
        additionalRoad2.transform.rotation = this.transform.rotation;
        additionalRoad2.transform.localScale = this.transform.localScale;
        additionalRoad2.transform.parent = this.transform;
    }

    private void CreateVerticalRoad()
    {
        CreateRoad(verticalRoad, verticalRoadInitialPosition, true);
    }

    private void CreateHorizontalRoad()
    {
        CreateRoad(horizontalRoad, horizontalRoadInitialPosition, false);
    }

    private void CreateAdditionalRoad1()
    {
        CreateRoad(additionalRoad1, verticalRoad2InitialPosition, false); // Assuming it's horizontal
    }

    private void CreateAdditionalRoad2()
    {
        CreateRoad(additionalRoad2, horizontalRoad2InitialPosition, true); // Assuming it's vertical
    }

    private void CreateRoad(GameObject road, Vector3 initialPosition, bool isVertical)
    {
        Vector3 leftPavementPosition = initialPosition;
        Vector3 leftLanePosition = isVertical ?
            new Vector3((leftPavementPosition.x + pavementWidth + laneWidth),
                        initialPosition.y,
                        initialPosition.z) :
            new Vector3(initialPosition.x,
                        initialPosition.y,
                        (leftPavementPosition.z + pavementWidth + laneWidth));
        Vector3 roadMarkerLinePosition = isVertical ?
            new Vector3((leftLanePosition.x + laneWidth + roadMarkerLineWidth),
                        initialPosition.y,
                        initialPosition.z) :
            new Vector3(initialPosition.x,
                        initialPosition.y,
                        (leftLanePosition.z + laneWidth + roadMarkerLineWidth));
        Vector3 rightLanePosition = isVertical ?
            new Vector3((roadMarkerLinePosition.x + roadMarkerLineWidth + laneWidth),
                        initialPosition.y,
                        initialPosition.z) :
            new Vector3(initialPosition.x,
                        initialPosition.y,
                        (roadMarkerLinePosition.z + roadMarkerLineWidth + laneWidth));
        Vector3 rightPavementPosition = isVertical ?
            new Vector3((rightLanePosition.x + laneWidth + pavementWidth),
                        initialPosition.y,
                        initialPosition.z) :
            new Vector3(initialPosition.x,
                        initialPosition.y,
                        (rightLanePosition.z + laneWidth + pavementWidth));

        Vector3 pavementSize = isVertical ? new Vector3(pavementWidth, roadHeight, roadLength) : new Vector3(roadLength, roadHeight, pavementWidth);
        Vector3 laneSize = isVertical ? new Vector3(laneWidth, roadHeight, roadLength) : new Vector3(roadLength, roadHeight, laneWidth);
        Vector3 roadMarkerLineSize = isVertical ? new Vector3(roadMarkerLineWidth, roadHeight, roadLength) : new Vector3(roadLength, roadHeight, roadMarkerLineWidth);

        CreateRoadSegment(pavementSize, "Left Pavement", leftPavementPosition, PavementMaterialList(), road);
        CreateRoadSegment(laneSize, "Left Lane", leftLanePosition, LaneMaterialList(), road);
        CreateRoadSegment(roadMarkerLineSize, "Road Marker Line", roadMarkerLinePosition, RoadMarkerMaterialList(), road);
        CreateRoadSegment(laneSize, "Right Lane", rightLanePosition, LaneMaterialList(), road);
        CreateRoadSegment(pavementSize, "Right Pavement", rightPavementPosition, PavementMaterialList(), road);
    }

    private void CreateRoadSegment(Vector3 roadSegmentSize, string name,
                                   Vector3 position,
                                   List<Material> roadSegmentMaterialList,
                                   GameObject parentGameObject)
    {
        GameObject cube = new GameObject();
        cube.name = name;
        Cube cubeScript = cube.AddComponent<Cube>();
        cubeScript.UpdateSubmeshCount(1);
        cubeScript.UpdateSubmeshIndex(0, 0, 0, 0, 0, 0);
        cubeScript.UpdateCubeMaterialsList(roadSegmentMaterialList);
        cube.transform.rotation = this.transform.rotation;
        cube.transform.localScale = roadSegmentSize;
        cube.transform.position = position;
        cube.transform.parent = parentGameObject.transform;
    }

    private List<Material> PavementMaterialList()
    {
        List<Material> pavementMaterialList = new List<Material>();

        Material grayMaterial = new Material(Shader.Find("Specular"));
        grayMaterial.color = Color.gray;

        pavementMaterialList.Add(grayMaterial);

        return pavementMaterialList;
    }

    private List<Material> LaneMaterialList()
    {
        List<Material> laneMaterialList = new List<Material>();

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black;

        laneMaterialList.Add(blackMaterial);

        return laneMaterialList;
    }

    private List<Material> RoadMarkerMaterialList()
    {
        List<Material> roadMarkerMaterialList = new List<Material>();

        Material whiteMaterial = new Material(Shader.Find("Specular"));
        whiteMaterial.color = Color.white;

        roadMarkerMaterialList.Add(whiteMaterial);

        return roadMarkerMaterialList;
    }
}

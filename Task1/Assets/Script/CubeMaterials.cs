using System.Collections.Generic;
using UnityEngine;

public class CubeMaterials
{
    private List<Material> cubeMaterialsList = new List<Material>();

    public CubeMaterials()
    {
        Material redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = Color.red;

        Material blueMaterial = new Material(Shader.Find("Specular"));
        blueMaterial.color = Color.blue;

        Material greenMaterial = new Material(Shader.Find("Specular"));
        greenMaterial.color = Color.green;

        Material yellowMaterial = new Material(Shader.Find("Specular"));
        yellowMaterial.color = Color.yellow;

        Material whiteMaterial = new Material(Shader.Find("Specular"));
        whiteMaterial.color = Color.white;

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black;

        Material bricksMaterial = Resources.Load<Material>("Materials/bricks");

        Material GrassMaterial = Resources.Load<Material>("Materials/Grass");


        // Add all materials to the list
        cubeMaterialsList.Add(redMaterial);
        cubeMaterialsList.Add(blueMaterial);
        cubeMaterialsList.Add(greenMaterial);
        cubeMaterialsList.Add(yellowMaterial);
        cubeMaterialsList.Add(whiteMaterial);
        cubeMaterialsList.Add(blackMaterial);
        cubeMaterialsList.Add(bricksMaterial); 
        cubeMaterialsList.Add(GrassMaterial);
    }

    public List<Material> GetCubeMaterialsList()
    {
        return cubeMaterialsList;
    }
}

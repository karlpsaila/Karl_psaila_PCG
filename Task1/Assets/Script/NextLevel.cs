using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided with a cube with the "Cube" tag
        if (collision.gameObject.tag == "Player")
        {
            // Load Level2 scene
            SceneManager.LoadScene("City2");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerscript : MonoBehaviour
{
    public float speed = 5.0f; // Movement speed

    private Vector3 direction; // Store movement direction

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D keys)
        float verticalInput = Input.GetAxis("Vertical"); // Get vertical input (W/S keys)

        direction = new Vector3(horizontalInput, 0.0f, verticalInput).normalized; // Normalize input
    }

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime); // Move the car based on direction and speed
    }
}

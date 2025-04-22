using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : MonoBehaviour
{
    float startingRotation;
    float time;

    public float rotationRange = 45f;
    public float rotationSpeed = 0.5f;

    void Start()
    {
        startingRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        time += Time.deltaTime * rotationSpeed;
        float deltaRotation = Mathf.Sin(time) * rotationRange;
        float newYRotation = startingRotation + deltaRotation;

        // Set absolute rotation
        transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
    }
}
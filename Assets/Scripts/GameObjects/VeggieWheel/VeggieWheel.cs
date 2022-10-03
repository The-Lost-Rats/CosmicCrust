using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieWheel : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 10.0f;

    [SerializeField]
    public float maxSpeed = 300.0f;

    [SerializeField]
    public float direction = 1;

    private float angle = 0.0f;

    public float initialSpeed;

    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local Z axis at defined speed
        angle += rotationSpeed * direction * Time.deltaTime;
        if (angle > 360)
        {
            angle = 0;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField]
    private float speed = 1;

    void Update()
    {
        Move();
    }

    void Move()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "ConveyorBeltMovable")
            {
                Vector3 pos = child.position;
                pos.y -= 0.001f * speed;
                child.position = pos;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject beltPrefab;

    private const float beltTopBound = 1;
    private const float beltBottomBound = -9;
    private const float newBeltOffset = 12.15f;

    [Range(1, 5)]
    [SerializeField]
    private float speed = 1;

    void Update()
    {
        Move();
        CheckBelt();
    }

    void Move()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "ConveyorBeltMovable")
            {
                Vector3 pos = child.position;
                pos.y -= speed * Time.deltaTime;
                child.position = pos;
            }
        }
    }

    void CheckBelt()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "ConveyorBeltMovable")
            {
                Belt beltData = child.GetComponent<Belt>();
                if (beltData != null)
                {
                    if (!beltData.spawnedNextBelt && child.localPosition.y <= beltTopBound)
                    {
                        Instantiate(beltPrefab, child.position + new Vector3(0, newBeltOffset), Quaternion.identity, transform);
                        beltData.spawnedNextBelt = true;
                    }
                }

                if (child.localPosition.y <= beltBottomBound)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}

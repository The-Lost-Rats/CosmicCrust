using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject beltPrefab;
    public Pizza pizzaPrefab;

    private const float beltTopBound = 73;
    private const float beltBottomBound = -73;
    private const float newBeltOffset = 130;

    [Range(0.01f, 1)]
    [SerializeField]
    private float speed = 0.01f;

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
                        GameObject newBelt = Instantiate(beltPrefab, child.position, Quaternion.identity, transform);
                        newBelt.transform.localPosition = new Vector2(newBelt.transform.localPosition.x, child.localPosition.y + newBeltOffset);
                        beltData.spawnedNextBelt = true;
                    }
                }

                if (child.localPosition.y <= beltBottomBound)
                {
                    if (beltData != null)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    else
                    {
                        PlayController.instance.EndLevel();
                    }
                }
            }
        }
    }

    public Pizza CreatePizza()
    {
        Pizza pizza = Instantiate(pizzaPrefab, transform.position, Quaternion.identity, transform);
        pizza.transform.localPosition = new Vector2(transform.localPosition.x, beltTopBound);
        pizza.name = "Pizza";
        return pizza;
    }
}

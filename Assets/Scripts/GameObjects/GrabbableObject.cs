using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    [Range(5, 20)]
    [SerializeField]
    protected float fallingSpeed = 5;

    protected bool isFollowingMouse = true;

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OnUpdate(mousePosition);
    }

    protected void OnUpdate(Vector2 mousePos)
    {
        if (isFollowingMouse)
        {
            transform.position = mousePos;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y -= fallingSpeed * Time.deltaTime;
            transform.position = pos;
            if (pos.y <= -9)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}

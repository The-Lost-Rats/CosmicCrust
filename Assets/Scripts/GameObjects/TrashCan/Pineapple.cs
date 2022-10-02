using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : GrabbableObject
{
    [Range(5, 20)]
    [SerializeField]
    private float fallingSpeed = 5;

    private bool isFollowingMouse = true;

    protected override void OnUpdate(Vector2 mousePos)
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

    public void DropPineapple(bool onPizza)
    {
        if (onPizza)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            isFollowingMouse = false;
        }
    }
}

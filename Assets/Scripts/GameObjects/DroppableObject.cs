using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : GrabbableObject
{
    protected override void OnUpdate(Vector2 mousePos)
    {
        transform.position = mousePos;
    }

    public void Drop(bool onPizza)
    {
        if (onPizza)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            DropObject();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : GrabbableObject
{
    protected override void OnUpdate(Vector2 mousePos)
    {
        transform.position = mousePos;
    }

    public void DropPineapple(bool onPizza)
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

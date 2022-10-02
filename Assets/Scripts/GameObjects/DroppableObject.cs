using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : GrabbableObject
{
    public void Drop(bool onPizza)
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

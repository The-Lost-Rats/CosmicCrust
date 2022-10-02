using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : GrabbableObject
{
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

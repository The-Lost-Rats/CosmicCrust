using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    protected abstract void OnUpdate(Vector2 mousePos);

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OnUpdate(mousePosition);
    }
}

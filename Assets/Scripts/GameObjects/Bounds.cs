using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public bool pizzaInBounds = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Pizza")
        {
            pizzaInBounds = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Pizza")
        {
            pizzaInBounds = false;
        }
    }
}

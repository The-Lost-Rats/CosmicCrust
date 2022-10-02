using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : InteractableObject
{
    public override bool isInteractable { get { return false; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    public void AddTopping(string topping)
    {
        Debug.Log("Adding topping " + topping);
    }
}

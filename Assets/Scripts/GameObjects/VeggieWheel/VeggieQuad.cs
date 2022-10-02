using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieQuad : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject objectPrefab;

    private DroppableObject currInstance;

    [SerializeField]
    public string toppingName = "Mushroom";

    private Vector3 originalScale = new Vector3(0.0f, 0.0f, 0.0f);

    public override void OnEnter()
    {
        // store old scale and mult by 1.2
        originalScale = transform.localScale;
        transform.localScale = originalScale * 1.2f;
    }

    public override void OnExit()
    {
        transform.localScale = originalScale;
    }

    public override InputController.InputState OnClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currInstance = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
        return InputController.InputState.Grabbing;
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        bool onPizza = false;
        foreach (InteractableObject interactable in interactedObjects)
        {
            if (interactable.name == "Pizza")
            {
                onPizza = true;
                (interactable as Pizza).AddTopping(toppingName);
            }
        }
        currInstance.Drop(onPizza);
        currInstance = null;
        return InputController.InputState.Default;
    }
}

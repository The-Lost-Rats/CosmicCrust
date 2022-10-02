using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerTopping : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject toppingPrefab;
    private DroppableObject currInstance;

    [SerializeField] private Constants.GenericToppings toppingType = Constants.GenericToppings.Anchovy;

    private Vector3 originalScale = Vector3.zero;

    public override void OnEnter()
    {
        // store old scale and mult by 1.1
        originalScale = transform.localScale;
        transform.localScale = originalScale * 1.1f;
    }

    public override void OnExit()
    {
        transform.localScale = originalScale;
    }

    public override InputController.InputState OnClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currInstance = Instantiate(toppingPrefab, mousePosition, Quaternion.identity);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer toppingRenderer = currInstance.GetComponent<SpriteRenderer>();
        Texture2D currTexture = sr.sprite.texture;
        toppingRenderer.sprite = Sprite.Create(currTexture, new Rect(0, 0, currTexture.width, currTexture.height), new Vector2(0.5f, 0.5f));
        return InputController.InputState.Grabbing;
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        bool onPizza = false;
        foreach (InteractableObject interactable in interactedObjects)
        {
            if (interactable.name == "Pizza")
            {
                PlayController.instance.AddGenericTopping(toppingType);
                onPizza = true;
            }
        }
        currInstance.Drop(onPizza);
        currInstance = null;
        return InputController.InputState.Default;
    }
}

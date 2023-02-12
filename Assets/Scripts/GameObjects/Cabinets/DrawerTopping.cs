using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerTopping : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject toppingPrefab;
    public Texture2D toppingTexture;
    private DroppableObject currInstance;

    [SerializeField] private Types.GenericToppings toppingType = Types.GenericToppings.Anchovy;

    public override void OnEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override InputController.InputState OnClick()
    {
        SoundController.scInstance.PlaySingle("itemGrab");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currInstance = Instantiate(toppingPrefab, mousePosition, Quaternion.identity);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer toppingRenderer = currInstance.GetComponent<SpriteRenderer>();
        toppingRenderer.sprite = Sprite.Create(toppingTexture, new Rect(0, 0, toppingTexture.width, toppingTexture.height), new Vector2(0.5f, 0.5f));
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

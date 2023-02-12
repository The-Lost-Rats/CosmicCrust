using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject pineapplePrefab;

    public Texture2D texture;

    private DroppableObject currPineapple;

    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    public override void OnExit()
    {
        transform.localScale = new Vector3(1, 1);
    }

    public override InputController.InputState OnClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPineapple = Instantiate(pineapplePrefab, mousePosition, Quaternion.identity);
        currPineapple.SetDroppableSprite(texture);
        SoundController.scInstance.PlaySingle("itemGrab");
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
                PlayController.instance.AddPineapple();
            }
        }
        currPineapple.Drop(onPizza);
        currPineapple = null;
        return InputController.InputState.Default;
    }
}

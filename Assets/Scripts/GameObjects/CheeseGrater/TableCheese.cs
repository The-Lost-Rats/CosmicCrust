using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCheese : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    public GrabCheese grabCheesePrefab;
    private GrabCheese grabbedCheese;

    public Types.CheeseTypes cheeseType;

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
        grabbedCheese = Instantiate(grabCheesePrefab, mousePosition, Quaternion.identity, transform);
        grabbedCheese.CopyCheeseValues(this);
        SetVisible(false);
        SoundController.scInstance.PlaySingle("itemGrab");
        return InputController.InputState.Grabbing;
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        GameObject.Destroy(grabbedCheese.gameObject);
        SetVisible(true);
        return InputController.InputState.Default;
    }

    private void SetVisible(bool visible)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = visible ? 1 : 0;
        renderer.color = color;
    }
}

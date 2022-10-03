using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetDrawer : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    private const float drawerHeight = .19f;

    [SerializeField] private Constants.GenericToppings topping;

    private bool isMoving;
    private float mouseOffset;
    private float[] movementBounds;

    private void Start()
    {
        movementBounds = new float[]{transform.position.y - drawerHeight, transform.position.y};
    }

    public override InputController.InputState OnClick()
    {
        isMoving = true;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseOffset = transform.position.y - mousePosition.y;
        return InputController.InputState.Grabbing;
    }
 
    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        isMoving = false;
        return InputController.InputState.Default;
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Add an offset so the anchor point of the sprite can be ignored
            float posY = Mathf.Clamp(mousePosition.y + mouseOffset, movementBounds[0], movementBounds[1]);
            transform.position = new Vector2(transform.position.x, posY);
        }
    }
}

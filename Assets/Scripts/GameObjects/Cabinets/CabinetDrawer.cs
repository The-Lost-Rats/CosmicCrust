using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetDrawer : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    private const float drawerHeight = .19f;

    [SerializeField] private IngredientTypes.GenericToppings topping;

    private bool isMoving;
    private float mouseOffset;
    private float[] movementBounds;
    private bool playedSound;

    private void Start()
    {
        movementBounds = new float[]{transform.position.y - drawerHeight, transform.position.y};
    }

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
        isMoving = true;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseOffset = transform.position.y - mousePosition.y;
        playedSound = false;
        return InputController.InputState.Grabbing;
    }
 
    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        isMoving = false;
        return InputController.InputState.Default;
    }

    protected override void SceneUpdate()
    {
        if (isMoving)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Add an offset so the anchor point of the sprite can be ignored
            float posY = Mathf.Clamp(mousePosition.y + mouseOffset, movementBounds[0], movementBounds[1]);
            if (!playedSound && posY != transform.position.y)
            {
                SoundController.scInstance.PlaySingle("drawerOpening");
            }
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, movementBounds[1], transform.position.z);
    }
}

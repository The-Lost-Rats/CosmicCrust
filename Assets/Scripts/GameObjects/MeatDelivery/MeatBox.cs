using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle click to open and then pick up meat
public class MeatBox : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject objectPrefab;

    private DroppableObject currInstance;

    [SerializeField]
    private Constants.Meats meatType = Constants.Meats.Pepperoni;

    [SerializeField]
    public Sprite openSprite;

    private bool isOpen = false;
    private bool isGrabbable = false;

    // private Vector3 originalScale = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        // originalScale = transform.localScale;
    }

    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    public override void OnExit()
    {
        transform.localScale = new Vector3(1, 1);
    }

    public override bool IsGrabbable()
    {
        return isGrabbable;
    }

    public override InputController.InputState OnClick()
    {
        if (!isOpen)
        {
            // Open box
            isOpen = true;

            // Update sprite
            GetComponent<SpriteRenderer>().sprite  = openSprite;

            return InputController.InputState.Default;
        }
        else
        {
            if (!isGrabbable)
            {
                isGrabbable = true;
            }
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currInstance = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
            return InputController.InputState.Grabbing;
        }
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        if (isOpen && currInstance != null)
        {
            bool onPizza = false;
            foreach (InteractableObject interactable in interactedObjects)
            {
                if (interactable.name == "Pizza")
                {
                    onPizza = true;
                    PlayController.instance.AddMeat(meatType);
                }
            }
            currInstance.Drop(onPizza);
            currInstance = null;
            return InputController.InputState.Default;
        }
        else
        {
            return InputController.InputState.Default;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle click to open and then pick up meat
public class MeatBox : InteractableObject
{
    // Player can interact with box
    public override bool isInteractable { get { return true; }}

    // Meat from box can interact with pizza
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    // Object prefab to instantiate (meat)
    public DroppableObject objectPrefab;

    // Sprite for meat
    private Texture2D meatImage;

    // Current grabbed object
    private DroppableObject currInstance;

    // Box meat type
    [SerializeField]
    private Types.Meats meatType = Types.Meats.Pepperoni;

    // Box open sprite to switch to when opened
    [SerializeField]
    public Sprite openSprite;

    // Bool for if box is open
    private bool isOpen = false;

    // Box is not grabbable
    private bool isGrabbable = false;

    // Override method
    public override bool IsGrabbable()
    {
        return isGrabbable;
    }

    // Override method to scale sprite when hover over
    public override void OnEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f);
    }

    // Set scale back to normal
    public override void OnExit()
    {
        transform.localScale = Vector3.one;
    }

    // On click open box and allow player to grab meat
    public override InputController.InputState OnClick()
    {
        if (!isOpen)
        {
            // Open box
            isOpen = true;

            // Update sprite
            GetComponent<SpriteRenderer>().sprite  = openSprite;
            SoundController.scInstance.PlaySingle("meatBoxOpen");

            return InputController.InputState.Default;
        }
        else
        {
            if (!isGrabbable)
            {
                isGrabbable = true;
            }
            SoundController.scInstance.PlaySingle("itemGrab");
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currInstance = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
            currInstance.SetDroppableSprite(meatImage);

            return InputController.InputState.Grabbing;
        }
    }

    // On release check if we are on pizza
    // TODO: can we make this generic? All toppings need to check if they are on pizza...
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
        }

        return InputController.InputState.Default;
    }

    public void SetMeat(Types.Meats type, Texture2D image)
    {
        meatImage = image;
        meatType = type;
    }
}

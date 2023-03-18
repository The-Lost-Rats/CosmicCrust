using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add option to disable wateringcan
public class PepperPlant : InteractableObject
{
    public override bool isInteractable { get { return true; }}

    // TODO: i feel like most of this should be empty list because this creates an entity to drop on a pizza, right? It is not, itself, able to interact with the pizza.
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    // Pepper to grab and drop
    public DroppableObject objectPrefab;

    public Texture2D fruitImage;

    // Current pepper instance
    private DroppableObject currInstance;

    // Array of plant stages as we water plant
    // plantStages[0] should be ungrown plant
    // plantStages[n] should be fully grown plant
    [SerializeField]
    private List<Sprite> plantStages;

    // This is the pepper to spawn on the pepper plant that the player can grab
    [SerializeField]
    private GameObject fruit;

    // Initial scale of fruit
    private Vector3 fruitLocalScale;

    // Type of pepper plant
    [SerializeField] private IngredientTypes.Peppers instanceType = IngredientTypes.Peppers.Serrano;

    // Is done growing bool
    private bool isGrown = false;

    // This is to track the total time watered
    private float accTime = 0.0f;

    // Time to reach to finish growing
    [SerializeField]
    private float totalTime = 1.5f;

    // Timer sprite that appears when watering plant
    [SerializeField]
    private GameObject timer;

    // How big and small we want the timer to oscillate between
    [SerializeField]
    private Vector3 upperScaleLimit = new Vector3(0.6f, 0.6f, 0.0f);

    [SerializeField]
    private Vector3 lowerScaleLimit = new Vector3(0.2f, 0.2f, 0.0f);

    // To see if timer is growing or shrinking right now
    private Vector3 currentBoundGoal;

    // Step size to grow and shrink timer
    [SerializeField]
    private float timerScaleStep = 0.006f;

    // If we are watering plant
    private bool isWatering = false;

    public override void OnEnter()
    {
        // Only make bigger if we are fully grown to indicate player can interact with plant
        if (isGrown)
        {
            fruit.gameObject.transform.localScale = Vector3.Scale(fruitLocalScale, new Vector3(1.1f, 1.1f, 1.1f));
        }
    }

    // Init
    void Awake()
    {
        // Timer first grows and then shrinks
        currentBoundGoal = upperScaleLimit;
        upperScaleLimit.z = timer.transform.localScale.z; // I don't remember why I did this...
        lowerScaleLimit.z = timer.transform.localScale.z; // I don't remember why I did this...
        timer.SetActive(false); // Timer is hidden by default
        fruit.SetActive(false); // Pepper is hidden by default

        fruitLocalScale = fruit.gameObject.transform.localScale;
    }

    public override void OnExit()
    {
        if (isGrown)
        {
            fruit.gameObject.transform.localScale = fruitLocalScale;
        }
    }

    protected override void SceneUpdate()
    {
        // Only do this stuff if the plant isn't grown
        // Plant can do nothing once it is grown
        if (!isGrown)
        {
            // If we have watered plant for total time
            if (accTime >= totalTime)
            {
                // Set is grown to true and hide timer
                isGrown = true;
                isWatering = false;
                timer.SetActive(false);

                // Show fruit!
                fruit.SetActive(true);
            }
            // Choose next sprite for plant based on number of sprites and time watered
            else if(isWatering)
            {
                int spriteIndex = (int)(plantStages.Count * accTime / totalTime);
                GetComponent<SpriteRenderer>().sprite = plantStages[spriteIndex];
            }

            // If watering plant, water plant
            if (isWatering)
            {
                WaterPlant();
            }
        }
    }

    //Overlapping a collider 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WaterCollider" && !isGrown)
        {
            isWatering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WaterCollider" && !isGrown)
        {
            isWatering = false;
        }
    }

    private void WaterPlant()
    {
        // Watering time increments
        accTime += Time.deltaTime;
        if (!timer.activeSelf)
        {
            // If timer hasn't been shown yet, show it
            timer.SetActive(true);
        }

        // Timer is active
        if (timer.activeSelf)
        {
            // Timer is active so lets scale up and down as an animation
            timer.transform.localScale = Vector3.MoveTowards(timer.transform.localScale, currentBoundGoal, timerScaleStep);
            
            // Have we reached goal?
            if (timer.transform.localScale == currentBoundGoal)
            {
                // Swap goal to either now shrink or grow timer sprite
                if (currentBoundGoal == upperScaleLimit)
                {
                    currentBoundGoal = lowerScaleLimit;
                }
                else
                {
                    currentBoundGoal = upperScaleLimit;
                }
            }
        }
    }

    public override InputController.InputState OnClick()
    {
        // If we are grown, allow player to get pepper
        if (isGrown)
        {
            SoundController.scInstance.PlaySingle("itemGrab");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currInstance = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
            currInstance.SetDroppableSprite(fruitImage);
        }
        return InputController.InputState.Grabbing;
    }

    public override InputController.InputState OnRelease(List<InteractableObject> interactedObjects)
    {
        if (currInstance != null)
        {
            bool onPizza = false;
            foreach (InteractableObject interactable in interactedObjects)
            {
                if (interactable.name == "Pizza")
                {
                    PlayController.instance.AddPepper(instanceType);
                    onPizza = true;
                }
            }
            currInstance.Drop(onPizza);
            currInstance = null;
        }

        return InputController.InputState.Default;
    }

    // Reset everything
    public void Reset()
    {
        isGrown = false;
        isWatering = false;
        accTime = 0;
        timer.SetActive(false);
        fruit.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = plantStages[0];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperPlant : InteractableObject
{
    public override bool isInteractable { get { return true; }}
    public override List<string> interactableObjects { get { return new List<string>{ "Pizza" }; }}

    public DroppableObject objectPrefab;

    private DroppableObject currInstance;

    [SerializeField]
    private Sprite grownPlant;

    [SerializeField]
    private Sprite adultPlant;

    [SerializeField]
    private Sprite ungrownPlant;

    [SerializeField]
    private GameObject fruit;

    [SerializeField] private Constants.Peppers instanceType = Constants.Peppers.Serrano;

    private bool isGrown = false;
    private float accTime = 0.0f;

    [SerializeField]
    private float totalTime = 1.5f;

    [SerializeField]
    private GameObject timer;

    // [SerializeField]
    private Vector3 upperScaleLimit = new Vector3(0.6f, 0.6f, 0.0f);
    // [SerializeField]
    private Vector3 lowerScaleLimit = new Vector3(0.2f, 0.2f, 0.0f);

    private Vector3 currentBoundGoal;

    private bool isWatering = false;

    public override void OnEnter()
    {
        if (isGrown)
        {
            transform.localScale = new Vector3(1.1f, 1.1f);
        }
    }

    void Awake()
    {
        currentBoundGoal = upperScaleLimit;
        upperScaleLimit.z = timer.transform.localScale.z;
        lowerScaleLimit.z = timer.transform.localScale.z;
        timer.SetActive(false);
        fruit.SetActive(false);
    }

    public override void OnExit()
    {
        transform.localScale = new Vector3(1, 1);
    }

    void Update()
    {
        if (accTime > totalTime)
        {
            accTime = 0;
            isGrown = true;
            timer.SetActive(false);

            // Show fruit!
            fruit.SetActive(true);
        }
        else if (accTime > 0.66f * totalTime)
        {
            // 2/3 way through
            GetComponent<SpriteRenderer>().sprite  = adultPlant;
        }
        else if (accTime > 0.33f * totalTime)
        {
            // 1/3 way through
            GetComponent<SpriteRenderer>().sprite  = grownPlant;
        }

        if (!isGrown && isWatering)
        {
            WaterPlant();
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
        accTime += Time.deltaTime;
        if (!timer.activeSelf)
        {
            timer.SetActive(true);
        }

        if (timer.activeSelf)
        {
            // Timer is active so lets scale up and down as an anim
            timer.transform.localScale = Vector3.MoveTowards(timer.transform.localScale, currentBoundGoal, 0.006f);
            
            // Have we reached goal?
            if (timer.transform.localScale == currentBoundGoal)
            {
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
        if (isGrown)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currInstance = Instantiate(objectPrefab, mousePosition, Quaternion.identity);
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

    public void Reset()
    {
        isGrown = false;
        isWatering = false;
        accTime = 0;
        timer.SetActive(false);
        fruit.SetActive(false);
        GetComponent<SpriteRenderer>().sprite  = ungrownPlant;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public static PlayController instance = null;

    public ConveyorBelt conveyorBelt;
    public Pizza displayPizza;
    public ToppingsDisplay toppingsDisplay;

    [SerializeField] private List<PizzaOrder> pizzaOrders = null;

    private int pizzaIndex;
    private int score;
    private Pizza currPizza;
    private PizzaOrder currPizzaOrder;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        InitalizeGame();
    }

    private List<int> ValidatePizzaOrders()
    {
        List<int> invalidLevels = new List<int>();
        for (int i = 0; i < pizzaOrders.Count; i++)
        {
            PizzaOrder order = pizzaOrders[i];
            if (order != null)
            {
                int numIngredients = order.meats.Count + order.peppers.Count +
                    order.vegetables.Count + order.genericToppings.Count + (order.hasPineapple ? 1 : 0);
                if (numIngredients > Constants.maxIngredients)
                {
                    invalidLevels.Add(i);
                }
            }
            else
            {
                invalidLevels.Add(i);
            }
        }
        return invalidLevels;
    }

    private void InitalizeGame()
    {
        List<int> invalidLevels = ValidatePizzaOrders();
        if (invalidLevels.Count > 0)
        {
            string invalidLevelsStr = "";
            foreach (int level in invalidLevels)
            {
                invalidLevelsStr += (invalidLevelsStr != "" ? ", " : "") + level;
            }
            Debug.LogError("Invalid levels! " + invalidLevelsStr);
            return;
        }

        // Initialize the game
        pizzaIndex = 0;
        score = 0;

        StartLevel();
    }

    private void StartLevel()
    {
        currPizza = conveyorBelt.CreatePizza();
        currPizzaOrder = pizzaOrders[pizzaIndex];

        displayPizza.gameObject.SetActive(true);
        displayPizza.ResetPizza();
        displayPizza.SetPizza(currPizzaOrder);

        toppingsDisplay.SetPizzaOrder(currPizzaOrder);

        // Ship meat
        DeliveryManager.dmInstance.DeliverMeat(currPizzaOrder.meats);
    
        // Init is done for level, so let's start the timer! (that doesn't really mean anything haha)
        TimerController.tcInstance.StartTimer();
    }

    public void EndLevel()
    {
        // TODO Check if complete and do other stuff
        bool pizzaCorrect = currPizza.IsOrderCorrect(currPizzaOrder);
        if (pizzaCorrect)
        {
            score++;
            Debug.Log("Pizza correct!");
        }
        else
        {
            Debug.Log("Pizza incorrect");
        }
        displayPizza.gameObject.SetActive(false);
        toppingsDisplay.ResetPizza();

        // pizzaIndex++; // TODO For now I just want to test with pizza 1
        GameObject.Destroy(currPizza.gameObject);

        // DESTROY THE PLANTS
        PlantManager.pmInstance.WipePlants();

        // Increase belt speed!
        BeltController.bcInstance.UpdateSpeed();

        // Increase veggie wheel speed/or change direction
        WheelController.wcInstance.UpdateSpeedAndDirection();

        Invoke("StartLevel", 1); // TODO Does using Invoke work with pause?
    }

    public bool SetSauce(Constants.Sauces sauce)
    {
        if (currPizzaOrder.sauce != sauce)
        {
            return false;
        }
        return currPizza.SetSauce(sauce);
    }

    public bool SetCheese(Constants.CheeseTypes cheese)
    {
        if (currPizzaOrder.cheese != cheese)
        {
            return false;
        }
        return currPizza.SetCheese(cheese);
    }

    public bool AddMeat(Constants.Meats meat)
    {
        if (!currPizzaOrder.meats.Contains(meat))
        {
            return false;
        }
        return currPizza.AddMeat(meat);
    }

    public bool AddPepper(Constants.Peppers pepper)
    {
        if (!currPizzaOrder.peppers.Contains(pepper))
        {
            return false;
        }
        return currPizza.AddPepper(pepper);
    }

    public bool AddVegetable(Constants.Vegetables vegetable)
    {
        if (!currPizzaOrder.vegetables.Contains(vegetable))
        {
            return false;
        }
        return currPizza.AddVegetable(vegetable);
    }

    public bool AddGenericTopping(Constants.GenericToppings topping)
    {
        if (!currPizzaOrder.genericToppings.Contains(topping))
        {
            return false;
        }
        return currPizza.AddGenericTopping(topping);
    }

    public bool AddPineapple()
    {
        if (!currPizzaOrder.hasPineapple)
        {
            return false;
        }
        return currPizza.AddPineapple();
    }
}

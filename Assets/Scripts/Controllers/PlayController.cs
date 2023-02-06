using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : ISceneController
{
<<<<<<< HEAD
    override protected GameState GetGameState() { return GameState.PLAY; }

    public static PlayController instance = null;
=======
    public static PlayController pcInstance = null;
>>>>>>> 12a08ae (Add beginnings of universal pause)

    public ConveyorBelt conveyorBelt;
    public Pizza displayPizza;
    public ToppingsDisplay toppingsDisplay;

    [SerializeField] private List<PizzaOrder> pizzaOrders = null;

    public int score { get; private set; }

    private int pizzaIndex;
    private const int MAX_SCORE = 999;
    private const int MAX_LIFE = 3;
    private int numLives;

    private Pizza currPizza;
    private PizzaOrder currPizzaOrder;

    void Start()
    {
        if (pcInstance == null)
        {
            pcInstance = this;
        }
        else if (pcInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        InitalizeGame();
    }

    override protected void SceneUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.instance.ChangeState(GameState.PAUSE);
        }
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

    public void Restart()
    {
        InitalizeGame();
    }

    private void InitalizeGame()
    {
        Time.timeScale = 1.0f;

        // TODO: we probably don't want to revalidate the same orders everytime
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
        numLives = MAX_LIFE;

        // Update score displayed by chef bot
        UIController.uicInstance.SetCurrentScore(score);

        // Initialize hearts
        for ( int i = 0; i < numLives; i++ ) {
            UIController.uicInstance.SetHearts( false );
        }

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
        DeliveryManager.dmInstance.DeliverMeat(currPizzaOrder.meats, currPizzaOrder.numMeatToShip);
    }

    public void EndLevel()
    {
        // TODO Check if complete and do other stuff
        bool pizzaCorrect = currPizza.IsOrderCorrect(currPizzaOrder);
        if (pizzaCorrect)
        {
            score += 4;
            if (score > MAX_SCORE)
            {
                score = MAX_SCORE;
            }
            // Update score
            UIController.uicInstance.SetCurrentScore(score);

            SoundController.scInstance.PlaySingle("pizzaCorrect");

            pizzaIndex++;

            Debug.Log("Pizza correct!");
        }
        else
        {
            UIController.uicInstance.SetHearts( true );
            numLives--;

            SoundController.scInstance.PlaySingle("pizzaWrong");

            Debug.Log("Pizza incorrect");
        }
        displayPizza.gameObject.SetActive(false);
        toppingsDisplay.ResetPizza();

        GameObject.Destroy(currPizza.gameObject);

        // DESTROY THE PLANTS
        PlantManager.pmInstance.WipePlants();

        // Wipe the meat!
        DeliveryTable.dtInstance.WipeBoxes();

        if (numLives == 0)
        {
            // You lost :(
            UIController.uicInstance.SetFinalScore(score);
            GameController.instance.ChangeState(GameState.GAME_OVER_SCREEN);
            ResetScene();
        }
        else 
        if (pizzaIndex >= pizzaOrders.Count - 1)
        {
            // You won!
            GameController.instance.ChangeState(GameState.WIN_SCREEN);
            ResetScene();
        }
        else
        {
            Invoke("StartLevel", 1); // TODO Does using Invoke work with pause?
        }
    }

    private void ResetScene()
    {
        // TODO: there are 100% things I have missed to reset -> must be a better way to do this
        DrawerController.dcInstance.Reset();

        PlantManager.pmInstance.Reset();
        DeliveryManager.dmInstance.Reset(); // Might catch lingering boxes in some weird state -> why did I do it this way god why
    }

    public bool SetSauce(Constants.Sauces sauce)
    {
        if (currPizzaOrder.sauce != sauce)
        {
            return false;
        }
        bool success = currPizza.SetSauce(sauce);
        if (success)
        {
            toppingsDisplay.SetSauceComplete();
        }
        return success;
    }

    public bool SetCheese(Constants.CheeseTypes cheese)
    {
        if (currPizzaOrder.cheese != cheese)
        {
            return false;
        }
        bool success = currPizza.SetCheese(cheese);
        if (success)
        {
            toppingsDisplay.SetCheeseComplete();
        }
        return success;
    }

    public bool AddMeat(Constants.Meats meat)
    {
        if (!currPizzaOrder.meats.Contains(meat))
        {
            return false;
        }
        bool success = currPizza.AddMeat(meat);
        if (success)
        {
            toppingsDisplay.SetMeatComplete(meat);
        }
        return success;
    }

    public bool AddPepper(Constants.Peppers pepper)
    {
        if (!currPizzaOrder.peppers.Contains(pepper))
        {
            return false;
        }
        bool success = currPizza.AddPepper(pepper);
        if (success)
        {
            toppingsDisplay.SetPepperComplete(pepper);
        }
        return success;
    }

    public bool AddVegetable(Constants.Vegetables vegetable)
    {
        if (!currPizzaOrder.vegetables.Contains(vegetable))
        {
            return false;
        }
        bool success = currPizza.AddVegetable(vegetable);
        if (success)
        {
            toppingsDisplay.SetVegetableComplete(vegetable);
        }
        return success;
    }

    public bool AddGenericTopping(Constants.GenericToppings topping)
    {
        if (!currPizzaOrder.genericToppings.Contains(topping))
        {
            return false;
        }
        bool success = currPizza.AddGenericTopping(topping);
        if (success)
        {
            toppingsDisplay.SetGenericToppingComplete(topping);
        }
        return success;
    }

    public bool AddPineapple()
    {
        if (!currPizzaOrder.hasPineapple)
        {
            return false;
        }
        bool success = currPizza.AddPineapple();
        if (success)
        {
            toppingsDisplay.SetPineappleComplete();
        }
        return success;
    }
}

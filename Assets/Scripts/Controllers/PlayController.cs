using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : ISceneController
{
    override protected GameState GetGameState() { return GameState.PLAY; }

    public static PlayController instance = null;

    public ConveyorBelt conveyorBelt;
    public Pizza displayPizza;
    public ToppingsDisplay toppingsDisplay;

    [SerializeField] private List<PizzaOrder> pizzaOrders = null;

    private int pizzaIndex;

    private const int MAX_LIFE = 3;
    private int numLives;

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
                if (numIngredients > Constants.MAX_INGREDIENTS)
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
        numLives = MAX_LIFE;

        // Update score displayed by chef bot
        UIController.uicInstance.SetCurrentScore(ScoreController.scInstance.GetCurrentScore());

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
        DeliveryManager.dmInstance.DeliverMeat(currPizzaOrder.meats, currPizzaOrder.numBoxesToShip);
    }

    public void EndLevel()
    {
        // TODO Check if complete and do other stuff
        bool pizzaCorrect = currPizza.IsOrderCorrect(currPizzaOrder);
        if (pizzaCorrect)
        {
            // TODO: think about how we want to handle communication across components?
            // maybe we want to maek a score manager and an event system that triggers score controller?
            // we could lessen our dependency on singletons that way maybe
            if (ScoreController.scInstance.UpdateScore(1) > Constants.MAX_SCORE)
            {
                ScoreController.scInstance.SetScore(Constants.MAX_SCORE);
            }
            // Update displayed score
            UIController.uicInstance.SetCurrentScore(ScoreController.scInstance.GetCurrentScore());

            AudioController.Instance.PlayOneShotAudio(SoundEffectKeys.PizzaCorrect);

            pizzaIndex++;

            Debug.Log("Pizza correct!");
        }
        else
        {
            UIController.uicInstance.SetHearts( true );
            numLives--;

            AudioController.Instance.PlayOneShotAudio(SoundEffectKeys.PizzaIncorrect);

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
            // TODO: do we still need this final high score stuff? with the new win/lose screens I think we just render text now?
            UIController.uicInstance.SetFinalScore(ScoreController.scInstance.GetCurrentScore());
            ScoreController.scInstance.SaveHighScore();
            GameController.instance.ChangeState(GameState.GAME_OVER_SCREEN);
        }
        else 
        if (pizzaIndex >= pizzaOrders.Count - 1)
        {
            // You won!
            GameController.instance.ChangeState(GameState.WIN_SCREEN);
        }
        else
        {
            Invoke("StartLevel", 1); // TODO Does using Invoke work with pause?
        }
    }

    public bool SetSauce(IngredientTypes.Sauces sauce)
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

    public bool SetCheese(IngredientTypes.CheeseTypes cheese)
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

    public bool AddMeat(IngredientTypes.Meats meat)
    {
        if (!Utilities.containsMeat(currPizzaOrder.meats, meat))
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

    public bool AddPepper(IngredientTypes.Peppers pepper)
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

    public bool AddVegetable(IngredientTypes.Vegetables vegetable)
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

    public bool AddGenericTopping(IngredientTypes.GenericToppings topping)
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

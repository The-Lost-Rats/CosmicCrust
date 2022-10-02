using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public static PlayController instance = null;

    public Pizza pizzaPrefab;

    [SerializeField] private List<PizzaOrder> pizzaOrders = null;

    private int numOrdersCompleted;

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
        numOrdersCompleted = 0;
    }
}

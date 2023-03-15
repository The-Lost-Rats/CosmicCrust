using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingsDisplay : MonoBehaviour
{
    public GameObject overlay;
 
    [System.Serializable]
    public struct SauceImage
    {
        public IngredientTypes.Sauces sauce;
        public Texture2D image;
    }
    [SerializeField] public List<SauceImage> sauceImages;

    [System.Serializable]
    public struct CheeseImage
    {
        public IngredientTypes.CheeseTypes cheese;
        public Texture2D image;
    }
    [SerializeField] public List<CheeseImage> cheeseImages;

    [System.Serializable]
    public struct MeatImage
    {
        public IngredientTypes.Meats meat;
        public Texture2D image;
    }
    [SerializeField] public List<MeatImage> meatImages;

    [System.Serializable]
    public struct PepperImage
    {
        public IngredientTypes.Peppers pepper;
        public Texture2D image;
    }
    [SerializeField] public List<PepperImage> pepperImages;

    [System.Serializable]
    public struct VegetableImage
    {
        public IngredientTypes.Vegetables vegetable;
        public Texture2D image;
    }
    [SerializeField] public List<VegetableImage> vegetableImages;

    [System.Serializable]
    public struct ToppingImage
    {
        public IngredientTypes.GenericToppings topping;
        public Texture2D image;
    }
    [SerializeField] public List<ToppingImage> toppingImages;

    [SerializeField] public Texture2D pineappleImage;

    [SerializeField] public List<DisplayCell.HelpObject> helpObjects;

    private List<DisplayCell> toppingCells;

    private IDictionary<IngredientTypes.Meats, string> meatMapping = new Dictionary<IngredientTypes.Meats, string>{
        {IngredientTypes.Meats.Beef, "Beef"},
        {IngredientTypes.Meats.Chicken, "Chicken"},
        {IngredientTypes.Meats.Pepperoni, "Pepperoni"},
        {IngredientTypes.Meats.Sausage, "Sausage"},
    };
    private IDictionary<IngredientTypes.Peppers, string> peppersMapping = new Dictionary<IngredientTypes.Peppers, string>{
        {IngredientTypes.Peppers.Bell, "Bell"},
        {IngredientTypes.Peppers.Jalapeno, "Jalapeno"},
        {IngredientTypes.Peppers.Serrano, "Serrano"},
    };
    private IDictionary<IngredientTypes.Vegetables, string> vegetablesMapping = new Dictionary<IngredientTypes.Vegetables, string>{
        {IngredientTypes.Vegetables.BlackOlive, "BlackOlive"},
        {IngredientTypes.Vegetables.GreenOlive, "GreenOlive"},
        {IngredientTypes.Vegetables.Mushroom, "Mushroom"},
        {IngredientTypes.Vegetables.Tomato, "Tomato"},
    };
    private IDictionary<IngredientTypes.GenericToppings, string> genericToppingMapping = new Dictionary<IngredientTypes.GenericToppings, string>{
        {IngredientTypes.GenericToppings.Anchovy, "Anchovy"},
        {IngredientTypes.GenericToppings.Garlic, "Garlic"},
        {IngredientTypes.GenericToppings.Onion, "Onion"},
        {IngredientTypes.GenericToppings.Shrimp, "Shrimp"},
        {IngredientTypes.GenericToppings.Spinach, "Spinach"},
        {IngredientTypes.GenericToppings.Squid, "Squid"},
    };
    private string pineappleMapping = "Pineapple";

    // Start is called before the first frame update
    void Awake()
    {
        toppingCells = new List<DisplayCell>();
        foreach (Transform child in transform)
        {
            DisplayCell cell = child.gameObject.GetComponent<DisplayCell>();
            cell.overlay = overlay;
            cell.helpObjects = helpObjects;
            toppingCells.Add(cell);
        }
    }

    public void SetPizzaOrder(PizzaOrder pizzaOrder)
    {
        List<Texture2D> toppings = new List<Texture2D>();
        foreach (MeatImage meatImage in meatImages)
        {
            if (Utilities.containsMeat(pizzaOrder.meats, meatImage.meat))
            {
                toppings.Add(meatImage.image);
            }
        }
        foreach (PepperImage pepperImage in pepperImages)
        {
            if (pizzaOrder.peppers.Contains(pepperImage.pepper))
            {
                toppings.Add(pepperImage.image);
            }
        }
        foreach (VegetableImage vegetableImage in vegetableImages)
        {
            if (pizzaOrder.vegetables.Contains(vegetableImage.vegetable))
            {
                toppings.Add(vegetableImage.image);
            }
        }
        foreach (ToppingImage toppingImage in toppingImages)
        {
            if (pizzaOrder.genericToppings.Contains(toppingImage.topping))
            {
                toppings.Add(toppingImage.image);
            }
        }
        if (pizzaOrder.hasPineapple)
        {
            toppings.Add(pineappleImage);
        }
        for ( int i = 0; i < toppings.Count; i++ ) {
            Texture2D temp = toppings[i];
            int randomIndex = Random.Range(i, toppings.Count);
            toppings[i] = toppings[randomIndex];
            toppings[randomIndex] = temp;
        }

        foreach (SauceImage sauceImage in sauceImages)
        {
            if (sauceImage.sauce == pizzaOrder.sauce)
            {
                toppings.Insert(0, sauceImage.image);
                break;
            }
        }
        foreach (CheeseImage cheeseImage in cheeseImages)
        {
            if (cheeseImage.cheese == pizzaOrder.cheese)
            {
                toppings.Insert(1, cheeseImage.image);
                break;
            }
        }

        PopulateToppings(toppings);
    }

    public void ResetPizza()
    {
        PopulateToppings(new List<Texture2D>());
    }

    private void PopulateToppings(List<Texture2D> toppings)
    {
        for (int i = 0; i < toppingCells.Count; i++)
        {
            toppingCells[i].sr.sprite = null;
            if (i < toppings.Count)
            {
                toppingCells[i].SetTexture(toppings[i]);
            }
            toppingCells[i].SetCheckmarkActive(false);
        }
    }

    public void SetSauceComplete()
    {
        toppingCells[0].SetCheckmarkActive(true);
    }

    public void SetCheeseComplete()
    {
        toppingCells[1].SetCheckmarkActive(true);
    }

    private void SetToppingComplete(string key)
    {
        for (int i = 0; i < toppingCells.Count; i++)
        {
            if (toppingCells[i].cellName == key)
            {
                toppingCells[i].SetCheckmarkActive(true);
            }
        }
    }

    public void SetMeatComplete(IngredientTypes.Meats meat)
    {
        SetToppingComplete(meatMapping[meat]);
    }

    public void SetPepperComplete(IngredientTypes.Peppers pepper)
    {
        SetToppingComplete(peppersMapping[pepper]);
    }

    public void SetVegetableComplete(IngredientTypes.Vegetables vegetable)
    {
        SetToppingComplete(vegetablesMapping[vegetable]);
    }

    public void SetGenericToppingComplete(IngredientTypes.GenericToppings topping)
    {
        SetToppingComplete(genericToppingMapping[topping]);
    }

    public void SetPineappleComplete()
    {
        SetToppingComplete(pineappleMapping);
    }
}

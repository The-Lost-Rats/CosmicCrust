using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingsDisplay : MonoBehaviour
{
    public GameObject overlay;
 
    [System.Serializable]
    public struct SauceImage
    {
        public Types.Sauces sauce;
        public Texture2D image;
    }
    [SerializeField] public List<SauceImage> sauceImages;

    [System.Serializable]
    public struct CheeseImage
    {
        public Types.CheeseTypes cheese;
        public Texture2D image;
    }
    [SerializeField] public List<CheeseImage> cheeseImages;

    [System.Serializable]
    public struct MeatImage
    {
        public Types.Meats meat;
        public Texture2D image;
    }
    [SerializeField] public List<MeatImage> meatImages;

    [System.Serializable]
    public struct PepperImage
    {
        public Types.Peppers pepper;
        public Texture2D image;
    }
    [SerializeField] public List<PepperImage> pepperImages;

    [System.Serializable]
    public struct VegetableImage
    {
        public Types.Vegetables vegetable;
        public Texture2D image;
    }
    [SerializeField] public List<VegetableImage> vegetableImages;

    [System.Serializable]
    public struct ToppingImage
    {
        public Types.GenericToppings topping;
        public Texture2D image;
    }
    [SerializeField] public List<ToppingImage> toppingImages;

    [SerializeField] public Texture2D pineappleImage;

    [SerializeField] public List<DisplayCell.HelpObject> helpObjects;

    private List<DisplayCell> toppingCells;

    private IDictionary<Types.Meats, string> meatMapping = new Dictionary<Types.Meats, string>{
        {Types.Meats.Beef, "Beef"},
        {Types.Meats.Chicken, "Chicken"},
        {Types.Meats.Pepperoni, "Pepperoni"},
        {Types.Meats.Sausage, "Sausage"},
    };
    private IDictionary<Types.Peppers, string> peppersMapping = new Dictionary<Types.Peppers, string>{
        {Types.Peppers.Bell, "Bell"},
        {Types.Peppers.Jalapeno, "Jalapeno"},
        {Types.Peppers.Serrano, "Serrano"},
    };
    private IDictionary<Types.Vegetables, string> vegetablesMapping = new Dictionary<Types.Vegetables, string>{
        {Types.Vegetables.BlackOlive, "BlackOlive"},
        {Types.Vegetables.GreenOlive, "GreenOlive"},
        {Types.Vegetables.Mushroom, "Mushroom"},
        {Types.Vegetables.Tomato, "Tomato"},
    };
    private IDictionary<Types.GenericToppings, string> genericToppingMapping = new Dictionary<Types.GenericToppings, string>{
        {Types.GenericToppings.Anchovy, "Anchovy"},
        {Types.GenericToppings.Garlic, "Garlic"},
        {Types.GenericToppings.Onion, "Onion"},
        {Types.GenericToppings.Shrimp, "Shrimp"},
        {Types.GenericToppings.Spinach, "Spinach"},
        {Types.GenericToppings.Squid, "Squid"},
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
            if (pizzaOrder.meats.Contains(meatImage.meat))
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

    public void SetMeatComplete(Types.Meats meat)
    {
        SetToppingComplete(meatMapping[meat]);
    }

    public void SetPepperComplete(Types.Peppers pepper)
    {
        SetToppingComplete(peppersMapping[pepper]);
    }

    public void SetVegetableComplete(Types.Vegetables vegetable)
    {
        SetToppingComplete(vegetablesMapping[vegetable]);
    }

    public void SetGenericToppingComplete(Types.GenericToppings topping)
    {
        SetToppingComplete(genericToppingMapping[topping]);
    }

    public void SetPineappleComplete()
    {
        SetToppingComplete(pineappleMapping);
    }
}

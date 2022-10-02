using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingsDisplay : MonoBehaviour
{
    [System.Serializable]
    public struct SauceImage
    {
        public Constants.Sauces sauce;
        public Texture2D image;
    }
    [SerializeField] public List<SauceImage> sauceImages;

    [System.Serializable]
    public struct CheeseImage
    {
        public Constants.CheeseTypes cheese;
        public Texture2D image;
    }
    [SerializeField] public List<CheeseImage> cheeseImages;

    [System.Serializable]
    public struct MeatImage
    {
        public Constants.Meats meat;
        public Texture2D image;
    }
    [SerializeField] public List<MeatImage> meatImages;

    [System.Serializable]
    public struct PepperImage
    {
        public Constants.Peppers pepper;
        public Texture2D image;
    }
    [SerializeField] public List<PepperImage> pepperImages;

    [System.Serializable]
    public struct VegetableImage
    {
        public Constants.Vegetables vegetable;
        public Texture2D image;
    }
    [SerializeField] public List<VegetableImage> vegetableImages;

    [System.Serializable]
    public struct ToppingImage
    {
        public Constants.GenericToppings topping;
        public Texture2D image;
    }
    [SerializeField] public List<ToppingImage> toppingImages;

    [SerializeField] public Texture2D pineappleImage;

    List<GameObject> toppingCells;

    // Start is called before the first frame update
    void Awake()
    {
        toppingCells = new List<GameObject>();
        foreach (Transform child in transform)
        {
            toppingCells.Add(child.gameObject);
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
            SpriteRenderer sr = toppingCells[i].GetComponent<SpriteRenderer>();
            sr.sprite = null;
            if (i < toppings.Count)
            {
                sr.sprite = Sprite.Create(toppings[i], new Rect(0, 0, toppings[i].width, toppings[i].height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}

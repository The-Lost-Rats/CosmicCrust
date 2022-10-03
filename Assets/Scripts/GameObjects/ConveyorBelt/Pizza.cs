using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : InteractableObject
{
    public override bool isInteractable { get { return false; }}
    public override List<string> interactableObjects { get { return new List<string>{}; }}

    public class PizzaData
    {
        public Constants.Sauces? sauce = null;
        public Constants.CheeseTypes? cheese = null;
        public HashSet<Constants.Meats> meats = new HashSet<Constants.Meats>();
        public HashSet<Constants.Peppers> peppers = new HashSet<Constants.Peppers>();
        public HashSet<Constants.Vegetables> vegetables = new HashSet<Constants.Vegetables>();
        public HashSet<Constants.GenericToppings> genericToppings = new HashSet<Constants.GenericToppings>();
        public bool hasPineapple = false;
    }
    public PizzaData pizzaData;

    [System.Serializable]
    public struct SauceImage
    {
        public Constants.Sauces sauce;
        public Texture2D image;
    }
    [SerializeField] public List<SauceImage> sauceImages;
    private int sauceBaseSortOrder = 1;

    [System.Serializable]
    public struct CheeseImage
    {
        public Constants.CheeseTypes cheese;
        public Texture2D image;
    }
    [SerializeField] public List<CheeseImage> cheeseImages;
    private int cheeseBaseSortOrder { get { return sauceBaseSortOrder + sauceImages.Count; } }

    [System.Serializable]
    public struct MeatImage
    {
        public Constants.Meats meat;
        public Texture2D image;
    }
    [SerializeField] public List<MeatImage> meatImages;
    private int meatBaseSortOrder { get { return cheeseBaseSortOrder + cheeseImages.Count; } }

    [System.Serializable]
    public struct PepperImage
    {
        public Constants.Peppers pepper;
        public Texture2D image;
    }
    [SerializeField] public List<PepperImage> pepperImages;
    private int pepperBaseSortOrder { get { return meatBaseSortOrder + meatImages.Count; } }

    [System.Serializable]
    public struct VegetableImage
    {
        public Constants.Vegetables vegetable;
        public Texture2D image;
    }
    [SerializeField] public List<VegetableImage> vegetableImages;
    private int vegetableBaseSortOrder { get { return pepperBaseSortOrder + pepperImages.Count; } }

    [System.Serializable]
    public struct ToppingImage
    {
        public Constants.GenericToppings topping;
        public Texture2D image;
    }
    [SerializeField] public List<ToppingImage> toppingImages;
    private int toppingBaseSortOrder { get { return vegetableBaseSortOrder + vegetableImages.Count; } }

    [SerializeField] public Texture2D pineappleImage;
    private int pineappleSortOrder { get { return toppingBaseSortOrder + toppingImages.Count; } }

    private void Start()
    {
        pizzaData = new PizzaData();
    }

    public void ResetPizza()
    {
        pizzaData = new PizzaData();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void SetPizza(PizzaOrder pizzaOrder)
    {
        SetSauce(pizzaOrder.sauce);
        SetCheese(pizzaOrder.cheese);
        foreach (Constants.Meats meat in pizzaOrder.meats)
        {
            AddMeat(meat);
        }
        foreach (Constants.Peppers pepper in pizzaOrder.peppers)
        {
            AddPepper(pepper);
        }
        foreach (Constants.Vegetables vegetable in pizzaOrder.vegetables)
        {
            AddVegetable(vegetable);
        }
        foreach (Constants.GenericToppings topping in pizzaOrder.genericToppings)
        {
            AddGenericTopping(topping);
        }
        if (pizzaOrder.hasPineapple)
        {
            AddPineapple();
        }
    }

    public bool SetSauce(Constants.Sauces sauce)
    {
        if (pizzaData.sauce == sauce)
        {
            pizzaData.sauce = sauce;
            for (int i = 0; i < sauceImages.Count; i++)
            {
                SauceImage sauceImage = sauceImages[i];
                if (sauceImage.sauce == sauce)
                {
                    AddPizzaLayer(sauceImage.image, sauceBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool SetCheese(Constants.CheeseTypes cheese)
    {
        if (pizzaData.cheese != cheese)
        {
            pizzaData.cheese = cheese;
            for (int i = 0; i < cheeseImages.Count; i++)
            {
                CheeseImage cheeseImage = cheeseImages[i];
                if (cheeseImage.cheese == cheese)
                {
                    AddPizzaLayer(cheeseImage.image, cheeseBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddMeat(Constants.Meats meat)
    {
        if (pizzaData.meats.Add(meat))
        {
            for (int i = 0; i < meatImages.Count; i++)
            {
                MeatImage meatImage = meatImages[i];
                if (meatImage.meat == meat)
                {
                    AddPizzaLayer(meatImage.image, meatBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddPepper(Constants.Peppers pepper)
    {
        if (pizzaData.peppers.Add(pepper))
        {
            for (int i = 0; i < pepperImages.Count; i++)
            {
                PepperImage pepperImage = pepperImages[i];
                if (pepperImage.pepper == pepper)
                {
                    AddPizzaLayer(pepperImage.image, pepperBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddVegetable(Constants.Vegetables vegetable)
    {
        if (pizzaData.vegetables.Add(vegetable))
        {
            for (int i = 0; i < vegetableImages.Count; i++)
            {
                VegetableImage vegetableImage = vegetableImages[i];
                if (vegetableImage.vegetable == vegetable)
                {
                    AddPizzaLayer(vegetableImage.image, vegetableBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddGenericTopping(Constants.GenericToppings topping)
    {
        if (pizzaData.genericToppings.Add(topping))
        {
            for (int i = 0; i < toppingImages.Count; i++)
            {
                ToppingImage toppingImage = toppingImages[i];
                if (toppingImage.topping == topping)
                {
                    AddPizzaLayer(toppingImage.image, toppingBaseSortOrder + i);
                    return true;
                }
            }
        }
        return false;
    }

    public bool AddPineapple()
    {
        if (!pizzaData.hasPineapple)
        {
            pizzaData.hasPineapple = true;
            AddPizzaLayer(pineappleImage, pineappleSortOrder);
            return true;
        }
        return false;
    }

    private void AddPizzaLayer(Texture2D layerTexture, int sortingOrder)
    {
        GameObject pizzaLayer = new GameObject();
        pizzaLayer.name = layerTexture.name;
        pizzaLayer.transform.parent = transform;
        pizzaLayer.transform.localPosition = Vector2.zero;
        pizzaLayer.transform.localScale = Vector3.one;

        SpriteRenderer layerRenderer = pizzaLayer.AddComponent<SpriteRenderer>();
        layerRenderer.sprite = Sprite.Create(layerTexture, new Rect(0, 0, layerTexture.width, layerTexture.height), new Vector2(0.5f, 0.5f));
        layerRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        layerRenderer.sortingOrder = sortingOrder;
    }

    public bool IsOrderCorrect(PizzaOrder pizzaOrder)
    {
        if (pizzaData.sauce != pizzaOrder.sauce)
        {
            return false;
        }
        if (pizzaData.cheese != pizzaOrder.cheese)
        {
            return false;
        }
        foreach (Constants.Meats meat in pizzaOrder.meats)
        {
            if (!pizzaData.meats.Contains(meat))
            {
                return false;
            }
        }
        foreach (Constants.Peppers pepper in pizzaOrder.peppers)
        {
            if (!pizzaData.peppers.Contains(pepper))
            {
                return false;
            }
        }
        foreach (Constants.Vegetables vegetable in pizzaOrder.vegetables)
        {
            if (!pizzaData.vegetables.Contains(vegetable))
            {
                return false;
            }
        }
        foreach (Constants.GenericToppings topping in pizzaOrder.genericToppings)
        {
            if (!pizzaData.genericToppings.Contains(topping))
            {
                return false;
            }
        }
        if (pizzaData.hasPineapple != pizzaOrder.hasPineapple)
        {
            return false;
        }
        return true;
    }
}

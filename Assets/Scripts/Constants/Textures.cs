using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textures
{
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
}
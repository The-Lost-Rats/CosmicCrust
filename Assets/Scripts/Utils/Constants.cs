public class Constants
{
    public enum CheeseTypes
    {
        Ball,
        Triangle,
        Cube
    }

    public enum Sauces
    {
        Marinara,
        BBQ,
        Alfredo
    }

    public enum Meats
    {
        Pepperoni,
        Sausage,
        Beef,
        Chicken
    }

    public enum Peppers
    {
        Serrano,
        Bell,
        Jalapeno,
        Ghost
    }

    public enum Vegetables
    {
        Mushroom,
        Tomato,
        GreenOlive,
        BlackOlive
    }

    public enum GenericToppings
    {
        Anchovy,
        Spinach,
        Onion,
        Garlic,
        Squid,
        Shrimp
    }

    // Well I made everything else a topping enum, might as well
    public enum Pineapple
    {
        Pineapple
    }

    public const int maxIngredients = 10;

    public const int MAX_MEAT_DELIVERY_SPOTS = 4;
}
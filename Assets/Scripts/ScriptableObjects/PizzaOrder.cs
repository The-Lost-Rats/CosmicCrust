using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PizzaOrder", order = 1)]
public class PizzaOrder : ScriptableObject
{
    public Constants.Sauces sauce;
    public Constants.CheeseTypes cheese;
    public List<Constants.Meats> meats;
    public List<Constants.Peppers> peppers;
    public List<Constants.Vegetables> vegetables;
    public List<Constants.GenericToppings> genericToppings;
    public bool hasPineapple;
}

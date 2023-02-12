using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PizzaOrder", order = 1)]
public class PizzaOrder : ScriptableObject
{
    public Types.Sauces sauce;
    public Types.CheeseTypes cheese;
    public List<Types.Meats> meats;
    public List<Types.Peppers> peppers;
    public List<Types.Vegetables> vegetables;
    public List<Types.GenericToppings> genericToppings;
    public bool hasPineapple;

    // Note: this specifies the total number of boxes to ship
    // If you specify 1 meat required and 4 boxes to ship:
    // 1 box will be the required meat
    // 3 boxes will be random sample from the meat options
    public int numMeatToShip;
}

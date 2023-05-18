using UnityEngine;
using System.Collections.Generic;

public class Utilities
{
    // Return number of digits in an int
    // Takes absolute value of input
    public static int countDigitsInt( int num )
    {
        int numDigits;
        num = (int) Mathf.Abs( num );
        numDigits = (int)Mathf.Floor( Mathf.Log10( num ) + 1 );

        return ( numDigits );
    }

    public static bool containsMeat( List<PizzaOrder.MeatItem> meats, IngredientTypes.Meats meatType )
    {
        return ( meats.FindIndex(meatItem => meatItem.meatType == meatType)  != -1 );
    }
}
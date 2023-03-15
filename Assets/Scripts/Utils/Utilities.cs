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

    public static bool containsMeat( List<PizzaOrder.MeatItem> meats, Constants.Meats meatType )
    {
        foreach ( PizzaOrder.MeatItem meatItem in meats )
        {
            if ( meatItem.meatType == meatType )
            {
                return true;
            }
        }

        return false;
    }
}
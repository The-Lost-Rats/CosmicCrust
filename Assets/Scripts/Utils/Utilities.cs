using UnityEngine;

public class Utilities
{
    // Return number of digits in an int
    public static int countDigitsInt( int num )
    {
        int numDigits;
        num = ( int ) Mathf.Abs( num );

        for ( numDigits = 0; num > 0; numDigits++ )
        {
            num /= 10;
        }

        return ( numDigits );
    }
}
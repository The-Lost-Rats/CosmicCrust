using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handle generating x boxes with required list + random to reach total y
// Send off to delivery table
public class DeliveryManager : MonoBehaviour
{
    // probably could just use isntances in scene, but I am doing this instead...
    public static DeliveryManager dmInstance = null;

    // List of all meat types cuz I couldnt generate a list of all meat types automatically cuz unity ;-;
    [SerializeField]
    public List<IngredientTypes.Meats> meatTypes;

    public void Awake() {
        if ( null == dmInstance ) {
            dmInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    // The reason we allow for maxBoxesToDeliver to be specified, is so if we want
    // we could require only 1 meat, but specify the number of boxes that show up to be 2
    // This way, we don't have to flood the player with boxes early in the game
    public bool DeliverMeat(List<PizzaOrder.MeatItem> requiredMeats, int maxBoxesToDeliver=Constants.MAX_MEAT_DELIVERY_SPOTS)
    {
        // Cant deliver enough
        if ( requiredMeats.Count > Constants.MAX_MEAT_DELIVERY_SPOTS )
        {
            return false;
        }

        // If we haven't set max boxes to deliver to be enough for required boxes
        // Set number of boxes to be delivered to be equal to required meats
        // ie. I need one box of pepperoni, but for whatever reason, I pass in maxBoxesToDeliver as 0
        // I want maxBoxesToDeliver to be 1 so I still get the pepperoni box
        int totalBoxesRequired = GetTotalBoxesCount( requiredMeats );
        if ( maxBoxesToDeliver < totalBoxesRequired )
        {
            maxBoxesToDeliver = totalBoxesRequired;
        }
        
        // Populate list up to maxBoxes to deliver
        List<Constants.Meats> meatBoxes = PopulateMeatList(requiredMeats, maxBoxesToDeliver, totalBoxesRequired);

        // Tell table to create and deliver them
        // Returns true if we did deliver the boxes
        return DeliveryTable.dtInstance.Deliver(meatBoxes);
    }

    // Create list of meats
    private List<Constants.Meats> PopulateMeatList(List<PizzaOrder.MeatItem> requiredMeats, int boxesToDeliver, int totalBoxesRequired)
    {
        List<Constants.Meats> finalMeats = new List<Constants.Meats>();

        // Copy over required meats
        foreach ( PizzaOrder.MeatItem meatItem in requiredMeats )
        {
            for ( int i = 0; i < meatItem.numBoxes; i++ )
            {
                finalMeats.Add(meatItem.meatType);
            }
        }

        // Get random meats
        // Need to fill in with random meat up to boxes to deliver
        if ( boxesToDeliver - totalBoxesRequired > 0 )
        {
            List<Constants.Meats> uniqueMeats = CreateUniqueMeatList(requiredMeats);

            for ( int i = 0; i < boxesToDeliver - totalBoxesRequired; i++ )
            {
                int meatIdx = Random.Range(0, uniqueMeats.Count);
                Constants.Meats meatType = uniqueMeats[meatIdx];

                // Eh what the heck - let's just add to required meats and return that...
                // This bit me in the butt gosh darn it - make a new list <_<
                finalMeats.Add(meatType);
            }
        }

        // Shuffle list of meats
        for ( int i = 0; i < finalMeats.Count; i++ ) {
            IngredientTypes.Meats temp = finalMeats[i];
            int randomIndex = Random.Range(i, finalMeats.Count);
            finalMeats[i] = finalMeats[randomIndex];
            finalMeats[randomIndex] = temp;
        }

        return finalMeats;
    }

    private int GetTotalBoxesCount(List<PizzaOrder.MeatItem> meats)
    {
        int total = 0;

        foreach ( PizzaOrder.MeatItem meatItem in meats )
        {
            total += meatItem.numBoxes;
        }

        return ( total );
    }

    // Create unique list of meats that exludes the required meat boxes
    // eg. if I want pepperoni as a meat, this list will not contain pepperoni
    private List<Constants.Meats> CreateUniqueMeatList(List<PizzaOrder.MeatItem> meats)
    {
        List<Constants.Meats> uniqueMeats = new List<Constants.Meats>();

        foreach ( Constants.Meats meat in meatTypes )
        {
            if ( !Utilities.containsMeat(meats, meat) )
            {
                uniqueMeats.Add(meat);
            }
        }

        return ( uniqueMeats );
    }

    // Reset delivery table
    public void Reset()
    {
        DeliveryTable.dtInstance.Reset();
    }
}

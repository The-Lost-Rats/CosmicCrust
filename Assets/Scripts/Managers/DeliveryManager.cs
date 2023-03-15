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
    public bool DeliverMeat(List<IngredientTypes.Meats> requiredMeats, int maxBoxesToDeliver=Constants.MAX_MEAT_DELIVERY_SPOTS)
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
        if ( maxBoxesToDeliver < requiredMeats.Count )
        {
            maxBoxesToDeliver = requiredMeats.Count;
        }
        
        // Populate list up to maxBoxes to deliver
        List<IngredientTypes.Meats> meatBoxes = PopulateMeatList(requiredMeats, maxBoxesToDeliver);

        // Tell table to create and deliver them
        // Returns true if we did deliver the boxes
        return DeliveryTable.dtInstance.Deliver(meatBoxes);
    }

    // Create list of meats
    private List<IngredientTypes.Meats> PopulateMeatList(List<IngredientTypes.Meats> requiredMeats, int boxesToDeliver)
    {
        // Copy over required meats
        List<IngredientTypes.Meats> finalMeats = new List<IngredientTypes.Meats>(requiredMeats);

        // Get random meats
        // TODO: do we want to exclude required meats from sampling?
        int initialCount = requiredMeats.Count;
        // Need to fill in with random meat up to boxes to deliver
        for ( int i = 0; i < boxesToDeliver - initialCount; i++ )
        {
            int meatIdx = Random.Range(0, meatTypes.Count);
            IngredientTypes.Meats meatType = meatTypes[meatIdx];

            // Eh what the heck - let's just add to required meats and return that...
            // This bit me in the butt gosh darn it - make a new list <_<
            finalMeats.Add(meatType);
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

    // Reset delivery table
    public void Reset()
    {
        DeliveryTable.dtInstance.Reset();
    }
}

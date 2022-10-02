using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handle generating x boxes with required list + random to reach total y
// Send off to delivery table
public class DeliveryManager : MonoBehaviour
{
    // probably could just use isntances in scene, but I am doing this instead...
    public static DeliveryManager dmInstance = null;

    // List of all meat types cuz I couldnt generate a list of all meat types sutomatically cuz imports ;-;
    [SerializeField]
    public List<Constants.Meats> meatTypes;

    // This should probably be in delivery table jesus
    private const int MAX_SPOTS = 4;

    public void Awake() {
        if ( null == dmInstance ) {
            dmInstance = this;
            DontDestroyOnLoad( this.gameObject );
        } else {
            Destroy( this.gameObject );
        }
    }

    public bool DeliverMeat(List<Constants.Meats> requiredMeats, int maxBoxesToDeliver=MAX_SPOTS)
    {
        // Cant deliver enough
        if ( maxBoxesToDeliver < requiredMeats.Count || requiredMeats.Count > MAX_SPOTS )
        {
            return false;
        }
        
        // Populate list up to maxBoxes to deliver
        List<Constants.Meats> meatBoxes = PopulateMeatList(requiredMeats, maxBoxesToDeliver);

        // Tell table to create and deliver them
        return DeliveryTable.dtInstance.Deliver(meatBoxes);
    }

    private List<Constants.Meats> PopulateMeatList(List<Constants.Meats> requiredMeats, int boxesToDeliver)
    {
        int initialCount = requiredMeats.Count;
        // Need to fill in with random meat up to boxes to deliver
        for ( int i = 0; i < boxesToDeliver - initialCount; i++ )
        {
            int meatIdx = Random.Range(0, meatTypes.Count);
            Constants.Meats meatType = meatTypes[meatIdx];

            // Eh what the heck - let's just add to required meats and return that...
            requiredMeats.Add(meatType);
        }

        return requiredMeats;
    }
}

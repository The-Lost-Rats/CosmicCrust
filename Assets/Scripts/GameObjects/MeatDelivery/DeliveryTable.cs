using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawn and move box from spawn loc to next free loc in list
public class DeliveryTable : MonoBehaviour
{
    // Singleton - there can only be one delivery table
    public static DeliveryTable dtInstance = null;

    // Delivery spots for boxes
    [SerializeField]
    public List<GameObject> deliverySpots;

    // Where boxes go when a round ends
    [SerializeField]
    public GameObject destroyTarget;

    // Set to true to remove boxes
    private bool wipeBoxes = false;

    // List of boxes delivered
    private List<GameObject> deliveredBoxes;

    // List of boxes to deliver
    private List<GameObject> boxesToDeliver;

    // List of boxes to delete
    private List<GameObject> boxesToDelete;

    // Current box to delete or deliver
    private GameObject currentBox;

    // Spot to deliver box
    private int deliverySpotIdx;

    public void Awake() {
        if ( null == dtInstance ) {
            dtInstance = this;
        } else {
            Destroy( this.gameObject );
        }
        
        deliveredBoxes = new List<GameObject>();
        boxesToDeliver = new List<GameObject>();
        boxesToDelete = new List<GameObject>();
        deliverySpotIdx = 0;
        currentBox = null;
    }

    void Start()
    {
    }

    // Wipe boxes
    public void WipeBoxes()
    {
        // Wipe boxes
        wipeBoxes = true;

        // Set delivery index to 0
        deliverySpotIdx = 0;
    }

    // Deliver a set of boxes
    public bool Deliver(List<IngredientTypes.Meats> meatBoxes)
    {
        // We are still delivering old boxes?
        // If so, ignore this
        if ( boxesToDeliver.Count > 0 )
        {
            return false;
        }

        // Remove previous boxes incase they are still around.
        // This will only be usefull if we do multiple deliveries in one round...
        WipeBoxes();

        // Create boxes of meat to deliver
        foreach (IngredientTypes.Meats meatType in meatBoxes)
        {
            boxesToDeliver.Add(DeliveryFactory.dfInstance.CreateMeatBox(meatType));
        }

        // We have started delivery
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if ( wipeBoxes )
        {
            // Loop through delivered boxes and move them at same time to destroy loc
            foreach (GameObject box in deliveredBoxes)
            {
                box.transform.position = Vector3.MoveTowards(box.transform.position, destroyTarget.transform.position, 0.01f);
                if (box.transform.position == destroyTarget.transform.position)
                {
                    boxesToDelete.Add(box);
                }
            }

            // Instead of doing this in a loop which is slightly more complicated and verbose with having to loop backwards etc
            // I just pop the head off the list and delete that every call to update  until all boxes are gone
            if ( boxesToDelete.Count > 0 )
            {
                GameObject boxToDelete = boxesToDelete[0];

                boxesToDelete.Remove(boxToDelete);
                deliveredBoxes.Remove(boxToDelete);
                GameObject.Destroy(boxToDelete);
            }

            // We have destroyed old boxes
            // Man there are so many extra checks here that are a result of bad code
            // Ideally this first check would never happen, but here we are
            if (deliveredBoxes.Count == 0 && boxesToDelete.Count == 0)
            {
                wipeBoxes = false;
            }
        }
        else
        {
            // Could control will a bool too but oh well
            if( boxesToDeliver.Count > 0 )
            {
                // Deliver 1 at a time
                if (currentBox == null)
                {
                    // Pop head of list
                    currentBox = boxesToDeliver[0];
                    boxesToDeliver.Remove(currentBox);
                }
            }

            // We don't need the second check here. We only every deliver that correct ammount...
            // But this makes it so if someone does try and force call deliver with too many, we don't error
            if (currentBox != null && deliverySpotIdx < Constants.MAX_MEAT_DELIVERY_SPOTS)
            {
                currentBox.transform.position = Vector3.MoveTowards(currentBox.transform.position, deliverySpots[deliverySpotIdx].transform.position, 0.02f);
                if (currentBox.transform.position == deliverySpots[deliverySpotIdx].transform.position)
                {
                    deliveredBoxes.Add(currentBox);
                    currentBox = null;

                    deliverySpotIdx++;
                }
            }
        }
    }

}

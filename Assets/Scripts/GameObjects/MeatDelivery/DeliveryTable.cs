using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawn and move box from spawn loc to next free loc in list
public class DeliveryTable : MonoBehaviour
{
    public static DeliveryTable dtInstance = null;

    [SerializeField]
    public List<GameObject> deliverySpots;

    [SerializeField]
    public GameObject destroyTarget;

    private bool wipeBoxes = false;

    private List<GameObject> deliveredBoxes;
    private List<GameObject> boxesToDeliver;
    private List<GameObject> boxesToDelete;

    private GameObject currentBox;

    private int deliverySpotIdx;

    public void Awake() {
        if ( null == dtInstance ) {
            dtInstance = this;
        } else {
            Destroy( this.gameObject );
        }
    }

    void Start()
    {
        deliveredBoxes = new List<GameObject>();
        boxesToDeliver = new List<GameObject>();
        boxesToDelete = new List<GameObject>();
        deliverySpotIdx = 0;
        currentBox = null;
    }

    public bool Deliver(List<Constants.Meats> meatBoxes)
    {
        // We are still delivering old boxes?
        if ( boxesToDeliver.Count > 0 )
        {
            return false;
        }

        wipeBoxes = true;

        foreach (Constants.Meats meatType in meatBoxes)
        {
            boxesToDeliver.Add(DeliveryFactory.dfInstance.CreateMeatBox(meatType));
        }

        deliverySpotIdx = 0;

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

            if ( boxesToDelete.Count > 0 )
            {
                GameObject boxToDelete = boxesToDelete[0];

                boxesToDelete.Remove(boxToDelete);
                deliveredBoxes.Remove(boxToDelete);
                GameObject.Destroy(boxToDelete);
            }

            // We have destroyed old boxes
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

            if (currentBox != null)
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

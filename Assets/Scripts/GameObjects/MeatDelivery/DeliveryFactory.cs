using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Given enum, instantiate correct prefab at locaion and return instance
public class DeliveryFactory : MonoBehaviour
{
    public static DeliveryFactory dfInstance = null;

    [SerializeField]
    public GameObject meatBoxPrefab;

    [SerializeField]
    public Transform initializeLocation;

    [System.Serializable]
    public struct MeatImage
    {
        public IngredientTypes.Meats meatType;
        public Texture2D image;
    }
    [SerializeField] public List<MeatImage> meatImages;

    private Hashtable meatImagesMap;

    public void Awake() {
        if ( null == dfInstance ) {
            dfInstance = this;
        } else {
            Destroy( this.gameObject );
        }

        // Translate list to hash table
        meatImagesMap = new Hashtable();
        foreach (MeatImage image in meatImages)
        {
            meatImagesMap[image.meatType] = image.image;
        }
    }

    // Create a meatBox
    public GameObject CreateMeatBox(IngredientTypes.Meats meatType)
    {
        GameObject meatBox = null;

        meatBox = Instantiate(meatBoxPrefab, initializeLocation.position, Quaternion.identity, transform);
        meatBox.GetComponent<MeatBox>().SetMeat(meatType, (Texture2D)meatImagesMap[meatType]);
        meatBox.SetActive(true);

        return meatBox;
    }
}
